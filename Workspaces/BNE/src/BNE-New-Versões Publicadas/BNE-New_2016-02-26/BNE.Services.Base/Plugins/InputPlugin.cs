using System;
using System.Collections.Generic;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Services.Base.Plugins
{
    /// <summary>
    /// Classe base para todos os plugins de entrada
    /// </summary>
    public abstract class InputPlugin : PluginBase, IInputPlugin
    {

        #region ExecuteTask
        /// <inheritdoc/>
        public IPluginResult ExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<String, byte[]> objAnexos)
        {
            return DoExecuteTask(objParametros, objAnexos);
        }
        #endregion

        #region DoExecuteTask
        /// <summary>
        /// Executa a tarefa do objPlugin
        /// </summary>
        /// <param name="objParametros">Os parametros da tarefa</param>
        /// <param name="objAnexos">Anexos caso existam (Será passado como coleção vazia caso não existam anexos) </param>
        /// <returns>O objPluginResult da tarefa</returns>
        protected abstract IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<String, byte[]> objAnexos);
        #endregion

    }
}