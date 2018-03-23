using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetSharp;

namespace BNE.RedesSociais
{
    /// <summary>
    /// Integração Twitter
    /// </summary>
    public class Twitter : BNE.RedesSociais.IRedeSocial
    {
        #region Private
        private TwitterService service = null;
        #endregion 

        #region Ctor
        /// <summary>
        /// Inicia instância do twitter
        /// </summary>
        public Twitter()
            : this(String.Empty, String.Empty)
        {

        }
        /// <summary>
        /// Inicia instância do twitter
        /// </summary>
        /// <param name="consumerKey">Consumer key</param>
        /// <param name="consumerSecret">Consumer secret</param>
        public Twitter(String consumerKey, String consumerSecret)
        {
            this.service = new TwitterService(consumerKey, consumerSecret);
        }
        #endregion 

        #region Methods
        #region AuthenticateAplication
        /// <summary>
        /// Autentica a aplicação com o token e token secret fornecidos pelo twitter
        /// </summary>
        /// <param name="token">A token</param>
        /// <param name="tokenSecret">A token secret</param>
        public void AuthenticateAplication(String token, String tokenSecret)
        {
            this.service.AuthenticateWith(token, tokenSecret);
        }
        #endregion 

        #region UpdateStatus
        /// <summary>
        /// Twitta um novo status
        /// </summary>
        /// <param name="message">A mensagem a ser twitada</param>
        /// <returns>True se a mensagem foi twitada com sucesso</returns>
        public Boolean UpdateStatus(String message)
        {
            TwitterStatus status = this.service.SendTweet(message);

            // Verifica status nulo
            if (status == null)
                return false;

            // Verifica status usuário
            if (status.User == null)
                return false;
            
            return true;
        }
        #endregion 
        #endregion

    }
}
