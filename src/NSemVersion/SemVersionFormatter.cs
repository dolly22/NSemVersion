using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    public class SemVersionFormatter : IFormatProvider, ICustomFormatter
    {
        public static SemVersionFormatter Default = new SemVersionFormatter();

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            SemVersion version = arg as SemVersion;
            if (version == null)
                return String.Empty;

            if (!String.IsNullOrEmpty(format))
                return Format(format[0], version);

            return String.Empty;
        }

        private static string Format(char c, SemVersion version)
        {
            switch (c)
            {
                case 'V':
                    return GetVersionString(version, false, false);
                case 'P':
                    return GetVersionString(version, true, false);
                case 'F':
                default:
                    return GetVersionString(version, true, true);              
            }
        }

        private static string GetVersionString(SemVersion version, bool includePreRelease, bool includeMetadata)
        {
            string v = String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Patch);

            if (!includePreRelease)
                return v;

            StringBuilder sb = new StringBuilder(v);
            if (version.IsPreRelease)
            {
                sb.Append('-');
                sb.Append(version.PreRelease.ToString());
            }

            if (!includeMetadata)
                return sb.ToString();

            if (version.HasBuildMetadata)
            {
                sb.Append('+');
                sb.Append(version.BuildMetadata.ToString());
            }

            return sb.ToString();
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)
                || formatType == typeof(SemVersion))
            {
                return this;
            }

            return null;
        }
    }
}
