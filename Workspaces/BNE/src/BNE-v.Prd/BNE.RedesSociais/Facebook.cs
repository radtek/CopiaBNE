using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facebook;

namespace BNE.RedesSociais
{
    /// <summary>
    /// Integração Facebook
    /// </summary>
    public class Facebook : IRedeSocial
    {
        #region Private
        private FacebookClient facebook = null; // Serviço facebook
        #endregion 

        #region Ctor
        /// <summary>
        /// Instância o wrapper do facebook
        /// </summary>
        /// <param name="token">A token de autenticação</param>
        public Facebook(String token)
        {
            this.facebook = new FacebookClient(token);
        }
        #endregion 

        #region Methods
        #region UpdateStatus
        /// <summary>
        /// Atualiza o feed do facebook
        /// </summary>
        /// <param name="message">Mensagem</param>
        /// <returns>True se bem sucedido </returns>
        public bool UpdateStatus(string message)
        {
            var dicParams = new Dictionary<string, object>();
            dicParams["message"] = message;
            //dicParams["caption"] = "This is caption!";
            //dicParams["description"] = "This is description!";            
            dicParams["req_perms"] = "publish_stream";
            dicParams["scope"] = "publish_stream";

            Object o = this.facebook.Post("/me/feed", dicParams);

            return o != null;
        }
        #endregion 
        #endregion 
    }
}
