using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.UI;

namespace BNE.Web
{
    public partial class SalaSelecionador : BasePage
    {

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Inicializar();
                ValidarAberturaBanner();
            }

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SalaSelecionadora");
        }
        #endregion

        #region btlTeste_Click
        /// <summary>
        /// Evento do botão Minhas Vagas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btlTeste_Click(object sender, EventArgs e)
        {
            ExibirMensagem("Sua empresa está bloqueada, para mais informações ligue 0800 41 2400", TipoMensagem.Aviso);
        }
        #endregion

        #region btlMinhasVagas_Click
        /// <summary>
        /// Evento do botão Minhas Vagas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btlMinhasVagas_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.VagasAnunciadas.ToString(), null));
        }
        #endregion

        #region btlRastreadorCV_Click
        protected void btlRastreadorCV_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect("SalaSelecionadorRastreadorCV.aspx");
        }
        #endregion

        #region btlMensagensEnviadas_Click
        protected void btlMensagensEnviadas_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionadorMensagens.ToString(), null));
        }
        #endregion

        #region btlCampanhaRecrutamento_Click
        protected void btlCampanhaRecrutamento_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CampanhaRecrutamento.ToString(), null));
        }
        #endregion

        #region btlCompraPlanos_Click
        protected void btlCompraPlanos_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaEmAuditoria(objFilial) && !EmpresaBloqueada(objFilial))
            {
                base.UrlDestino.Value = "SalaSelecionador.aspx";
                base.UrlDestinoPagamento.Value = "SalaSelecionador.aspx";
                base.PagamentoUrlRetorno.Value = "SalaSelecionador.aspx";
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIAPlano.ToString(), null));
            }
        }
        #endregion

        #region btlPesquisaAvancada_OnClick
        protected void btlPesquisaAvancada_OnClick(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
        }
        #endregion

        #region btlRastreadorCurriculos_Click
        protected void btlRastreadorCurriculos_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.AlertaCurriculos.ToString(), null));
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            AjustarPermissoes();

            AjustarTituloTela("Sala da Selecionadora");
            ExibirMenuSecaoEmpresa();

            //É preciso ajustar o topo caso o usuario tenha o perfil empresa e candidato
            PropriedadeAjustarTopoUsuarioEmpresa(true);

            CarregarBoxVagas();
            CarregarBoxMeuPlano();
            CarregarBoxMensagens();
            CarregarBoxCampanha();

            upMenuNavegacao.Update();
        }
        #endregion

        #region ValidarAberturaBanner()
        private void ValidarAberturaBanner()
        {
            try
            {
                //Abertura de Banner, condicional ao retorno da function bne.sf_alerta_login
                //Abre apenas uma vez ao realizar login.
                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    //Verifica se já foi aberta
                    bool validouModal = Session["ValidouModalBannerSalaSelecionadora"] == null ? false : true;

                    if (validouModal) //Caso já tenha validado a abertura uma vez, não valida novamente
                        return;

                    Session["ValidouModalBannerSalaSelecionadora"] = true;
                    if (UsuarioFilialPerfil.ValidaExibicaoBannerSalaSelecionadora(base.IdUsuarioFilialPerfilLogadoEmpresa.Value))
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "AbrirBanner(true);", true);
                    else
                        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "mensagem", "AbrirBanner(false);", true);
                }
            }
            catch
            { }
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o usuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                var permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!permissoes.Contains((int)BLL.Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.SalaSelecionador }));
        }
        #endregion

        #region CarregarBoxVagas
        public void CarregarBoxVagas()
        {
            #region Vagas

            int quantidadeVagasFilial = Filial.RecuperarQuantidadeVagasAnunciadas(base.IdFilial.Value, null);

            if (quantidadeVagasFilial > 0)
            {
                lblVagasAnunciadasQuantidade.Text = quantidadeVagasFilial.ToString(CultureInfo.CurrentCulture);

                if (quantidadeVagasFilial.Equals(1))
                    lblVagasAnunciadasMensagem.Text = " Vaga Anunciada";
                else
                    lblVagasAnunciadasMensagem.Text = " Vagas Anunciadas";
            }
            else
                lblVagasAnunciadasMensagem.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaBoxVagaSalaSelecionadorInicial);

            #endregion

            #region Curriculos

            int quantidadeCurriculosCandidatados = VagaCandidato.CandidatoVagaPorFilial(base.IdFilial.Value);

            if (quantidadeCurriculosCandidatados > 0)
            {
                lblCurriculosRecebidosQuantidade.Text = quantidadeCurriculosCandidatados.ToString();

                if (quantidadeCurriculosCandidatados.Equals(1))
                    lblCurriculosRecebidosMensagem.Text = " CV Recebido";
                else
                    lblCurriculosRecebidosMensagem.Text = " novos CV's Recebidos";

            }

            #endregion
        }
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

        #region CarregarBoxMensagens
        public void CarregarBoxMensagens()
        {
            int quantidadeMensagensEnviadas = MensagemCS.QuantidadeMensagensEnviadas(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            lblMensagemPossuiMensagem.Text = "Você possui ";

            lblQuantidadeMensagens.Text = quantidadeMensagensEnviadas.ToString(CultureInfo.CurrentCulture);

            if (quantidadeMensagensEnviadas.Equals(1))
                lblTextoMensagem.Text = "mensagem enviada";
            else
                lblTextoMensagem.Text = "mensagens enviadas";
        }
        #endregion

        #region CarregarBoxCampanha
        public void CarregarBoxCampanha()
        {
            var saldo = new Filial(base.IdFilial.Value).SaldoCampanha();
            litSaldoCampanha.Text = string.Format("Você tem {0} {1}", saldo, saldo > 0 ? "disponível" : "disponíveis");
        }
        #endregion

        #endregion

    }
}
