﻿using System.ServiceProcess;

namespace BNE.Services.RastreadorCurriculo
{
    internal static class Program
    {

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            ServiceBase.Run(new RastreadorCurriculo());
        }
    }
}