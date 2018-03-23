using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Componentes
{
    /// <summary>
    /// Argumentos do evento levantado quando uma imagem é cortada no componente de imagem
    /// </summary>
    public class ThumbnailCreatedArgs : EventArgs
    {
        #region fields
       private String relativeUrl;
        private String clientUrl;
       #endregion 

        #region Properties
        #region RelativeUrl
       /// <summary>
       /// Url relativa da imagem
       /// </summary>
        public String RelativeUrl
        {
            get { return this.relativeUrl; }
        }
        #endregion 
        #region ClientUrl
       /// <summary>
       /// Url cliente da imagem
       /// </summary>
        public String ClientUrl
        {
            get { return this.clientUrl; }
        }
        #endregion 
        #endregion

        #region Ctor
       /// <summary>
       /// Construtor da classe
       /// </summary>
       /// <param name="relative">A url relativa da imagem</param>
       /// <param name="client">A url cliente da imagem</param>
        public ThumbnailCreatedArgs(String relative, String client)
        {
            this.relativeUrl = relative;
            this.clientUrl = client;
        }
        #endregion 
    }

    /// <summary>
    /// O evento disparado quando uma imagem é cortada
    /// </summary>
    /// <param name="sender">O objeto que disparou o evento</param>
    /// <param name="e">Os parametros do evento</param>
    public delegate void ThumbnailCreatedEvent(Object sender, ThumbnailCreatedArgs e);
}
