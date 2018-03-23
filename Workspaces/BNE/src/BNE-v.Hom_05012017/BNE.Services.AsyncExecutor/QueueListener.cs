using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BNE.BLL.AsyncServices.Enumeradores;
using BNE.EL;
using BNE.Services.Base.Messaging;
using BNE.Services.Base.ProcessosAssincronos;
using Parametro = BNE.BLL.AsyncServices.Parametro;

namespace BNE.Services.AsyncExecutor
{
    /// <summary>
    ///     Thread trabalhador que vai escutar a fila de mensagens
    /// </summary>
    public class QueueListener : IDisposable
    {
        /// <summary>
        ///     Constrói o listener apontando para uma atividade
        /// </summary>
        /// <param name="tipoAtividade">O tipo de atividade</param>
        public QueueListener(TipoAtividade tipoAtividade)
        {
            _tipoAtividade = tipoAtividade;
            _queue = new QueueService<MensagemAtividade>(ProcessoAssincrono.RecuperarNomeFilaLocal(tipoAtividade), false);
            _queue.Receive += QueueReceive;
            Task.Run(() => BeginTask());
        }

        private readonly QueueService<MensagemAtividade> _queue;
        private readonly TipoAtividade _tipoAtividade;
        private readonly List<Task> _workers = new List<Task>();

        /// <summary>
        ///     Registra um worker neste listener
        /// </summary>
        /// <param name="objWorker">O worker a ser registrado</param>
        public void RegisterWorker(Task objWorker)
        {
            lock (_workers)
            {
                _workers.Add(objWorker);
            }
        }

        /// <summary>
        ///     Remove um worker deste listener
        /// </summary>
        /// <param name="objWorker">O worker a ser removido</param>
        public void UnregisterWorker(Task objWorker)
        {
            lock (_workers)
            {
                _workers.Remove(objWorker);
            }
        }

        /// <summary>
        ///     Evento disparado quando o listener recebe uma task a ser executada
        /// </summary>
        /// <param name="objMessage">A mensagem recebida</param>
        protected void QueueReceive(MensagemAtividade objMessage)
        {
            try
            {
                var qtdThreads = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.AsyncServices.Enumeradores.Parametro.QtdThreadsProcessoAssincrono));

                Task[] tasks = null;

                lock (_workers)
                {
                    if (_workers.Count >= qtdThreads)
                    {
                        tasks = _workers.ToArray();
                    }
                }

                if (tasks != null && tasks.Length > 0)
                    Task.WaitAny(tasks);

                Controller.StartTask(this, objMessage, _tipoAtividade);
            }
            catch (RecordNotFoundException ex)
            {
                Controller.Capabilities.LogError(ex);
            }
            catch (Exception ex)
            {
                Controller.Capabilities.LogError(ex);
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

        /// <summary>
        ///     Inicia a tarefa do thread
        /// </summary>
        protected void BeginTask()
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

        /// <summary>
        ///     Limpa os objetos
        /// </summary>
        public void Dispose()
        {
            if (_queue != null)
                _queue.Dispose();
        }

    }
}