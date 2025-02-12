﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Session;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Handlers;
using BNE.Web.UserControls;
using JSONSharp;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.EL;
using System.Data;
using BNE.Auth.Core.Enumeradores;
using BNE.Auth.EventArgs;
using BNE.Web.Code.ViewStateObjects;

namespace BNE.Web.Master
{
    public partial class Principal : MasterPage
    {

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
        public SessionVariable<string> PagamentoUrlRetorno = new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString());

        //Origem Acesso
        public SessionVariable<string> OrigemUrlReferr = new SessionVariable<string>("OrigemUrlReferr");
        public SessionVariable<string> OrigemQuery = new SessionVariable<string>("OrigemQuery");
        public SessionVariable<string> OrigemUtmSource = new SessionVariable<string>("OrigemUtmSource");
        public SessionVariable<string> OrigemUtmMedium = new SessionVariable<string>("OrigemUtmMedium");
        public SessionVariable<string> OrigemUtmCampaign = new SessionVariable<string>("OrigemUtmCampaign");

        #region LimparSession
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
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

            var originalSession = (HttpContext.Current != null ? HttpContext.Current.Session : Session); // mvc

            // não funciona
            //var saveState = new List<KeyValuePair<string, object>>();

            //foreach (var item in originalSession.Keys)
            //{
            //    var key = (string)item;
            //    saveState.Add(new KeyValuePair<string, object>(key, Session[key]));
            //}

            BNE.Auth.AuthEventAggregator.Instance.OnClosingManuallySessionWithUserAuth(this, new BNELogoffAuthControlEventArgs(originalSession) { LogoffType = LogoffType.BY_USER });
            originalSession.Abandon();  // mantém outras informações

            // não funciona
            //foreach (var pair in saveState)
            //{
            //    originalSession[pair.Key] = pair.Value;
            //}
        }
        #endregion

        #region LimparSessionPagamento
        /// <summary>
        /// Limpa as propriedades relacionado a compra de planos armazenadas na sessão do usuário
        /// </summary>
        public void LimparSessionPagamento()
        {
            PagamentoIdentificadorPagamento.Clear();
            PagamentoIdentificadorPlano.Clear();
            PagamentoUrlRetorno.Clear();
        }
        #endregion

        #region SessionDefault
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
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

        private Control ModalIndicarAmigo;

        #region Propriedades SEO
        public string DesBreadcrumb;
        public string DesBreadcrumbURL;
        #endregion

        #region PropriedadeAjustarTopoUsuarioCandidato
        public bool PropriedadeAjustarTopoUsuarioCandidato
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel7.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
            }
        }
        #endregion

        #region PropriedadeAjustarTopoUsuarioEmpresa
        public bool PropriedadeAjustarTopoUsuarioEmpresa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel8.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
            }
        }
        #endregion

        #region BoolModalIndicarAmigo - Variável 9
        private bool BoolModalIndicarAmigo
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel9.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel9.ToString()] = value;
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

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

           

            ucModalLogin.Logar += ucModalLogin_Logar;
            ucModalLogin.Fechar += ucModalLogin_Fechar;
            Ajax.Utility.RegisterTypeForAjax(typeof(Principal));
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            UrlDestino.Value = urlDestino;

            if (LoginEfetuadoSucesso != null)
                LoginEfetuadoSucesso();
            else if (!String.IsNullOrEmpty(urlDestino))
                Redirect(urlDestino);
        }
        #endregion

        #region ucModalLogin_Fechar
        void ucModalLogin_Fechar()
        {
            if (FecharModalLogin != null)
                FecharModalLogin();
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

            ucAtendimentoOnline.Visible = false;
            ExibirAtendimentoOnline();

            if (IdCurriculo.HasValue)
            {
                var objCurriculo = new Curriculo(IdCurriculo.Value);
                if (objCurriculo.VIP())
                    ExibirAtendimentoOnline();
            }

            if (IdFilial.HasValue)
            {
                var objFilial = new Filial(this.IdFilial.Value);
                if (!EmpresaBloqueada(objFilial))
                {
                    var possuiPlano = PlanoAdquirido.ExistePlanoAdquiridoFilial(IdFilial.Value, Enumeradores.PlanoSituacao.Liberado);
                    if (possuiPlano)
                        ExibirAtendimentoOnline();
                }


                if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    EmpresaNecessitaAceiteContrato(objFilial, new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value));
                }
            }

            btiTwitter.Attributes["OnClick"] += "AbrirPopup('http://twitter.com/bneempregos', 600, 800)";

            txtFuncaoMaster.Attributes["onkeyup"] += "FuncaoOnChange()";
            txtCidadeMaster.Attributes["onkeyup"] += "CidadeOnChange(this)";

            string urlAmbiente = UIHelper.RecuperarURLAmbiente();
            string urlVagas = Helper.RecuperarURLVagas();

            hlBuscarVagas.NavigateUrl = string.Concat("http://", urlVagas, "/busca-de-vagas");
            hlUltimasVagas.NavigateUrl = string.Concat("http://", urlVagas, "/vagas-de-emprego");

            //Para funcionar o safari com foco no Ipad
            btlSistmars.Attributes["OnClick"] +=
            btiSistmars.Attributes["OnClick"] += string.Format("AbrirPopup('http://{0}/Utilitarios/Sistmars/default.aspx', 600, 800)", urlAmbiente);

            btlTesteCores.Attributes["OnClick"] +=
            btiTesteCores.Attributes["OnClick"] += string.Format("AbrirPopup('http://{0}/Utilitarios/Cores/default.asp', 600, 800)", urlAmbiente);

            btlTesteCoresRHOffice.Attributes["OnClick"] +=
            btlTesteCoresRHOffice.Attributes["OnClick"] += string.Format("AbrirPopup('http://{0}/Utilitarios/Cores/default.asp', 600, 800)", urlAmbiente);

            hlAgradecimentos.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.Agradecimentos.ToString(), null);
            hlCadastrarCurriculoRHOffice.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null);

            hlCadastrarEmpresa.NavigateUrl = "/cadastro-de-empresa-gratis";

            hlFalePresidente.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.FalePresidente.ToString(), null);
            hlOndeEstamos.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.OndeEstamos.ToString(), null);
            hlOndeEstamosRodape.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.OndeEstamos.ToString(), null);
            hlPesquisaVagas.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.PesquisaVagaAvancada.ToString(), null);
            hlPesquisaVagasRhOffice.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.PesquisaVagaAvancada.ToString(), null);
            //hlRecrutamentoR1.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.ApresentarR1.ToString(), null);
            hlCampanha.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.CampanhaRecrutamento.ToString(), null);
            hlCompreCVs.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null);
            hlCompreServicoVIP.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null);

            //Carregar valores digitados na master se tiver valor
            if (FuncaoMaster.HasValue)
                txtFuncaoMaster.Text = FuncaoMaster.Value;

            if (CidadeMaster.HasValue)
                txtCidadeMaster.Text = CidadeMaster.Value;

            if (PalavraChaveMaster.HasValue)
                txtPalavraChaveMaster.Text = PalavraChaveMaster.Value;
        }
        #endregion

        #region ExibirAtendimentoOnline
        private void ExibirAtendimentoOnline()
        {
            ucAtendimentoOnline.Visible = true;

            ucAtendimentoOnline.LocalizadoRodape = false;
            ucAtendimentoOnline.AjustarSOS();
        }
        #endregion

        #region btiPesquisarVaga_Click
        protected void btiPesquisarVaga_Click(object sender, EventArgs e)
        {
            var funcao = Helper.RemoverAcentos((txtFuncaoMaster.Text ?? string.Empty));

            if (funcao.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiario", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiaria", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase))
            {
                this.PesquisaPadrao.Value = new PesquisaPadrao { Funcao = funcao, Cidade = txtCidadeMaster.Text };
                Redirect(Rota.RecuperarURLRota(Enumeradores.RouteCollection.PesquisaVagaAvancada));
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
                /*
                TipoBusca.Value = TipoBuscaMaster.Vaga;

                TipoGatilho.DispararGatilhoPesquisaCandidato(HttpContext.Current ?? Context, objPesquisaVaga);
                var url = string.Concat("http://", Helper.RecuperarURLVagas(), "/resultado-pesquisa-avancada-de-vagas/", objPesquisaVaga.IdPesquisaVaga);
                Redirect(url);
                */

                TipoBusca.Value = TipoBuscaMaster.Vaga;

                if (STC.HasValue && STC.Value) //Se for STC mantém o redirecionamento para o projeto antigo
                {
                    Session.Add(Chave.Temporaria.Variavel6.ToString(), objPesquisaVaga.IdPesquisaVaga);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaVaga.ToString(), null));
                }
                else
                {
                    TipoGatilho.DispararGatilhoPesquisaCandidato(HttpContext.Current ?? Context, objPesquisaVaga);
                    var url = SitemapHelper.MontarUrlVagas();
                    if (!String.IsNullOrWhiteSpace(txtFuncaoMaster.Text) && !String.IsNullOrWhiteSpace(txtCidadeMaster.Text))
                        url = SitemapHelper.MontarUrlVagasPorFuncaoCidade(objPesquisaVaga.DescricaoFuncao, objPesquisaVaga.NomeCidade, objPesquisaVaga.SiglaEstado);
                    else if (!String.IsNullOrWhiteSpace(txtFuncaoMaster.Text))
                        url = SitemapHelper.MontarUrlVagasPorFuncao(objPesquisaVaga.DescricaoFuncao);
                    else if (!String.IsNullOrWhiteSpace(txtCidadeMaster.Text))
                        url = SitemapHelper.MontarUrlVagasPorCidade(objPesquisaVaga.NomeCidade, objPesquisaVaga.SiglaEstado);

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
            /*02/10/2014 - Removido redirecionamento de pesquisa de CV de Estagio para busca avançada, atividade runrunit 1799
             * 
             * var funcao = Helper.RemoverAcentos((txtFuncaoMaster.Text ?? string.Empty));
            
            if (funcao.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiario", StringComparison.OrdinalIgnoreCase)
                || funcao.Equals("Estagiaria", StringComparison.OrdinalIgnoreCase)
                //|| funcao.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase)
                )
            {
                this.PesquisaPadrao.Value = new PesquisaCurriculoPadrao { Funcao = funcao, Cidade = txtCidadeMaster.Text, PalavraChave = this.txtPalavraChaveMaster.Text };
                //Redirect(Rota.RecuperarURLRota(Enumeradores.RouteCollection.PesquisaCurriculoAvancada));
                Redirect("~/PesquisaCurriculoAvancada.aspx");
                return;
            }*/

            BLL.PesquisaCurriculo objPesquisaCurriculo;
            RecuperarDadosPesquisaCurriculo(txtFuncaoMaster.Text, txtCidadeMaster.Text, txtPalavraChaveMaster.Text, out objPesquisaCurriculo);

            //Caso haja um delegate
            if (PesquisaRapidaCurriculo != null)// && TipoBusca.Value.Equals(TipoBuscaMaster.Curriculo))
            {
                TipoBusca.Value = TipoBuscaMaster.Curriculo;
                PesquisaRapidaCurriculo.Invoke(objPesquisaCurriculo.IdPesquisaCurriculo);
            }
            else
            {
                TipoBusca.Value = TipoBuscaMaster.Curriculo;
                Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
            }
        }
        #endregion

        #region btiSair_Click
        protected void btiSair_Click(object sender, EventArgs e)
        {
            if (MostrarModalaoSair() && hddJaClicou.Value == "0")
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "modaldesconto", "ModalDesconto()", true);
            }
            else
            {
                SairGlobal();
                Redirect("~/");
            }
        }

        /// <summary>
        /// Método usado para sair com um link diferente do link sair da masterpage
        /// Utilizado nas páginas internas e/ou em um model
        /// </summary>
        public void SairModal()
        {
            SairGlobal();
            Redirect("~/");
        }

        public void SairGlobal()
        {
            if (IdPessoaFisicaLogada.HasValue)
                new PessoaFisica(IdPessoaFisicaLogada.Value).ZerarDataInteracaoUsuario();

            BNE.Auth.BNEAutenticacao.DeslogarPadrao();

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

        #region btiSistmarsRHOffice_Click
        protected void btlSistmarsRHOffice_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(upBtlSistmarsRHOffice, upBtlSistmarsRHOffice.GetType(), "AbrirURL", "javascript:AbrirPopup('Utilitarios/Sistmars/default.aspx', 600, 800)", true);
        }
        #endregion

        #region btlSistmars_Click
        protected void btlTesteCores_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(upBtlTesteCores, upBtlTesteCores.GetType(), "AbrirURL", "javascript:AbrirPopup('/Utilitarios/Cores/default.asp', 600, 800)", true);
        }
        #endregion

        #region btiAtendimentoOnline_Click
        protected void btiAtendimentoOnline_Click(object sender, ImageClickEventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                if (IdFilial.HasValue)
                    ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "<script type='text/jscript'> AbrirPopup('/AbrirAtendimentoOnLine.aspx?TIPO=2',600, 800);</script>", false); //Empresa
                else if (IdPessoaFisicaLogada.HasValue)
                    ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "<script type='text/jscript'> AbrirPopup('/AbrirAtendimentoOnLine.aspx?TIPO=1',600, 800);</script>", false); //Usuario    
                else
                    ScriptManager.RegisterStartupScript(upAtendimentoOnline, upAtendimentoOnline.GetType(), "AbrirURL", "<script type='text/jscript'> AbrirPopup('/AbrirAtendimentoOnLine.aspx?TIPO=3',600, 800);</script>", false); //Não Logado
            }
        }
        #endregion

        #region btlSalaVIP_Click
        protected void btlSalaVIP_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)BNE.BLL.Enumeradores.TipoPerfil.Candidato))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString(), null));
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString(), null));
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region btlSalaVipQuemMeViu_Click
        protected void btlSalaVipQuemMeViu_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)BNE.BLL.Enumeradores.TipoPerfil.Candidato))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuVip.ToString(), null));
            else
            {
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(Enumeradores.RouteCollection.QuemMeViuVip.ToString(), null));
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region btiCvsRecebidos_Click
        protected void btiCvsRecebidos_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                AjustarRedirectCvsRecebidos();
            }
        }
        #endregion

        #region btlSalaSelecionador_Click
        protected void btlSalaSelecionador_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionador.ToString(), null));
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.SalaSelecionador }));
        }
        #endregion

        #region btlCVsRecebidos_Click
        protected void btlCVsRecebidos_Click(object sender, EventArgs e)
        {
            AjustarRedirectCvsRecebidos();
        }
        #endregion

        #region btlAnuncieVagas_Click
        protected void btlAnuncieVagas_Click(object sender, EventArgs e)
        {
            AjustarRedirectAnunciarVaga();
        }
        #endregion

        #region btlSiteGratisCurriculo_Click
        protected void btlSiteGratisCurriculo_Click(object sender, EventArgs e)
        {
            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                Redirect("SiteTrabalheConoscoCriacao.aspx");
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.STC }));
        }
        #endregion

        #region btiAnuncieVagas_Click
        protected void btiAnuncieVagas_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                AjustarRedirectAnunciarVaga();
            }
        }
        #endregion

        #region btlAtualizarCurriculo_Click
        protected void btlAtualizarCurriculo_Click(object sender, EventArgs e)
        {
            if ((IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)Enumeradores.TipoPerfil.Candidato)) || STC.Value)
                AjustaRedirectCadastroCurriculo();
            else
            {
                //Setando o UrlDestinoSession
                Session.Add(Chave.Temporaria.Variavel2.ToString(), GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));

                //Setando o Bool AtualizarCurriculo
                Session.Add(Chave.Temporaria.Variavel4.ToString(), true);

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }
        #endregion

        #region btlAtualizarEmpresa_Click
        protected void btlAtualizarEmpresa_Click(object sender, EventArgs e)
        {
            if (IdPessoaFisicaLogada.HasValue)
            {
                Filial objFilial = null;

                if (IdFilial.HasValue)
                {
                    objFilial = new Filial(IdFilial.Value);
                }


                if (EmpresaBloqueada(objFilial))
                {
                    Redirect("Principal.aspx");
                    return;
                }
                else if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                {
                    //TODO: Melhorar o uso da view state e session
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), IdFilial.Value);
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), IdPessoaFisicaLogada.Value);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.AtualizarDadosEmpresa.ToString(), null));
                }
                else
                    ExibirMensagem(MensagemAviso._101001, Code.Enumeradores.TipoMensagem.Aviso);
            }
            else
            {
                //Setando o Bool AtualizarEmpresa
                Session.Add(Chave.Temporaria.Variavel4.ToString(), true);

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.Cadastro }));
            }
        }
        #endregion

        #region btlPesquisaCurriculo_Click
        protected void btlPesquisaCurriculo_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null));
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
                btiPesquisarCurriculo.Focus();
            else
                btiPesquisarVaga.Focus();
        }
        #endregion

        #region btiTrocarEmpresa_Click
        protected void btiTrocarEmpresa_Click(object sender, EventArgs e)
        {
            TrocarEmpresa();
        }
        #endregion

        #region btlConfiguracoes_Click
        protected void btlConfiguracoes_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                if (STC.Value)
                {
                    Redirect("~/SiteTrabalheConoscoSalaSelecionadorConteudos.aspx");
                }
                else
                {
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionadorConfiguracoes.ToString(), null));
                }
            }
        }
        #endregion

        #region btiAtualizarEmpresa_Click
        protected void btiAtualizarEmpresa_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                btlAtualizarEmpresa_Click(sender, e);
            }
        }
        #endregion

        #region lnkAtualizarEmpresa_Click
        protected void lnkAtualizarEmpresa_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                btlAtualizarEmpresa_Click(sender, e);

        }
        #endregion

        #region btiAtualizarUsuario_Click
        protected void btiAtualizarUsuario_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(this.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                AtualizarUsuario();
        }
        #endregion

        #region lnkAtualizarUsuario_Click
        protected void lnkAtualizarUsuario_Click(object sender, EventArgs e)
        {
            AtualizarUsuario();
        }
        #endregion

        #region lnkExcluirCv_Click
        protected void lnkExcluirCv_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ExcluirCurriculo.ToString(), null));
        }
        #endregion

        #region btiCadastrarCurriculo_Click
        protected void btiCadastrarCurriculo_Click(object sender, EventArgs e)
        {
            AjustaRedirectCadastroCurriculo();
        }
        #endregion

        #region btiAlterarCurriculo_Click
        protected void btiAlterarCurriculo_Click(object sender, EventArgs e)
        {
            AjustaRedirectCadastroCurriculo();
        }
        #endregion

        #region btiMensagens_Click
        protected void btiMensagens_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVipMensagens.ToString(), null));
        }
        #endregion

        #region lnkCadastrarCurriculo_Click
        protected void lnkCadastrarCurriculo_Click(object sender, EventArgs e)
        {
            AjustaRedirectCadastroCurriculo();
        }
        #endregion

        #region btlIndiqueBNE_Click
        protected void btlIndiqueBNE_Click(object sender, EventArgs e)
        {
           
            ucIndicarAmigo.MostrarModal();
           // AjustarMostrarModalIndicarAmigo(true, false);
        }
        #endregion

        #region ucModalIndicarAmigo_Sucesso
        void ucModalIndicarAmigo_Sucesso()
        {
            ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._23016, false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #region btlUltimasVagasRHOffice_Click
        protected void btlUltimasVagasRHOffice_Click(object sender, EventArgs e)
        {
            UltimasVagas();
        }
        #endregion

        #region btlUltimasVagas_Click
        protected void btlUltimasVagas_Click(object sender, EventArgs e)
        {
            UltimasVagas();
        }
        #endregion

        #region btiUltimasVagas_Click
        protected void btiUltimasVagas_Click(object sender, EventArgs e)
        {
            UltimasVagas();
        }
        #endregion

        #region btiJaEnviei_Click
        protected void btiJaEnviei_Click(object sender, EventArgs e)
        {
            if (JaEnvieiVirgemRedirect)
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVipJaEnvieiNaoCandidatura.ToString(), null));
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVipJaEnviei.ToString(), null));
        }
        #endregion

        #endregion

        #region Métodos

        #region PegarOrigemAcesso
        /// <summary>
        /// Pegar os dados de origem da navegação se existirem
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

        #region MostrarModalaoSair
        /// <summary>
        /// Mostrar modal ao sair do BNE, baseado em uma rota e a atual e URL
        /// </summary>
        /// <returns></returns>
        public bool MostrarModalaoSair()
        {
            string url = Request.AppRelativeCurrentExecutionFilePath;
            string rota = Rota.RecuperarURLRota(Enumeradores.RouteCollection.EscolhaPlano);

            url = url.Remove(0, url.LastIndexOf("/") + 1);

            //Checar ao sair na tela de escolha de plano
            return url == rota;
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
                Control auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx");
                cphModaisMaster.Controls.Add(auditoria);

                ((UserControls.Modais.EmpresaBloqueadaAguardandoPub)auditoria).MostrarModal();

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
                Control auditoria = Page.LoadControl("~/UserControls/Modais/EmpresaSemDadosReceita.ascx");
                cphModaisMaster.Controls.Add(auditoria);

                ((UserControls.Modais.EmpresaSemDadosReceita)auditoria).MostrarModal();

                upCphModais.Update();

                if (this.Page.GetType().BaseType == typeof(Default))
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
                BloquearAcesso(Enumeradores.SituacaoFilial.Bloqueado);
                return true;
            }
            if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.ForaDoHorarioComercial))
            {
                BloquearAcesso(Enumeradores.SituacaoFilial.ForaDoHorarioComercial);
                return true;
            }
            PlanoAdquirido objPlanoAdquirido = null;
            PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Bloqueado, out objPlanoAdquirido);
            if (objPlanoAdquirido != null && objPlanoAdquirido.PlanoAdquiridoBloqueado())
            {
                BloquearAcesso(Enumeradores.SituacaoFilial.Bloqueado);
                return true;
            }

            return false;
        }
        protected void BloquearAcesso(Enumeradores.SituacaoFilial situacao)
        {
            Control bloqueada = Page.LoadControl("~/UserControls/Modais/EmpresaBloqueada.ascx");
            cphModaisMaster.Controls.Add(bloqueada);

            if(situacao.Equals(Enumeradores.SituacaoFilial.Bloqueado))
                ((UserControls.Modais.EmpresaBloqueada)bloqueada).MostrarModal();
            else
                ((UserControls.Modais.EmpresaBloqueada)bloqueada).MostrarModalForaHorario();

            chat_controle.Visible = false;
            chat_controle.ForceUpdateRender();

            upCphModais.Update();

            if (this.Page.GetType().BaseType == typeof(Default))
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
        /// Metodo responsável por exibir a mensage no rodapé do site.
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

            if (String.IsNullOrEmpty(mensagem))
                ScriptManager.RegisterStartupScript(updAviso, updAviso.GetType(), "OcultarAviso", "javaScript:OcultarAviso();", true);
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
                SessionDefault();

            if (STC.Value)
            {
                if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
                    AjustarTopoIconesUsuarioEmpresaRHOffice();
                else
                    AjustarTopoIconesSiteRHOffice();
            }
            else
            {
                if (IdPessoaFisicaLogada.HasValue)
                {
                    if (PropriedadeAjustarTopoUsuarioCandidato)
                        AjustarTopoIconesUsuarioCandidato();
                    else if (PropriedadeAjustarTopoUsuarioEmpresa)
                        AjustarTopoIconesUsuarioEmpresa();
                    else
                    {
                        if (IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, (int)Enumeradores.TipoPerfil.Interno))
                            AjustarTopoIconesSiteUsuarioInterno();
                        else if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))  //Se usuário empresa
                            AjustarTopoIconesUsuarioEmpresa();
                        else if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && (UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)Enumeradores.TipoPerfil.Candidato)))  //Se candidato
                            AjustarTopoIconesUsuarioCandidato();
                    }
                }
                else //Usuário não logado
                    AjustarTopoIconesUsuarioNaoLogado();
            }
        }
        #endregion

        #region AjustarTopoIconesSiteRHOffice
        public void AjustarTopoIconesSiteRHOffice()
        {
            liBtiUltimasVagas.Visible = true;
            liBtiCadastrarCurriculo.Visible = true;
            liBtiAlterarCurriculo.Visible = true;
            liBtiSalaVip.Visible = false;

            OrigemFilial objOrigemFilial;
            if (OrigemFilial.CarregarPorOrigem(IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial.Filial.PossuiSTCUniversitario() || objOrigemFilial.Filial.PossuiSTCLanhouse()))
                liBtiSalaVip.Visible = true;
        }
        #endregion

        #region AjustarTopoIconesSiteUsuarioInterno

        public void AjustarTopoIconesSiteUsuarioInterno()
        {
            liBtiPesquisaSalarial.Visible = liBtiSistmars.Visible =
            liBtiSalaAdmFaleComPresidente.Visible = liBtiSalaAdministrador.Visible = true;
            liAtendimentoOnline.Visible = false;
        }

        #endregion

        #region AjustarTopoIconesUsuarioEmpresa

        public void AjustarTopoIconesUsuarioEmpresa()
        {
            liBtiAnuncieVagas.Visible =
            liBtiCvsRecebidos.Visible =
            liBtiSalaSelecionador.Visible = true;
        }

        #endregion

        #region AjustarTopoIconesUsuarioEmpresaRHOffice
        public void AjustarTopoIconesUsuarioEmpresaRHOffice()
        {
            liBtiPesquisaAvancada.Visible = true;
            liBtiAnuncieVagas.Visible = true;
            liBtiSalaSelecionadorSTC.Visible = true;
            liAtendimentoOnline.Visible = false;
        }
        #endregion

        #region AjustarTopoIconesUsuarioCandidato
        public void AjustarTopoIconesUsuarioCandidato()
        {
            liBtiAlterarCurriculo.Visible =
            liBtiQuemMeViu.Visible =
            liBtiSalaVip.Visible = true;
        }
        #endregion

        #region AjustarTopoIconesUsuarioNaoLogado

        public void AjustarTopoIconesUsuarioNaoLogado()
        {
            liBtiTesteCores.Visible =
            liBtiPesquisaSalarial.Visible =
            liBtiSistmars.Visible = true;
        }

        #endregion

        #region AjustarLogo
        private void AjustarLogo()
        {
            if (STC.Value)
            {
                if (IdOrigem.HasValue)
                {
                    OrigemFilial objOrigemFilial = OrigemFilial.CarregarPorOrigem(IdOrigem.Value);
                    var numeroCNPJ = objOrigemFilial.Filial.RecuperarNumeroCNPJ();

                    //Empresa possui logo
                    if (!PessoaJuridicaLogo.ExisteLogo(numeroCNPJ))
                    {
                        litRazaoEmpresa.Text = objOrigemFilial.Filial.RazaoSocial;
                        this.imgLogo.Visible = false;
                    }
                    else
                        this.imgLogo.ImageUrl = UIHelper.RetornarUrlLogo(numeroCNPJ.ToString(), PessoaJuridicaLogo.OrigemLogo.Local, null, null);
                }
                else
                    this.imgLogo.Visible = false;
            }
            else
                this.imgLogo.ImageUrl = "/img/logo_bne.gif";
        }
        #endregion

        #region AjustarLogin
        public void AjustarLogin()
        {
            btiEntrar.Visible = !IdPessoaFisicaLogada.HasValue;
            btiSair.Visible = IdPessoaFisicaLogada.HasValue;

            if (IdPessoaFisicaLogada.HasValue && IdCurriculo.ValueOrDefault > 0 && !IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                litNomeUsuarioLogado.Text = string.Concat("<span id='NomeCandidato'>Olá, ", new PessoaFisica(IdPessoaFisicaLogada.Value).PrimeiroNome, " (CV ", IdCurriculo.ValueOrDefault.ToString(), ")</span>");
            else
                litNomeUsuarioLogado.Text = string.Empty;

            upBotoesSistemaLogin.Update();
        }
        #endregion

        #region AjustarClickLogo
        public void AjustarClickLogo()
        {
            if (MostrarModalaoSair())
            {
                lnkLogo.HRef = "javascript:ModalDesconto('Home');";
            }
            else
            {
                lnkLogo.HRef = "/";
            }
        }
        #endregion

        #region LimparSessionERedirecionarParaBNE
        /// <summary>
        /// Desloga o usuario limpando a session e redireciona para a Default
        /// </summary>
        public void DeslogarLimparSessionERedirecionarParaBNE()
        {
            BNE.Auth.BNEAutenticacao.DeslogarPadrao();
            SessionDefault();
            Redirect("~/");
        }
        #endregion

        #region AjustarTopoRodape
        public void AjustarTopoRodape()
        {
            DivInfosBNE.Visible = !this.STC.ValueOrDefault;
            divInformacoes.Visible = hlkContato.Visible = btiTwitter.Visible = btiAtendimentoOnline.Visible = pnlLinkBNE.Visible = pnlOndeEstamos.Visible = !STC.Value;
            liBtlBNE.Visible = pnlLinkRHOffice.Visible = litRazaoEmpresa.Visible = STC.Value;

            if (STC.Value)
            {
                if (IdOrigem.HasValue)
                {
                    OrigemFilial objOrigemFilial = OrigemFilial.CarregarPorOrigem(IdOrigem.Value);

                    ParametroFilial objParametroFilial;
                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.STCMostrarLinkBNENoRodapeMaster, objOrigemFilial.Filial, out objParametroFilial))
                        liBtlBNE.Visible = Convert.ToBoolean(objParametroFilial.ValorParametro);
                }

                aBuscarCurriculos.Visible = IdUsuarioFilialPerfilLogadoEmpresa.HasValue;
                aBuscarVagas.Visible = !IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

                liHlPesquisaSalarialRHOffice.Visible = IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

                liAtendimentoOnlineRodape.Visible = false;
                ucAtendimentoOnlineRHOffice.LocalizadoRodape = true;
                ucAtendimentoOnlineRHOffice.AjustarSOS();

                //aceFuncaoMaster.ContextKey = IdOrigem.Value.ToString(CultureInfo.CurrentCulture);
            }
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                List<Enumeradores.Parametro> parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao, 
                    Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade, 
                    Enumeradores.Parametro.UrlAutoCompleteCidade, 
                    Enumeradores.Parametro.UrlAutoCompleteFuncao, 
                    Enumeradores.Parametro.UrlAutoCompleteBairro
                };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                var parametrosJSON = new
                {
                    URLCompleteCidade = valoresParametros[Enumeradores.Parametro.UrlAutoCompleteCidade],
                    LimiteCompleteCidade = valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade],
                    URLCompleteFuncao = valoresParametros[Enumeradores.Parametro.UrlAutoCompleteFuncao],
                    LimiteCompleteFuncao = valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao],
                    URLCompleteBairro = valoresParametros[Enumeradores.Parametro.UrlAutoCompleteBairro],
                    LimiteCompleteBairro = valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade]
                };
                ScriptManager.RegisterStartupScript(this, GetType(), DateTime.Now.Ticks.ToString(), "javaScript:InicializarAutoCompletes(" + new JSONReflector(parametrosJSON) + ");", true);
            }
            catch (Exception ex)
            {
                string message;

                EL.GerenciadorException.GravarExcecao(ex, out message);

                this.ExibirMensagem(message, TipoMensagem.Erro);
            }
        }
        #endregion

        #region RecuperarDadosPesquisaCurriculo
        public void RecuperarDadosPesquisaCurriculo(string funcao, string cidade, string palavraChave, out BLL.PesquisaCurriculo objPesquisaCurriculo)
        {
            objPesquisaCurriculo = new BLL.PesquisaCurriculo();

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objPesquisaCurriculo.UsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);

            if (IdCurriculo.HasValue)
                objPesquisaCurriculo.Curriculo = new Curriculo(IdCurriculo.Value);

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

            upTopo.Update();
        }
        #endregion

        #region RecuperarDadosPesquisaVaga
        private void RecuperarDadosPesquisaVaga(out BLL.PesquisaVaga objPesquisaVaga)
        {
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);

            Curriculo objCurriculo = null;
            if (IdCurriculo.HasValue)
                objCurriculo = new Curriculo(IdCurriculo.Value);

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
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroEmpresaUsuario.ToString(), null));
        }
        #endregion

        #region AjustaRedirectCadastroCurriculo
        private void AjustaRedirectCadastroCurriculo()
        {
            if (IdPessoaFisicaLogada.HasValue)
                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisicaLogada.Value);

            Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #region  AjustarRedirectAnunciarVaga
        public void AjustarRedirectAnunciarVaga()
        {
            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
            {
                if (IdFilial.HasValue)
                {
                    if (!EmpresaBloqueada(new Filial(IdFilial.Value)))
                    {
                        UrlDestino.Value = "Default.aspx"; // "~/";
                        Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                    }
                }
            }
            else
                Redirect("empresa#cadastroVaga");
        }
        #endregion

        #region  AjustarRedirectCvsRecebidos
        public void AjustarRedirectCvsRecebidos()
        {
            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
            {
                if (IdFilial.HasValue)
                {
                    if (!EmpresaBloqueada(new Filial(IdFilial.Value)))
                    {
                        Session.Add(Chave.Temporaria.Variavel13.ToString(), true);
                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.VagasAnunciadas.ToString(), null));
                    }
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.AnunciarVaga }));
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
                    EL.GerenciadorException.GravarExcecao(ex);
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
                url = GetRouteUrl(BLL.Enumeradores.RouteCollection.PesquisaVaga.ToString(), null);
            else
                url = string.Concat("http://", Helper.RecuperarURLVagas(), "/vagas-de-emprego");

            Redirect(url);
        }
        #endregion

        #region ExibirMenuSecaoEmpresa
        public void ExibirMenuSecaoEmpresa()
        {
            pnlMenuSecaoEmpresa.Visible = true;
        }
        #endregion

        #region ExibirMenuApenasAtualizarEmpresa
        public void ExibirMenuApenasAtualizarEmpresa()
        {
            pnlMenuSecaoEmpresa.Visible = true;
            btiTrocarEmpresa.Visible = false;
            btiAtualizarUsuario.Visible = false;
        }
        #endregion

        #region ExibirMenuCandidatoEmpresa
        public void ExibirMenuCandidatoEmpresa()
        {
            changeUser.Visible = true;
        }
        #endregion

        #region ExibirMenuSecaoCandidato
        public void ExibirMenuSecaoCandidato()
        {
            pnlMenuSecaoCandidato.Visible = true;
        }
        #endregion

        #region AjustarTituloTela
        public void AjustarTituloTela(string tituloTela)
        {
            h1TituloTela.Visible = true;
            litTituloTela.Text = tituloTela;
        }
        #endregion

        #region RecuperarCookieAcesso
        public HttpCookie RecuperarCookieAcesso()
        {
            return ((BasePage)this.Page).RecuperarCookieAcesso();
        }
        #endregion

        #region GravarCookieAcesso
        public void GravarCookieAcesso(PessoaFisica objPessoaFisica)
        {
            ((BasePage)this.Page).GravarCookieAcesso(objPessoaFisica);
        }
        #endregion

        #region LimparCookieAcesso
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieAcesso()
        {
            ((BasePage)this.Page).LimparCookieAcesso();
        }
        #endregion

        #region GravarCookieLoginVagas
        public void GravarCookieLoginVagas(PessoaFisica objPessoaFisica)
        {
            ((BasePage)this.Page).GravarCookieLoginVagas(objPessoaFisica);
        }
        #endregion

        #region LimparCookieLoginVagas
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparCookieLoginVagas()
        {
            ((BasePage)this.Page).LimparCookieLoginVagas();
        }
        #endregion

        #endregion

        #region LimparCookieTracker
        public void LimparCookieTracker()
        {
            HttpCookie c;

            var req = (HttpContext.Current != null ? HttpContext.Current.Request : Request);
            if (req.Cookies["TRACKER_CONTROLLER_SESSION_ID"] != null) // mvc
                c = req.Cookies["TRACKER_CONTROLLER_SESSION_ID"];
            else
            {
                var tempo = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CookieAcessoHorasExpiracao));

                c = new HttpCookie("TRACKER_CONTROLLER_SESSION_ID");
            }

            c.Expires = DateTime.Now.AddDays(-1d);
            (HttpContext.Current != null ? HttpContext.Current.Response : Response).Cookies.Add(c); // mvc
        }
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
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #region RecuperarCidade
        /// <summary>
        /// Recuperar cidade
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            Cidade objCidade;
            if (valor.LastIndexOf('/').Equals(-1))
                if (Cidade.CarregarPorNome(valor, out objCidade))
                    return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;

            return String.Empty;
        }
        #endregion

        #region CarregarCidade
        /// <summary>
        /// carregar Cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string CarregarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(valor, out objCidade))
                return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;

            return String.Empty;
        }
        #endregion



        #region ValidarFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            int? idOrigem = null;

            if (IdOrigem.HasValue)
                idOrigem = IdOrigem.Value;

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }
        #endregion

        #endregion

       
    }
}
