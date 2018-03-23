using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.Exceptions
{
    public class PrimaryKeyException : Exception
    {
        public PrimaryKeyException() : base() { }
        public PrimaryKeyException(string message) : base(message) { }
        public PrimaryKeyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
