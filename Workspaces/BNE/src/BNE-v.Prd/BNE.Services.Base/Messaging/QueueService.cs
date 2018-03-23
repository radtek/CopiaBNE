using System;
using System.Messaging;
using BNE.EL;

namespace BNE.Services.Base.Messaging
{
    /// <summary>
    ///     Serviço de messageria
    /// </summary>
    /// <typeparam name="T">O tipo da mensagem</typeparam>
    public sealed class QueueService<T> : IDisposable
    {
        #region Campos
        private readonly MessageQueue _queue;
        private readonly bool _transactional;
        #endregion

        #region Eventos
        /// <summary>
        ///     Delegate que descreve o evento receive
        /// </summary>
        /// <param name="objMessage">A mensagem recebida</param>
        public delegate void DelegateReceive(T objMessage);

        /// <summary>
        ///     Evento receive
        /// </summary>
        public event DelegateReceive Receive;
        #endregion

        #region Métodos

        #region QueueService
        /// <summary>
        ///     Cria uma nova instância do objeto
        /// </summary>
        /// <param name="queueName">O nome da fila</param>
        /// <param name="transactional">True se a fila for transacional</param>
        public QueueService(string queueName, bool transactional)
        {
            try
            {
                if (string.IsNullOrEmpty(queueName))
                    throw new ArgumentNullException("queueName");

                _transactional = transactional;

                var formatter = new XmlMessageFormatter(new[] {typeof(T)});

                //Removida validação da existencia da queue pois o assíncrono estará em outro server
                _queue = new MessageQueue(queueName) {Formatter = formatter};
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Falha ao enfileirar mensagem");
                throw;
            }
        }
        #endregion

        #region SendMessage
        /// <summary>
        ///     Envia uma mensagem com prioridade normal
        /// </summary>
        /// <param name="message">A mensagem</param>
        /// <returns>O sucesso da operação</returns>
        public bool SendMessage(T message)
        {
            return SendMessage(message, MessagePriority.Normal);
        }

        /// <summary>
        ///     Envia uma mensagem com prioridade definida
        /// </summary>
        /// <param name="message">A mensagem</param>
        /// <param name="priority">A prioridade</param>
        /// <returns>O sucesso da operação</returns>
        public bool SendMessage(T message, MessagePriority priority)
        {
            var envelope = new Message(message)
            {
                Recoverable = false,
                Priority = priority
            };

            if (_transactional)
            {
                using (var objTrans = new MessageQueueTransaction())
                {
                    objTrans.Begin();
                    _queue.Send(envelope, objTrans);
                    objTrans.Commit();
                }
            }
            else
                _queue.Send(envelope);

            return true;
        }
        #endregion

        #region ReceiveQueueMessage
        /// <summary>
        ///     Recupera uma mensagem da Message Queue em seu formato nativo
        /// </summary>
        /// <param name="id">O identificador da mensagem</param>
        /// <returns>A mensagem nativa da Message Queue</returns>
        public Message ReceiveQueueMessage(string id)
        {
            return _queue.ReceiveById(id);
        }
        #endregion

        #region ReceiveMessage
        /// <summary>
        ///     Recupera uma mensagem da Message Queue
        /// </summary>
        /// <param name="id">O identificador da mensagem</param>
        /// <returns>A mensagem recebida</returns>
        public T ReceiveMessage(string id)
        {
            var objMessage = ReceiveQueueMessage(id);
            if (objMessage != null)
                return (T) objMessage.Body;

            return default(T);
        }
        #endregion

        #region BeginReceive
        /// <summary>
        ///     Inicia o recebimento de mensagens.
        ///     As mensagens são recebidas no evento Receive
        /// </summary>
        public void BeginReceive()
        {
            _queue.ReceiveCompleted += QueueReceiveCompleted;
            _queue.BeginReceive();
        }
        #endregion

        #region QueueReceiveCompleted
        /// <summary>
        ///     Tratador do evento de receive completed
        /// </summary>
        /// <param name="sender">O objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        private void QueueReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var objMessage = (T) e.Message.Body;
            if (Receive != null)
                Receive(objMessage);
            _queue.BeginReceive();
        }
        #endregion

        #region Dispose
        /// <summary>
        ///     Limpa os objetos
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