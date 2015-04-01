using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NSemVersion.Test
{
    public class PreReleasePartComparerFacts
    {
        [Theory]
        [InlineData("alpha.0", "alpha.0", 0)]
        [InlineData("alpha.0.1", "alpha.0", 1)] // more parts wins
        [InlineData("alpha.1", "alpha.0", 1)]
        [InlineData("alpha.10", "alpha.9", 1)]
        [InlineData("alpha.b", "alpha.a", 1)]
        [InlineData("alpha.0", "alpha.1", -1)]
        [InlineData("alpha.a", "alpha.b", -1)]
        public void CompareTheory(string part1, string part2, int expectedResult)
        {
            var result = PreReleasePartComparer.Default.Compare(part1, part2);

            if (expectedResult < 0)
                Assert.InRange(result, Int32.MinValue, -1);
            else if (expectedResult > 0)
                Assert.InRange(result, 0, Int32.MaxValue);
            else
                Assert.Equal(0, result);
        }
    }
}
