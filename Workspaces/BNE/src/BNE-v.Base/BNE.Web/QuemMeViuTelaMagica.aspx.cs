using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;

namespace BNE.Web
{
    public partial class QuemMeViuTelaMagica : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarBoxQuemMeViu();
        }

        #region CarregarBoxQuemMeViu
        public void CarregarBoxQuemMeViu()
        {
            int quantidadeVisualizacaoCurriculo = CurriculoQuemMeViu.QuantidadeVisualizacaoCurriculo(base.IdPessoaFisicaLogada.Value);

            divQuantidade.Visible = quantidadeVisualizacaoCurriculo > 0;

            litQtdeEmpresas.Text = String.Format("{0} empresa{1}", quantidadeVisualizacaoCurriculo,
                quantidadeVisualizacaoCurriculo > 1 ? "s" : String.Empty);

            litVisualizaram.Text = String.Format("visualiz{0}",
                quantidadeVisualizacaoCurriculo > 1 ? "aram" : "ou");
        }
        #endregion

        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            Redirect("~/PlanoVIP.aspx");
        }
    }
}