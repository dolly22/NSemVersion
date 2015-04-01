using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Semantic version
    /// </summary>
    public sealed class SemVersion : IComparable<SemVersion>, IEquatable<SemVersion>
    {
        private int major;
        private int minor;
        private int patch;

        public SemVersion(
            int major = default(int),
            int minor = default(int),
            int patch = default(int),
            PreReleasePart preRelease = null,
            BuildInfoPart buildInfo = null)
        {
            InitFromParts(major, minor, patch, preRelease, buildInfo);
        }

        public SemVersion(string version)
        {
            if (String.IsNullOrWhiteSpace(version))
                throw new ArgumentNullException("version");

            InitFromFragments(SemVersionParser.Default.Parse(version));
        }

        public SemVersion(SemVersionParser.ParsedFragments fragments)
        {
            if (fragments == null)
                throw new ArgumentNullException("fragments");

            InitFromFragments(fragments);
        }

        private void InitFromFragments(SemVersionParser.ParsedFragments fragments)
        {
            InitFromParts(
                fragments.Major,
                fragments.Minor,
                fragments.Patch,
                new PreReleasePart(fragments.PreRelease),
                new BuildInfoPart(fragments.BuildInfo));
        }

        private void InitFromParts(int major, int minor, int patch, PreReleasePart preRelease, BuildInfoPart buildInfo)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.PreRelease = preRelease;
            this.BuildInfo = buildInfo;
        }

        public int Major {
            get
            { 
                return this.major; 
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Major version cannot be negative");
                this.major = value;
            }
        }

        public int Minor 
        {
            get
            {
                return this.minor;
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Minor version cannot be negative");
                this.minor = value;
            }
        }

        public int Patch 
        {
            get
            {
                return this.patch;
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Patch version cannot be negative");
                this.patch = value;
            }
        }

        public PreReleasePart PreRelease { get; private set; }

        public BuildInfoPart BuildInfo { get; private set; }

        #region Compare

        public static bool operator ==(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) == 0;
        }

        public static bool operator !=(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) != 0;
        }

        public static bool operator <(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) < 0;
        }

        public static bool operator >(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) > 0;
        }

        public static bool operator <=(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) <= 0;
        }

        public static bool operator >=(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2) >= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as SemVersion);
        }

        public override int GetHashCode()
        {
            var versionHash = Major.GetHashCode() ^ Minor.GetHashCode() ^ Patch.GetHashCode();
            if (PreRelease != null)
                versionHash ^= PreRelease.GetHashCode();
            if (BuildInfo != null)
                versionHash ^= BuildInfo.GetHashCode();

            return versionHash;
        }

        public bool Equals(SemVersion other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return SemVersion.Compare(this, other) == 0;
        }

        public int CompareTo(SemVersion other)
        {
            return SemVersion.Compare(this, other);
        }

        public static int Compare(SemVersion v1, SemVersion v2)
        {
            return SemVersion.Compare(v1, v2, SemVersionComparer.Default);
        }

        public static int Compare(SemVersion v1, SemVersion v2, IComparer<SemVersion> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return comparer.Compare(v1, v2);
        }

        #endregion

        #region Parsing

        public static SemVersion Parse(string version)
        {
            return new SemVersion(version);
        }

        public static bool TryParse(string version, out SemVersion semVersion)
        {
            semVersion = null;
            try
            {
                if (String.IsNullOrWhiteSpace(version))
                    return false;

                semVersion = Parse(version);
                return true;
            }
            catch (Exception)
            {
                // consume parsing exceptions
                return false;
            }
        }

        #endregion

        #region Operators

        public static implicit operator SemVersion(string value)
        {
            return new SemVersion(value);
        }

        #endregion

        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}{3}{4}", 
                Major, Minor, Patch, PreRelease.FormatPart(), BuildInfo.FormatPart());
        }
    }
}
