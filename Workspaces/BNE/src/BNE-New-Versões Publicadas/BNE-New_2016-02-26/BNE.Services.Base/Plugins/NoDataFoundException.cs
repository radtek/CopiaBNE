using System;

namespace BNE.Services.Base.Plugins
{
    /// <summary>
    /// Exceção a ser lançada quando o objPlugin não encontrou dados para processar
    /// </summary>
    [Serializable]
    public sealed class NoDataFoundException:Exception
    {
        /// <summary>
        /// Construtor Padrão
        /// </summary>
        public NoDataFoundException()
        {
        }
        /// <summary>
        /// Construtor com Mensagem customizada
        /// </summary>
        /// <param name="message">Mensagem Customizada</param>
        public NoDataFoundException(String origem, String message)
            : base(String.Format("A ação \"{0}\" disparou a Exceção: \"{1}\"", origem, message))
        {

        }
    }
}