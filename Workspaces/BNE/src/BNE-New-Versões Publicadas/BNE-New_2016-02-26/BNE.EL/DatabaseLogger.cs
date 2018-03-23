using System;
using BNE.EL.Interface;

namespace BNE.EL
{
    public class DatabaseLogger : ILogger
    {

        public DatabaseLogger()
        {
            Name = "Database Logger";
        }
        public string Name { get; set; }
        public Guid Error(Exception ex)
        {
            return Employer.PlataformaLog.LogError.WriteLog(ex);
        }
        public Guid Error(Exception ex, string customMessage)
        {
            return Employer.PlataformaLog.LogError.WriteLog(ex, customMessage);
        }
    }
}
