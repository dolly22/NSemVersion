using System;
using Xunit;

namespace NSemVersion.Test
{
    public class BuildInfoPartComparerFacts
    {
        [Theory]
        [InlineData("build.0", "build.0", 0)]
        [InlineData("build.0.1", "build.0", 1)] // more parts wins
        [InlineData("build.1", "build.0", 1)]
        [InlineData("build.10", "build.9", -1)] // lexicographic compare
        [InlineData("build.b", "build.a", 1)]
        [InlineData("build.0", "build.1", -1)]
        [InlineData("build.a", "build.b", -1)]
        public void CompareTheory(string part1, string part2, int expectedResult)
        {
            var result = BuildInfoPartComparer.Default.Compare(part1, part2);

            if (expectedResult < 0)
                Assert.InRange(result, Int32.MinValue, -1);
            else if (expectedResult > 0)
                Assert.InRange(result, 0, Int32.MaxValue);
            else
                Assert.Equal(0, result);
        }
    }
}