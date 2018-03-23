namespace BNE.Chat.DTO.Log
{
    public class JsonLocationInfo
    {
        public JsonLocationInfo()
        {

        }
   
        public string ClassName { get; set; }

        public string FileName { get; set; }

        public string FullInfo { get; set; }

        public string LineNumber { get; set; }

        public string MethodName { get; set; }

        public System.Diagnostics.StackFrame[] StackFrames { get; set; }
    }
}
