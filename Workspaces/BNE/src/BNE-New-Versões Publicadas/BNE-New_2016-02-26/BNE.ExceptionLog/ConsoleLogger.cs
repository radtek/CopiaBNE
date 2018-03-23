using System;
using BNE.ExceptionLog.Interface;

namespace BNE.ExceptionLog
{
    public class ConsoleLogger : ILogger
    {

        public ConsoleLogger()
        {
            Name = "Console Logger";
        }
        public string Name { get; set; }
        public void Error(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        public void Error(Exception ex, string customMessage)
        {
            Console.WriteLine(ex.Message + " Custom Message: " + customMessage);
        }
        public void Error(Exception ex, string customMessage, string payload)
        {
            Console.WriteLine(ex.Message + " Custom Message: " + customMessage);
        }

        public void Information(string message)
        {
            Console.WriteLine(message);
        }
        public void Information(string message, string payload)
        {
            Console.WriteLine(message + " Payload: " + payload);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }
        public void Warning(string message, string payload)
        {
            Console.WriteLine(message + " Payload: " + payload);
        }

    }
}
