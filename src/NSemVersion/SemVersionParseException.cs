using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSemVersion
{    
    public class SemVersionParseException : Exception
    {
        public SemVersionParseException() { }
        public SemVersionParseException(string message) : base(message) { }
        public SemVersionParseException(string message, Exception inner) : base(message, inner) { }
    }
}
