using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    public static class BuildInfoPartExtensions
    {
        public static bool IsEmpty(this BuildInfoPart buildInfoPart)
        {
            if (ReferenceEquals(buildInfoPart, null))
                return true;
            return buildInfoPart.Count == 0;
        }

        public static string FormatPart(this BuildInfoPart buildInfoPart)
        {
            if (buildInfoPart.IsEmpty())
                return string.Empty;

            return "+" + buildInfoPart.ToString();
        }
    }
}
