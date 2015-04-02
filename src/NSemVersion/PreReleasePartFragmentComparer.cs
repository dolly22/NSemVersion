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
        /// <summary>
        /// Default comparer instance
        /// </summary>
        public static readonly PreReleasePartFragmentComparer Default = new PreReleasePartFragmentComparer();

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

            // ignore case when compare (this is against semver 2.0.0 specification)
            return String.Compare(fragment1.TextValue, fragment2.TextValue, StringComparison.OrdinalIgnoreCase);
        }
    }
}
