using System;
using System.Collections.Generic;
using AjaxControlToolkit;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipMeuPlano : BasePagePagamento
    {
        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// 
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #region IdPlano
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected int? IdPlano
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlano.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlano.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlano.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucModalConfirmacaoRetornoPagamento.redirectVoltarSite += ucModalConfirmacaoRetornoPagamento_redirectVoltarSite;
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "SalaVipMeuPlano");
        }
        #endregion

        #region ucModalConfirmacaoRetornoPagamento_redirectVoltarSite
        void ucModalConfirmacaoRetornoPagamento_redirectVoltarSite()
        {
            if (base.UrlDestinoPagamento.HasValue)
            {
                string paginaRedirect = base.UrlDestinoPagamento.Value;
                base.UrlDestinoPagamento.Clear();
                Redirect(paginaRedirect);
            }
            else
                ucModalConfirmacaoRetornoPagamento.FecharModal();
        }
        #endregion

        #region lnkRenovarPlano_Click
        protected void lnkRenovarPlano_Click(object sender, EventArgs e)
        {
            ValidaPlanoAdquiridoLiberado();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (base.UrlDestinoPagamento.HasValue)
            {
                string paginaRedirect = base.UrlDestinoPagamento.Value;
                base.UrlDestinoPagamento.Clear();
                Redirect(paginaRedirect);
            }
            else
            {
                if (!string.IsNullOrEmpty(UrlOrigem) && UrlOrigem.Contains(".aspx"))
                    Redirect(UrlOrigem);
                else
                    Redirect("Default.aspx");
            }
        }
        #endregion

        #region btnTenteNovamente_Click
        protected void btnTenteNovamente_Click(object sender, EventArgs e)
        {
            Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.CandidaturaVagas));
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            try
            {
                AjustarTituloTela("Meu Plano");

                AjustarPermissoes();

                if (Request.UrlReferrer != null)
                    UrlOrigem = Request.UrlReferrer.AbsoluteUri;

                List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

                // Se não houver planos liberados ou aguardando liberação redireciona para a página.
                if (lstPlanoAdquirido == null)
                {
                    Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), null));
                }
                else
                {
                    // Valida se existe plano liberado.
                    PlanoAdquirido objPlanoAdquiridoLiberado;
                    if (VerificaSeTemPlanoAdquiridoLiberadoEmVigencia(lstPlanoAdquirido, out objPlanoAdquiridoLiberado))
                    {
                        // Valida se existe plano adquirido para ser iniciado após a vigencia, se não exibe o botão renovar.
                        PlanoAdquirido objPlanoAdquiridoLiberadoNaoIniciado;
                        if (VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado(lstPlanoAdquirido, out objPlanoAdquiridoLiberadoNaoIniciado))
                            PreencheCampos(objPlanoAdquiridoLiberado, false);
                        else
                            PreencheCampos(objPlanoAdquiridoLiberado, true);
                    }
                    else
                    {
                        // Valida se existe planoAdquirido vencido e exibe o botão renovar.
                        if (VerificaSeTemPlanoAdquiridoLiberadoVencido(lstPlanoAdquirido, out objPlanoAdquiridoLiberado))
                            PreencheCampos(objPlanoAdquiridoLiberado, true);
                        else
                            Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), null));// Se não houver plano então redireciona para a pagina de pagamento.
                    }

                    // Valida o status da transacao referente ao pagamento do tipo cartão.
                    if (StatusTransacaoCartao != null)
                        CarregarRetornoCartao();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region CarregarRetornoCartao
        private void CarregarRetornoCartao()
        {
            if (StatusTransacaoCartao == true)
                //Exibe a modal de confirmação de pagamento
                ((ModalPopupExtender)ucModalConfirmacaoRetornoPagamento.FindControl("mpeModalConfirmacaoRetornoPagamento")).Show();

            StatusTransacaoCartao = null;
        }
        #endregion

        #region ExibirMensagemErro
        public void ExibirMensagemErro()
        {
            //mostrar panel de erro
            upErroTransacaoCartao.Visible = true;
            upMeuPlano.Visible = false;
            upMeuPlano.Update();
            upErroTransacaoCartao.Update();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoCandidato.Value, Enumeradores.CategoriaPermissao.CompraPlanoVIP);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.CompraPlanoVIP.AcessarTelaCompraPlanoVIP))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), "SalaVip.aspx");
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region PreencheCampos
        /// <summary>
        /// Popula campos com as informações referentes ao "PlanoAdquirido" e exibe o link de renovar plano caso necessário.
        /// </summary>
        /// <param name="objPlanoAdquiridoLiberado"></param>
        /// <param name="mostraLinkRenovar"></param>
        private void PreencheCampos(PlanoAdquirido objPlanoAdquiridoLiberado, bool mostraLinkRenovar)
        {
            if (objPlanoAdquiridoLiberado != null)
            {
                objPlanoAdquiridoLiberado.Plano.CompleteObject();
                lblPlanoValorTexto.Text = String.Format("R$ {0}", objPlanoAdquiridoLiberado.Plano.ValorBase);
                lblDataPlanoAdquiridoValor.Text = objPlanoAdquiridoLiberado.DataInicioPlano.ToShortDateString();
                lblPlanoValidadeValor.Text = objPlanoAdquiridoLiberado.DataFimPlano.ToShortDateString();
                lblTipoPlanoValor.Text = objPlanoAdquiridoLiberado.Plano.DescricaoPlano;

                if (objPlanoAdquiridoLiberado.DataFimPlano.AddDays(-5).Date <= DateTime.Now.Date)
                {
                    if (mostraLinkRenovar)
                    {
                        lblSeparador.Visible = true;
                        lnkRenovarPlano.Visible = true;
                    }
                    else
                    {
                        lblSeparador.Visible = true;
                        lblPlanoRenovado.Visible = true;
                    }
                }

                upMeuPlano.Update();
            }
        }

        #endregion

        #region ValidaPlanoAdquiridoLiberado
        /// <summary>
        /// Método que valida se existe plano liberado ainda não iniciado, cria um novo plano adquirido caso não exista e redireciona para a pagina de pagamento.
        /// </summary>
        public void ValidaPlanoAdquiridoLiberado()
        {
            try
            {
                var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(base.IdCurriculo.Value));

                IdPlano = Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria);

                List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
                PlanoAdquirido objPlanoAdquiridoLiberadoNaoIniciado;

                if (VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado(lstPlanoAdquirido, out objPlanoAdquiridoLiberadoNaoIniciado))
                    PreencheCampos(objPlanoAdquiridoLiberadoNaoIniciado, false);
                else
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion
    }
}