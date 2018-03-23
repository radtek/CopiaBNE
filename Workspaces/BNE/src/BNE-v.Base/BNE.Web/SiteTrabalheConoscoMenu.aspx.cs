using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SiteTrabalheConoscoMenu : BasePage
    {

        #region Propriedades

        #region QuantidadeVagasAnunciadas
        public int QuantidadeVagasAnunciadas
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SalaSelecionadora");
            if (!IsPostBack)
            {
                AjustarTituloTela("Sala da Selecionadora");
                QuantidadeVagasAnunciadas = Filial.RecuperarQuantidadeVagasAnunciadas(base.IdFilial.Value, null);

                btnRecrutamentoR1.PostBackUrl = Page.GetRouteUrl(Enumeradores.RouteCollection.ApresentarR1.ToString(), null);
            }
        }
        #endregion

        #region btlVagas_Click
        protected void btlVagas_Click(object sender, EventArgs e)
        {
            if (QuantidadeVagasAnunciadas > 0)
                Redirect("SalaSelecionadorVagasAnunciadas.aspx");
            else
            {
                base.UrlDestino.Value = "SiteTrabalheConoscoMenu.aspx";
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
            }
        }
        #endregion

        #endregion

    }
}