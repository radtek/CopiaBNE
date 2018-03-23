using System;

namespace BNE.Log
{
    public class Log : IDisposable
    {
        #region Dispose
        /// <summary>
        ///     Grava o que ficou pendente no Buffer do log.
        /// </summary>
        public static void Dispose()
        {
            BufferLog.GravarLogDB();
        }
        void IDisposable.Dispose()
        {
            Dispose();
        }
        #endregion

        /// <summary>
        ///     Chamar para parar a Thread que atualiza o buffer.
        ///     Usar somente quando for encerar o processo.
        /// </summary>
        public static void StopProcess()
        {
            BufferLog.Stop();
        }
    }
}
