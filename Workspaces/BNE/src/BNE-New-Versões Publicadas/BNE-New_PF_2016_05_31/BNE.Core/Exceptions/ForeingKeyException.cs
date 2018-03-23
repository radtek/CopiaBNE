using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.Exceptions
{
    public class ForeingKeyException : Exception
    {
        public ForeingKeyException() : base() { }
        public ForeingKeyException(string message) : base(message) { }
        public ForeingKeyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
