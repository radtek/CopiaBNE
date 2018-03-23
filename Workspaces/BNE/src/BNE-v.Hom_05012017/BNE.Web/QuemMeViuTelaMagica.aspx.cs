using System;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class QuemMeViuTelaMagica : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregarBoxQuemMeViu();
        }

        public void CarregarBoxQuemMeViu()
        {
            if (IdCurriculo.HasValue)
            {
                Curriculo objCurriculo = new Curriculo(IdCurriculo.Value);

                if (objCurriculo.VIP())//Task 41743
                    Redirect(Page.GetRouteUrl(RouteCollection.QuemMeViuVip.ToString(),null));

                var quantidadeVisualizacaoCurriculo = CurriculoQuemMeViu.QuantidadeVisualizacaoCurriculo(objCurriculo);

                
                divQuantidade.Visible = quantidadeVisualizacaoCurriculo > 0;

                litQtdeEmpresas.Text = string.Format("{0} empresa{1}", quantidadeVisualizacaoCurriculo,
                    quantidadeVisualizacaoCurriculo > 1 ? "s" : string.Empty);

                litVisualizaram.Text = string.Format("visualiz{0}",
                    quantidadeVisualizacaoCurriculo > 1 ? "aram" : "ou");
            }
            else
            {
                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }

        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            Redirect(Page.GetRouteUrl(RouteCollection.EscolhaPlano.ToString(), null));
        }
    }
}