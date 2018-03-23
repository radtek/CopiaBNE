using System;
using System.Runtime.Serialization;

namespace BNE.Model
{

    [Serializable]
    public class Exception : System.Exception
    {
        public string ClassName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string MethodName { get; set; }
        public Exception InnerException { get; set; }
        /*
         public string ClassName { get; set; }
         public string Message { get; set; }
         public string StackTrace { get; set; }
         public TraceLog InnerException { get; set; }
         public string MethodName { get; set; }
          */

        public Exception(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                /*
                this.Message = info.GetString("Message");
                this.ClassName = info.GetString("ClassName");
                this.StackTrace = info.GetString("StackTrace");
                this.MethodName = info.GetString("MethodName");

                var innerException = info.GetValue("InnerException", typeof (Exception));
                if (innerException != null)
                    this.InnerException = (ExceptionError)innerException;
                 */

                this.Message = Core.Helpers.SerializationInfo.GetValue<string>(info, "Message");
                this.ClassName = Core.Helpers.SerializationInfo.GetValue<string>(info, "ClassName");
                this.StackTrace = Core.Helpers.SerializationInfo.GetValue<string>(info, "StackTrace");
                this.MethodName = Core.Helpers.SerializationInfo.GetValue<string>(info, "MethodName");
                this.InnerException = Core.Helpers.SerializationInfo.GetValue<Exception>(info, "InnerException");
            }
        }

    }
}
