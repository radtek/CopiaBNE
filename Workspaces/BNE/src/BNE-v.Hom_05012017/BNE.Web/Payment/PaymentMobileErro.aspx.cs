using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.Payment
{
    public partial class PaymentMobileErro : BasePagePagamento
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnPagamento_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["IdPlano"]) || base.PagamentoIdentificadorPlano.HasValue)
            {
                if (!base.PagamentoIdentificadorPlano.HasValue)
                    base.PagamentoIdentificadorPlano.Value = Convert.ToInt32(Request.QueryString["IdPlano"].ToString());
                txtPlano.Value = base.PagamentoIdentificadorPlano.HasValue ? base.PagamentoIdentificadorPlano.Value.ToString() : Request.QueryString["IdPlano"].ToString();
#if DEBUG
                Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), string.Format("Payment/PaymentMobile.aspx?IdPlano={1}", txtPlano.Value)));
#else
            Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(),string.Format( "Payment/PaymentMobile.aspx?IdPlano={1}",txtPessoaFisica.Value,txtPlano.Value)));
#endif
            }
            else
                Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileFluxoVip.aspx"));
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Redirect(String.Format("http://{0}", UIHelper.RecuperarURLAmbiente()));
        }
    }
}