using BNE.Services.Base.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Services.Base.Plugins.Interface
{
    /// <summary>
    /// Interface que descreve o objPlugin de saída
    /// </summary>
    public interface IOutputPlugin: IPlugin
    {
        /// <summary>
        /// Executa a tarefa do objPlugin
        /// </summary>
        /// <param name="objResult">O Resultado do processamento do objPlugin de entrada</param>        
        /// <param name="aditionalParameters">Parâmetros adicionais para execução (pode ser nulo)</param>        
        void ExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters);
    }
}