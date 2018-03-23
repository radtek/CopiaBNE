using System;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Este componente é um ImageButton com a ImageUrlDisabled a mais que o padrão.
    /// </summary>
    public class ImageButton : System.Web.UI.WebControls.ImageButton
    {
        #region ImageUrlDisabled
        /// <summary>
        /// Imagem do botão quando está desabilitado
        /// </summary>
        public String ImageUrlDisabled
        {
            get
            {
                return Convert.ToString(this.ViewState["ImageButtonDisabled"]);
            }
            set
            {
                this.ViewState["ImageButtonDisabled"] = value;
            }
        }
        #endregion

        #region ImageUrl
        /// <inheritdoc/>
        public override string ImageUrl
        {
            get
            {
                if (!this.IsEnabled)
                {
                    if (!String.IsNullOrEmpty(this.ImageUrlDisabled))
                        return this.ImageUrlDisabled;
                }

                return base.ImageUrl;
            }
            set
            {
                base.ImageUrl = value;
            }
        }
        #endregion 
    }
}
