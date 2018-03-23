using System;
using System.Collections;

namespace BNE.ExceptionLog.Model
{
    [Serializable]
    public class TraceLog : Exception
    {
        public new string Message { get; set; }
        public new string StackTrace { get; set; }
        public new TraceLog InnerException { get; set; }
        public new IDictionary Data { get; set; }
    }
}
