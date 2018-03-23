using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using Ajax;
using BNE.Auth;
using BNE.Auth.Core.Enumeradores;
using BNE.Auth.EventArgs;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;
using BNE.Common.Session;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.ViewStateObjects;
using BNE.Web.Handlers;
using BNE.Web.UserControls;
using BNE.Web.UserControls.Modais;
using JSONSharp;
using Resources;
using Funcao = BNE.BLL.Funcao;
using Parametro = BNE.BLL.Parametro;
using PlanoSituacao = BNE.BLL.Enumeradores.PlanoSituacao;
using SituacaoFilial = BNE.BLL.Enumeradores.SituacaoFilial;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;
using TipoPerfil = BNE.BLL.Enumeradores.TipoPerfil;

namespace BNE.Web.Master
{
    public partial class Principal : MasterPage
    {
        private Control ModalIndicarAmigo;

        #region BoolModalIndicarAmigo - Variável 9

        private bool BoolModalIndicarAmigo
        {
            get { return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel9.ToString()]); }
            set { ViewState[Chave.Temporaria.Variavel9.ToString()] = value; }
        }

        #endregion

        #region LimparCookieTracker

        public void LimparCookieTracker()
        {
            HttpCookie c;

            var req = HttpContext.Current != null ? HttpContext.Current.Request : Request;
            if (req.Cookies["TRACKER_CONTROLLER_SESSION_ID"] != null) // mvc
            {
                c = req.Cookies["TRACKER_CONTROLLER_SESSION_ID"];
            }
            else
            {
                c = new HttpCookie("TRACKER_CONTROLLER_SESSION_ID");
            }

            c.Expires = DateTime.Now.AddDays(-1d);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Add(c); // mvc
        }

        #endregion

        #region Session

        protected SessionVariable<PesquisaPadrao> PesquisaPadrao = new SessionVariable<PesquisaPadrao>(Chave.Temporaria.PesquisaPadrao.ToString());

        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdUsuarioLogado = new SessionVariable<int>(Chave.Permanente.IdUsuarioLogado.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoUsuarioInterno = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoUsuarioInterno.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoEmpresa = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString());
        public SessionVariable<int> IdPerfil = new SessionVariable<int>(Chave.Permanente.IdPerfil.ToString());
        public SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());
        public SessionVariable<string> Tema = new SessionVariable<string>(Chave.Permanente.Theme.ToString());
        public SessionVariable<string> UrlDestinoPagamento = new SessionVariable<string>(Chave.Permanente.UrlDestinoPagamento.ToString());
        public SessionVariable<string> UrlDestino = new SessionVariable<string>(Chave.Permanente.UrlDestino.ToString());

        public SessionVariable<TipoBuscaMaster> TipoBusca = new SessionVariable<TipoBuscaMaster>(Chave.Permanente.TipoBuscaMaster.ToString());
        public SessionVariable<string> FuncaoMaster = new SessionVariable<string>(Chave.Permanente.FuncaoMaster.ToString());
        public SessionVariable<string> CidadeMaster = new SessionVariable<string>(Chave.Permanente.CidadeMaster.ToString());
        public SessionVariable<string> PalavraChaveMaster = new SessionVariable<string>(Chave.Permanente.PalavraChaveMaster.ToString());

        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());

        //Pagamento
        public SessionVariable<int> PagamentoIdentificadorPagamento = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPagamento.ToString());

        public SessionVariable<int> PagamentoIdentificadorPlano = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlano.ToString());
        public SessionVariable<Boolean> PrimeiraGratis = new SessionVariable<Boolean>(Chave.Permanente.PrimeiraGratis.ToString());
        public SessionVariable<string> PagamentoUrlRetorno = new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString());

        //Origem Acesso
        public SessionVariable<string> OrigemUrlReferr = new SessionVariable<string>("OrigemUrlReferr");

        public SessionVariable<string> OrigemQuery = new SessionVariable<string>("OrigemQuery");
        public SessionVariable<string> OrigemUtmSource = new SessionVariable<string>("OrigemUtmSource");
        public SessionVariable<string> OrigemUtmMedium = new SessionVariable<string>("OrigemUtmMedium");
        public SessionVariable<string> OrigemUtmCampaign = new SessionVariable<string>("OrigemUtmCampaign");
        
        #region LimparSession

        /// <summary>
        ///     Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparSession()
        {
            IdPessoaFisicaLogada.Clear();
            IdCurriculo.Clear();
            IdFilial.Clear();
            IdUsuarioLogado.Clear();
            IdUsuarioFilialPerfilLogadoCandidato.Clear();
            IdUsuarioFilialPerfilLogadoEmpresa.Clear();
            IdUsuarioFilialPerfilLogadoUsuarioInterno.Clear();
            FuncaoMaster.Clear();
            CidadeMaster.Clear();
            PalavraChaveMaster.Clear();
            IdPerfil.Clear();

            var originalSession = HttpContext.Current != null ? HttpContext.Current.Session : Session; // mvc

            // não funciona
            //var saveState = new List<KeyValuePair<string, object>>();

            //foreach (var item in originalSession.Keys)
            //{
            //    var key = (string)item;
            //    saveState.Add(new KeyValuePair<string, object>(key, Session[key]));
            //}

            AuthEventAggregator.Instance.OnClosingManuallySessionWithUserAuth(this, new BNELogoffAuthControlEventArgs(originalSession) { LogoffType = LogoffType.BY_USER });
            originalSession.Abandon(); // mantém outras informações

            // não funciona
            //foreach (var pair in saveState)
            //{
            //    originalSession[pair.Key] = pair.Value;
            //}
        }

        #endregion

        #region LimparSessionPagamento

        /// <summary>
        ///     Limpa as propriedades relacionado a compra de planos armazenadas na sessão do usuário
        /// </summary>
        public void LimparSessionPagamento()
        {
            PagamentoIdentificadorPagamento.Clear();
            PagamentoIdentificadorPlano.Clear();
            PagamentoUrlRetorno.Clear();
            PrimeiraGratis.Clear();
        }

        #endregion

        #region SessionDefault

        /// <summary>
        ///     Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        public void SessionDefault()
        {
            Tema.Value = string.Empty;
            STC.Value = false;
            IdOrigem.Value = 1; //BNE
            TipoBusca.Value = TipoBuscaMaster.Vaga;

            //@reinaldo: Limpa o lixo que fica na sessão
            Session["OrigemHTTP_REFERER"] = null;
            Session["OrigemQUERY_STRING"] = null;
            Session["OrigemUtmSource"] = null;
            Session["OrigemUtmMedium"] = null;
            Session["OrigemUtmCampaign"] = null;
            Session["OrigemUtmTerm"] = null;
            Session["OrigemSalva"] = null;
            //---
        }

        #endregion

        #endregion

        #region Propriedades SEO

        public string DesBreadcrumb;
        public string DesBreadcrumbURL;

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Inicializar();
            }

            ucModalLogin.Logar += ucModalLogin_Logar;
            ucModalLogin.Fechar += ucModalLogin_Fechar;
            Utility.RegisterTypeForAjax(typeof(Principal));
        }

        #endregion

        #region ucModalLogin_Logar

        private void ucModalLogin_Logar(string urlDestino)
        {
            UrlDestino.Value = urlDestino;

            if (LoginEfetuadoSucesso != null)
            {
                LoginEfetuadoSucesso();
            }
            else if (!string.IsNullOrEmpty(urlDestino))
            {
                Redirect(urlDestino);
            }
        }

        #endregion

        #region ucModalLogin_Fechar

        private void ucModalLogin_Fechar()
        {
            if (FecharModalLogin != null)
            {
                FecharModalLogin();
            }
        }

        #endregion

        #region Page_PreRender

        protected void Page_PreRender(object sender, EventArgs e)
        {
            CarregarParametros();
            ScriptManager.RegisterStartupScript(upPesquisaCurriculo, upPesquisaCurriculo.GetType(), "AjustarAbasBuscaPage_PreRender", "javaScript:AjustarAbasBusca();", true);
        }

        #endregion

        #region Inicializar

        private void Inicializar()
        {
            if (BoolModalIndicarAmigo)
            {
                AjustarMostrarModalIndicarAmigo(false, false);
                up.Update();
            }

            AjustarNavegacao();
            AjustarTopoRodape();
            AjustarLogo();
            AjustarLogin();
            AjustarClickLogo();
            PegarOrigemAcesso();

            if (IdCurriculo.HasValue)
            {
                var objCurriculo = new Curriculo(IdCurriculo.Value);
                if (objCurriculo.VIP())
                {
                    ExibirAtendimentoOnline(true);
                }
            }

            if (IdFilial.HasValue)
            {
                var objFilial = new Filial(IdFilial.Value);
                if (!EmpresaBloqueada(objFilial))
                {
                    var possuiPlano = PlanoAdquirido.ExistePlanoAdquiridoFilial(IdFilial.Value, PlanoSituacao.Liberado);
                    if (possuiPlano)
                    {
                        ExibirAtendimentoOnline(false);
                    }
                }


                if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    divTelefone.Visible = true;
                    EmpresaNecessitaAceiteContrato(objFilial, new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value));
                }
            }

            txtFuncaoMaster.Attributes["onkeyup"] += "FuncaoOnChange()";
            txtCidadeMaster.Attributes["onkeyup"] += "CidadeOnChange(this)";

            var urlAmbiente = UIHelper.RecuperarURLAmbiente();

            //Para funcionar o safari com foco no Ipad
            var testeSistmars = $"AbrirPopup('http://{urlAmbiente}/Utilitarios/Sistmars/default.aspx', 600, 800)";
            btlSistmars.Attributes["OnClick"] += testeSistmars;
            btlSistmarsRHOffice.Attributes["OnClick"] += testeSistmars;

            var testeCores = $"AbrirPopup('http://{urlAmbiente}/Utilitarios/Cores/default.asp', 600, 800)";
            btlTesteCores.Attributes["OnClick"] += testeCores;
            btlTesteCoresRHOffice.Attributes["OnClick"] += testeCores;

            hlAgradecimentos.NavigateUrl = GetRouteUrl(RouteCollection.Agradecimentos.ToString(), null);
            hlCadastrarCurriculoRHOffice.NavigateUrl = hlCadastrarCurriculoSTC.NavigateUrl = GetRouteUrl(RouteCollection.CadastroCurriculoMini.ToString(), null);
            hlFalePresidente.NavigateUrl = GetRouteUrl(RouteCollection.FalePresidente.ToString(), null);
            hlOndeEstamos.NavigateUrl = GetRouteUrl(RouteCollection.OndeEstamos.ToString(), null);
            hlPesquisaVagasRhOffice.NavigateUrl = GetRouteUrl(RouteCollection.PesquisaVagaAvancada.ToString(), null);
            hlSalaSelecionador.NavigateUrl = hlSalaSelecionadorMenu.NavigateUrl = GetRouteUrl(RouteCollection.SalaSelecionador.ToString(), null);
            hlSalaVip.NavigateUrl = GetRouteUrl(RouteCollection.SalaVIP.ToString(), null);
            hlAtualizarCurriculo.NavigateUrl = GetRouteUrl(RouteCollection.CadastroCurriculoMini.ToString(), null);
            hlSalaAdministrador.NavigateUrl = "/SalaAdministrador.aspx";
            hlSalaAdminsitradorEmpresa.NavigateUrl = "/SalaAdministradorEmpresas.aspx";
            hlSalaAdministradorCurriculo.NavigateUrl = "/SalaAdministradorEdicaoCV.aspx";
            hlSalaAdministradorVagas.NavigateUrl = "/SalaAdministradorVagas.aspx";
            hlSalaAdministradorFinanceiro.NavigateUrl = "/SalaAdministradorFinanceiro.aspx";
            hlAnunciarVaga.NavigateUrl = hlAnunciarVagaSTC.NavigateUrl = GetRouteUrl(RouteCollection.AnunciarVaga.ToString(), null);
            hlCriarConta.NavigateUrl = "/cadastro-de-empresa-gratis";
            hlCriarCampanha.NavigateUrl = GetRouteUrl(RouteCollection.CampanhaRecrutamento.ToString(), null);
            hlSalaSelecionadorSTC.NavigateUrl = "/SiteTrabalheConoscoMenu.aspx";

            pnlTopo.CssClass = STC.ValueOrDefault | IdPessoaFisicaLogada.HasValue ? "topo" : "topo homebanner";

            //Carregar valores digitados na master se tiver valor
            if (FuncaoMaster.HasValue)
            {
                txtFuncaoMaster.Text = FuncaoMaster.Value;
            }

            if (CidadeMaster.HasValue)
            {
                txtCidadeMaster.Text = CidadeMaster.Value;
            }

            if (PalavraChaveMaster.HasValue)
            {
                txtPalavraChaveMaster.Text = PalavraChaveMaster.Value;
            }
        }

        #endregion

        #region ExibirAtendimentoOnline

        private void ExibirAtendimentoOnline(bool SomenteVIP)
        {
            BannerPossoAjudarPanel.Visible = SomenteVIP;
            atendimentoOnlineMenu.Visible = SomenteVIP;
        }

        #endregion

        #region btiPesquisarVaga_Click

        protected void btiPesquisarVaga_Click(object sender, EventArgs e)
        {
            var funcao = Helper.RemoverAcentos(txtFuncaoMaster.Text ?? string.Empty);

            if (funcao.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiario", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiaria", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase))
            {
                PesquisaPadrao.Value = new PesquisaPadrao { Funcao = funcao, Cidade = txtCidadeMaster.Text };
                Redirect(Rota.RecuperarURLRota(RouteCollection.PesquisaVagaAvancada));
                return;
            }

            BLL.PesquisaVaga objPesquisaVaga;
            RecuperarDadosPesquisaVaga(out objPesquisaVaga);

            //Caso haja um delegate
            if (PesquisaRapidaVaga != null && TipoBusca.Value.Equals(TipoBuscaMaster.Vaga))
            {
                TipoBusca.Value = TipoBuscaMaster.Vaga;
                PesquisaRapidaVaga.Invoke(objPesquisaVaga.IdPesquisaVaga);
            }
            else
            {
                TipoBusca.Value = TipoBuscaMaster.Vaga;

                if (STC.HasValue && STC.Value) //Se for STC mantém o redirecionamento para o projeto antigo
                {
                    Session.Add(Chave.Temporaria.Variavel6.ToString(), objPesquisaVaga.IdPesquisaVaga);
                    Redirect(GetRouteUrl(RouteCollection.PesquisaVaga.ToString(), null));
                }
                else
                {
                    var url = SitemapHelper.MontarUrlVagas();
                    if (!string.IsNullOrWhiteSpace(txtFuncaoMaster.Text) && !string.IsNullOrWhiteSpace(txtCidadeMaster.Text))
                    {
                        url = SitemapHelper.MontarUrlVagasPorFuncaoCidade(objPesquisaVaga.DescricaoFuncao, objPesquisaVaga.NomeCidade, objPesquisaVaga.SiglaEstado);
                    }
                    else if (!string.IsNullOrWhiteSpace(txtFuncaoMaster.Text))
                    {
                        url = SitemapHelper.MontarUrlVagasPorFuncao(objPesquisaVaga.DescricaoFuncao);
                    }
                    else if (!string.IsNullOrWhiteSpace(txtCidadeMaster.Text))
                    {
                        url = SitemapHelper.MontarUrlVagasPorCidade(objPesquisaVaga.NomeCidade, objPesquisaVaga.SiglaEstado);
                    }

                    url += "?idPesquisa=" + objPesquisaVaga.IdPesquisaVaga;
                    Redirect(url);
                }
            }
        }

        #endregion

        #region BtlBNE_Click

        protected void BtlBNE_Click(object sender, EventArgs e)
        {
            DeslogarLimparSessionERedirecionarParaBNE();
        }

        #endregion

        #region btiPesquisarCurriculo_Click

        protected void btiPesquisarCurriculo_Click(object sender, EventArgs e)
        {
            BLL.PesquisaCurriculo objPesquisaCurriculo;
            RecuperarDadosPesquisaCurriculo(txtFuncaoMaster.Text, txtCidadeMaster.Text, txtPalavraChaveMaster.Text, out objPesquisaCurriculo);

            //Caso haja um delegate
            if (PesquisaRapidaCurriculo != null) // && TipoBusca.Value.Equals(TipoBuscaMaster.Curriculo))
            {
                TipoBusca.Value = TipoBuscaMaster.Curriculo;
                PesquisaRapidaCurriculo.Invoke(objPesquisaCurriculo.IdPesquisaCurriculo);
            }
            else
            {
                TipoBusca.Value = TipoBuscaMaster.Curriculo;
                Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));
                Redirect(GetRouteUrl(RouteCollection.PesquisaCurriculo.ToString(), null));
            }
        }

        #endregion

        #region btiSair_Click

        protected void btiSair_Click(object sender, EventArgs e)
        {
             SairGlobal();
             Redirect("~/");
        }

        /// <summary>
        ///     Método usado para sair com um link diferente do link sair da masterpage
        ///     Utilizado nas páginas internas e/ou em um model
        /// </summary>
        public void SairModal()
        {
            SairGlobal();
            Redirect("~/");
        }

        public void SairGlobal()
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                new PessoaFisica(IdPessoaFisicaLogada.Value).ZerarDataInteracaoUsuario();
            }

            BNEAutenticacao.DeslogarPadrao();

            //Removendo parametros de pagamento, codigo de desconto na sessão
            LimparSessionPagamento();

            //Remove controle de exibição da Modal Banner na sala da selecionadora
            Session.Remove("ValidouModalBannerSalaSelecionadora");

            //@reinaldo: Limpa o lixo que fica na sessão
            Session["OrigemHTTP_REFERER"] = null;
            Session["OrigemQUERY_STRING"] = null;
            Session["OrigemUtmSource"] = null;
            Session["OrigemUtmMedium"] = null;
            Session["OrigemUtmCampaign"] = null;
            Session["OrigemUtmTerm"] = null;
            Session["OrigemSalva"] = null;
            //---
        }

        #endregion

        #region btiEntrar_Click

        protected void btiEntrar_Click(object sender, EventArgs e)
        {
            ucModalLogin.Inicializar();
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }

        #endregion

        #region btlSalaVIP_Click

        protected void btlSalaVIP_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)TipoPerfil.Candidato))
            {
                Redirect(GetRouteUrl(RouteCollection.SalaVIP.ToString(), null));
            }
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(RouteCollection.SalaVIP.ToString(), null));
                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }

        #endregion

        #region btlAtualizarCurriculo_Click

        protected void btlAtualizarCurriculo_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)TipoPerfil.Candidato) || STC.Value)
            {
                AjustaRedirectCadastroCurriculo();
            }
            else
            {
                //Setando o UrlDestinoSession
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(RouteCollection.CadastroCurriculoMini.ToString(), null));

                //Setando o Bool AtualizarCurriculo
                Session.Add(Chave.Temporaria.Variavel4.ToString(), true);

                Redirect(GetRouteUrl(RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }

        #endregion

        #region txtFuncaoMaster_TextChanged

        protected void txtFuncaoMaster_TextChanged(object sender, EventArgs e)
        {
            FuncaoMaster.Value = txtFuncaoMaster.Text;
            txtCidadeMaster.Focus();
        }

        #endregion

        #region txtCidadeMaster_TextChanged

        protected void txtCidadeMaster_TextChanged(object sender, EventArgs e)
        {
            CidadeMaster.Value = txtCidadeMaster.Text;
            txtPalavraChaveMaster.Focus();
        }

        #endregion

        #region txtPalavraChaveMaster_TextChanged

        protected void txtPalavraChaveMaster_TextChanged(object sender, EventArgs e)
        {
            PalavraChaveMaster.Value = txtPalavraChaveMaster.Text;

            if (TipoBusca.Value.Equals(TipoBuscaMaster.Curriculo))
            {
                btiPesquisarCurriculo.Focus();
            }
            else
            {
                btiPesquisarVaga.Focus();
            }
        }

        #endregion

        #region btiTrocarEmpresa_Click

        protected void btiTrocarEmpresa_Click(object sender, EventArgs e)
        {
            TrocarEmpresa();
        }

        #endregion

        #region lnkExcluirCv_Click

        protected void lnkExcluirCv_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(RouteCollection.ExcluirCurriculo.ToString(), null));
        }

        #endregion

        #region lnkCadastrarCurriculo_Click

        protected void lnkCadastrarCurriculo_Click(object sender, EventArgs e)
        {
            AjustaRedirectCadastroCurriculo();
        }

        #endregion

        #region ucModalIndicarAmigo_Sucesso

        private void ucModalIndicarAmigo_Sucesso()
        {
            ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._23016, false);
            ucModalConfirmacao.MostrarModal();
        }

        #endregion

        #region btlUltimasVagasSTC_Click

        protected void btlUltimasVagasSTC_Click(object sender, EventArgs e)
        {
            UltimasVagas();
        }

        #endregion

        #endregion

        #region Métodos

        #region PegarOrigemAcesso

        /// <summary>
        ///     Pegar os dados de origem da navegação se existirem
        /// </summary>
        public void PegarOrigemAcesso()
        {
            if (Session["OrigemSalva"] == null)
            {
                Session["OrigemHTTP_REFERER"] = Request.ServerVariables["HTTP_REFERER"] != null ? Request.ServerVariables["HTTP_REFERER"] : "";
                Session["OrigemQUERY_STRING"] = Request.ServerVariables["QUERY_STRING"];
                Session["OrigemUtmSource"] = Request.QueryString["utm_Source"] != null ? Request.QueryString["utm_Source"] : "";
                Session["OrigemUtmMedium"] = Request.QueryString["utm_Medium"] != null ? Request.QueryString["utm_Medium"] : "";
                Session["OrigemUtmCampaign"] = Request.QueryString["utm_campaign"] != null ? Request.QueryString["utm_campaign"] : "";
                Session["OrigemUtmTerm"] = Request.QueryString["utm_term"] != null ? Request.QueryString["utm_term"] : "";
                Session["OrigemSalva"] = "1";
            }
        }

        #endregion

        #region InicializarBarraBusca

        public void InicializarBarraBusca(TipoBuscaMaster tipoBuscaMaster, bool expandida, string strKey)
        {
            TipoBusca.Value = tipoBuscaMaster;

            var parametros = new
            {
                tipoBusca = TipoBusca.Value.Equals(TipoBuscaMaster.Vaga) ? "V" : "C",
                expandido = expandida.ToString()
            };
            ScriptManager.RegisterStartupScript(this, GetType(), strKey, "javaScript:InicializarAbasBusca(" + new JSONReflector(parametros) + ");", true);
        }

        #endregion

        #region EmpresaEmAuditoria

        public bool EmpresaEmAuditoria(Filial objFilial)
        {
            if (objFilial.EmpresaEmAuditoria())
            {
                var auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx");
                cphModaisMaster.Controls.Add(auditoria);

                ((EmpresaBloqueadaAguardandoPub)auditoria).MostrarModal();

                upCphModais.Update();
                return true;
            }

            return false;
        }

        #endregion

        #region EmpresaSemDadosReceita

        public bool EmpresaSemDadosReceita(Filial objFilial)
        {
            if (objFilial.EmpresaSemDadosReceita())
            {
                var auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaSemDadosReceita.ascx");
                cphModaisMaster.Controls.Add(auditoria);

                ((EmpresaSemDadosReceita)auditoria).MostrarModal();

                upCphModais.Update();

                if (Page.GetType().BaseType == typeof(Default))
                {
                    SairGlobal();
                }
                else
                {
                    Redirect("~/");
                }
                return true;
            }
            return false;
        }

        #endregion

        #region EmpresaBloqueada

        public bool EmpresaBloqueada(Filial objFilial)
        {
            if (objFilial.EmpresaBloqueada())
            {
                BloquearAcesso(SituacaoFilial.Bloqueado);
                return true;
            }
            if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)SituacaoFilial.ForaDoHorarioComercial))
            {
                BloquearAcesso(SituacaoFilial.ForaDoHorarioComercial);
                return true;
            }
            PlanoAdquirido objPlanoAdquirido = null;
            PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)PlanoSituacao.Bloqueado, out objPlanoAdquirido);
            if (objPlanoAdquirido != null && objPlanoAdquirido.PlanoAdquiridoBloqueado())
            {
                BloquearAcesso(SituacaoFilial.Bloqueado);
                return true;
            }

            return false;
        }

        protected void BloquearAcesso(SituacaoFilial situacao)
        {
            var bloqueada = Page.LoadControl("~/UserControls/Modais/EmpresaBloqueada.ascx");
            cphModaisMaster.Controls.Add(bloqueada);

            if (situacao.Equals(SituacaoFilial.Bloqueado))
            {
                ((EmpresaBloqueada)bloqueada).MostrarModal();
            }
            else
            {
                ((EmpresaBloqueada)bloqueada).MostrarModalForaHorario();
            }

            chat_controle.Visible = false;
            chat_controle.ForceUpdateRender();

            upCphModais.Update();

            if (Page.GetType().BaseType == typeof(Default))
            {
                SairGlobal();
            }
            else
            {
                Redirect("~/");
            }
        }

        #endregion

        #region EmpresaNecessitaAceiteContrato

        public bool EmpresaNecessitaAceiteContrato(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            if (PlanoAdquirido.ExistePlanoAdquiridoPrecisandoAceiteContrato(objFilial) || PlanoAdquirido.ExistePlanoAdquiridoPrecisandoAceiteContrato(objUsuarioFilialPerfil))
            {
                ucEmpresaBloqueadaAceiteContrato.MostrarModal();
                return true;
            }

            return false;
        }

        #endregion

        #region ExibirMensagem

        /// <summary>
        ///     Metodo responsável por exibir a mensage no rodapé do site.
        /// </summary>
        /// <param name="mensagem">string mensagem</param>
        /// <param name="tipo">tipo da mensagem</param>
        /// <param name="aumentarTamanhoPainelMensagem"></param>
        public void ExibirMensagem(string mensagem, TipoMensagem tipo, bool aumentarTamanhoPainelMensagem = false)
        {
            lblAviso.Text = mensagem;
            switch (tipo)
            {
                case TipoMensagem.Aviso:
                    lblAviso.CssClass = "texto_avisos_padrao";
                    break;
                case TipoMensagem.Erro:
                    lblAviso.CssClass = "texto_avisos_erro";
                    break;
            }
            updAviso.Update();

            if (string.IsNullOrEmpty(mensagem))
            {
                ScriptManager.RegisterStartupScript(updAviso, updAviso.GetType(), "OcultarAviso", "javaScript:OcultarAviso();", true);
            }
            else
            {
                var parametros = new
                {
                    aumentarPainelMensagem = aumentarTamanhoPainelMensagem
                };
                ScriptManager.RegisterStartupScript(updAviso, updAviso.GetType(), "ExibirAviso", "javaScript:ExibirAviso(" + new JSONReflector(parametros) + ");", true);
            }
        }

        #endregion

        #region ExibirLogin

        public void ExibirLoginEmpresa()
        {
            ucModalLogin.InicializarEmpresa();
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }

        public void ExibirLogin()
        {
            ucModalLogin.Inicializar();
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }

        #endregion

        #region ExibirLoginPara

        public void ExibirLoginPara(string Destino)
        {
            ucModalLogin.Inicializar(false, false, Destino);
            ucModalLogin.Mostrar();
            pnlL.Visible = true;
            upL.Update();
        }

        #endregion

        #region ExibirMensagemAlerta

        public void ExibirMensagemAlerta(string titulo, string mensagem)
        {
            ucModalConfirmacao.PreencherCampos(titulo, mensagem);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
        }

        #endregion

        #region ExibirMensagemConfirmacao

        public void ExibirMensagemConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            ucModalConfirmacao.PreencherCampos(titulo, mensagem, cliqueAqui);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
        }

        public void ExibirMensagemConfirmacao(string titulo, string mensagem, bool cliqueAqui, string nomeBotao)
        {
            ucModalConfirmacao.PreencherCampos(titulo, mensagem, cliqueAqui, nomeBotao);
            ucModalConfirmacao.MostrarModal();
            pnlM.Visible = true;
            upM.Update();
        }

        #endregion

        #region SetarFoco

        public void SetarFoco(Control controle)
        {
            var script = new StringBuilder();
            script.AppendLine("employer.form.util.ffe('" + controle.ClientID + "');");
            ScriptManager.RegisterStartupScript(Page, GetType(), "FixFirstElement", script.ToString(), true);
        }

        #endregion

        #region AjustarNavegacao

        private void AjustarNavegacao()
        {
            if (!STC.HasValue)
            {
                SessionDefault();
            }

            if (!STC.Value)
            {
                pnlLinkTopoBNE.Visible = btlEntrar.Visible = !IsCandidato() && !IsEmpresa() && !Request.Browser.IsMobileDevice && !IsInterno();
                pnlMenu.Visible = IsCandidato() || IsEmpresa() || Request.Browser.IsMobileDevice || IsInterno(); 

                if (IdPessoaFisicaLogada.HasValue)
                {
                    salaSelecionadoraMenu.Visible = atendimentoOnlineMenu.Visible = salaSelecionadoraMenu.Visible = trocarEmpresaMenu.Visible = IsEmpresa();
                    salaVIPMenu.Visible = atualizarCurriculoMenu.Visible = IsCandidato();
                    salaAdministradorCurriculo.Visible = SalaAdministradorVagas.Visible =  SalaAdministradorEmpresa.Visible = salaAdministradorMenu.Visible = IsInterno();

                    if (IsInterno())
                    {
                        var Permissoes = UsuarioFilialPerfil.CarregarPermissoes(IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, BLL.Enumeradores.CategoriaPermissao.Administrador);
                        if (Permissoes.Contains((int)BLL.Enumeradores.Permissoes.Administrador.AcessarTelaSalaAdministradorFinanceiro))
                            salaAdministradorFinanceiro.Visible = true;
                    }

                    souCandidatoMenu.Visible = souEmpresaMenu.Visible = false;
                }
            }
            else
            {
                pnlLinkTopoSTC.Visible = !Request.Browser.IsMobileDevice;
                pnlLinkBNE.Visible = false;
                liBtlBNE.Visible = pnlLinkSTC.Visible = true;
                hlSalaVIPSTC.Visible = IsCandidato() && IsSTCcomVIP();
                btlAtualizarCurriculoSTC.Visible = IsCandidato();

                hlCadastrarCurriculoSTC.Visible = true;
                btlUltimasVagas.Visible = true;

                hlSalaSelecionadorSTC.Visible = hlAnunciarVagaSTC.Visible = IsEmpresa();
                if (IsCandidato() || IsEmpresa())
                {
                    btlSairSTC.Visible = true;
                    hlCadastrarCurriculoSTC.Visible = false;
                }
                if (IsEmpresa())
                {
                    btlUltimasVagas.Visible = false;
                }
            }
        }

        #endregion

        private bool IsCandidato()
        {
            return IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)TipoPerfil.Candidato);
        }
        private bool IsEmpresa()
        {
            return IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)TipoPerfil.Empresa);
        }
        //!EmpresaBloqueada(objFilial) && !EmpresaEmAuditoria(objFilial) && !EmpresaSemDadosReceita(objFilial)

        private bool IsInterno()
        {
            return IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, (int)TipoPerfil.Interno);
        }

        private bool IsSTCcomVIP()
        {
            OrigemFilial objOrigemFilial;
            return STC.Value && OrigemFilial.CarregarPorOrigem(IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial.Filial.PossuiSTCUniversitario() || objOrigemFilial.Filial.PossuiSTCLanhouse());
        }

        #region AjustarLogo

        private void AjustarLogo()
        {
            if (STC.Value)
            {
                if (IdOrigem.HasValue)
                {
                    var objOrigemFilial = OrigemFilial.CarregarPorOrigem(IdOrigem.Value);
                    decimal numeroCNPJ = 0;

                    if (objOrigemFilial != null)
                    {
                        numeroCNPJ = objOrigemFilial.Filial.RecuperarNumeroCNPJ();
                    }

                    //Empresa possui logo
                    if (!PessoaJuridicaLogo.ExisteLogo(numeroCNPJ))
                    {
                        litRazaoEmpresa.Text = objOrigemFilial.Filial.RazaoSocial;
                        imgLogo.Visible = false;
                    }
                    else
                    {
                        imgLogo.ImageUrl = UIHelper.RetornarUrlLogo(numeroCNPJ.ToString(), PessoaJuridicaLogo.OrigemLogo.Local, null, null);
                    }
                }
                else
                {
                    imgLogo.Visible = false;
                }
            }
            else
            {
                imgLogo.ImageUrl = "/img/logo_bne.png";
            }
        }

        #endregion

        #region AjustarLogin

        public void AjustarLogin()
        {
            btlEntrar.Visible = btlEntrarMenu.Visible = !IdPessoaFisicaLogada.HasValue;
            btlSair.Visible = IdPessoaFisicaLogada.HasValue;

            if (IdPessoaFisicaLogada.HasValue)
            {
                litNomeUsuarioLogado.Text = new PessoaFisica(IdPessoaFisicaLogada.Value).PrimeiroNome;
                pnlNomeUsuarioLogado.Visible = true;
            }
            else
            {
                litNomeUsuarioLogado.Text = string.Empty;
                pnlNomeUsuarioLogado.Visible = false;
            }

            upBotoesSistemaLogin.Update();
        }

        #endregion

        #region AjustarClickLogo

        public void AjustarClickLogo()
        {
              lnkLogo.HRef = "/";
        }

        #endregion

        #region LimparSessionERedirecionarParaBNE

        /// <summary>
        ///     Desloga o usuario limpando a session e redireciona para a Default
        /// </summary>
        public void DeslogarLimparSessionERedirecionarParaBNE()
        {
            BNEAutenticacao.DeslogarPadrao();
            SessionDefault();
            Redirect("~/");
        }

        #endregion

        #region AjustarTopoRodape

        public void AjustarTopoRodape()
        {
            if (STC.Value)
            {
                litRazaoEmpresa.Visible = true;

                if (IdOrigem.HasValue)
                {
                    var objOrigemFilial = OrigemFilial.CarregarPorOrigem(IdOrigem.Value);

                    if (objOrigemFilial == null)
                    {
                        objOrigemFilial = new OrigemFilial();
                    }

                    ParametroFilial objParametroFilial;
                    if (ParametroFilial.CarregarParametroPorFilial(BLL.Enumeradores.Parametro.STCMostrarLinkBNENoRodapeMaster, objOrigemFilial.Filial, out objParametroFilial))
                    {
                        liBtlBNE.Visible = Convert.ToBoolean(objParametroFilial.ValorParametro);
                    }
                }

                //aBuscarCurriculos.Visible = btiPesquisarCurriculo.Visible = buscaavancadacv.Visible = IdUsuarioFilialPerfilLogadoEmpresa.HasValue;
                //aBuscarVagas.Visible = btiPesquisarVaga.Visible = buscaavancada.Visible = !IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

                aBuscarCurriculos.Visible = btiPesquisarCurriculo.Visible = IdUsuarioFilialPerfilLogadoEmpresa.HasValue;
                aBuscarVagas.Visible = btiPesquisarVaga.Visible = !IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

                liHlPesquisaSalarialRHOffice.Visible = IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

                liAtendimentoOnlineRodapeRHOffice.Visible = false;
                ucAtendimentoOnlineRHOffice.LocalizadoRodape = true;
                ucAtendimentoOnlineRHOffice.AjustarSOS();
            }
        }

        #endregion

        #region CarregarParametros

        /// <summary>
        ///     Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<BLL.Enumeradores.Parametro>
                {
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCurso,
                    BLL.Enumeradores.Parametro.UrlAutoCompleteCidade,
                    BLL.Enumeradores.Parametro.UrlAutoCompleteFuncao,
                    BLL.Enumeradores.Parametro.UrlAutoCompleteBairro,
                    BLL.Enumeradores.Parametro.UrlAutoCompleteCurso
                };

                var valoresParametros = Parametro.ListarParametros(parametros);

                var parametrosJSON = new
                {
                    URLCompleteCidade = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteCidade],
                    LimiteCompleteCidade = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade],
                    URLCompleteFuncao = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteFuncao],
                    LimiteCompleteFuncao = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao],
                    URLCompleteBairro = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteBairro],
                    LimiteCompleteBairro = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade],
                    URLCompleteCurso = valoresParametros[BLL.Enumeradores.Parametro.UrlAutoCompleteCurso],
                    LimiteCompleteCurso = valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCurso]
                };
                ScriptManager.RegisterClientScriptBlock(this, GetType(), DateTime.Now.Ticks.ToString(), "javaScript:InicializarAutoCompletes(" + new JSONReflector(parametrosJSON) + ");", true);
            }
            catch (Exception ex)
            {
                string message;

                GerenciadorException.GravarExcecao(ex, out message);

                ExibirMensagem(message, TipoMensagem.Erro);
            }
        }

        #endregion

        #region RecuperarDadosPesquisaCurriculo

        public void RecuperarDadosPesquisaCurriculo(string funcao, string cidade, string palavraChave, out BLL.PesquisaCurriculo objPesquisaCurriculo)
        {
            objPesquisaCurriculo = new BLL.PesquisaCurriculo();

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                objPesquisaCurriculo.UsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);
            }

            if (IdCurriculo.HasValue)
            {
                objPesquisaCurriculo.Curriculo = new Curriculo(IdCurriculo.Value);
            }

            objPesquisaCurriculo.DescricaoIP = Common.Helper.RecuperarIP();

            Cidade objCidade;
            if (Cidade.CarregarPorNome(cidade, out objCidade))
            {
                objPesquisaCurriculo.Cidade = objCidade;
                objPesquisaCurriculo.Estado = objCidade.Estado;
                CidadeMaster.Value = txtCidadeMaster.Text = objCidade.ToString();
            }

            objPesquisaCurriculo.DescricaoPalavraChave = palavraChave;
            objPesquisaCurriculo.FlagPesquisaAvancada = false;

            var listaFuncao = new List<PesquisaCurriculoFuncao>();

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(funcao, out objFuncao))
            {
                listaFuncao.Add(new PesquisaCurriculoFuncao { Funcao = objFuncao });

                FuncaoMaster.Value = txtFuncaoMaster.Text = objFuncao.DescricaoFuncao;
            }

            objPesquisaCurriculo.Salvar(listaFuncao);
        }

        #endregion

        #region RecuperarDadosPesquisaVaga

        private void RecuperarDadosPesquisaVaga(out BLL.PesquisaVaga objPesquisaVaga)
        {
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);
            }

            Curriculo objCurriculo = null;
            if (IdCurriculo.HasValue)
            {
                objCurriculo = new Curriculo(IdCurriculo.Value);
            }

            objPesquisaVaga = BLL.PesquisaVaga.RecuperarDadosPesquisaVaga(objUsuarioFilialPerfil, objCurriculo, Common.Helper.RecuperarIP(), txtFuncaoMaster.Text, txtCidadeMaster.Text);
        }

        #endregion

        #region TrocarEmpresa

        public void TrocarEmpresa()
        {
            ucTrocarEmpresa.Inicializar();
            ucTrocarEmpresa.MostrarModal();
            pnlTE.Visible = true;
            upTE.Update();
        }

        #endregion

        #region AtualizarUsuario

        public void AtualizarUsuario()
        {
            Session.Add(Chave.Temporaria.Variavel1.ToString(), IdFilial.Value);
            Redirect(GetRouteUrl(RouteCollection.CadastroEmpresaUsuario.ToString(), null));
        }

        #endregion

        #region AjustaRedirectCadastroCurriculo

        private void AjustaRedirectCadastroCurriculo()
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisicaLogada.Value);
            }

            Redirect(Page.GetRouteUrl(RouteCollection.CadastroCurriculoMini.ToString(), null));
        }

        #endregion

        #region  AjustarRedirectAnunciarVaga

        public void AjustarRedirectAnunciarVaga()
        {
            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)TipoPerfil.Empresa))
            {
                if (IdFilial.HasValue)
                {
                    if (!EmpresaBloqueada(new Filial(IdFilial.Value)))
                    {
                        UrlDestino.Value = "Default.aspx"; // "~/";
                        Redirect(Page.GetRouteUrl(RouteCollection.AnunciarVaga.ToString(), null));
                    }
                }
            }
            else
            {
                Redirect("empresa#cadastroVaga");
            }
        }
        #endregion

        #region AjustarMostrarModalIndicarAmigo

        private void AjustarMostrarModalIndicarAmigo(bool show, bool hide)
        {
            ModalIndicarAmigo = Page.LoadControl("~/UserControls/IndicarAmigo.ascx");
            pnl.Controls.Clear();
            pnl.Controls.Add(ModalIndicarAmigo);
            ((IndicarAmigo)ModalIndicarAmigo).Sucesso += ucModalIndicarAmigo_Sucesso;
            if (show)
            {
                ((IndicarAmigo)ModalIndicarAmigo).MostrarModal();
                BoolModalIndicarAmigo = true;
            }

            if (hide)
            {
                ((IndicarAmigo)ModalIndicarAmigo).FecharModal();
                BoolModalIndicarAmigo = false;
            }

            up.Update();
        }

        #endregion

        #region Redirect

        protected void Redirect(string url)
        {
            try
            {
                Response.Redirect(url, true);
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                {
                    GerenciadorException.GravarExcecao(ex);
                }
            }
        }

        #endregion

        #region UltimasVagas

        private void UltimasVagas()
        {
            FuncaoMaster.Clear();
            CidadeMaster.Clear();

            string url;
            if (STC.HasValue && STC.Value) //Se for STC mantém o redirecionamento para o projeto antigo
            {
                url = GetRouteUrl(RouteCollection.PesquisaVaga.ToString(), null);
            }
            else
            {
                url = string.Concat("http://", Helper.RecuperarURLVagas(), "/vagas-de-emprego");
            }

            Redirect(url);
        }
        #endregion

        #region AjustarTituloTela
        public void AjustarTituloTela(string tituloTela, string cssClass = null)
        {
            h1TituloTela.Visible = true;
            litTituloTela.Text = $"<div class=\"{cssClass}\">{tituloTela}</div>";
        }
        #endregion

        #region RecuperarCookieAcesso

        public HttpCookie RecuperarCookieAcesso()
        {
            return ((BasePage)Page).RecuperarCookieAcesso();
        }

        #endregion

        #region GravarCookieAcesso

        public void GravarCookieAcesso(PessoaFisica objPessoaFisica)
        {
            ((BasePage)Page).GravarCookieAcesso(objPessoaFisica);
        }

        #endregion

        #region LimparCookieAcesso

        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieAcesso()
        {
            ((BasePage)Page).LimparCookieAcesso();
        }

        #endregion

        #region GravarCookieLoginVagas

        public void GravarCookieLoginVagas(PessoaFisica objPessoaFisica)
        {
            ((BasePage)Page).GravarCookieLoginVagas(objPessoaFisica);
        }

        #endregion

        #region LimparCookieLoginVagas

        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieLoginVagas()
        {
            ((BasePage)Page).LimparCookieLoginVagas();
        }

        #endregion

        #endregion

        #region Delegates

        #region PesquisaVagaPadrao

        public delegate void delegatePesquisaRapidaVaga(int idPesquisaVaga);

        public event delegatePesquisaRapidaVaga PesquisaRapidaVaga;

        public delegate void delegatePesquisaRapidaCurriculo(int idPesquisaCurriculo);

        public event delegatePesquisaRapidaCurriculo PesquisaRapidaCurriculo;

        #endregion

        public delegate void LoginEfetuadoSucessoHandler();

        public event LoginEfetuadoSucessoHandler LoginEfetuadoSucesso;

        public delegate void FecharModalLoginHandler();

        public event FecharModalLoginHandler FecharModalLogin;

        #endregion

        #region AjaxMehods

        #region ValidarCidade

        /// <summary>
        ///     Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
            {
                return true;
            }

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }

        #endregion

        #region RecuperarCidade

        /// <summary>
        ///     Recuperar cidade
        /// </summary>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public static string RecuperarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
            {
                return string.Empty;
            }

            Cidade objCidade;
            if (valor.LastIndexOf('/').Equals(-1))
            {
                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;
                }
            }

            return string.Empty;
        }

        #endregion

        #region CarregarCidade

        /// <summary>
        ///     carregar Cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public static string CarregarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
            {
                return string.Empty;
            }

            Cidade objCidade;
            if (Cidade.CarregarPorNome(valor, out objCidade))
            {
                return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;
            }

            return string.Empty;
        }

        #endregion


        #region ValidarFuncao

        /// <summary>
        ///     Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
            {
                return true;
            }

            int? idOrigem = null;

            if (IdOrigem.HasValue)
            {
                idOrigem = IdOrigem.Value;
            }

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }

        #endregion

        #endregion
    }
}