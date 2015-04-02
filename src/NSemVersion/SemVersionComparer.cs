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
        SemVersionCompareMode compareMode;

        public SemVersionComparer(SemVersionCompareMode compareMode)
        {
            this.compareMode = compareMode;
        }

        #region Predefined instances

        /// <summary>
        /// Default comparer (compares using version, preRelase and build metadata
        /// </summary>
        public static SemVersionComparer Default
        {
            get { return VersionPreReleaseAndMetadata; }
        }

        /// <summary>
        /// Compare using version only
        /// </summary>
        public static readonly SemVersionComparer VersionOnly
            = new SemVersionComparer(SemVersionCompareMode.Version);

        /// <summary>
        /// Compare using version and preRelease (build metadata is ignored)
        /// </summary>
        public static readonly SemVersionComparer VersionAndPreRelease
            = new SemVersionComparer(SemVersionCompareMode.VersionAndPrerelease);

        /// <summary>
        /// Compare using version and preRelease (build metadata is ignored)
        /// </summary>
        public static readonly SemVersionComparer VersionPreReleaseAndMetadata
            = new SemVersionComparer(SemVersionCompareMode.VersionPreReleaseAndMetadata);

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
            var result = v1.Major.CompareTo(v2.Major);
            if (result != 0)
                return result;

            // diff at minor
            result = v1.Minor.CompareTo(v2.Minor);
            if (result != 0)
                return result;

            // diff at patch
            result = v1.Patch.CompareTo(v2.Patch);
            if (result != 0 || compareMode == SemVersionCompareMode.Version)
                return result;

            result = PreReleasePart.Compare(v1.PreRelease, v2.PreRelease);
            if (result != 0 || compareMode == SemVersionCompareMode.VersionAndPrerelease)
                return result;

            return BuildMetadataPart.Compare(v1.BuildMetadata, v2.BuildMetadata);
        }
    }

    public enum SemVersionCompareMode
    {
        /// <summary>Compare by version only</summary>
        Version,

        /// <summary>Compare by version and pre-release</summary>
        VersionAndPrerelease,

        /// <summary>Compare by version pre-release and metadata</summary>
        VersionPreReleaseAndMetadata
    }
}
