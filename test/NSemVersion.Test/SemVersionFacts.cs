using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSemVersion.Test
{
    public class SemVersionFacts
    {
        [Theory]
        [InlineData("1.2.3")]
        [InlineData("123.456.789")]
        [InlineData("1.2.3-revision.123")]
        [InlineData("1.2.3-revision.123+build.456")]
        public void ParseAndSerializeTest(string version)
        {
            var ver = new SemVersion(version);

            Assert.Equal(version, ver.ToString());
        }
    }
}
