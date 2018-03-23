using System;
using System.Collections;
using System.Runtime.Serialization;

namespace BNE.ExceptionLog.WebAPI.Models
{

    [Serializable]
    public class Exception : System.Exception
    {
        public new string Source { get; set; }
        public new string Message { get; set; }
        public new string StackTrace { get; set; }
        public new IDictionary Data { get; set; }
        public new Exception InnerException { get; set; }
        
        public Exception(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                this.Message = Core.Helpers.SerializationInfo.GetValue<string>(info, "Message");
                this.Source = Core.Helpers.SerializationInfo.GetValue<string>(info, "Source");
                this.StackTrace = Core.Helpers.SerializationInfo.GetValue<string>(info, "StackTrace");
                if (string.IsNullOrWhiteSpace(this.StackTrace))
                    this.StackTrace = Core.Helpers.SerializationInfo.GetValue<string>(info, "StackTraceString");
                this.Data = Core.Helpers.SerializationInfo.GetValue<IDictionary>(info, "Data");
                this.InnerException = Core.Helpers.SerializationInfo.GetValue<Exception>(info, "InnerException");
            }
        }
    }
}
