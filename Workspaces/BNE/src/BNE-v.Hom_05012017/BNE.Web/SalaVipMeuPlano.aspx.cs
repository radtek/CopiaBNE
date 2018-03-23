using System;
using System.Collections.Generic;
using AjaxControlToolkit;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.UserControls.Modais;
using System.Web.UI;


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

        #region PlanoAdquiridoId
        public int PlanoAdquiridoId
        {
            get
            {
                var planoObject = Session[Chave.Temporaria.Variavel1.ToString() + "PlanoAdquiridoId"];
                if (planoObject != null)
                {
                    int planoId;
                    if (Int32.TryParse(planoObject.ToString(), out planoId))
                    {
                        return planoId;
                    }
                }

                return -1;
            }
            set
            {
                Session[Chave.Temporaria.Variavel1.ToString() + "PlanoAdquiridoId"] = value;
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

        #region [lnkCancelarEtapa01_Click]
        protected void lnkCancelarEtapa01_Click(object sender, EventArgs e)
        {
            pnlCancelarPlanoRecorrenteEtapa01.Visible = false;
            pnlCancelarPlanoRecorrenteEtapa02.Visible = true;
        }
        #endregion

        #region [lnkCancelarEtapa02_Click]
        protected void lnkCancelarEtapa02_Click(object sender, EventArgs e)
        {
            pnlCancelarPlanoRecorrenteEtapa02.Visible = false;
            pnlCancelarPlanoRecorrenteEtapa03.Visible = true;
        }
        #endregion

        #region [lnkCancelarEtapa03_Click]
        protected void lnkCancelarEtapa03_Click(object sender, EventArgs e)
        {
            EfetivarCancelamentoAssinatura();
            pnlCancelarPlanoRecorrenteEtapa03.Visible = false;
            pnlCancelarPlanoRecorrenteEtapa04.Visible = true;
        }
        #endregion

        #region [lnkCancelarEtapa04_Click]
        protected void lnkCancelarEtapa04_Click(object sender, EventArgs e)
        {
            Redirect("Default.aspx");
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
                DefineModalParaExibir_WebCallBack();
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
                PlanoAdquiridoId = objPlanoAdquiridoLiberado.IdPlanoAdquirido;
                objPlanoAdquiridoLiberado.Plano.CompleteObject();
                lblPlanoValorTexto.Text = String.Format("R$ {0}", objPlanoAdquiridoLiberado.ValorBase);
                lblDataPlanoAdquiridoValor.Text = objPlanoAdquiridoLiberado.DataInicioPlano.ToShortDateString();
                if (objPlanoAdquiridoLiberado.FlagRecorrente) 
                    lblPlanoValidade.Visible = lblPlanoValidadeValor.Visible = false;
                else
                    lblPlanoValidadeValor.Text = objPlanoAdquiridoLiberado.DataFimPlano.ToShortDateString();
                lblDataVencimentoVip.Text = objPlanoAdquiridoLiberado.DataFimPlano.ToShortDateString();
                lblTipoPlanoValor.Text = objPlanoAdquiridoLiberado.Plano.DescricaoPlano;

                lblDataInicioPlanoAdquiridoValor.Text = "<b>" + objPlanoAdquiridoLiberado.DataInicioPlano.ToShortDateString() + "</b>.";
                lblDataFimPlanoAdquiridoValor.Text = lblPlanoRecorrenteValidade.Text = "<b>" + objPlanoAdquiridoLiberado.DataFimPlano.ToShortDateString() + "</b>.";
                
                if (objPlanoAdquiridoLiberado.FlagRecorrente)
                {
                    btnCancelarAssinatura.Visible = true; // Exibir botao Cancelar Assinatura somente para usuário Master
                }
                else if (objPlanoAdquiridoLiberado.DataFimPlano.AddDays(-5).Date <= DateTime.Now.Date && !objPlanoAdquiridoLiberado.Plano.FlagRecorrente)
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

        protected void btnEfetivarCancelamentoAssinatura_Click(object sender, EventArgs e)
        {
            
            AjustarTituloTela("");
            UIHelper.CarregarCheckBoxList(cblMotivoCancelar, BLL.MotivoCancelamento.ListarVIP());

            using(var dr = BLL.PlanoMotivoCancelamento.Metricas(base.IdCurriculo.Value)){
                if (dr.Read())
                {
                    lblCvVisualizado.Text = lblCvVisualizado02.Text = dr["QtdQuemMeViu"].ToString();
                    lblApareceuNasPesquisas.Text = lblApareceuNasPesquisas02.Text = dr["QtdVezesApareciNabusca"].ToString();
                    lblPesquisaramSeuPerfil.Text = dr["QtdEmpresasPesquisaramnoPerfil"].ToString();
                }
            }

            
            pnlPlano.Visible = false;
            pnlCancelarPlanoRecorrenteEtapa01.Visible = true;
            upPlano.Update();
            upCancelamento.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closeModel", "closeModel();", true);
           
        }

        private void EfetivarCancelamentoAssinatura()
        {
            if (PlanoAdquirido.CancelarAssinaturaPlanoRecorrente(PlanoAdquiridoId, base.IdUsuarioFilialPerfilLogadoCandidato.Value))
            {
                divSucessoCancelamentoAssinatura.Visible = true;
                btnCancelarAssinatura.Visible = false;
            }
            else
                divErroCancelamentoAssinatura.Visible = true;

            for (int i =0; i<  cblMotivoCancelar.Items.Count; i++){
                if (cblMotivoCancelar.Items[i].Selected)
                {
                    PlanoMotivoCancelamento objPlanoMotivoBloqueio = new PlanoMotivoCancelamento();
                    objPlanoMotivoBloqueio.IdPlanoAdquirido = PlanoAdquiridoId;
                    objPlanoMotivoBloqueio.IdMotivoCancelamento = Convert.ToInt32(cblMotivoCancelar.Items[i].Value);
                    objPlanoMotivoBloqueio.DescricaoDetalheMotivoCancelamento = Convert.ToInt32(cblMotivoCancelar.Items[i].Value).Equals((int)Enumeradores.MotivoCancelamento.OutrosVIP) ? txtOutro.Text : null;
                    objPlanoMotivoBloqueio.Save();
                }
            }

            divConteudoCancelarAssinatura.Visible = false;
        }

        protected void DefineModalParaExibir_WebCallBack()
        {
            try
            {
                ucWebCallBack_Modais objWebCallBack_Dependencia = new BNE.Web.UserControls.Modais.ucWebCallBack_Modais();
                var objRetornoStatusAtendimento = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Atendimento));
                if (objRetornoStatusAtendimento != null && objRetornoStatusAtendimento.disponivel > 0)
                {
                    modalWebCallBack.Attributes.Add("data-target", "#myModalComercial,#modalCancelarAssinaturaRecorrente");
                }
                else
                {
                    modalWebCallBack.Attributes.Add("data-target", "#myModalMensagem,#modalCancelarAssinaturaRecorrente");
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        #endregion

    }
}