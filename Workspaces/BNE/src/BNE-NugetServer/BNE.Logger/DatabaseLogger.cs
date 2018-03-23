using System;
using BNE.Logger.Interface;

namespace BNE.Logger
{
    public class DatabaseLogger : ILogger
    {

        public DatabaseLogger()
        {
            Name = "Database Logger";
        }
        public string Name { get; set; }
        public void Error(Exception ex)
        {
            BNE.Log.Error.WriteLog(ex);
        }

        public void Error(Exception ex, string customMessage)
        {
            BNE.Log.Error.WriteLog(ex, customMessage, string.Empty);
        }

        public void Error(Exception ex, string customMessage, string payload)
        {
            BNE.Log.Error.WriteLog(ex, customMessage, payload);
        }

        public void Information(string message)
        {
            BNE.Log.Information.WriteLog(message);
        }
        public void Information(string message, string customMessage)
        {
            BNE.Log.Information.WriteLog(message, customMessage, string.Empty);
        }
        public void Information(string message, string customMessage, string payload)
        {
            BNE.Log.Information.WriteLog(message, string.Empty, payload);
        }

        public void Warning(string message)
        {
            BNE.Log.Warning.WriteLog(message);
        }
        public void Warning(string message, string customMessage)
        {
            BNE.Log.Warning.WriteLog(message, customMessage, string.Empty);
        }
        public void Warning(string message, string customMessage, string payload)
        {
            BNE.Log.Warning.WriteLog(message, string.Empty, payload);
        }

    }
}