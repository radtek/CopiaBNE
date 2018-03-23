using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class MenuSalaVip : BaseUserControl
    {

        #region Propriedades

        #region QuemMeViuVirgemRedirect
        public bool QuemMeViuVirgemRedirect
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel1.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region JaEnvieiVirgemRedirect
        public bool JaEnvieiVirgemRedirect
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel2.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region MeuPlanoVirgemRedirect
        public bool MeuPlanoVirgemRedirect
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel3.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }

        #endregion

        #region IdPlanoAdquirido
        public int IdPlanoAdquirido
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region QuemMeViuVip
        public bool QuemMeViuVip
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel5.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region btnPesquisarVagas_Click
        protected void btnPesquisarVagas_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaVagaAvancada.ToString(), null));
        }
        #endregion

        #region btnQuemMeViu_Click
        protected void btnQuemMeViu_Click(object sender, EventArgs e)
        {
            if (QuemMeViuVirgemRedirect)
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuNaoVisualizado.ToString(), null));
            else if (!QuemMeViuVip)
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuSemPlano.ToString(), null));
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuVip.ToString(), null));
        }
        #endregion

        #region btnEscolherEmpresa_Click
        protected void btnEscolherEmpresa_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolherEmpresa.ToString(), null));
        }
        #endregion

        #region btnMeuPlano_Click
        protected void btnMeuPlano_Click(object sender, EventArgs e)
        {
            if (MeuPlanoVirgemRedirect)
            {
                base.UrlDestinoPagamento.Value = GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString());
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVipPlano.ToString(), null));
            }
            else
            {
                base.UrlDestinoPagamento.Value = GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString());
                Session.Add(Chave.Temporaria.Variavel2.ToString(), IdPlanoAdquirido);
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVipPlano.ToString(), null));
            }
        }
        #endregion

        #region btnRSM_Click
        protected void btnRSM_Click(object sender, EventArgs e)
        {
            if (new Curriculo(base.IdCurriculo.Value).VIP())
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.RelatorioSalarialVip.ToString(), null));
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.RelatorioSalarialSemPlano.ToString(), null));
        }
        #endregion

        #region btnAlertaVagas_Click

        protected void btnAlertaVagas_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.AlertaVagas.ToString(), null));
        }

        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            CarregarPesquisarVagas();
            CarregarBoxQuemMeViu();
            CarregarBoxEscolherEmpresa();
            CarregarBoxJaEnviei();
            CarregarBoxMeuPlano();
        }
        #endregion

        #region CarregarPesquisarVagas
        public void CarregarPesquisarVagas()
        {
            lblPesquisaVagas.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxPesquisaVagaSalaVip);
        }
        #endregion

        #region CarregarBoxQuemMeViu
        public void CarregarBoxQuemMeViu()
        {
            var objCurriculo = new Curriculo(base.IdCurriculo.Value);

            QuemMeViuVip = objCurriculo.VIP();

            int quantidadeVisualizacaoCurriculo = CurriculoQuemMeViu.QuantidadeVisualizacaoCurriculo(objCurriculo);
            if (quantidadeVisualizacaoCurriculo > 0)
            {
                lblQuantidadeQuemMeViu.Text = quantidadeVisualizacaoCurriculo.ToString();
                if (quantidadeVisualizacaoCurriculo.Equals(1))
                    lblQuemMeViu.Text = "empresa visualizou o seu currículo";
                else
                    lblQuemMeViu.Text = "empresas visualizaram o seu currículo";
            }
            else
            {
                QuemMeViuVirgemRedirect = true;
                lblQuemMeViu.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxQuemMeViuSalaVip);
            }
        }
        #endregion

        #region CarregarBoxEscolherEmpresa
        public void CarregarBoxEscolherEmpresa()
        {
            int quantidadeEmpresasCadastradas = Filial.QuantidadeEmpresasCadastradas();

            if (quantidadeEmpresasCadastradas > 0)
            {
                lblQuantidadeEmpresasCadastradas.Text = quantidadeEmpresasCadastradas.ToString();
                if (quantidadeEmpresasCadastradas.Equals(1))
                    lblEmpresasCadastradas.Text = "nova empresa cadastrada";
                else
                    lblEmpresasCadastradas.Text = "novas empresas cadastradas";
            }
            else
                lblEmpresasCadastradas.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxEscolherEmpresaSalaVip);
        }
        #endregion

        #region CarregarBoxJaEnviei
        public void CarregarBoxJaEnviei()
        {
            int quantidadeCurriculosEnviadosVaga = VagaCandidato.QuantidadeVagaCandidato(base.IdCurriculo.Value);

            if (quantidadeCurriculosEnviadosVaga > 0)
            {
                //lblJaEnviei1.Visible = true;
                //lblQuantidadeJaEnviei.Text = quantidadeCurriculosEnviadosVaga.ToString(CultureInfo.CurrentCulture);

                //lblJaEnviei2.Text = quantidadeCurriculosEnviadosVaga.Equals(1) ? "vaga" : "vagas";
            }
            else
            {
                //lblJaEnviei2.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxJaEnvieiSalaVip);
                //lblJaEnviei1.Visible = false;

                JaEnvieiVirgemRedirect = true;
            }
        }
        #endregion

        #region CarregarBoxMeuPlano
        public void CarregarBoxMeuPlano()
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                PlanoAdquirido objPlanoAdquirido;

                // Carrega lista de planos liberados ou em aberto do usuario.
                List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(base.IdUsuarioFilialPerfilLogadoCandidato.Value, null);

                // Seleciona se existir o plano liberado vigente ou o plano liberado que sera iniciado.
                objPlanoAdquirido = lstPlanoAdquirido.Where(p => p.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado
                                                             && p.DataFimPlano >= DateTime.Now)
                                                     .OrderBy(p => p.DataInicioPlano).FirstOrDefault();

                if (objPlanoAdquirido == null)
                {
                    MeuPlanoVirgemRedirect = true;
                    var valorConteudo = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxMeuPlanoSalaVip);
                    imgVip.Visible = true;
                    pnlPlanoVIP.Visible = false;
                    litNaoVIP.Visible = true;
                    litNaoVIP.Text = valorConteudo;
                }
                else
                {
                    lblMeuPlano1.Text = "Plano ";
                    objPlanoAdquirido.Plano.CompleteObject();
                    lblPlano.Text = objPlanoAdquirido.Plano.DescricaoPlano;
                    lblMeuPlano2.Text = " Válido até ";
                    lblMeuPlano3.Text = objPlanoAdquirido.DataFimPlano.ToShortDateString();
                    IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                    pnlPlanoVIP.Visible = true;
                    litNaoVIP.Visible = false;
                    litNaoVIP.Text = String.Empty;
                }
            }
        }
        #endregion

        #endregion

    }
}