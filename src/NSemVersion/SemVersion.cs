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
    public sealed class SemVersion : IComparable<SemVersion>, IEquatable<SemVersion>, IFormattable
    {
        private int major;
        private int minor;
        private int patch;

        public SemVersion(
            int major = default(int),
            int minor = default(int),
            int patch = default(int),
            PreReleasePart preRelease = null,
            BuildMetadataPart buildMetadata = null)
        {
            InitFromParts(major, minor, patch, preRelease, buildMetadata);
        }

        public SemVersion(
            int major = default(int),
            int minor = default(int),
            int patch = default(int),
            string preRelease = null,
            string buildMetadata = null)
        {
            InitFromParts(major, minor, patch, (PreReleasePart)preRelease, (BuildMetadataPart)buildMetadata);
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
                new BuildMetadataPart(fragments.BuildMetadata));
        }

        private void InitFromParts(int major, int minor, int patch, PreReleasePart preRelease, BuildMetadataPart buildMetadata)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.PreRelease = preRelease;
            this.BuildMetadata = buildMetadata;
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

        public bool IsPreRelease
        {
            get { return !PreRelease.IsEmpty(); }
        }

        public BuildMetadataPart BuildMetadata { get; private set; }

        public bool HasBuildMetadata
        {
            get { return !BuildMetadata.IsEmpty(); }
        }

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
            if (IsPreRelease)
                versionHash ^= PreRelease.GetHashCode();
            if (HasBuildMetadata)
                versionHash ^= BuildMetadata.GetHashCode();

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

        public int CompareTo(SemVersion other, IComparer<SemVersion> comparer)
        {
            return SemVersion.Compare(this, other, comparer);
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

        #region operators

        public static explicit operator SemVersion(string value)
        {
            return new SemVersion(value);
        }

        #endregion

        #region Formatting

        public override string ToString()
        {
            return ToString("F");
        }

        public string ToString(string format)
        {
            return ToString(format, SemVersionFormatter.Default);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                    return formatter.Format(format, this, formatProvider);
            }
            return ToString();
        }

        #endregion
    }
}
