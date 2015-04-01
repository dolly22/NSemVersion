using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Comparer for semantic versions
    /// </summary>
    public class SemVersionComparer : IComparer<SemVersion>
    {
        bool ignoreBuildInfo;

        public SemVersionComparer(bool ignoreBuildInfo = false)
        {
            this.ignoreBuildInfo = ignoreBuildInfo;
        }

        #region Predefined instances

        private static readonly Lazy<SemVersionComparer> @default = new Lazy<SemVersionComparer>();
        private static readonly Lazy<SemVersionComparer> comparerIgnoreBuildInfo = new Lazy<SemVersionComparer>(() => new SemVersionComparer(true));

        /// <summary>
        /// Default comparer (compares using version, preRelase and build metadata
        /// </summary>
        public static SemVersionComparer Default
        {
            get { return @default.Value; }
        }

        /// <summary>
        /// Comparer using version and preRelease (build metadata is ignored)
        /// </summary>
        public static SemVersionComparer IgnoreBuildInfo
        {
            get { return comparerIgnoreBuildInfo.Value; }
        }

        #endregion

        public int Compare(SemVersion v1, SemVersion v2)
        {
            if (ReferenceEquals(v1, v2))
                return 0;
            if (ReferenceEquals(v1, null))
                return 1;
            if (ReferenceEquals(v2, null))
                return -1;

            // diff at major
            var majorDiff = v1.Major - v2.Major;
            if (majorDiff != 0)
                return majorDiff;

            // diff at minor
            var minorDiff = v1.Minor - v2.Minor;
            if (minorDiff != 0)
                return minorDiff;

            // diff at patch
            var patchDiff = v1.Patch - v2.Patch;
            if (patchDiff != 0)
                return patchDiff;

            var preReleaseCompare = PreReleasePart.Compare(v1.PreRelease, v2.PreRelease);
            if (preReleaseCompare != 0 || ignoreBuildInfo)
                return preReleaseCompare;

            return BuildInfoPart.Compare(v1.BuildInfo, v2.BuildInfo);
        }
    }
}
