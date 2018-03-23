using System;
using System.Collections.Generic;

namespace BNE.Chat.DTO.Log
{
    public class JsonLoggingEventData
    {
        public JsonLoggingEventData()
        {

        }

        public string Domain { get; set; }

        public string ExceptionString { get; set; }

        public string Identity { get; set; }

        public JsonLevel Level { get; set; }

        public JsonLocationInfo LocationInfo { get; set; }

        public string LoggerName { get; set; }

        public string Message { get; set; }

        public Dictionary<string,object> Properties { get; set; }

        public string ThreadName { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserName { get; set; }
    }
}
