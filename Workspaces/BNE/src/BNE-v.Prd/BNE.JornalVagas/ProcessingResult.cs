namespace BNE.JornalVagas
{
    public class ProcessingResult
    {
        public ProcessingResult(string subject, string body)
        {
            Subject = subject;
            Body = body;
        }

        public string Subject { get; private set; }
        public string Body { get; private set; }
    }
}
