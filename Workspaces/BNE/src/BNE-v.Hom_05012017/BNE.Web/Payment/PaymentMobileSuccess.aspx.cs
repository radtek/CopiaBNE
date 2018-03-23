using BNE.BLL;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.Payment
{
    public partial class PaymentMobileSuccess : BasePagePagamento
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerVagas_Click(object sender, EventArgs e)
        {
            if(base.IdPessoaFisicaLogada.HasValue)
                base.RedirecionarCandidatoPesquisaVaga(new PessoaFisica(base.IdPessoaFisicaLogada.Value));
            Redirect(String.Format("http://{0}", UIHelper.RecuperarURLAmbiente()));
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Redirect(String.Format("http://{0}", UIHelper.RecuperarURLAmbiente()));
        }
    }
}