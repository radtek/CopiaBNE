using AjaxControlToolkit;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaSelecionadorPlanoIlimitado : BasePagePagamento
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
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

        #region UrlOrigem - Variável 2
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel2.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());
            }
        }

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

        #region StatusTransacaoCartao
        /// <summary>
        /// PropriedadeRetorna o status da transação de pagamento do tipo cartão.
        /// </summary>
        public bool? StatusTransacaoCartao
        {
            get
            {
                if (Session["StatusTrancao"] != null)
                    return Convert.ToBoolean(Session["StatusTrancao"]);
                return null;
            }
            set
            {
                Session["StatusTrancao"] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "SalaSelecionadorPlanoIlimitado");
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

        #region lbRenovarPlano_Click
        protected void lbRenovarPlano_Click(object sender, EventArgs e)
        {
            ValidaPlanoAdquiridoLiberado();

            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
        }
        #endregion

        #region lbComprarSMS_Click
        protected void lbComprarSMS_Click(object sender, EventArgs e)
        {
            Redirect("CIAVendaSMS.aspx");
        }
        #endregion

        #region gvParcelas

        #region gvParcelas_ItemDataBound
        protected void gvParcelas_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var objPagamento = Pagamento.LoadObject(Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]));
                if (objPagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto)
                    ((HyperLink)e.Item.FindControl("btlBoleto")).NavigateUrl = CobrancaBoleto.RetornarBoletoPDF(objPagamento);
            }
        }
        #endregion


        #region gvParcelas_PageIndexChanged
        protected void gvParcelas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvParcelas.CurrentPageIndex = e.NewPageIndex;
            CarregarGridParcelasPlano();
        }
        #endregion

        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            try
            {
                AjustarTituloTela("Meu Plano");
                ExibirMenuSecaoEmpresa();

                AjustarPermissoes();

                if (Request.UrlReferrer != null)
                    UrlOrigem = Request.UrlReferrer.AbsoluteUri;
                List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(null, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null);

                // Se não houver planos liberados ou aguardando liberação redireciona para a página.
                if (lstPlanoAdquirido == null)
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
                else
                {
                    // Valida se existe plano liberado.
                    PlanoAdquirido objPlanoAdquiridoLiberado;
                    if (VerificaSeTemPlanoAdquiridoLiberadoEmVigencia(lstPlanoAdquirido, out objPlanoAdquiridoLiberado))
                    {
                        // Valida se existe plano adquirido para ser iniciado após a vigencia, se não exibe o botão renovar.
                        PlanoAdquirido objPlanoAdquiridoLiberadoNaoIniciado;
                        if (VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado(lstPlanoAdquirido, out objPlanoAdquiridoLiberadoNaoIniciado))
                            PreencherCampos(objPlanoAdquiridoLiberado, false);
                        else
                            PreencherCampos(objPlanoAdquiridoLiberado, true);
                    }
                    else
                    {
                        // Valida se existe planoAdquirido vencido e exibe o botão renovar.
                        if (VerificaSeTemPlanoAdquiridoLiberadoVencido(lstPlanoAdquirido, out objPlanoAdquiridoLiberado))
                            PreencherCampos(objPlanoAdquiridoLiberado, true);
                        else
                            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null)); // Se não houver plano então redireciona para a pagina de pagamento.
                    }

                    // Valida o status da transacao referente ao pagamento do tipo cartão.
                    if (base.StatusTransacaoCartao != null)
                        CarregarRetornoCartao();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region PreencheCampos
        /// <summary>
        /// Popula campos com as informações referentes ao "PlanoAdquirido" e exibe o link de renovar plano caso necessário.
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <param name="mostraLinkRenovar"></param>
        private void PreencherCampos(PlanoAdquirido objPlanoAdquirido, bool mostraLinkRenovar)
        {
            if (objPlanoAdquirido == null)
            {
                PlanoAdquiridoId = -1;
                return;
            }

            objPlanoAdquirido.CompleteObject();

            PlanoAdquiridoId = objPlanoAdquirido.IdPlanoAdquirido;
            objPlanoAdquirido.Plano.CompleteObject();

            lblPlanoValorTexto.Text = String.Format("R$ {0}", objPlanoAdquirido.ValorBase);
            lblDataPlanoAdquiridoValor.Text = objPlanoAdquirido.DataInicioPlano.ToShortDateString();
            lblPlanoValidadeValor.Text = objPlanoAdquirido.DataFimPlano.ToShortDateString();
            lblTipoPlanoValor.Text = objPlanoAdquirido.Plano.DescricaoPlano;

            PlanoQuantidade objPlanoQuantidade;
            if (PlanoQuantidade.CarregarPlanoAtualVigente(base.IdFilial.Value, out objPlanoQuantidade))
            {
                lblQuantidadeSMSDisponivelValor.Text = Convert.ToString(objPlanoQuantidade.QuantidadeSMS - objPlanoQuantidade.QuantidadeSMSUtilizado);
                litQuantidadeVisualizacaoDisponivelValor.Text = Convert.ToString(objPlanoQuantidade.QuantidadeVisualizacao - objPlanoQuantidade.QuantidadeVisualizacaoUtilizado);
            }
            else
            {
                lblQuantidadeSMSDisponivelValor.Text = Decimal.Zero.ToString(CultureInfo.CurrentCulture);
                litQuantidadeVisualizacaoDisponivelValor.Text = Decimal.Zero.ToString(CultureInfo.CurrentCulture);
            }

            lblCurriculosVisualizadosValor.Text = CurriculoVisualizacao.ListarQuantidadeCurriculoVisualizados(base.IdFilial.Value).ToString(CultureInfo.CurrentCulture);

            if (objPlanoAdquirido.DataFimPlano.AddDays(-5).Date <= DateTime.Now.Date)
            {
                if (mostraLinkRenovar)
                {
                    lblSeparador.Visible = true;
                    lbRenovarPlano.Visible = true;
                }
                else
                {
                    lblSeparador.Visible = true;
                    lblPlanoRenovado.Visible = true;
                }
            }

            pnlParcelas.Visible = true;
            CarregarGridParcelasPlano();

            upPlanoIlimitado.Update();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, Enumeradores.CategoriaPermissao.TelaSalaSelecionadorPlanoIlimitado);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.TelaSalaSelecionadorPlanoIlimitado.AcessarTelaSalaSelecionadorPlanoIlimitado))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
        }
        #endregion

        #region CarregarRetornoCartao
        private void CarregarRetornoCartao()
        {
            if (StatusTransacaoCartao != null && StatusTransacaoCartao == true)
                //Exibe a modal de confirmação de pagamento
                ((ModalPopupExtender)ucModalConfirmacaoRetornoPagamento.FindControl("mpeModalConfirmacaoRetornoPagamento")).Show();

            StatusTransacaoCartao = null;
        }
        #endregion

        #region ValidacaoPlanoAdquirido
        /// <summary>
        /// Método que valida se existe plano liberado ainda não iniciado, cria um novo plano adquirido caso não exista e redireciona para a pagina de pagamento.
        /// </summary>
        public void ValidaPlanoAdquiridoLiberado()
        {
            try
            {
                List<PlanoAdquirido> lstPlanoAdquirido = PlanoAdquirido.CarregaListaPlanoAdquiridoLiberadoOuEmAberto(null, base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null);
                PlanoAdquirido objPlanoAdquiridoLiberadoNaoIniciado;

                if (VerificaSeTemPlanoAdquiridoLiberadoAindaNaoIniciado(lstPlanoAdquirido, out objPlanoAdquiridoLiberadoNaoIniciado))
                {
                    PreencherCampos(objPlanoAdquiridoLiberadoNaoIniciado, false);
                }
                else
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region CarregarGridParcelasPlano
        private void CarregarGridParcelasPlano()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvParcelas, PlanoParcela.ListaParcelasPorPlanoAdquirido(PlanoAdquiridoId, gvParcelas.CurrentPageIndex, gvParcelas.PageSize, out totalRegistros), totalRegistros);
            upParcelas.Update();
        }
        #endregion

        #region AjustarVisibilidadeVisualizarBoleto
        /// <summary>
        /// Metodo resposavel por ajustar a visibilidade do botão visualzar boleto
        /// </summary>
        /// <param name="idTipoPagamento"></param>
        /// <param name="idPagamentoSituacao"></param>
        /// <param name="cortesia"></param>
        /// <returns></returns>
        public bool AjustarVisibilidadeVisualizarBoleto(int idPagamentoSituacao, bool cortesia, int idTipoPagamento)
        {
            if (cortesia)
                return false;

            if (idTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario && idPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto)
                return true;

            return false;
        }
        #endregion

        #region AjustarVisibilidadeNotaFiscal
        /// <summary>
        /// Metodo resposavel por ajustar a visibilidade da visualização da nota
        /// </summary>
        /// <param name="idPagamentoSituacao"></param>
        /// <param name="cortesia"></param>
        /// <returns>visible</returns>
        public bool AjustarVisibilidadeNotaFiscal(int idPagamentoSituacao, bool cortesia, bool notaAntecipada)
        {
            if (cortesia)
                return false;

            if (idPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.Pago || notaAntecipada)
                return true;

            return false;
        }
        #endregion

        #endregion

        #region btnTenteNovamente_Click
        protected void btnTenteNovamente_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
        }
        #endregion

        protected void lbVerContrato_OnClick(object sender, EventArgs e)
        {
            AjustarPermissoes();

            PlanoAdquirido planoAd;
            if (!ValidacaoVisualizacaoDeContrato(out planoAd))
                return;

            string razaoSocial;
            string numCNPJ;
            string descRua;
            string numeroRua;
            string nomeCidade;
            string estado;
            string numeroCEP;
            Filial.RecuperarConteudoFilialParaContratoPorFilial(planoAd.Filial.IdFilial, out razaoSocial, out numCNPJ, out descRua, out numeroRua, out nomeCidade, out estado, out numeroCEP);

            string nomePessoa;
            string numRG;
            decimal numCPF;
            string email;
            PessoaFisica.CarregarPorIdUsuarioFilialPerfil(planoAd.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out nomePessoa, out numRG, out numCPF, out email);

            var quantidadeUsuarios = planoAd.Filial.RecuperarQuantidadeAcessosAdquiridos();

            var parcelas = PlanoParcela.ListaParcelasPorPlanoAdquirido(planoAd, null);
            int sms = planoAd.QuantidadeSMS;

            int contratoParcelas;
            if (parcelas.Count > 0)
            {
                contratoParcelas =
                    parcelas
                        .OrderBy(a => a.IdPlanoParcela)
                        .Count(a => Helper.EstaNoAlcance(new TimeSpan(a.DataCadastro.Ticks), TimeSpan.FromMinutes(2), new TimeSpan(parcelas[0].DataCadastro.Ticks)));

                sms = sms / contratoParcelas;
            }
            else
            {
                contratoParcelas = 0;
            }

            var tempoPlano = Convert.ToInt32(Math.Ceiling(((double)planoAd.DataFimPlano.Subtract(planoAd.DataInicioPlano).Days) / (365.25 / 12)));

            var pdf = GerarContratoPlano.ContratoPadraoPdf(razaoSocial, numCNPJ, descRua, numeroRua, estado, nomeCidade, numeroCEP, nomePessoa, numRG, numCPF, planoAd.ValorBase, contratoParcelas, tempoPlano, sms, quantidadeUsuarios);

            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato_BNE.pdf");
            //Response.OutputStream.Write(pdf, 0, pdf.Length);
            Response.BinaryWrite(pdf);
            Response.End();
        }

        private bool ValidacaoVisualizacaoDeContrato(out PlanoAdquirido planoAd)
        {
            var planoId = PlanoAdquiridoId;
            if (planoId <= 0)
            {
                ExibirMensagem("Plano adquirido desconhecido ou inválido, contate o administrador.", TipoMensagem.Erro);
                planoAd = null;
                return false;
            }

            planoAd = PlanoAdquirido.LoadObject(planoId);
            if (planoAd.Filial.IdFilial <= 0)
            {
                ExibirMensagem("Filial inválida, contate o adminstrador.", TipoMensagem.Erro);
                return false;
            }

            if (planoAd.UsuarioFilialPerfil.IdUsuarioFilialPerfil <= 0)
            {
                ExibirMensagem("Usuário inválido, contate o adminstrador.", TipoMensagem.Erro);
                return false;
            }
            return true;
        }



    }
}