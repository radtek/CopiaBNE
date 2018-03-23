using System;

namespace BNE.Services.Base.Plugins.Interface
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
        /// <summary>
        /// Se true, determina que a execução do processo vai ser finalizada pelo ponte azul
        /// </summary>
        Boolean FinishWithPonteAzul { get; }
    }
}