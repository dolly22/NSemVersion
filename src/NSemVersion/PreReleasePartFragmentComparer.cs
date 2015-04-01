using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Comparer for pre release part fragments
    /// </summary>
    public sealed class PreReleasePartFragmentComparer : IComparer<PreReleasePartFragment>
    {
        private static readonly Lazy<PreReleasePartFragmentComparer> @default = new Lazy<PreReleasePartFragmentComparer>();

        public static PreReleasePartFragmentComparer Default 
        { 
            get { return @default.Value; } 
        }

        public int Compare(PreReleasePartFragment fragment1, PreReleasePartFragment fragment2)
        {
            if (ReferenceEquals(fragment1, fragment2))
                return 0;

            // null has lower precedence
            if (ReferenceEquals(fragment1, null))
                return -1;
            if (ReferenceEquals(fragment2, null))
                return 1;

            // numeric fragments are compared by value
            if (fragment1.IsNumeric && fragment2.IsNumeric)
                return fragment1.NumericValue.Value - fragment2.NumericValue.Value;

            // number has lower precedence
            if (fragment1.IsNumeric)
                return -1;
            if (fragment2.IsNumeric)
                return 1;

            return String.Compare(fragment1.TextValue, fragment2.TextValue, false);
        }
    }
}
