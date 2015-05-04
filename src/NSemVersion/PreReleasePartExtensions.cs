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
    }
}
