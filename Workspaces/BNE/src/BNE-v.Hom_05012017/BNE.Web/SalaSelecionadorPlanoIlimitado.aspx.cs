using AjaxControlToolkit;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.EL;
using BNE.Web.UserControls.Modais;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.Routing;
using BNE.Componentes.Extensions;

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

        #region PlanoAdquiridoLiberacaoAutomaticaId
        public int PlanoAdquiridoLiberacaoAutomaticaId
        {
            get
            {
                var planoObject = Session[Chave.Temporaria.Variavel3.ToString() + "PlanoAdquiridoLiberacaoAutomaticaId"];
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
                Session[Chave.Temporaria.Variavel1.ToString() + "PlanoAdquiridoLiberacaoAutomaticaId"] = value;
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

        #region btnEnviarPorEmail_Click
        protected void btnEnviarPorEmail_Click(object sender, EventArgs e)
        {
            try
            {
                ucEnvioEmail.MostrarModal(EnvioEmail.TipoEnvioEmail.ExtratoPlano);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }

        protected void btnEnviarPorEmailLiberacaoAutomatica_Click(object sender, EventArgs e)
        {
            try
            {
                ucEnvioEmailLiberacaoAutomatica.MostrarModal(EnvioEmail.TipoEnvioEmail.ExtratoPlano);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
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

        protected void btnEfetivarCancelamentoAssinatura_Click(object sender, EventArgs e)
        {
            pnlPlano.Visible = false;
            pnlCancelamento.Visible = true;
            int totCvNVisualizados = CurriculoVisualizacao.QuantidadeCurriculosNaoVisuazados((int)IdFilial.Value);

            int? idPesquisa = null;
            if (totCvNVisualizados > 0)
            {
                lblCurriculosNaoVisualizados.Text = totCvNVisualizados.ToString();
            }
            else
            {//não tem vaga anunciada ativa ou não tem cvs não visualizados em suas vagas.
                totCvNVisualizados = BLL.PesquisaCurriculo.QuantidadeCurriculosNaoVisualizados(base.IdFilial.Value, out idPesquisa);
                pnlInscritosNaoVisualizados.Visible = false;
                pnlCurriculosPesquisa.Visible = true;
                hfIDPesquisaCurriculo.Value = idPesquisa.Value.ToString();
                lblQtdCvNaoVisualizadosDePesq.Text = totCvNVisualizados.ToString();
            }
            int Total;
            var repeaterSource = CurriculoVisualizacao.ListaCandidatosNaoVisualizados((int)base.IdFilial.Value, idPesquisa,out Total);
         
            UIHelper.CarregarRepeater(rpCvsNaoVisualizados, repeaterSource);
            hfTotalCv.Value = repeaterSource.Rows.Count.ToString();
            if (repeaterSource.Rows.Count > 0)
            {
                lblCidadePesquisa.Text = repeaterSource.Rows[0]["Cidade"].ToString();
                lblFuncaoPesquisa.Text = repeaterSource.Rows[0]["Funcao"].ToString();
            }

            //não tem vaga anunciada ou não tem cv não visto nos seus candidatos. mostar dados da ultima pesquisa.

            //Não possuir sms disponivel mostrar quantidade de pesquisas feitas.
            if (Convert.ToInt32(lblQtdSms.Text) <= 0) {
                pnlTemSms.Visible = false;
                pnlSemSMS.Visible = true;
                lblqtdPesquisa.Text = BLL.PesquisaCurriculo.QuantidadePesquisaRealizadas(base.IdFilial.Value).ToString();
             }

            
            if (Total > 0)//Curriculos da ultima vaga
                lblQtdCurriculosPesquisa.Text = Total.ToString();
            else//Curriculos da ultima pesquisa
                lblQtdCurriculosPesquisa.Text = totCvNVisualizados.ToString();

            lblNomeResponsavel.Text = UsuarioFilialPerfil.RecuperarNomeUsuario((int)base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
           
            upPlano.Update();
            upCancelamento.Update();
            
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closeModel", "closeModel();", true);
        }

        #region gvParcelas

        #region gvParcelas_ItemDataBound
        protected void gvParcelas_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                try
                {
                    var objPagamento = Pagamento.LoadObject(Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]));
                    if (objPagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        ((HyperLink)e.Item.FindControl("btlBoleto")).NavigateUrl = "/Boleto.aspx?Id=" + objPagamento.IdPagamento;
                }
                catch (Exception ex)
                {
                    BNE.EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region gvParcelas_PageIndexChanged
        protected void gvParcelas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvParcelas.CurrentPageIndex = e.NewPageIndex;
            CarregarGridParcelasPlano(gvParcelas, upParcelas, PlanoAdquiridoId);
        }
        #endregion

        #region gvParcelas_ItemDataBound
        protected void gvParcelasLiberacaoAutomatica_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                try
                {
                    var objPagamento = Pagamento.LoadObject(Convert.ToInt32(gvParcelasLiberacaoAutomatica.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]));
                    if (objPagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        ((HyperLink)e.Item.FindControl("btlBoletoLiberacaoAutomatica")).NavigateUrl = "/Boleto.aspx?Id=" + objPagamento.IdPagamento;
                }
                catch (Exception ex)
                {
                    BNE.EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region gvParcelas_PageIndexChanged
        protected void gvParcelasLiberacaoAutomatica_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvParcelasLiberacaoAutomatica.CurrentPageIndex = e.NewPageIndex;
            CarregarGridParcelasPlano(gvParcelasLiberacaoAutomatica, upParcelasLiberacaoAutomatica, PlanoAdquiridoLiberacaoAutomaticaId);
        }

        #endregion

        #endregion

        #region gvAdicional

        #region gvAdicional_ItemDataBound
        protected void gvAdicional_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                try
                {
                    var objPagamento = Pagamento.LoadObject(Convert.ToInt32(gvAdicional.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]));
                    if (objPagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        ((HyperLink)e.Item.FindControl("btlBoleto")).NavigateUrl = "/Boleto.aspx?Id=" + objPagamento.IdPagamento;
                }
                catch (Exception ex)
                {
                    BNE.EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region gvAdicional_PageIndexChanged
        protected void gvAdicional_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvAdicional.CurrentPageIndex = e.NewPageIndex;
            CarregarGridAdicionalPlano(gvAdicional, upAdicional, PlanoAdquirido.LoadObject(PlanoAdquiridoId));
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
                DefineModalParaExibir_WebCallBack();

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

            PlanoQuantidade objPlanoQuantidade;
            PlanoQuantidade.CarregarPorPlanoAdquirido(objPlanoAdquirido, out objPlanoQuantidade);

            string nomePlano = objPlanoAdquirido.Plano.DescricaoPlano;
            string valorPlano = String.Format("R$ {0}", objPlanoAdquirido.ValorBase);
            string dataInicio = objPlanoAdquirido.DataInicioPlano.ToShortDateString();
            string dataFim = objPlanoAdquirido.DataFimPlano.ToShortDateString();
            string cotaSms = objPlanoAdquirido.Plano.FlagLiberaUsuariosTanque ? CelularSelecionador.RecuperarCotaDisponivelEmpresa(base.IdFilial.Value).ToString() : objPlanoAdquirido.Filial.SaldoSMS().ToString();
            string cotaVisualizacao = objPlanoAdquirido.Plano.FlagIlimitado ? "Ilimitado" : objPlanoAdquirido.Filial.SaldoVisualizacao().ToString();
            string curriculosVisualizados = objPlanoQuantidade.QuantidadeVisualizacaoUtilizado.ToString(CultureInfo.CurrentCulture);
            string cotaCampanha = objPlanoQuantidade.SaldoCampanha().ToString(CultureInfo.CurrentCulture);
            string smsEnviados = objPlanoQuantidade.QuantidadeSMSUtilizado.ToString(CultureInfo.CurrentCulture);
            string vagasAnunciadas = Vaga.RecuperarQtdVagasPorFilialEPlanoAdquirido(base.IdFilial.Value, PlanoAdquiridoId).ToString(CultureInfo.CurrentCulture);

            lblPlanoValorTexto.Text = valorPlano;
            lblDataPlanoAdquiridoValor.Text = dataInicio;
            lblPlanoValidadeValor.Text = dataFim;
            lblTipoPlanoValor.Text = nomePlano;
            lblQtdSms.Text = lblQuantidadeSMSDisponivelValor.Text = cotaSms;
          
            litQuantidadeVisualizacaoDisponivelValor.Text = cotaVisualizacao;
            lblCurriculosVisualizadosValor.Text = curriculosVisualizados;
            lblQuantidadeCampanhaDisponivel.Text = cotaCampanha;

            //Campos da modalCancelarAssinaturaRecorrente 
            lblDataInicioPlanoAdquiridoValor.Text = "<b>" + dataInicio + "</b>.";
            lblDataFimPlanoAdquiridoValor.Text = lblPlanoRecorrenteValidade.Text = "<b>" + dataFim + "</b>.";
            lblQtdCurriculosVisualizadosValor.Text = "<b>" + curriculosVisualizados + "</b> currículos.";
            lblQtdEnvioMensagensValor.Text = "<b>" + smsEnviados + "</b> mensagens.";
            lblQtdAnuncioVagasValor.Text = "<b>" + vagasAnunciadas + "</b> vagas.";

            if (objPlanoAdquirido.FlagRecorrente)
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                if (objUsuarioFilialPerfil.Perfil.IdPerfil == (int)Enumeradores.Perfil.AcessoEmpresaMaster)
                    btnCancelarAssinatura.Visible = true; // Exibir botao Cancelar Assinatura somente para usuário Master
                else
                    btnCancelarAssinatura.Visible = false;
            }
            else if (objPlanoAdquirido.DataFimPlano.AddDays(-5).Date <= DateTime.Now.Date && !objPlanoAdquirido.Plano.FlagRecorrente)
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


            //Plano Normal
            ExtratoParcelasPorEmail(CarregarGridParcelasPlano(gvParcelas, upParcelas, objPlanoAdquirido.IdPlanoAdquirido), ucEnvioEmail, nomePlano, valorPlano, dataInicio, dataFim, cotaSms, cotaVisualizacao, curriculosVisualizados);
            CarregarGridAdicionalPlano(gvAdicional, upAdicional, objPlanoAdquirido);
            //Plano Futuro
            PlanoAdquirido objPlanoAdquiridoFuturo;
            objPlanoAdquirido.Filial.CompleteObject();
            if (PlanoAdquirido.CarregaUltimoPlanoFilialPorLiberacaoFuturaOuAutomatica(objPlanoAdquirido.Filial, out objPlanoAdquiridoFuturo))
            {
                PlanoAdquiridoLiberacaoAutomaticaId = objPlanoAdquiridoFuturo.IdPlanoAdquirido;
                ExtratoParcelasPorEmail(CarregarGridParcelasPlano(gvParcelasLiberacaoAutomatica, upParcelasLiberacaoAutomatica, objPlanoAdquiridoFuturo.IdPlanoAdquirido), ucEnvioEmailLiberacaoAutomatica, nomePlano, valorPlano, dataInicio, dataFim, cotaSms, cotaVisualizacao, curriculosVisualizados);
                pnlParcelasLiberacaoAutomatica.Visible = true;
                pnlBotoesLiberacaoAutomatica.Visible = true;
            }

            lblVerContratoClique.Visible = lbVerContrato.Visible = objPlanoAdquirido.Plano.FlagEnviarContrato;

            upPlanoIlimitado.Update();
        }

        #endregion ExtratoParcelasPorEmail

        private void ExtratoParcelasPorEmail(DataTable dataTable, EnvioEmail ucEnvioEmail, PlanoAdquirido objPlanoAdquirido)
        {
            string nomePlano = objPlanoAdquirido.Plano.DescricaoPlano;
            string valorPlano = String.Format("R$ {0}", objPlanoAdquirido.ValorBase);
            string dataInicio = objPlanoAdquirido.DataInicioPlano.ToShortDateString();
            string dataFim = objPlanoAdquirido.DataFimPlano.ToShortDateString();
            string cotaSms = objPlanoAdquirido.Plano.FlagLiberaUsuariosTanque ? CelularSelecionador.RecuperarCotaDisponivelEmpresa(base.IdFilial.Value).ToString() : objPlanoAdquirido.Filial.SaldoSMS().ToString();
            string cotaVisualizacao = objPlanoAdquirido.Plano.FlagIlimitado ? "Ilimitado" : objPlanoAdquirido.Filial.SaldoVisualizacao().ToString();
            string curriculosVisualizados = CurriculoVisualizacao.ListarQuantidadeCurriculoVisualizados(base.IdFilial.Value).ToString(CultureInfo.CurrentCulture);
            ExtratoParcelasPorEmail(dataTable, ucEnvioEmail, nomePlano, valorPlano, dataInicio, dataFim, cotaSms, cotaVisualizacao, curriculosVisualizados);
        }

        private void ExtratoParcelasPorEmail(DataTable dataTable, EnvioEmail ucEnvioEmail, string nomePlano, string valorPlano, string dataInicio, string dataFim, string cotaSms, string cotaVisualizacao, string curriculosVisualizados)
        {
            string parcelas = string.Empty;
            foreach (DataRow dr in dataTable.Rows)
            {
                var dataPagamento = dr["Dta_Pagamento"].ToString();

                parcelas += string.Format(@"<tr><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{0}</font></td><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{1}</font></td><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{2}</font></td><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{3}</font></td><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{4}</font></td><td align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>{5}</font></td></tr>", dr["Parcela"], Convert.ToDateTime(dr["Dta_Vencimento"]).ToShortDateString(), !string.IsNullOrWhiteSpace(dataPagamento) ? Convert.ToDateTime(dataPagamento).ToShortDateString() : string.Empty, dr["Vlr_Pagamento"], dr["Des_Pagamento_Situacao"], dr["Des_Tipo_Pagamaneto"]);
            }
            parcelas = string.Format("<tr><td colspan='2'><table align='center' cellpadding='0' cellspacing='0' width='540'><tr><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Parcela</font></th><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Data de Vencimento</font></th><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Data de Pagamento</font></th><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Valor</font></th><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Situação</font></th><th align='center'><font color='#333333' face='Arial, Helvetica, sans-serif' size='2'>Forma Pagamento</font></th></tr> {0} </table></td></tr>", parcelas);

            var parametros = new
            {
                Parcelas = parcelas,
                NomePlano = nomePlano,
                DataPlano = dataInicio,
                DataPlanoValidade = dataFim,
                ValorParcela = valorPlano,
                CurriculosVisualizados = curriculosVisualizados,
                Visualizacoes = cotaVisualizacao,
                SMS = cotaSms
            };

            string templateAssunto;

            var templateEmail = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ExtratoPlano, out templateAssunto);

            ucEnvioEmail.TemplateEmail = parametros.ToString(templateEmail);
            ucEnvioEmail.TemplateAssunto = parametros.ToString(templateAssunto);
        }

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
        private DataTable CarregarGridParcelasPlano(RadGrid gv, System.Web.UI.UpdatePanel up, int idPlanoAd)
        {
            int totalRegistros;
            DataTable dataTable = PlanoParcela.ListaParcelasPorPlanoAdquirido(idPlanoAd, gv.CurrentPageIndex, gv.PageSize, out totalRegistros);
            UIHelper.CarregarRadGrid(gv, dataTable, totalRegistros);
            up.Update();

            return dataTable;
        }
        #endregion

        #region CarregarGridAdicionalPlano
        private void CarregarGridAdicionalPlano(RadGrid gv, UpdatePanel up, PlanoAdquirido objPlanoAdquirido)
        {
            int totalRegistros;
            DataTable dataTable = AdicionalPlano.ListarAdicionaisPorPlanoAdquirido(objPlanoAdquirido, gv.CurrentPageIndex, gv.PageSize, out totalRegistros);
            UIHelper.CarregarRadGrid(gv, dataTable, totalRegistros);
            if (totalRegistros > 0)
                pnlAdicional.Visible = true;
            up.Update();
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
        //public bool AjustarVisibilidadeNotaFiscal(int idPagamentoSituacao, bool cortesia)
        //{
        //    if (cortesia)
        //        return false;

        //    if (idPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.Pago)
        //        return true;

        //    return false;
        //}
        #endregion

        #region AjustarVisibilidadeNotaFiscal
        /// <summary>
        /// Metodo resposavel por ajustar a visibilidade da visualização da nota
        /// </summary>
        /// <param name="idPagamentoSituacao"></param>
        /// <param name="cortesia"></param>
        /// <returns>visible</returns>
        public bool AjustarVisibilidadeNotaFiscal(string UrlNotaFiscal)
        {
            if (!string.IsNullOrEmpty(UrlNotaFiscal))
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

        #region [lbVerContrato_OnClick]
        protected void lbVerContrato_OnClick(object sender, EventArgs e)
        {
            AjustarPermissoes();

            PlanoAdquirido planoAd;
            if (!ValidacaoVisualizacaoDeContrato(out planoAd))
                return;

            planoAd.Plano.CompleteObject();
            if (planoAd.Plano.FlagRecorrente)
            {
                var contrato = planoAd.ContratoPlanoRecorrenteCia(planoAd);
                var pdf = GerarContratoPlano.ContratoPadraoPdf(contrato);

                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato_BNE.pdf");
                Response.BinaryWrite(pdf);
                Response.End();
            }
            else
            {
                var contrato = planoAd.Contrato();
                var pdf = GerarContratoPlano.ContratoPadraoPdf(contrato);

                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=Contrato_BNE.pdf");
                Response.BinaryWrite(pdf);
                Response.End();
            }
        }
        #endregion

        #region [ValidacaoVisualizacaoDeContrato]
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
        #endregion

        #region [DefineModalParaExibir_WebCallBack]
        protected void DefineModalParaExibir_WebCallBack()
        {
            try
            {
                ucWebCallBack_Modais objWebCallBack_Dependencia = new BNE.Web.UserControls.Modais.ucWebCallBack_Modais(); //new Modais.ucWebCallBack_Dependencia();
                var objRetornoStatusCIA = objWebCallBack_Dependencia.RetornarStatus(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PilotoDeFila_Cia));

                if (objRetornoStatusCIA != null && objRetornoStatusCIA.disponivel > 0)
                {
                    modalWebCallBack.Attributes.Add("data-target", "#myModalComercial,#modalCancelarAssinaturaRecorrente");
                }
                else
                {
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
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region [btnFinalizarCancelamento_Click]
        protected void btnFinalizarCancelamento_Click(object sender, EventArgs e)
        {
            try
            {
                List<PlanoMotivoCancelamento> lista = new List<PlanoMotivoCancelamento>();

                #region [Motivos de Cancelamento]

                #region [rbCandidatoBNE]
                if (rbCandidatoBNE.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.JaFinalizeiMeuProcessoSeletivo_CandidatoBNE,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);

                }
                #endregion

                #region [rbIndicacao]
                if (rbIndicacao.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.JaFinalizeiMeuProcessoSeletivo_Indicacao,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [outro_site]
                if (outro_site.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.JaFinalizeiMeuProcessoSeletivo_Outro,
                        DescricaoDetalheMotivoCancelamento = qual.Value,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [ckNaoConsegui]
                if (ckNaoConsegui.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.NaoConseguiUtilizarOSite,
                        DescricaoDetalheMotivoCancelamento = txtMotivoNaoConseguir.Value,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [ckNaoObtiveResultados]
                if (ckNaoObtiveResultados.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.NaoObtiveResultadosComAnuncio,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [ckNaoConseguiContato]
                if (ckNaoConseguiContato.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.NaoConseguiContatoComOsCandidatos,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [ValorAlto]
                if (motivo2.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.ValorDaAssinaturaMuitoAlto,
                        DescricaoDetalheMotivoCancelamento = txtValorAssinatura.Value,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [VouTestarOutrasFerramentas]
                if (motivo3.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.VouTestarOutrasFerramentas,
                        DescricaoDetalheMotivoCancelamento = txtFerramentasTestar.Value,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #region [Outros]
                if (motivo4.Checked)
                {
                    PlanoMotivoCancelamento objPlanoMotivoCancelamento = new PlanoMotivoCancelamento
                    {
                        IdMotivoCancelamento = (int)Enumeradores.MotivoCancelamento.OutrosCIA,
                        DescricaoDetalheMotivoCancelamento = txtOutros.Value,
                        IdPlanoAdquirido = PlanoAdquiridoId,
                    };
                    lista.Add(objPlanoMotivoCancelamento);
                }
                #endregion

                #endregion


               // if (PlanoAdquirido.CancelarAssinaturaPlanoRecorrente(PlanoAdquiridoId, base.IdUsuarioFilialPerfilLogadoEmpresa.Value, lista))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Finalizar", "Finalizar();", true);
               // else
               //     ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "erroEncerrar", "erroEncerrar();", true);

            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro ao Tentar encerrar plano recorrente/Ou ao Salvar o motivo do cancelamento Idf_Filial =" + base.IdFilial.Value);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "erroEncerrar", "erroEncerrar();", true);
            }
           
        }
        #endregion

        #region [btnVisualizarCurriculos_Click]
        protected void btnVisualizarCurriculos_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.VagasAnunciadas.ToString(), null));
        }
        #endregion

        #region [BntRecrutarSms_Click]
        protected void BntRecrutarSms_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
        }

        #endregion

        protected void btnCurriculosPesquisa_Click(object sender, EventArgs e)
        {
            RouteValueDictionary rota = new RouteValueDictionary();
            rota.Add("IdPesquisaAvancada", hfIDPesquisaCurriculo.Value);
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoFiltro.ToString(), rota));
        }

        protected void btnManterPlano_Click(object sender, EventArgs e)
        {
            pnlCancelamento.Visible = false;
            pnlPlano.Visible = true;
            upCancelamento.Update();
            upPlano.Update();
        }
    }
}