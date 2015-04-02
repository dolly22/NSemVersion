using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Comparer for build metadata parts
    /// </summary>
    public sealed class BuildMetadataPartComparer : IComparer<BuildMetadataPart>
    {
        private static readonly Lazy<BuildMetadataPartComparer> @default = new Lazy<BuildMetadataPartComparer>();

        /// <summary>
        /// Default comparer
        /// </summary>
        public static BuildMetadataPartComparer Default 
        { 
            get { return @default.Value; } 
        }

        public int Compare(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            if (ReferenceEquals(part1, part2))
                return 0;

            var part1empty = part1.IsEmpty();
            var part2empty = part2.IsEmpty();

            // handle special cases with empty prerelease
            if (part1empty && part2empty)
                return 0;
            else if (part1empty && !part2empty)
                return 1;
            else if (!part1empty && part2empty)
                return -1;

            for (int i = 0; i < part1.Count; i++)
            {
                // no more tokens right
                if (part2.Count <= i)
                    return 1;

                var compare = String.Compare(part1[i], part2[i]);
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
