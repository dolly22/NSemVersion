using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace NSemVersion.Test
{
    public class SemVersionParserFacts
    {
        [Theory]
        [MemberData("GetPositiveParseTestData")]
        public void PositiveParseTest(string version, SemVersionParser.ParsedFragments fragments)
        {
            var parsedFragments = new SemVersionParser().Parse(version);

            Assert.Equal(fragments.Major, parsedFragments.Major);
            Assert.Equal(fragments.Minor, parsedFragments.Minor);
            Assert.Equal(fragments.Patch, parsedFragments.Patch);
            //Assert.Equal(fragments.PreRelease, parsedFragments.PreRelease, StringComparer.InvariantCulture);
            //Assert.Equal(fragments.AdditionalInfo, parsedFragments.AdditionalInfo, StringComparer.InvariantCulture);
        }

        public static IEnumerable<object[]> GetPositiveParseTestData()
        {
            yield return new object[] { "1.2.3", new SemVersionParser.ParsedFragments(1, 2, 3) };
            yield return new object[] { "123.456.789", new SemVersionParser.ParsedFragments(123, 456, 789) };

            /*
            yield return new object[] { "1.2.3-beta", new SemVersionParser.Fragments(1, 2, 3, "beta") };
            yield return new object[] { "1.2.3-beta.1", new SemVersionParser.Fragments(1, 2, 3, "beta.1") };
            yield return new object[] { "1.2.3-beta.1+build666", new SemVersionParser.Fragments(1, 2, 3, "beta.1", "build666") };
             */
        }

        [Theory]
        [MemberData("GetNegativeParseTestData")]
        public void NegativeParseTest(string version, Type expectedException)
        {
            Assert.Throws(expectedException, () =>
            {
                var ver = new SemVersionParser().Parse(version);
            });            
        }

        public static IEnumerable<object[]> GetNegativeParseTestData()
        {
            // basic version tests - as per point 2. of semver.org 2.0.0 spec
            yield return new object[] { null, typeof(ArgumentNullException) };
            yield return new object[] { "", typeof(ArgumentNullException) };
            yield return new object[] { "01.0.1", typeof(SemVersionParseException) }; // no leading zeroes in version parts
            yield return new object[] { "1.0", typeof(SemVersionParseException) }; // incomplete version
            yield return new object[] { "1", typeof(SemVersionParseException) }; // incomplete version
            yield return new object[] { "-1", typeof(SemVersionParseException) }; // incomplete version
            yield return new object[] { "-1.-1.-1", typeof(SemVersionParseException) }; // negative numbers
            yield return new object[] { "abc", typeof(SemVersionParseException) }; // bogus version

            // version with prerelase parts tests - as per point 9. of semver.org 2.0.0 spec
            yield return new object[] { "1.0.0-", typeof(SemVersionParseException) }; // empty prerelease
            yield return new object[] { "1.0.0-.", typeof(SemVersionParseException) }; // empty prerelease fragment
            yield return new object[] { "1.0.0-build.", typeof(SemVersionParseException) }; // empty prerelease fragment
            yield return new object[] { "1.0.0-build.01", typeof(SemVersionParseException) }; // leading zero on numeric part
            yield return new object[] { "1.0.0-b*ui*ld", typeof(SemVersionParseException) }; // non alphanumeric characters

            // version with buildinfo tests - as per point 10. of semver.org 2.0.0 spec
            yield return new object[] { "1.0.0+token1+token2", typeof(SemVersionParseException) }; // multiple buildinfo tokens
            yield return new object[] { "1.0.0+token1.", typeof(SemVersionParseException) }; // empty buildinfo fragment
            yield return new object[] { "1.0.0+to%ke&n1", typeof(SemVersionParseException) }; // non alphanumeric characters
        }
    }
}
