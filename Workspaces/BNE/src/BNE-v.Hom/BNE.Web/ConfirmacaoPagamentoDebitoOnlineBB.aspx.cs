using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class ConfirmacaoPagamentoDebitoOnlineBB : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Request["refTran"]) == 0) return;
            Transacao objTransacao = Transacao.LoadObject(Convert.ToInt32(Request["refTran"]));
            if (objTransacao.StatusTransacao.IdStatusTransacao == 0)
                Transacao.AtualizarStatusTransacaoDebitoOnline(objTransacao.IdTransacao.ToString());

            if (objTransacao.PlanoAdquirido.ParaPessoaFisica())
            {
                pnlDebitoRecorrente_Aguardando_vip.Visible = true;
                pnlDebitoRecorrente_Aguardando_selecionadora.Visible = false;
            }
            else
            {
                pnlDebitoRecorrente_Aguardando_selecionadora.Visible = true;
                pnlDebitoRecorrente_Aguardando_vip.Visible = false;
            }


        }
    }
}