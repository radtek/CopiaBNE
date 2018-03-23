using System;
using System.IO;
using BNE.Log.Base;

namespace BNE.Log
{
    public sealed class Warning : Log
    {
        /// <summary>
        ///     Grava Warning
        /// </summary>
        /// <param name="warning">Informacao que será grava</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string warning)
        {
            return WriteLog(warning, string.Empty, string.Empty);
        }

        /// <summary>
        ///     Grava Warning
        /// </summary>
        /// <param name="warning">Informacao que será grava</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string warning, string customMessage)
        {
            return WriteLog(warning, customMessage, string.Empty);
        }

        /// <summary>
        ///     Grava Warning
        /// </summary>
        /// <param name="warning">Informacao que será grava</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <param name="payload">Payload de API. Grava no campo Payload na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string warning, string customMessage, string payload)
        {
            try
            {
                var log = new WarningMessage
                {
                    Message = warning,
                    CustomMessage = customMessage,
                    Payload = payload
                };

                BufferLog.AddLog(log);

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
                    // ignored
                }
            }
            return Guid.Empty;
        }
    }
}