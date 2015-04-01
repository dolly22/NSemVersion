using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Pre release part comparer
    /// </summary>
    public sealed class PreReleasePartComparer : IComparer<PreReleasePart>
    {
        private static readonly Lazy<PreReleasePartComparer> @default = new Lazy<PreReleasePartComparer>();

        /// <summary>
        /// Default comparer
        /// </summary>
        public static PreReleasePartComparer Default 
        { 
            get { return @default.Value; } 
        }

        public int Compare(PreReleasePart part1, PreReleasePart part2)
        {
            if (ReferenceEquals(part1, part2))
                return 0;

            var v1empty = part1.IsEmpty();
            var v2empty = part2.IsEmpty();

            // handle special cases with empty prerelease
            if (v1empty && v2empty)
                return 0;
            else if (v1empty && !v2empty)
                return 1;
            else if (!v1empty && v2empty)
                return -1;

            for (int i = 0; i < part1.Count; i++)
            {
                // no more tokens right
                if (part2.Count <= i)
                    return 1;

                var compare = PreReleasePartFragmentComparer.Default.Compare(part1[i], part2[i]);
                if (compare != 0)
                    return compare;
            }

            // all previous tokens were equal
            if (part2.Count > part1.Count)
                return -1;
            else
                return 0;
        }
    }
}
