using System;

namespace BNE.Services.AsyncServices.Base.Plugins.Interface
{
    /// <summary>
    /// Interface que define o comportamento do objPluginResult dos plugins
    /// </summary>
    public interface IPluginResult
    {
        /// <summary>
        /// O nome do plugin que originou essa resposta
        /// </summary>
        String InputPluginName { get; }
        /// <summary>
        /// Se true determina que a exeução do processo pode terminar após a execução deste plugin
        /// </summary>
        Boolean FinishTask { get; }
    }
}