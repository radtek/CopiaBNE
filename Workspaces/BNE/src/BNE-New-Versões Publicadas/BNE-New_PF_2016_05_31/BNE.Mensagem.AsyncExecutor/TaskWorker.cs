using System;
using BNE.Mensagem.AsyncServices.Base.ProcessosAssincronos;
using BNE.Services.AsyncServices.Base.Plugins;
using BNE.Services.AsyncServices.Base.Plugins.Interface;
using BNE.Services.AsyncServices.Base.Threading;
using Enumeradores = BNE.Mensagem.AsyncServices.BLL.Enumeradores;

namespace BNE.Mensagem.AsyncExecutor
{
    /// <summary>
    /// Executa uma tarefa
    /// </summary>
    public class TaskWorker : Worker
    {
        #region Campos
        /// <summary>
        /// A tarefa que está sendo executada
        /// </summary>
        private readonly TarefaAssincrona _objTask;
        private readonly QueueListener _parent;
        #endregion

        #region Construtor

        /// <summary>
        /// Cria uma instância do objeto
        /// </summary>
        /// <param name="parent"> </param>
        /// <param name="objTask">A tarefa a ser executada</param>
        public TaskWorker(QueueListener parent, TarefaAssincrona objTask)
        {
            AutoSetResetEvent = true;
            _objTask = objTask;
            _parent = parent;
            _parent.RegisterWorker(this);
        }
        #endregion

        #region Métodos

        #region BeginTask
        /// <summary>
        /// Executa a tarefa
        /// </summary>
        /// <param name="objParameter">O parâmetro do trabalhador</param>             
        protected override void BeginTask(object objParameter)
        {
            try
            {
                try
                {
                    #region // 1 - Muda o status da atividade para executando
                    ProcessoAssincrono.MudarStatusAtividade(_objTask.IdfAtividade, Enumeradores.StatusAtividade.Executando);
                    #endregion

                    #region // 2 - Seleciona o objPlugin de entrada correto a ser executado
                    IInputPlugin objPluginEntrada = Controller.InputPlugins.GetPlugin(_objTask.InputPlugin);

                    if (objPluginEntrada == null)
                        throw new PluginNotFoundException(_objTask.InputPlugin);
                    #endregion

                    #region // 3 - Pôe o objPlugin de entrada em execução
                    IPluginResult objResultado;
                    try
                    {
                        objResultado = objPluginEntrada.ExecuteTask(_objTask.ParametrosEntrada, _objTask.Anexos);
                    }
                    catch (NoDataFoundException)
                    {
                        // Trata quando o robô deverá encerrar a execução da atividade pois não foram encontrados dados para execução
                        ProcessoAssincrono.MudarStatusAtividade(_objTask.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoSemDados);
                        return;
                    }
                    #endregion

                    #region // 4 - Se o plugin de entrada retornou o FinishTask=True então pode parar a execução da atividade
                    if (objResultado.FinishTask)
                    {
                        ProcessoAssincrono.MudarStatusAtividade(_objTask.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoComSucesso);
                        return;
                    }
                    #endregion

                    #region // 5 - Recupera o objPlugin de saída correto a ser executado
                    IOutputPlugin objPluginSaida = Controller.OutputPlugins.GetPlugin(_objTask.OutputPlugin);

                    if (objPluginSaida == null)
                        throw new PluginNotFoundException(_objTask.OutputPlugin);
                    #endregion

                    #region // 6 - Pôe o objPlugin de saída em execução
                    objPluginSaida.ExecuteTask(objResultado, _objTask.ParametrosSaida);
                    #endregion

                    #region // 7 - Se não houve nenhum erro durante a execução grava o estado de executado com sucesso
                    ProcessoAssincrono.MudarStatusAtividade(_objTask.IdfAtividade, Enumeradores.StatusAtividade.FinalizadoComSucesso);
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

                    // Trata casos especiais de erros
                    if (ex.Message.IndexOf("Deadlock", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        // Recoloca a atividade na fila
                        ProcessoAssincrono.ReiniciarAtividade(_objTask.IdfAtividade, _objTask.TipoAtividade, _objTask.Sistema, _objTask.Template);
                        Controller.Capabilities.LogError(ex);
                        return;
                    }

                    // Erros não esperados
                    Controller.Capabilities.LogError(ex);
                    // Try..Catch para prevenção de erros em casos especiais.
                    try
                    {
                        ProcessoAssincrono.GravarErroAtividade(_objTask.IdfAtividade, ex.Message);
                    }
                    catch (Exception ex2)
                    {
                        Controller.Capabilities.LogError(ex2);
                    }
                }
            }
            finally
            {
                _parent.UnregisterWorker(this);
            }
        }
        #endregion

        #endregion
    }
}
