using System;

namespace BNE.ExceptionLog.Interface
{
    public interface ILogger
    {
        string Name { get; set; }
        void Error(Exception ex);
        void Error(Exception ex, string customMessage);
        void Error(Exception ex, string customMessage, string payload);
        void Information(string message);
        void Information(string message, string payload);
        void Warning(string message);
        void Warning(string message, string payload);
    }
}
