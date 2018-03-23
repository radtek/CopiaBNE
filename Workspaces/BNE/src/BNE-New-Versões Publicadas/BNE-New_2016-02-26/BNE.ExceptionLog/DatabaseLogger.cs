using System;
using BNE.ExceptionLog.Interface;

namespace BNE.ExceptionLog
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
            Employer.PlataformaLog.LogError.WriteLog(ex);
        }
        public void Error(Exception ex, string customMessage)
        {
            Employer.PlataformaLog.LogError.WriteLog(ex, customMessage);
        }
        public void Error(Exception ex, string customMessage, string payload)
        {
            Employer.PlataformaLog.LogError.WriteLog(ex, customMessage + " Payload:" + payload);
        }

        public void Information(string message)
        {
            Employer.PlataformaLog.LogError.WriteLog(new Exception(message));
        }
        public void Information(string message, string payload)
        {
            Employer.PlataformaLog.LogError.WriteLog(new Exception(message), " Payload:" + payload);
        }

        public void Warning(string message)
        {
            Employer.PlataformaLog.LogError.WriteLog(new Exception(message));
        }
        public void Warning(string message, string payload)
        {
            Employer.PlataformaLog.LogError.WriteLog(new Exception(message), " Payload:" + payload);
        }

    }
}