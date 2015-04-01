using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Semantic version preRelease part
    /// </summary>
    public sealed class PreReleasePart : IReadOnlyList<PreReleasePartFragment>, IEquatable<PreReleasePart>, IComparable<PreReleasePart>
    {
        readonly List<PreReleasePartFragment> fragments;

        public PreReleasePart(string preReleasePart)
            : this(new SemVersionParser().ParsePreRelease(preReleasePart))
        {
        }

        public PreReleasePart(IEnumerable<PreReleasePartFragment> fragments = null)
        {
            if (fragments != null)
                this.fragments = new List<PreReleasePartFragment>(fragments);
            else
                this.fragments = new List<PreReleasePartFragment>();
        }

        #region IReadOnlyList<Fragment>

        public PreReleasePartFragment this[int index]
        {
            get { return fragments[index]; }
        }

        public int Count
        {
            get { return fragments.Count; }
        }

        public IEnumerator<PreReleasePartFragment> GetEnumerator()
        {
            return fragments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)fragments).GetEnumerator();
        }

        #endregion

        #region Compare

        public static bool operator ==(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) == 0;
        }

        public static bool operator !=(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) != 0;
        }

        public static bool operator <(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) < 0;
        }

        public static bool operator >(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) > 0;
        }

        public static bool operator <=(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) <= 0;
        }

        public static bool operator >=(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2) >= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PreReleasePart);
        }

        public override int GetHashCode()
        {
            return fragments.GetHashCode();
        }

        public bool Equals(PreReleasePart other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return PreReleasePart.Compare(this, other) == 0;
        }

        public int CompareTo(PreReleasePart other)
        {
            return PreReleasePart.Compare(this, other);
        }

        public static int Compare(PreReleasePart part1, PreReleasePart part2)
        {
            return PreReleasePart.Compare(part1, part2, PreReleasePartComparer.Default);
        }

        public static int Compare(PreReleasePart part1, PreReleasePart part2, IComparer<PreReleasePart> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return comparer.Compare(part1, part2);
        }

        #endregion

        #region Parsing

        public static PreReleasePart Parse(string preRelease)
        {
            return new PreReleasePart(preRelease);
        }

        public static bool TryParse(string text, out PreReleasePart preRelease)
        {
            preRelease = null;
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                    return false;

                preRelease = Parse(text);
                return true;
            }
            catch (Exception)
            {
                // consume parsing exceptions
                return false;
            }
        }

        #endregion

        public static implicit operator PreReleasePart(string value)
        {
            return new PreReleasePart(value);
        }

        public override string ToString()
        {
            return String.Join(".", fragments);
        }
    }
}
