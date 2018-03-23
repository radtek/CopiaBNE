using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class ConfirmacaoPagamentoDebitoOnlineBradesco : BasePage
    {
        #region UrlRedirect - Variável 6
        /// <summary>
        /// </summary>
        public string UrlRedirect
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel6.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                {
                    Session.Add(Chave.Temporaria.Variavel6.ToString(), value);
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                }
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Request["numOrder"]) == 0) return;
            Transacao objTransacao = Transacao.LoadObject(Convert.ToInt32(Request["numOrder"]));

            if (objTransacao.PlanoAdquirido.ParaPessoaFisica())
            {
                divPlanoLiberadoVip.Visible = true;
                divPlanoLiberadoCia.Visible = false;
            }
            else
            {
                divPlanoLiberadoCia.Visible = true;
                divPlanoLiberadoVip.Visible = false;
            }
        }

        #region BtnIrParaSalaVipClick
        protected void BtnIrParaSalaVipClick(object sender, EventArgs e)
        {
            Redirect(!string.IsNullOrEmpty(UrlRedirect) ? UrlRedirect : "Default.aspx");
        }
        #endregion
    }
}