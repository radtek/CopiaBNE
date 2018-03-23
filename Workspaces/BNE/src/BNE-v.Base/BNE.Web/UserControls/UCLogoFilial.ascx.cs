using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Handlers;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class UCLogoFilial : BaseUserControl
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            DefinirImagem();
        }
        #endregion

        #region Properties
        #region Height
        public Unit Height
        {
            get { return img.Height; }
            set { img.Height = value; }
        }
        #endregion

        #region Width
        public Unit Width
        {
            get { return img.Width; }
            set { img.Width = value; }
        }
        #endregion

        #region ImageUrl
        public String ImageUrl
        {
            get
            {
                return img.ImageUrl;
            }
            set
            {
                img.ImageUrl = value;
            }
        }
        #endregion

        #region Cnpj
        public decimal? Cnpj
        {
            get
            {
                if (ViewState["uclogofilialcnpj"] != null)
                {
                    decimal dec;
                    if (decimal.TryParse(Convert.ToString(ViewState["uclogofilialcnpj"]), out dec))
                        return dec;
                }
                return null;
            }
            set
            {
                ViewState["uclogofilialcnpj"] = value;
                if (value != null)
                {
                    DefinirImagem();
                }
            }

        }
        #endregion

        #region CssClass
        public String CssClass
        {
            get { return img.CssClass; }
            set { img.CssClass = value; }
        }
        #endregion

        #region Style
        public CssStyleCollection Style
        {
            get { return img.Style; }
        }
        #endregion

        #endregion

        #region Methods
        
        #region DefinirImagem
        private void DefinirImagem()
        {
            if (Cnpj.HasValue)
                img.ImageUrl = UIHelper.RetornarUrlLogo(Cnpj.Value.ToString(), PessoaJuridicaLogo.OrigemLogo.Local);
        }
        #endregion

        #endregion
    }
}