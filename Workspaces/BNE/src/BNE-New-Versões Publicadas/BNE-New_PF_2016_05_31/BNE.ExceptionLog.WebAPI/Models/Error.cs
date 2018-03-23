using BNE.ExceptionLog.Model;

namespace BNE.ExceptionLog.WebAPI.Models
{
    public class Error : MessageBase
    {

        public string CustomMessage { get; set; }
        public Exception Exception { get; set; }

    }
}