using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace BNE.Web
{
    public partial class SiteTrabalheConoscoMenu : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "SalaSelecionadora");
            if (!IsPostBack)
            {
                AjustarTituloTela("Sala da Selecionadora");
                CarregarBoxMeuPlano();
                CarregarBoxCampanha();
            }
        }
        #endregion

        #region btlVagas_Click
        protected void btlVagas_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.VagasAnunciadas.ToString(), null));
        }
        #endregion

        #region LinkMeuPlano_Click
        protected void LinkMeuPlano_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
            {
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIAPlano.ToString(), null));
            }
        }
        #endregion

        #region btnCampanhaRecrutamento
        protected void btnCampanhaRecrutamento(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CampanhaRecrutamento.ToString(), null));
        }
        #endregion

        #region btlRastreadorCurriculos_Click
        protected void btlRastreadorCurriculos_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AlertaCurriculos.ToString(), null));
        }
        #endregion

        #endregion

        #region CarregarBoxMeuPlano
        public void CarregarBoxMeuPlano()
        {
            //TODO: Implementar isso na classe. Estag foca.
            // Carrega lista de planos liberados ou em aberto do usuario.
            List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(null, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null);

            // Seleciona se existir o plano liberado vigente ou o plano liberado que sera iniciado.
            PlanoAdquirido objPlanoAdquirido = lstPlanoAdquirido.Where(p => p.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado && p.DataFimPlano.Date >= DateTime.Now.Date).OrderBy(p => p.DataInicioPlano).FirstOrDefault();

            if (objPlanoAdquirido != null)
            {
                litComPlano.Visible = true;
                objPlanoAdquirido.Plano.CompleteObject();

                lblPlanoAcessoValidade.Text = string.Format("Plano {0} válido até {1}", objPlanoAdquirido.Plano.DescricaoPlano, objPlanoAdquirido.DataFimPlano.ToShortDateString());

                PlanoQuantidade objPlanoQuantidade;
                if (PlanoQuantidade.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade))
                {
                    if (objPlanoQuantidade.QuantidadeSMSUtilizado.Equals(0))
                        lblQuantidadeSMSUtilizado.Text = "0";
                    else
                        lblQuantidadeSMSUtilizado.Text = objPlanoQuantidade.QuantidadeSMSUtilizado.ToString(CultureInfo.CurrentCulture);

                    lblSMSUtilizadoMensagem.Text = " SMS Utilizados";
                }
            }
            else
            {
                litSemPlano.Visible = true;
                lblPlanoAcessoValidade.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxMeuPlanoSalaSelecionadorInicial);
            }
        }
        #endregion

        #region CarregarBoxCampanha
        public void CarregarBoxCampanha()
        {
            var saldo = new Filial(base.IdFilial.Value).SaldoCampanha();
            litSaldoCampanha.Text = string.Format("Você tem {0} {1}", saldo, saldo > 0 ? "disponível" : "disponíveis");
        }
        #endregion

    }
}