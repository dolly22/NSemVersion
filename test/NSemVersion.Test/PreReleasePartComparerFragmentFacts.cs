using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSemVersion.Test
{
    public class PreReleasePartComparerFragmentFacts
    {
        [Theory]
        [InlineData("alpha", "alpha", 0)]
        [InlineData(1, 1, 0)]
        [InlineData(1, "alpha", -1)] // numeric has lower precedence
        [InlineData("alpha", 1, 1)] // numeric has lower precedence
        [InlineData("alpha", "beta", -1)] // ASCII order for strings
        [InlineData("beta", "alpha", 1)] // ASCII order for strings
        [InlineData("alpha", "ALPHA", 0)] // ASCII order for strings
        [InlineData(1, 0, 1)] // numbers compared by value
        [InlineData(0, 1, -1)] // numbers compared by value
        public void CompareTheory(string fragment1, string fragment2, int expectedResult)
        {
            var result = PreReleasePartComparer.Default.CompareFragments(fragment1, fragment2);

            if (expectedResult < 0)
                Assert.InRange(result, Int32.MinValue, -1);
            else if (expectedResult > 0)
                Assert.InRange(result, 0, Int32.MaxValue);
            else
                Assert.Equal(0, result);
        }
    }
}
