using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Services.Base.Plugins
{
    /// <summary>
    /// Classe base para todos os plugins de saída
    /// </summary>
    public abstract class OutputPlugin : PluginBase, IOutputPlugin
    {

        #region ExecuteTask
        /// <inheritdoc/>
        public void ExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            DoExecuteTask(objResult,aditionalParameters);
        }
        #endregion

        #region DoExecuteTask
        /// <summary>
        /// Executa a tarefa do objPlugin
        /// </summary>
        /// <param name="objResult">O Resultado do processamento do objPlugin de entrada</param>
        /// <param name="aditionalParameters">Parâmetros adicionais para execução (pode ser nulo)</param>        
        /// <returns>True em caso de sucesso</returns>
        protected abstract void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters);
        #endregion

    }
}