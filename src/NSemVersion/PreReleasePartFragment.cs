using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Semantic version pre release part fragment
    /// </summary>
    public struct PreReleasePartFragment : IComparable<PreReleasePartFragment>, IEquatable<PreReleasePartFragment>
    {
        public PreReleasePartFragment(string text)
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

        public PreReleasePartFragment(int value)
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

        #region Compare

        public static bool operator ==(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) == 0;
        }

        public static bool operator !=(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) != 0;
        }

        public static bool operator <(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) < 0;
        }

        public static bool operator >(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) > 0;
        }

        public static bool operator <=(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) <= 0;
        }

        public static bool operator >=(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2) >= 0;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PreReleasePartFragment))
                return false;
            return this.Equals((PreReleasePartFragment)obj);
        }

        public override int GetHashCode()
        {
            return TextValue.GetHashCode();
        }

        public bool Equals(PreReleasePartFragment other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return PreReleasePartFragment.Compare(this, other) == 0;
        }

        public int CompareTo(PreReleasePartFragment other)
        {
            return PreReleasePartFragment.Compare(this, other);
        }

        public static int Compare(PreReleasePartFragment f1, PreReleasePartFragment f2)
        {
            return PreReleasePartFragment.Compare(f1, f2, PreReleasePartFragmentComparer.Default);
        }

        public static int Compare(PreReleasePartFragment f1, PreReleasePartFragment f2, IComparer<PreReleasePartFragment> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return comparer.Compare(f1, f2);
        }

        #endregion

        public static implicit operator PreReleasePartFragment(string value)
        {
            return new PreReleasePartFragment(value);
        }

        public static implicit operator PreReleasePartFragment(int value)
        {
            return new PreReleasePartFragment(value);
        }

        public override string ToString()
        {
            return TextValue;
        }
    }
}
