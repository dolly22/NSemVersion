using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    public static class PreReleasePartExtensions
    {
        public static bool IsEmpty(this PreReleasePart preReleasePart)
        {
            if (ReferenceEquals(preReleasePart, null))
                return true;
            return preReleasePart.Count == 0;
        }

        public static string FormatPart(this PreReleasePart preReleasePart)
        {
            if (preReleasePart.IsEmpty())
                return string.Empty;

            return "-"+ preReleasePart.ToString();
        }
    }
}
