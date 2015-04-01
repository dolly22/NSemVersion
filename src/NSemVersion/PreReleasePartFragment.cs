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
    public struct PreReleasePartFragment
    {
        public PreReleasePartFragment(string text)
            : this()
        {
            if (String.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException("text", "Parameter 'text' cannot be null or empty string");

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
