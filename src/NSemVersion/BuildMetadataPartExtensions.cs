﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{
    public static class BuildMetadataPartExtensions
    {
        public static bool IsEmpty(this BuildMetadataPart buildInfoPart)
        {
            if (ReferenceEquals(buildInfoPart, null))
                return true;
            return buildInfoPart.Count == 0;
        }
    }
}
