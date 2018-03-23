using System;
using System.Threading;

namespace BNE.Services.Base.Threading
{
    /// <summary>
    /// Classe abstrata base para Threads trabalhadoras
    /// </summary>
    public abstract class Worker
    {

        #region Campos
        private Boolean _autoSetResetEvent = true;
        #endregion

        #region Properties

        #region ManualResetEvent
        /// <summary>
        /// O evento de reset do Thread. Sempre que a task for terminada o ManualResetEvent é colocado como "setado"
        /// Observando esse evento com o WaitHandle.WaitAll, outro Thread pode ser notificado do término desta thread
        /// </summary>
        public ManualResetEvent ManualResetEvent
        {
            get;
            private set;
        }
        #endregion

        #region AutoSetResetEvent
        /// <summary>
        /// Define se vai setar automáticamente o ManualResetEvent quando o Thread terminar sua execução. 
        /// Padrão: true
        /// </summary>
        protected Boolean AutoSetResetEvent
        {
            get { return _autoSetResetEvent; }
            set { _autoSetResetEvent = value; }
        }
        #endregion

        #endregion

        #region Métodos

        #region DoTask
        /// <summary>
        /// Callback do thread
        /// </summary>
        /// <param name="objParameter">O parâmetro do Thread</param>
        private void DoTask(Object objParameter)
        {
            try
            {
                BeginTask(objParameter);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            if (AutoSetResetEvent)
                ManualResetEvent.Set();
        }
        #endregion

        #region BeginTask
        /// <summary>
        /// O método executado pelo Thread
        /// </summary>
        /// <param name="objParameter">O parâmetro do Thread</param>
        protected abstract void BeginTask(Object objParameter);
        #endregion

        #region Start
        /// <summary>
        /// Inicia a execução do Thread 
        /// </summary>
        public void Start()
        {
            Start(null);
        }
        /// <summary>
        /// Inicia a execução do Thread
        /// </summary>
        /// <param name="objParameter">O parâmetro para o Thread</param>
        public void Start(Object objParameter)
        {
            // Precisa criar estes objetos aqui
            ManualResetEvent = new ManualResetEvent(false);
            ManualResetEvent.Reset();

            if (objParameter == null)
                ThreadPool.QueueUserWorkItem(DoTask);
            else
                ThreadPool.QueueUserWorkItem(DoTask, objParameter);
        }
        #endregion

        #region Sleep
        /// <summary>
        /// Pausa o Worker por um número determinado de milisegundos
        /// </summary>
        /// <param name="miliseconds">O intervalo de tempo que o thread vai ficar parado</param>
        protected void Sleep(int miliseconds)
        {
            Thread.Sleep(miliseconds);
        }
        #endregion

        #endregion

    }
}
