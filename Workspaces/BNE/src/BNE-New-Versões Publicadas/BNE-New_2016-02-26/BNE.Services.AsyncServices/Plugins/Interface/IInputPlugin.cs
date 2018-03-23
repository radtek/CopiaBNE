using System;
using System.Collections.Generic;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Services.AsyncServices.Plugins.Interface
{
    /// <summary>
    /// Interface que descreve o objPlugin de entrada
    /// </summary>
    public interface IInputPlugin: IPlugin
    {
        /// <summary>
        /// Executa a tarefa do objPlugin 
        /// </summary>
        /// <param name="objParametros">Os parametros do objPlugin</param>
        /// <param name="objAnexos">Anexos caso existam (Será passado como coleção vazia caso não existam anexos) </param>
        /// <returns>O objPluginResult do processamento do objPlugin</returns>
        IPluginResult ExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<String, byte[]> objAnexos);
    }
}