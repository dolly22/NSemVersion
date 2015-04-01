﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    /// <summary>
    /// Parser for semantic version
    /// </summary>
    public sealed partial class SemVersionParser
    {
        private static readonly Lazy<SemVersionParser> @default = new Lazy<SemVersionParser>();

        public static SemVersionParser Default
        {
            get { return @default.Value; }
        }

        public ParsedFragments Parse(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException("input", "String 'input' cannot be null or empty");

            return RagelParseCore(semver_en_main, input);
        }

        public IList<PreReleasePartFragment> ParsePreRelease(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException("input", "String 'input' cannot be null or empty");

            var parsed = RagelParseCore(semver_en_prerelease, input);
            return parsed.PreRelease;
        }

        public IList<string> ParseBuildInfo(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException("input", "String 'input' cannot be null or empty");

            var parsed = RagelParseCore(semver_en_buildinfo, input);
            return parsed.BuildInfo;
        }

        private static SemVersionParseException CreateParsingException(string input, int index, bool isEof, string context = null)
        {
            var message = isEof ? "Unexpected eof" : String.Format("Unexpected char '{0}' at position {1}", input[index], index);
            if (!String.IsNullOrWhiteSpace(context))
                message = String.Format("{0} while parsing {1}", message, context);

            return new SemVersionParseException(message);
        }

        public class ParsedFragments
        {
            public ParsedFragments()
            {
            }

            public ParsedFragments(int major, int minor, int patch)
            {
                this.Major = major;
                this.Minor = minor;
                this.Patch = patch;
            }

            public int Major { get; set; }

            public int Minor { get; set; }

            public int Patch { get; set; }

            public IList<PreReleasePartFragment> PreRelease { get; set; }

            public IList<string> BuildInfo { get; set; }
        }
    }
}