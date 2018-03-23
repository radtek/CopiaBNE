using BNE.Services.AsyncServices.Base.Plugins.Interface;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;

namespace BNE.Mensagem.AsyncExecutor.Plugins.Plugins
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