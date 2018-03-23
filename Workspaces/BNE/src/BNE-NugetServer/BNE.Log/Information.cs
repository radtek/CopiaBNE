using System;
using System.IO;
using BNE.Log.Base;

namespace BNE.Log
{
    public sealed class Information : Log
    {
        /// <summary>
        ///     Grava informacao
        /// </summary>
        /// <param name="information">Informacao que será grava</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string information)
        {
            return WriteLog(information, string.Empty, string.Empty);
        }
        /// <summary>
        ///     Grava informacao
        /// </summary>
        /// <param name="information">Informacao que será grava</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string information, string customMessage)
        {
            return WriteLog(information, customMessage, string.Empty);
        }
        /// <summary>
        ///     Grava informacao
        /// </summary>
        /// <param name="information">Informacao que será grava</param>
        /// <param name="customMessage">Mensagem customizada. Grava no campo CustoMessage na tabela de log</param>
        /// <param name="payload">Payload de API. Grava no campo Payload na tabela de log</param>
        /// <returns>Id do Erro no BD</returns>
        public static Guid WriteLog(string information, string customMessage, string payload)
        {
            try
            {
                var log = new InformationMessage
                {
                    Message = information,
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