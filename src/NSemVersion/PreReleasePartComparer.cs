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
        /// <summary>
        /// Default comparer
        /// </summary>
        public static readonly PreReleasePartComparer Default = new PreReleasePartComparer();

        public int Compare(PreReleasePart part1, PreReleasePart part2)
        {
            int result = 0;

            if (ReferenceEquals(part1, part2))
                return 0;

            var p1Empty = part1.IsEmpty();
            var p2Empty = part2.IsEmpty();

            // handle special cases with empty prerelease
            if (p1Empty && p2Empty)
                return 0;
            else if (p1Empty && !p2Empty)
                return 1;
            else if (!p1Empty && p2Empty)
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

                result = p1Fragments.Current.CompareTo(p2Fragments.Current);
                if (result != 0)
                    return result;

                f1Exists = p1Fragments.MoveNext();
                f2Exists = p2Fragments.MoveNext();
            }

            return result;
        }
    }
}
