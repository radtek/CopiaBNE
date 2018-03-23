namespace BNE.Log.Base
{
    public class ErrorMessage : BaseMessage
    {
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public ErrorMessage InnerException { get; set; }
    }
}
