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
    public partial class PaymentMobileRegistered : BasePagePagamento
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnVagas_Click(object sender, EventArgs e)
        {
            base.RedirecionarCandidatoPesquisaVaga(new PessoaFisica(base.IdPessoaFisicaLogada.Value));
        }


        protected void btnHome_Click(object sender, EventArgs e)
        {
            Redirect(String.Format("http://{0}", UIHelper.RecuperarURLAmbiente()));
        }
	}
}