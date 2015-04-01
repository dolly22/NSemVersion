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
    public sealed class BuildInfoPart : IReadOnlyList<string>, IEquatable<BuildInfoPart>, IComparable<BuildInfoPart>
    {
        readonly List<string> fragments;

        public BuildInfoPart(string buildInfoPart)
           : this(new SemVersionParser().ParseBuildInfo(buildInfoPart))
        {
        }

        public BuildInfoPart(IEnumerable<string> fragments = null)
        {
            if (fragments != null)
                this.fragments = new List<string>(fragments);
            else
                this.fragments = new List<string>();
        }

        #region IReadOnlyList<Fragment>

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

        public static bool operator ==(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) == 0;
        }

        public static bool operator !=(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) != 0;
        }

        public static bool operator <(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) < 0;
        }

        public static bool operator >(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) > 0;
        }

        public static bool operator <=(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) <= 0;
        }

        public static bool operator >=(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2) >= 0;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BuildInfoPart);
        }

        public override int GetHashCode()
        {
            return fragments.GetHashCode();
        }

        public bool Equals(BuildInfoPart other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return BuildInfoPart.Compare(this, other) == 0;
        }

        public int CompareTo(BuildInfoPart other)
        {
            return BuildInfoPart.Compare(this, other);
        }

        public static int Compare(BuildInfoPart part1, BuildInfoPart part2)
        {
            return BuildInfoPart.Compare(part1, part2, BuildInfoPartComparer.Default);
        }

        public static int Compare(BuildInfoPart part1, BuildInfoPart part2, IComparer<BuildInfoPart> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return comparer.Compare(part1, part2);
        }

        #endregion

        #region Parsing

        public static BuildInfoPart Parse(string buildInfo)
        {
            return new BuildInfoPart(buildInfo);
        }

        public static bool TryParse(string text, out BuildInfoPart buildInfo)
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


        public static implicit operator BuildInfoPart(string value)
        {
            return new BuildInfoPart(value);
        }

        public override string ToString()
        {
            return String.Join(".", fragments);
        }
    }
}
