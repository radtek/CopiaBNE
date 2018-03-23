using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.EnvioMensagens;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using JSONSharp;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using BNE.Web.Code.ViewStateObjects;
using Curriculo = BNE.BLL.Curriculo;
using Enumeradores = BNE.BLL.Enumeradores;
using Filial = BNE.BLL.Filial;

namespace BNE.Web
{
    public partial class VisualizacaoCurriculo : BasePage
    {

        public NavegacaoCurriculos NavegacaoCurriculos = new NavegacaoCurriculos();

        #region Propriedades

        #region IdCurriculoVisualizacaoCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int IdCurriculoVisualizacaoCurriculo
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region IdPesquisaCurriculo - Query String: IdPesquisaCurriculo
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdPesquisaCurriculo
        {
            get
            {
                if (Request.QueryString["idpesquisacurriculo"] != null)
                    return Int32.Parse(Request.QueryString["idpesquisacurriculo"]);
                return null;
            }
        }
        #endregion

        #region IdVaga - Query String: IdVaga
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdVaga
        {
            get
            {
                if (Request.QueryString["idvaga"] != null)
                    return Int32.Parse(Request.QueryString["idvaga"]);
                return null;
            }
        }
        #endregion

        #region IdRastreadorCurriculo - Query String: IdRastreadorCurriculo
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdRastreadorCurriculo
        {
            get
            {
                if (Request.QueryString["idrastreadorcurriculo"] != null)
                    return Int32.Parse(Request.QueryString["idrastreadorcurriculo"]);
                return null;
            }
        }
        #endregion

        #region QuantidadeCurriculosVIPVisualizadosPelaEmpresa - Variável 18
        /// <summary>
        /// Propriedade que armazena e recupera o total de visualizações de curriculos VIP
        /// </summary>
        private int? QuantidadeCurriculosVIPVisualizadosPelaEmpresa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel18.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel18.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel18.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel18.ToString());
            }
        }
        #endregion

        #region PalavraChavePesquisa
        /// <summary>
        /// Usado para transporta para a session e usar na visualização do cv aberto
        /// </summary>
        private string PalavraChavePesquisa
        {
            get
            {
                if (Session[Chave.Permanente.PalavraChavePesquisa.ToString()] != null)
                    return Session[Chave.Permanente.PalavraChavePesquisa.ToString()].ToString();
                return string.Empty;
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region SalvarMensagemChameFacil
        /// <summary>
        /// Salva a Mensagem do Chame Facil
        /// </summary>
        private void SalvarMensagemChameFacil(string mensagem)
        {
            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
            string mensagemObservacoes;

            //Criar Objeto da Campanha
            CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
            objCampanhaMensagem.DataDisparo = DateTime.Now;
            objCampanhaMensagem.DescricaomensagemEmail = mensagem;
            objCampanhaMensagem.DescricaomensagemSMS = mensagem;
            objCampanhaMensagem.FlagEnviaEmail = true;
            objCampanhaMensagem.FlagEnviaSMS = true;
            objCampanhaMensagem.UsuarioFilialPerfil = objUsuarioFilialPerfil;
            objCampanhaMensagem.Save();

            //Inserir destinatário
            CampanhaMensagemEnvios objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
            objCampanhaMensagemEnvios.Curriculo = new Curriculo(IdCurriculoVisualizacaoCurriculo);
            objCampanhaMensagemEnvios.CampanhaMensagem = objCampanhaMensagem;
            objCampanhaMensagemEnvios.Save();

            if (EnvioMensagens.EnviarMensagemCV(objCampanhaMensagem, new List<CampanhaMensagemEnvios> { objCampanhaMensagemEnvios }, out mensagemObservacoes))
            {
                pnlChameFacil.Visible = false;

                if (string.IsNullOrWhiteSpace(mensagemObservacoes))
                    ucModalConfirmacao.PreencherCampos("Confirmação", "Sua solicitação de contato foi enviada para o candidato com sucesso!", string.Empty, false);
                else
                    ucModalConfirmacao.PreencherCampos("Erro", mensagemObservacoes, string.Empty, false);

                ucModalConfirmacao.MostrarModal();
            }
            else
            {
                base.ExibirMensagem(mensagemObservacoes, TipoMensagem.Erro);
            }
        }
        #endregion

        #region ExibirMensagem
        /// <summary>
        /// Metodo responsável por exibir a mensage no rodapé do site.
        /// </summary>
        /// <param name="mensagem">string mensagem</param>
        /// <param name="tipo">tipo da mensagem</param>
        /// <param name="aumentarTamanhoPainelMensagem"></param>
        public new void ExibirMensagem(string mensagem, TipoMensagem tipo, bool aumentarTamanhoPainelMensagem = false)
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

        #region CarregarCamposChameFacil
        /// <summary>
        /// Carrega os campos do chame fácil
        /// </summary>
        private void CarregarCamposChameFacil()
        {
            Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
            var nomeUsuario = new PessoaFisica(base.IdPessoaFisicaLogada.Value).PrimeiroNome;

            var convite = RecuperarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "CONVITE");
            var convocacao = RecuperarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "CONVOCACAO");
            var livre = RecuperarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "LIVRE");

            UsuarioFilial objUsuarioFilial;
            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
            {
                //Lendo telefone do cookie, se houver
                if (string.IsNullOrWhiteSpace(convite))
                    lblConvite.Text = txtConvite.Text = String.Format("Tenho uma vaga de emprego para você. Ligue {0}. Fale comigo {1}", Helper.FormatarTelefone(objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial), nomeUsuario);
                else
                    lblConvite.Text = txtConvite.Text = HttpUtility.UrlDecode(convite);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(convite))
                    lblConvite.Text = txtConvite.Text = String.Format("Tenho uma vaga de emprego para você. Ligue {0}. Fale comigo {1}", Helper.FormatarTelefone(objFilial.NumeroDDDComercial, objFilial.NumeroComercial), nomeUsuario);
                else
                    lblConvite.Text = txtConvite.Text = HttpUtility.UrlDecode(convite);
            }

            if (string.IsNullOrWhiteSpace(convocacao))
            {
                objFilial.Endereco.CompleteObject();
                lblConvocacao.Text = txtConvocacao.Text = String.Format("Temos uma vaga para você. Compareça {0}, {1}. {2}", objFilial.Endereco.DescricaoLogradouro, objFilial.Endereco.NumeroEndereco, nomeUsuario);
            }
            else
                lblConvocacao.Text = txtConvocacao.Text = HttpUtility.UrlDecode(convocacao);

            if (!string.IsNullOrWhiteSpace(livre))
                txtLivre.Text = HttpUtility.UrlDecode(livre);

            litNomeCandidatoConvite.Text = litNomeCandidatoConvocacao.Text = litNomeCandidatoLivre.Text = Curriculo.LoadObject(IdCurriculoVisualizacaoCurriculo).PessoaFisica.PrimeiroNome;

            AtualizarSaldoSMS();
        }
        #endregion

        #region AtualizarSaldoSMS
        protected void AtualizarSaldoSMS()
        {
            if (CelularSelecionador.VerificaCelularEstaLiberadoParaTanque(base.IdUsuarioFilialPerfilLogadoEmpresa.Value))
                litSaldoSMS.Text = string.Format("<span class=\"badge\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Você tem disponível {0} SMS.\"><span>Você tem disponível {0} SMS.</span></span>", CelularSelecionador.RecuperarCotaDisponivel(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, base.IdFilial.Value));
            else
                litSaldoSMS.Text = string.Format("<span class=\"badge\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Você tem disponível {0} SMS.\"><span>Você tem disponível {0} SMS.</span></span>", new Filial(base.IdFilial.Value).SaldoSMS());

            upSaldoSMS.Update();
        }
        #endregion

        #region AjustarPanelChameFacil
        private void AjustarPanelChameFacil()
        {
            //Ajustando visualização da mensagem do chame fácil.
            UsuarioFilialPerfil objUsuarioFilialPerfilDestinatario;
            if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(new Curriculo(IdCurriculoVisualizacaoCurriculo))), out objUsuarioFilialPerfilDestinatario))
            {
                string mensagem;
                var dataEnvioMensagem = MensagemCS.RecuperarDataUltimaMensagemEnviadaFilial(new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value), objUsuarioFilialPerfilDestinatario, new TipoMensagemCS((int)Enumeradores.TipoMensagem.SMS), out mensagem);
                if (dataEnvioMensagem.HasValue)
                {
                    lblUltima.Text = txtUltima.Text = mensagem.EndsWith("www.bne.com.br") ? mensagem.Replace("www.bne.com.br", string.Empty).Trim() : mensagem.Trim();
                    pnlUltimaMensagem.Visible = true;
                    litUltimaMensagem.Text = dataEnvioMensagem.Value.ToString();
                }
            }

            CarregarCamposChameFacil();
            pnlChameFacil.Visible = true;

            upChameFacil.Update();
        }
        #endregion

        #region CarregarAvaliacao
        private void CarregarAvaliacao()
        {
            ucAvaliacaoCurriculo.Visible = true;
            ucAvaliacaoCurriculo.Inicializar(new Filial(base.IdFilial.Value), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value), new Curriculo(IdCurriculoVisualizacaoCurriculo));

            upAvaliacao.Update();
        }
        #endregion

        #region AjustarBotoesEAvaliacao
        private void AjustarBotoesEAvaliacao()
        {
            bool mostrarAlertarBNE = false;
            bool mostrarFechar = false;
            bool mostrarEnviarMensagem = false;
            bool mostrarAssociarVaga = false;
            bool mostrarImprimir = false;

            if (base.IdPessoaFisicaLogada.HasValue)
            {
                mostrarFechar = true;

                if (base.IdFilial.HasValue)
                {
                    mostrarEnviarMensagem = true;
                    mostrarAssociarVaga = true;
                    mostrarAlertarBNE = true;
                    mostrarImprimir = true;

                    var objFilial = new Filial(base.IdFilial.Value);

                    if (!objFilial.EmpresaEmAuditoria() && !objFilial.EmpresaBloqueada())
                        AjustarPanelChameFacil();

                    CarregarAvaliacao();
                }
            }

            btnSolicitar.Visible = btnAlertar.Visible = mostrarAlertarBNE;
            btnFechar.Visible = mostrarFechar;
            btnEnviarMensagemVisualizacao.Visible = mostrarEnviarMensagem;
            btnAssociar.Visible = mostrarAssociarVaga;
            btnImprimirVisualizacaoCurriculo.Visible = mostrarImprimir;

            upBotoes.Update();
        }
        #endregion

        #region IniciarVisualizacaoCurriculo
        public void IniciarVisualizacaoCurriculo()
        {
            ucVisualizacaoCurriculo.PalavraChavePesquisa = PalavraChavePesquisa;
            ucVisualizacaoCurriculo.Inicializar(IdCurriculoVisualizacaoCurriculo, IdPesquisaCurriculo, IdVaga, IdRastreadorCurriculo);
            btnAlertar.Text = base.STC.Value ? "Currículo Desatualizado" : "Denunciar Currículo";
            AjustarBotoesEAvaliacao();
        }
        #endregion

        #region AjustarBotoesNavegacao
        private void AjustarBotoesNavegacao()
        {
            btlNext.Visible = GetNext() > 0;
            btlPrevious.Visible = GetPrevious() > 0;
        }
        #endregion

        #region GetPrevious
        private int GetPrevious()
        {
            int retorno = -1;

            if (IdPesquisaCurriculo.HasValue)
                retorno = NavegacaoCurriculos.CurriculoAnterior(ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo, IdPesquisaCurriculo.Value, IdCurriculoVisualizacaoCurriculo);
            if (IdRastreadorCurriculo.HasValue)
                retorno = NavegacaoCurriculos.CurriculoAnterior(ResultadoPesquisaCurriculo.TipoPesquisa.Rastreador, IdRastreadorCurriculo.Value, IdCurriculoVisualizacaoCurriculo);
            if (IdVaga.HasValue)
                retorno = NavegacaoCurriculos.CurriculoAnterior(ResultadoPesquisaCurriculo.TipoPesquisa.Vaga, IdVaga.Value, IdCurriculoVisualizacaoCurriculo);

            return retorno;
        }
        #endregion

        #region GetNext
        private int GetNext()
        {
            int retorno = -1;

            if (IdPesquisaCurriculo.HasValue)
                retorno = NavegacaoCurriculos.ProximoCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Curriculo, IdPesquisaCurriculo.Value, IdCurriculoVisualizacaoCurriculo);
            if (IdRastreadorCurriculo.HasValue)
                retorno = NavegacaoCurriculos.ProximoCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Rastreador, IdRastreadorCurriculo.Value, IdCurriculoVisualizacaoCurriculo);
            if (IdVaga.HasValue)
                retorno = NavegacaoCurriculos.ProximoCurriculo(ResultadoPesquisaCurriculo.TipoPesquisa.Vaga, IdVaga.Value, IdCurriculoVisualizacaoCurriculo);

            if (retorno == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), this.GetType().ToString(), " if(window.opener != null){ window.opener.GoNext(callbackPaging);}", true);

            return retorno;
        }
        #endregion

        #region Redirecionar
        private void Redirecionar(string url)
        {
            if (IdPesquisaCurriculo.HasValue)
                url = string.Format("{0}?idpesquisacurriculo={1}", url, IdPesquisaCurriculo);
            if (IdRastreadorCurriculo.HasValue)
                url = string.Format("{0}?idrastreadorcurriculo={1}", url, IdRastreadorCurriculo);
            if (IdVaga.HasValue)
                url = string.Format("{0}?idvaga={1}", url, IdVaga);

            Response.Redirect(url);
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int dummy;
                if (Int32.TryParse(RouteData.Values["Identificador"].ToString(), out dummy))
                {
                    IdCurriculoVisualizacaoCurriculo = dummy;

                    /*Salvar visualização Curriculo*/
                    if (IdVaga.HasValue && base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                        VagaCandidato.SalvarVisualizacaoCandidato(IdCurriculoVisualizacaoCurriculo, IdVaga.Value, base.IdFilial.Value, base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                }

                if (this.IdFilial.HasValue)
                {
                    Filial objFilial = Filial.LoadObject(this.IdFilial.Value);

                    if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao))
                    {
                        QuantidadeCurriculosVIPVisualizadosPelaEmpresa = CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

                        if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
                        {
                            divBotoes.Visible = false;
                        }
                        else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
                        {
                            divBotoes.Visible = false;
                        }
                    }
                }

                AjustarBotoesNavegacao();
                IniciarVisualizacaoCurriculo();
            }

            ucAlertarBNE.AlertaEnviado += ucAlertarBNE_AlertaEnviado;
            ucAvaliacaoCurriculo.CurriculoAvaliado += ucAvaliacaoCurriculo_CurriculoAvaliado;
            ucEnvioMensagem.EnviarConfirmacao += ucEnvioMensagem_EnviarConfirmacao;
            ucVisualizacaoCurriculo.VerDadosContato += ucVisualizacaoCurriculo_VerDadosContato;
        }
        #endregion

        #region btlEnviarConvite_Click
        protected void btlEnviarConvite_Click(object sender, EventArgs e)
        {
            GravarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "CONVITE", txtConvite.Text);

            SalvarMensagemChameFacil(txtConvite.Text);

            AtualizarSaldoSMS();
        }
        #endregion

        #region btlEnviarConvocacao_Click
        protected void btlEnviarConvocacao_Click(object sender, EventArgs e)
        {
            GravarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "CONVOCACAO", txtConvocacao.Text);

            SalvarMensagemChameFacil(txtConvocacao.Text);

            AtualizarSaldoSMS();
        }
        #endregion

        #region btlEnviarLivre_Click
        protected void btlEnviarLivre_Click(object sender, EventArgs e)
        {
            GravarValorCookie(Enumeradores.Cookie.MensagemChameFacil, "LIVRE", txtLivre.Text);

            SalvarMensagemChameFacil(txtLivre.Text);

            AtualizarSaldoSMS();
        }
        #endregion

        #region btlUltima_Click
        protected void btlUltima_Click(object sender, EventArgs e)
        {
            SalvarMensagemChameFacil(txtUltima.Text);

            AtualizarSaldoSMS();
        }
        #endregion

        #region btnEnviarMensagemVisualizacao_Click
        protected void btnEnviarMensagemVisualizacao_Click(object sender, EventArgs e)
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && base.IdFilial.HasValue)
                {
                    var objFilial = new Filial(base.IdFilial.Value);
                    if (objFilial.EmpresaEmAuditoria())
                    {
                        ucEmpresaAguardandoPublicacao.MostrarModal();
                        return;
                    }

                    if (objFilial.EmpresaBloqueada())
                    {
                        ucEmpresaBloqueada.MostrarModal();
                        return;
                    }

                    ucEnvioMensagem.Curriculos = new List<int> { IdCurriculoVisualizacaoCurriculo };
                    ucEnvioMensagem.InicializarComponentes();
                }
                else
                {
                    ucModalConfirmacao.PreencherCampos(string.Empty, MensagemAviso._101001, false);
                    ucModalConfirmacao.MostrarModal();
                }
            }
            else
            {
                ucModalConfirmacao.PreencherCampos(string.Empty, "Por favor, faça o Login", false);
                ucModalConfirmacao.MostrarModal();
            }
        }
        #endregion

        #region btnAssociar_Click
        protected void btnAssociar_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
                ucAssociarCurriculoVaga.Inicializar(IdCurriculoVisualizacaoCurriculo);
        }
        #endregion

        #region btnAlertar_Click
        protected void btnAlertar_Click(object sender, EventArgs e)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                ucAlertarBNE.IdCurriculoAlertarBNE = IdCurriculoVisualizacaoCurriculo;
                ucAlertarBNE.AbrirModal();
            }
        }
        #endregion

        #region ucAlertarBNE_AlertaEnviado
        void ucAlertarBNE_AlertaEnviado()
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                ucModalConfirmacao.PreencherCampos("Sucesso", "Mensagem enviada com sucesso", string.Empty, false);
                ucModalConfirmacao.MostrarModal();
            }
        }
        #endregion

        #region ucAvaliacaoCurriculo_CurriculoAvaliado
        void ucAvaliacaoCurriculo_CurriculoAvaliado()
        {
            ucModalConfirmacao.PreencherCampos("Confirmação", "Currículo avaliado com sucesso!", false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #region ucEnvioMensagem_EnviarConfirmacao
        void ucEnvioMensagem_EnviarConfirmacao(string titulo, string mensagem, bool cliqueAqui)
        {
            var objFilial = new Filial(base.IdFilial.Value);
            if (!EmpresaBloqueada(objFilial))
            {
                ucModalConfirmacao.PreencherCampos(titulo, mensagem, cliqueAqui);
                ucModalConfirmacao.MostrarModal();
            }
        }
        #endregion

        #region ucVisualizacaoCurriculo_VerDadosContato
        void ucVisualizacaoCurriculo_VerDadosContato()
        {
            AjustarBotoesEAvaliacao();
        }
        #endregion

        #region btlPrevious_OnClick
        protected void btlPrevious_OnClick(object sender, EventArgs e)
        {
            string url = new Curriculo(GetPrevious()).URL();
            Redirecionar(url);
        }
        #endregion

        #region btlNext_OnClick
        protected void btlNext_OnClick(object sender, EventArgs e)
        {
            string url = new Curriculo(GetNext()).URL();
            Redirecionar(url);
        }
        #endregion

        #region btnAdjustPaging_OnClick
        protected void btnAdjustPaging_OnClick(object sender, EventArgs e)
        {
            AjustarBotoesNavegacao();
        }

        #endregion

        #endregion

        protected void btnSolicitar_Click(object sender, EventArgs e)
        {
            if (!base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue || !UsuarioFilialPerfil.ValidarTipoPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)Enumeradores.TipoPerfil.Empresa))
            {
                ExibirMensagem(MensagemAviso._101001, TipoMensagem.Aviso);
                return;
            }

            try
            {
                Curriculo.SolicitarAtualizacao(IdCurriculoVisualizacaoCurriculo, base.IdUsuarioFilialPerfilLogadoEmpresa.Value, base.IdFilial.Value);
                ucModalConfirmacao.PreencherCampos("Solicitação enviada com sucesso!", "Você será notificado quando o candidato atualizar o currículo", false, false);
                ucModalConfirmacao.MostrarModal();
            }
            catch (Exception)
            {
                ExibirMensagem("Ocorreu um erro ao enviar a solicitação.", TipoMensagem.Erro);
            }
        }
    }
}