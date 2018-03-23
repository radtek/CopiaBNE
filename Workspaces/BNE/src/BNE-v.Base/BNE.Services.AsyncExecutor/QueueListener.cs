using System.Diagnostics;
using BNE.BLL.AsyncServices;
using BNE.EL;
using BNE.Services.Base.Messaging;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Base.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    /// Thread trabalhador que vai escutar a fila de mensagens
    /// </summary>
    public class QueueListener : Worker, IDisposable
    {
        #region Campos
        private readonly QueueService<MensagemAtividade> _queue;
        private readonly Enumeradores.TipoAtividade _tipoAtividade;
        private List<TaskWorker> _workers = new List<TaskWorker>();
        #endregion

        #region [ Propriedades ]
        public int CountWorker
        {
            get
            {
                return _workers.Count;
            }
        }
        #endregion

        #region Contrutor
        /// <summary>
        /// Constrói o listener apontando para um centro de serviço e atividade
        /// </summary>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public QueueListener(Enumeradores.TipoAtividade tipoAtividade)
        {
            _tipoAtividade = tipoAtividade;
            AutoSetResetEvent = false;
            _queue = new QueueService<MensagemAtividade>(ProcessoAssincrono.RecuperarNomeFila(tipoAtividade), false);
            _queue.Receive += QueueReceive;
        }
        #endregion

        #region Métodos

        #region RegisterWorker
        /// <summary>
        /// Registra um worker neste listener
        /// </summary>
        /// <param name="objWorker">O worker a ser registrado</param>
        public void RegisterWorker(TaskWorker objWorker)
        {
            lock (_workers)
            {
                _workers.Add(objWorker);
            }
        }
        #endregion

        #region UnregisterWorker
        /// <summary>
        /// Remove um worker deste listener
        /// </summary>
        /// <param name="objWorker">O worker a ser removido</param>
        public void UnregisterWorker(TaskWorker objWorker)
        {
            lock (_workers)
            {
                _workers.Remove(objWorker);
            }
        }
        #endregion

        #region QueueReceive
        /// <summary>
        /// Evento disparado quando o listener recebe uma task a ser executada
        /// </summary>
        /// <param name="objMessage">A mensagem recebida</param>
        protected void QueueReceive(MensagemAtividade objMessage)
        {
            try
            {
                int qtdThreads = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QtdThreadsProcessoAssincrono));

                WaitHandle[] handlers = null;

                lock (_workers)
                {
                    if (_workers.Count >= qtdThreads)
                    {
                        handlers = _workers.Select(wk => wk.ManualResetEvent).ToArray();
                    }
                }
                
                if (handlers != null && handlers.Length > 0)
                    WaitHandle.WaitAny(handlers);

                Controller.StartTask(this, objMessage, _tipoAtividade);
            }
            catch (RecordNotFoundException rnfEx)
            {
                Controller.Capabilities.LogError(rnfEx);
            }
            catch (Exception ex)
            {
                // Try...Catch para evitar que o robô pare para alguns tipos de erro
                try
                {
                    ProcessoAssincrono.ReiniciarAtividade(objMessage.IdfAtividade, _tipoAtividade);
                }
                catch (Exception ex2)
                {
                    Controller.Capabilities.LogError(ex2);
                }
            }

        }
        #endregion

        #region BeginTask
        /// <summary>
        /// Inicia a tarefa do thread
        /// </summary>
        /// <param name="objParameter">O parâmetro do Thread</param>
        protected override void BeginTask(object objParameter)
        {
            try
            {
                _queue.BeginReceive();
            }
            catch (Exception ex)
            {
                Controller.Capabilities.LogError(ex);

            }
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Limpa os objetos
        /// </summary>
        public void Dispose()
        {
            if (_queue != null)
                _queue.Dispose();
        }
        #endregion

        #endregion
    }
}
