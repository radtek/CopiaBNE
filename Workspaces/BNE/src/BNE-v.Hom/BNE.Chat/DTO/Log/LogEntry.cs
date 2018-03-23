namespace BNE.Chat.DTO.Log
{
    public class LogEntry
    {
        public string FormattedEvent { get; set; }
        public JsonLoggingEventData LoggingEvent { get; set; }

        public LogEntry(string formttedEvent, JsonLoggingEventData loggingEvent)
        {
            FormattedEvent = formttedEvent;
            LoggingEvent = loggingEvent;
        }
    }
}