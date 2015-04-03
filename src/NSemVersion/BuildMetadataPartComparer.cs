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
            if (part1empty && !part2empty)
                return 1;
            if (!part1empty && part2empty)
                return -1;

            var p1Fragments = part1.GetEnumerator();
            var p2Fragments = part2.GetEnumerator();

            var f1Exists = p1Fragments.MoveNext();
            var f2Exists = p2Fragments.MoveNext();

            while (f1Exists || f2Exists)
            {
                if (!f1Exists && f2Exists)
                    return -1;
                if (f1Exists && !f2Exists)
                    return 1;

                var result = String.Compare(p1Fragments.Current, p2Fragments.Current, StringComparison.OrdinalIgnoreCase);
                if (result != 0)
                    return result;

                f1Exists = p1Fragments.MoveNext();
                f2Exists = p2Fragments.MoveNext();
            }

            return 0;
        }
    }
}
