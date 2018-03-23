using System;
using BNE.EL.Interface;

namespace BNE.EL
{
    public class ConsoleLogger : ILogger
    {

        public ConsoleLogger()
        {
            Name = "Console Logger";
        }
        public string Name { get; set; }
        public Guid Error(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Guid();
        }
        public Guid Error(Exception ex, string customMessage)
        {
            Console.WriteLine(ex.Message + " Custom Message: " + customMessage);
            return new Guid();
        }

    }
}
