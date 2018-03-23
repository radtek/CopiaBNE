using System;
namespace BNE.RedesSociais
{
    /// <summary>
    /// Comporamentos comuns a todas as redes sociais
    /// </summary>
    interface IRedeSocial
    {        
        /// <summary>
        /// Atualiza o status do usuário na rede social
        /// </summary>
        /// <param name="message">Mensagem de status da rede social</param>
        /// <returns>True se bem sucedido</returns>
        bool UpdateStatus(string message);
    }
}
