﻿using System.ServiceProcess;

namespace BNE.Services.OperadoraCelular
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new OperadoraCelular()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}