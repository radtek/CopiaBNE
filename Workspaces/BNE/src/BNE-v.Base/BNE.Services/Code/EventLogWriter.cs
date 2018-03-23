using System;
using System.Diagnostics;
using BNE.Services.Properties;

namespace BNE.Services.Code
{
    public class EventLogWriter
    {

        #region LogEvent
        public static void LogEvent(String eventSourceName, string mensagem, EventLogEntryType entryType, int eventID)
        {
            AjustarEventSource(eventSourceName, Settings.Default.LogName);

            var eventLog = new EventLog(Settings.Default.LogName)
            {
                Source = eventSourceName
            };
            eventLog.WriteEntry(mensagem, entryType, eventID);
        }
        #endregion

        #region AjustarEventSource
        public static void AjustarEventSource(String eventSourceName, string logName)
        {
            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }
        }
        #endregion

    }

    public enum EventID
    {
        InicioExecucao = 1,
        FimExecucao = 2,
        ErroExecucao = 3,
        AjusteExecucao = 4,
        WarningAjusteExecucao = 5
    }
}
