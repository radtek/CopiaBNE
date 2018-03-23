using System;
using System.Threading.Tasks;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    /// Executa uma tarefa
    /// </summary>
    public class TaskWorker
    {
        /// <summary>
        /// A tarefa que está sendo executada
        /// </summary>
        private readonly TarefaAssincrona _objTarefaAssincrona;
        private readonly QueueListener _parent;
        private readonly Task _task;

        public TaskWorker(QueueListener parent, TarefaAssincrona objTarefaAssincrona)
        {
            _objTarefaAssincrona = objTarefaAssincrona;
            _parent = parent;
            _task = Task.Run(() => BeginTask());
            _parent.RegisterWorker(_task);
        }

        /// <summary>
        /// Executa a tarefa
        /// </summary>
        protected void BeginTask()
        {
            try
            {
                try
                {
                    #region // 1 - Muda o status da atividade para executando
                    ProcessoAssincrono.MudarStatusAtividade(_objTarefaAssincrona.IdfAtividade, Enumeradores.StatusAtividade.Executando);
                    #endregion

                    #region // 2 - Seleciona o objPlugin de entrada correto a ser executado
                    IInputPlugin objPluginEntrada = Controller.InputPlugins.GetPlugin(_objTarefaAssincrona.InputPlugin);

                    if (objPluginEntrada == null)
                        throw new PluginNotFoundException(_objTarefaAssincrona.InputPlugin);
                    #endregion

                    #region // 3 - Pôe o objPlugin de entrada em execução
                    IPluginResult objResultado;
                    try
                    {
                        objResultado = objPluginEntrada.ExecuteTask(_objTarefaAssincrona.ParametrosEntrada, _objTarefaAssincrona.Anexos);
                    }
                    catch (NoDataFoundException)
                    {
                        // Trata quando o robô deverá encerrar a execução da atividade pois não foram encontrados dados para execução
                        ProcessoAssincrono.MudarStatusAtividade(_objTarefaAssincrona.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoSemDados);
                        return;
                    }
                    #endregion

                    #region // 4 - Se o plugin de entrada retornou o FinishTask=True então pode parar a execução da atividade
                    if (objResultado.FinishTask)
                    {
                        ProcessoAssincrono.MudarStatusAtividade(_objTarefaAssincrona.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoComSucesso);
                        return;
                    }
                    #endregion

                    #region // 5 - Se o plugin de entrada retornou o  FinishWithPonteAzul=True então pode parar a execução da atividade, quem vai finalizar é o Ponte Azul
                    if (objResultado.FinishWithPonteAzul)
                    {
                        ProcessoAssincrono.MudarStatusAtividade(_objTarefaAssincrona.IdfAtividade, Enumeradores.StatusAtividade.EnviadoPonteAzul);
                        return;
                    }
                    #endregion 

                    #region // 6 - Recupera o objPlugin de saída correto a ser executado
                    IOutputPlugin objPluginSaida = Controller.OutputPlugins.GetPlugin(_objTarefaAssincrona.OutputPlugin);

                    if (objPluginSaida == null)
                        throw new PluginNotFoundException(_objTarefaAssincrona.OutputPlugin);
                    #endregion

                    #region // 7 - Pôe o objPlugin de saída em execução
                    objPluginSaida.ExecuteTask(objResultado, _objTarefaAssincrona.ParametrosSaida);
                    #endregion

                    #region // 8 - Se não houve nenhum erro durante a execução grava o estado de executado com sucesso
                    ProcessoAssincrono.MudarStatusAtividade(_objTarefaAssincrona.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoComSucesso);
                    #endregion
                }
                catch (Exception ex)
                {
                    var exception = ex as AggregateException;
                    if (exception != null)
                    {
                        var agg = exception;
                        if (agg.InnerExceptions != null && agg.InnerExceptions.Count == 1)
                        {
                            ex = agg.InnerException;
                        }
                    }
                    
                    Controller.Capabilities.LogError(ex);

                    try
                    {
                        ProcessoAssincrono.GravarErroAtividade(_objTarefaAssincrona.IdfAtividade, ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        Controller.Capabilities.LogError(ex2);
                    }
                }
            }
            finally
            {
                _parent.UnregisterWorker(_task);
            }
        }

    }
}
