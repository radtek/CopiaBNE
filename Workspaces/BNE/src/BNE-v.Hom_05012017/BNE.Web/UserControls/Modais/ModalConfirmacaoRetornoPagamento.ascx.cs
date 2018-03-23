using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalConfirmacaoRetornoPagamento : BaseUserControl
    {
        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region Page_Init
        protected void Page_Init(object sender, EventArgs e)
        {
            HtmlLink linkCss = new HtmlLink();
            linkCss.Href = "~/css/local/UserControls/Modais/ModalConfirmacaoRetornoPagamento.css";
            linkCss.Attributes.Add("rel", "stylesheet");
            linkCss.Attributes.Add("type", "text/css");
    
            // Add the HtmlLink to the Head section of the page.
            Page.Header.Controls.Add(linkCss);
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnVoltarSite_Click

        protected void btnVoltarSite_Click(object sender, EventArgs e)
        {
            if (redirectVoltarSite != null)
                redirectVoltarSite();
        }

        #endregion

        #endregion

        #region Métodos

        #region FecharModal
        public void FecharModal()
        {
            mpeModalConfirmacaoRetornoPagamento.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void fechar();
        public event fechar Fechar;


        public delegate void RedirectVoltarSite();
        public event RedirectVoltarSite redirectVoltarSite;

        #endregion
    }
}