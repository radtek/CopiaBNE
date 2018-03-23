using BNE.BLL.AsyncServices;
using BNE.Services.AsyncServices;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Data;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    /// Controlador do robô de processos assíncronos
    /// </summary>
    public static class Controller
    {
        #region Campos
        private static readonly Collection<QueueListener> ColListener = new Collection<QueueListener>();
        private static CleanFiles _objCleanFiles;
        #endregion

        #region Delegates

        /// <summary>
        /// Delegate para o evento que recupera o catálogo de plugins
        /// </summary>
        /// <returns>O catálogo de plugins</returns>
        public delegate ComposablePartCatalog DelegateGetPluginCatalog();
        /// <summary>
        /// Delegate para o evento que recupera as capacidades do núcleo
        /// </summary>
        /// <returns>As capacidades do núcleo</returns>
        public delegate CoreCapabilities DelegateGetControlerCapabilities();

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que recupera o catálogo de plugins
        /// </summary>
        public static event DelegateGetPluginCatalog GetPluginCatalog;
        /// <summary>
        /// Evento que recupera as capacidades do núcleo
        /// </summary>
        public static event DelegateGetControlerCapabilities GetControlerCapabilities;

        #endregion

        #region Properties

        #region InputPlugins
        /// <summary>
        /// Factory dos plugins de entrada
        /// </summary>
        public static PluginFactory<IInputPlugin> InputPlugins
        {
            get;
            private set;
        }
        #endregion

        #region OutputPlugins
        /// <summary>
        /// Factory dos plugins de saída
        /// </summary>
        public static PluginFactory<IOutputPlugin> OutputPlugins
        {
            get;
            private set;
        }
        #endregion

        #region Capabilities
        /// <summary>
        /// As capacidades deste controlador
        /// </summary>
        public static CoreCapabilities Capabilities
        {
            get;
            private set;
        }
        #endregion

        #endregion

        #region Métodos

        #region InitializeController
        /// <summary>
        /// Inicializa o controller
        /// </summary>
        public static void InitializeController()
        {
            if (GetControlerCapabilities == null)
                throw new InvalidOperationException("Não assinou o evento de OnGetControlerCapabilities.");

            Capabilities = GetControlerCapabilities();
            ReloadPlugins();
        }
        #endregion

        #region StartController
        /// <summary>
        /// Inicia o processo do controller
        /// </summary>
        public static void StartController()
        {
            try
            {
                ReiniciarFila();
                // Limpa a os listeners existentes
                foreach (QueueListener q in ColListener)
                {
                    q.Dispose();
                }
                ColListener.Clear();

                List<TipoAtividade.RelacaoTipoAtividade> objRelacaoTipoAtividade = TipoAtividade.ListarTiposAtividade();

                foreach (TipoAtividade.RelacaoTipoAtividade rel in objRelacaoTipoAtividade)
                {
                    QueueListener objListener = StartListener(rel.TipoAtividade);
                    ColListener.Add(objListener);
                }

                // Thread que limpa os arquivos
                _objCleanFiles = new CleanFiles();
                _objCleanFiles.Start();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

        }
        #endregion

        #region ReloadPlugins
        /// <summary>
        /// Recarrega os plugins de entrada e saída
        /// </summary>
        private static void ReloadPlugins()
        {
            if (GetPluginCatalog == null)
                throw new InvalidOperationException("Não assinou o evento OnGetPluginCatalog");

            InputPlugins = new PluginFactory<IInputPlugin>(GetPluginCatalog());
            // Inicia os plugins
            foreach (IInputPlugin ip in InputPlugins.GetAllPlugins())
                ip.InitializeComponent(Capabilities);

            OutputPlugins = new PluginFactory<IOutputPlugin>(GetPluginCatalog());
            // Inicia os plugins
            foreach (IOutputPlugin op in OutputPlugins.GetAllPlugins())
                op.InitializeComponent(Capabilities);
        }
        #endregion

        #region StartTask
        /// <summary>
        /// Inicia uma tarefa
        /// </summary>
        /// <param name="objListener">A tarefa a ser iniciada</param>
        /// <param name="objMensagem">O centro de serviço</param>
        /// <param name="tipoAtividade">O tipo da atividade</param>
        public static TaskWorker StartTask(QueueListener objListener, MensagemAtividade objMensagem, BLL.AsyncServices.Enumeradores.TipoAtividade tipoAtividade)
        {
            try
            {
                var threadParam = new List<object>
                    {
                        AppDomain.GetCurrentThreadId()
                    };

                var objTaskWorker = new TaskWorker(objListener,ProcessoAssincrono.RecuperarAtividade(objMensagem.IdfAtividade));

                objTaskWorker.Start(threadParam);
                return objTaskWorker;
            }
            catch (Exception ex)
            {
                // Tem que previnir timout e outofmemory na recuperação da tarefa e criação da thread
                if (ex.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) > -1 ||
                    ex.Message.IndexOf("OutOfMemory", StringComparison.OrdinalIgnoreCase) > -1 ||
                    ex.Message.IndexOf("Deadlock", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    // Recoloca a atividade na fila
                    ProcessoAssincrono.ReiniciarAtividade(objMensagem.IdfAtividade, tipoAtividade);
                }
                else
                {
                    ProcessoAssincrono.GravarErroAtividade(objMensagem.IdfAtividade, ex.Message);
                }

                EL.GerenciadorException.GravarExcecao(ex);
                return null;
            }
        }
        #endregion

        #region StartListener
        /// <summary>
        /// Inicia um listener de fila escutando um centro de serviço e atividade 
        /// </summary>
        /// <param name="tipoAtividade">A atividade</param>
        public static QueueListener StartListener(BLL.AsyncServices.Enumeradores.TipoAtividade tipoAtividade)
        {
            var objListener = new QueueListener(tipoAtividade);
            objListener.Start();
            return objListener;
        }
        #endregion

        #region CleanFiles
        /// <summary>
        /// Limpa os arquivos anexos e gerados dentro do prazo de exclusão
        /// </summary>        
        public static void CleanFiles()
        {
            try
            {            
                DataTable dtArquivos = Atividade.RecuperarArquivosAExcluir();

                if (dtArquivos.Rows.Count > 0)
                {
                    foreach (DataRow row in dtArquivos.Rows)
                    {
                        // Anexos
                        if (!String.IsNullOrEmpty(Convert.ToString(row["Des_Caminho_Arquivo_Upload"])))
                            ProcessoAssincrono.ExcluirArquivoAnexo(Convert.ToString(row["Des_Caminho_Arquivo_Upload"]));
                        // Resultados de geração
                        if (!String.IsNullOrEmpty(Convert.ToString(row["Des_Caminho_Arquivo_Gerado"])))
                            ProcessoAssincrono.ExcluirArquivoGerado(Convert.ToString(row["Des_Caminho_Arquivo_Gerado"]));
                    }
                }
            }
            catch (Exception ex)
            {
                Capabilities.LogError(ex);
            }
        }
        #endregion

        #region ReiniciarFila
        /// <summary>
        /// Reinicia os ítens parados na fila de todos os centros de serviço
        /// </summary>
        public static void ReiniciarFila()
        {
            try
            {
                Collection<Atividade.AtividadeResumida> itens = Atividade.RecuperarAtividadesParadas();

                foreach (Atividade.AtividadeResumida item in itens)
                {                        
                    ProcessoAssincrono.EnfileirarAtividade(item.IdAtividade, item.TipoAtividade);
                }

                Atividade.ReiniciarAtividades();
            }
            catch (Exception ex)
            {
                Capabilities.LogError(ex);
            }
        }
        #endregion 

        #endregion
    }
}
