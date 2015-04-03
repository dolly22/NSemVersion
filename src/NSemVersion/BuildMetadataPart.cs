using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Semantic version build metadata part
    /// </summary>
    public sealed class BuildMetadataPart : IReadOnlyList<string>, IEquatable<BuildMetadataPart>, IComparable<BuildMetadataPart>
    {
        readonly List<string> fragments;

        public BuildMetadataPart(string buildInfoPart)
           : this(new SemVersionParser().ParseBuildInfo(buildInfoPart))
        {
        }

        public BuildMetadataPart(IEnumerable<string> fragments = null)
        {
            if (fragments != null)
                this.fragments = new List<string>(fragments);
            else
                this.fragments = new List<string>();
        }

        #region IReadOnlyList<string>

        public string this[int index]
        {
            get { return fragments[index]; }
        }

        public int Count
        {
            get { return fragments.Count; }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return fragments.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)fragments).GetEnumerator();
        }

        #endregion

        #region Compare

        public static bool operator ==(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) == 0;
        }

        public static bool operator !=(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) != 0;
        }

        public static bool operator <(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) < 0;
        }

        public static bool operator >(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) > 0;
        }

        public static bool operator <=(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) <= 0;
        }

        public static bool operator >=(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2) >= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BuildMetadataPart);
        }

        public override int GetHashCode()
        {
            return fragments.GetHashCode();
        }

        public bool Equals(BuildMetadataPart other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return BuildMetadataPart.Compare(this, other) == 0;
        }

        public int CompareTo(BuildMetadataPart other)
        {
            return BuildMetadataPart.Compare(this, other);
        }

        public static int Compare(BuildMetadataPart part1, BuildMetadataPart part2)
        {
            return BuildMetadataPart.Compare(part1, part2, BuildMetadataPartComparer.Default);
        }

        public static int Compare(BuildMetadataPart part1, BuildMetadataPart part2, IComparer<BuildMetadataPart> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return comparer.Compare(part1, part2);
        }

        #endregion

        #region Parsing

        public static BuildMetadataPart Parse(string buildInfo)
        {
            return new BuildMetadataPart(buildInfo);
        }

        public static bool TryParse(string text, out BuildMetadataPart buildInfo)
        {
            buildInfo = null;
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                    return false;

                buildInfo = Parse(text);
                return true;
            }
            catch (Exception)
            {
                // consume parsing exceptions
                return false;
            }
        }

        #endregion

        public static explicit operator BuildMetadataPart(string value)
        {
            return new BuildMetadataPart(value);
        }

        public override string ToString()
        {
            return String.Join(".", fragments);
        }
    }
}
