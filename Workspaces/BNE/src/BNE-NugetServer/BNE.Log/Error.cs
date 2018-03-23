using System;
using System.Diagnostics;
using System.IO;
using BNE.Log.Base;

namespace BNE.Log
{
    public sealed class Error : Log
    {

        #region WriteLog
        /// <summary>
        ///     Grava log de erro
        /// </summary>
        /// <param name="ex">Exceção</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(Exception ex)
        {
            return WriteLog(ex, string.Empty, string.Empty);
        }

        /// <summary>
        ///     Grava log de erro
        /// </summary>
        /// <param name="ex">Exceção</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(Exception ex, string customMessage)
        {
            return WriteLog(ex, customMessage, string.Empty);
        }

        /// <summary>
        ///     Grava log de erro
        /// </summary>
        /// <param name="ex">Exceção</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <param name="payload">Payload de API. Grava no campo Payload na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(Exception ex, string customMessage, string payload)
        {
            try
            {
                var log = new ErrorMessage();
                WriteLog(ex, log, customMessage, payload);
                return log.Id;
            }
            catch (Exception ex1)
            {
                try
                {
                    Helper.GravarLogDisco(ex1);
                }
                catch
                {
                }
            }
            return Guid.Empty;
        }

        private static void WriteLog(Exception ex, ErrorMessage log, string customMessage, string payload, bool isInnerException = false)
        {
            if (string.IsNullOrEmpty(ex.StackTrace))
            {
                var pilha = new StackTrace();
                log.StackTrace = pilha.ToString();
            }
            else
                log.StackTrace = ex.StackTrace;

            log.Message = ex.Message;
            log.CustomMessage = customMessage;
            log.Payload = payload;

            if (ex.Source != null)
                log.Source = ex.Source;

            if (ex.InnerException != null)
            {
                var logInner = new ErrorMessage();
                log.InnerException = logInner;
                WriteLog(ex.InnerException, logInner, customMessage, string.Empty, true);
            }

            if (isInnerException == false)
                BufferLog.AddLog(log);
        }
        #endregion
    }
}