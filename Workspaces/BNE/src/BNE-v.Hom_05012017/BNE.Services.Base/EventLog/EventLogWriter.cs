using System;
using System.Diagnostics;

namespace BNE.Services.Base.EventLog
{
    public class EventLogWriter
    {
        public EventLogWriter(string logname, string eventsourcename)
        {
            LogName = logname;
            EventSourceName = eventsourcename;
        }

        public string LogName { get; private set; }
        public string EventSourceName { get; private set; }

        #region LogEvent
        public void LogEvent(string mensagem, EventLogEntryType entryType, Event evt)
        {
#if !DEBUG
            AjustarEventSource(EventSourceName, LogName);

            var eventLog = new System.Diagnostics.EventLog(LogName)
            {
                Source = EventSourceName
            };
            eventLog.WriteEntry(mensagem, entryType, (int) evt);
#else
            Console.WriteLine(String.Format("[{0}] {1}", entryType.ToString(), mensagem));
#endif
        }
        #endregion

        #region AjustarEventSource
        private void AjustarEventSource(string eventSourceName, string logName)
        {
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
        }
        #endregion
    }

    public enum Event
    {
        InicioExecucao = 1,
        FimExecucao = 2,
        ErroExecucao = 3,
        AjusteExecucao = 4,
        WarningAjusteExecucao = 5
    }
}