using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{    
    [Serializable]
    public class SemVersionParseException : Exception
    {
        public SemVersionParseException() { }
        public SemVersionParseException(string message) : base(message) { }
        public SemVersionParseException(string message, Exception inner) : base(message, inner) { }
        protected SemVersionParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
