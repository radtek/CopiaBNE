using System;

namespace BNE.Componentes
{
    /// <summary>
    /// Argumentos do evento levantado quando uma imagem é cortada no componente de imagem
    /// </summary>
    public class ThumbnailCreatedArgs : EventArgs
    {
        #region fields
        private readonly String _clientUrl;
        private readonly byte[] _imageData;
        #endregion

        #region Properties
        #region ClientUrl
        /// <summary>
        /// Url cliente da imagem
        /// </summary>
        public String ClientUrl
        {
            get { return this._clientUrl; }
        }
        #endregion
        #region ImageData
        /// <summary>
        /// Byte array da imagem
        /// </summary>
        public byte[] ImageData
        {
            get { return this._imageData; }
        }
        #endregion
        #endregion

        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="imageData">Byte array de data</param>
        /// <param name="client">A url cliente da imagem</param>
        public ThumbnailCreatedArgs(byte[] imageData, String client)
        {
            this._imageData = imageData;
            this._clientUrl = client;
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
