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
    public sealed class PreReleasePart : IReadOnlyList<PreReleasePart.Fragment>, IEquatable<PreReleasePart>, IComparable<PreReleasePart>
    {
        readonly List<Fragment> fragments;

        public PreReleasePart(string preReleasePart)
            : this(SemVersionParser.Default.ParsePreRelease(preReleasePart))
        {
        }

        public PreReleasePart(IEnumerable<Fragment> fragments = null)
        {
            if (fragments != null)
                this.fragments = new List<Fragment>(fragments);
            else
                this.fragments = new List<Fragment>();
        }

        #region IReadOnlyList<Fragment>

        public Fragment this[int index]
        {
            get { return fragments[index]; }
        }

        public int Count
        {
            get { return fragments.Count; }
        }

        public IEnumerator<Fragment> GetEnumerator()
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

        public static explicit operator PreReleasePart(string value)
        {
            return new PreReleasePart(value);
        }

        public override string ToString()
        {
            return String.Join(".", fragments);
        }

        /// <summary>
        /// Semantic version pre release part fragment
        /// </summary>
        public struct Fragment
        {
            public Fragment(string text)
                : this()
            {
                if (String.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException("text", "Parameter 'text' cannot be null or empty string");

                // try to handle numeric values
                int value;
                if (int.TryParse(text, out value) && !IsNumericFormatValid(text, value))
                    throw new ArgumentException("Numeric text representation is not valid, contains leading zeroes", "text");

                this.TextValue = text;
                this.NumericValue = null;
            }

            public Fragment(int value)
                : this()
            {
                this.NumericValue = value;
                this.TextValue = value.ToString();
            }

            public int? NumericValue { get; private set; }

            public string TextValue { get; private set; }

            public bool IsNumeric { get { return NumericValue.HasValue; } }

            internal static bool IsNumericFormatValid(string text, int val)
            {
                if (String.IsNullOrWhiteSpace(text))
                    throw new ArgumentNullException("text", "Parameter 'text' cannot be null or empty string");

                return !((text.Length > 1 && val == 0) || (text[0] == '0' && val > 0));
            }

            public static implicit operator Fragment(string value)
            {
                return new Fragment(value);
            }

            public static implicit operator Fragment(int value)
            {
                return new Fragment(value);
            }

            public override string ToString()
            {
                return TextValue;
            }
        }

    }
}
