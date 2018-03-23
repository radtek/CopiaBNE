using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.UserControls.Modais;
using JSONSharp;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Curriculo = BNE.BLL.Curriculo;
using Enumeradores = BNE.BLL.Enumeradores;
using Filial = BNE.BLL.Filial;

namespace BNE.Web
{
    public partial class VisualizacaoCurriculo : BasePage
    {

        #region Propriedades

        #region IdCurriculoVisualizacaoCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int IdCurriculoVisualizacaoCurriculo
        {
            get
            {
                return Convert.ToInt32(RouteData.Values["Identificador"]);
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
            var objFilial = new Filial(base.IdFilial.Value);
            var objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
            string mensagemErro;
            int quantidadeSMS, quantidadeEmail;

            if (MensagemCS.EnviarChameFacil(objUsuarioFilialPerfil, objFilial, new List<Curriculo> { new Curriculo(IdCurriculoVisualizacaoCurriculo) }, mensagem, mensagem, true, true, out mensagemErro, out quantidadeSMS, out quantidadeEmail))
            {
                pnlChameFacil.Visible = false;
                ucModalConfirmacao.PreencherCampos("Confirmação", "Sua solicitação de contato foi enviada para o candidato com sucesso!", string.Empty, false);
                ucModalConfirmacao.MostrarModal();
            }
            else
            {
                base.ExibirMensagem(mensagemErro, TipoMensagem.Erro);
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
            HttpCookie tel = RecuperarCookieChameFacil();

            Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
            var nomeUsuario = new PessoaFisica(base.IdPessoaFisicaLogada.Value).PrimeiroNome;

            UsuarioFilial objUsuarioFilial;
            if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
            {
                //Lendo telefone do cookie, se houver
                if (tel == null || String.IsNullOrEmpty(tel.Values["Convite"]))
                    lblConvite.Text = txtConvite.Text = String.Format("Tenho uma vaga de emprego para você. Ligue {0}. Fale comigo {1}", Helper.FormatarTelefone(objUsuarioFilial.NumeroDDDComercial, objUsuarioFilial.NumeroComercial), nomeUsuario);
                else
                    lblConvite.Text = txtConvite.Text = HttpUtility.UrlDecode(tel.Values["Convite"]);
            }
            else
            {
                if (tel == null || String.IsNullOrEmpty(tel.Values["Convite"]))
                    lblConvite.Text = txtConvite.Text = String.Format("Tenho uma vaga de emprego para você. Ligue {0}. Fale comigo {1}", Helper.FormatarTelefone(objFilial.NumeroDDDComercial, objFilial.NumeroComercial), nomeUsuario);
                else
                    lblConvite.Text = txtConvite.Text = HttpUtility.UrlDecode(tel.Values["Convite"]);
            }

            if (tel == null || String.IsNullOrEmpty(tel.Values["CONVOCACAO"]))
            {
                objFilial.Endereco.CompleteObject();
                lblConvocacao.Text = txtConvocacao.Text = String.Format("Temos uma vaga para você. Compareça {0}, {1}. {2}", objFilial.Endereco.DescricaoLogradouro, objFilial.Endereco.NumeroEndereco, nomeUsuario);
            }
            else
                lblConvocacao.Text = txtConvocacao.Text = HttpUtility.UrlDecode(tel.Values["CONVOCACAO"]);

            if (tel != null && !String.IsNullOrEmpty(tel.Values["LIVRE"]))
                txtLivre.Text = HttpUtility.UrlDecode(tel.Values["LIVRE"]);

            litNomeCandidatoConvite.Text = litNomeCandidatoConvocacao.Text = litNomeCandidatoLivre.Text = Curriculo.LoadObject(IdCurriculoVisualizacaoCurriculo).PessoaFisica.PrimeiroNome;
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
                    var objCurriculo = new BLL.Curriculo(IdCurriculoVisualizacaoCurriculo);

                    bool empresaEmAuditoria = objFilial.EmpresaEmAuditoria();

                    if (!empresaEmAuditoria)
                        AjustarPanelChameFacil();

                    CarregarAvaliacao();
                }
            }

            btnAlertar.Visible = mostrarAlertarBNE;
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
            ucVisualizacaoCurriculo.Inicializar(IdCurriculoVisualizacaoCurriculo);
            btnAlertar.Text = base.STC.Value ? "Currículo Desatualizado" : "Alertar BNE";
            AjustarBotoesEAvaliacao();
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.IdFilial.HasValue)
                {
                    Filial objFilial = Filial.LoadObject(this.IdFilial.Value);

                    if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.AguardandoPublicacao))
                    {
                        QuantidadeCurriculosVIPVisualizadosPelaEmpresa =
                            BLL.CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

                        if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
                        {
                            divBotoes.Visible = false;
                            //ExibirMensagem("O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.", TipoMensagem.Aviso);

                        }
                        else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
                        {
                            divBotoes.Visible = false;
                            //ExibirMensagem("O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.", TipoMensagem.Aviso);
                        }
                    }
                }

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
            GravarCookieChameFacil("CONVITE", txtConvite.Text);

            SalvarMensagemChameFacil(txtConvite.Text);
        }
        #endregion

        #region btlEnviarConvocacao_Click
        protected void btlEnviarConvocacao_Click(object sender, EventArgs e)
        {
            GravarCookieChameFacil("CONVOCACAO", txtConvocacao.Text);

            SalvarMensagemChameFacil(txtConvocacao.Text);
        }
        #endregion

        #region btlEnviarLivre_Click
        protected void btlEnviarLivre_Click(object sender, EventArgs e)
        {
            GravarCookieChameFacil("LIVRE", txtLivre.Text);

            SalvarMensagemChameFacil(txtLivre.Text);
        }
        #endregion

        #region btlUltima_Click
        protected void btlUltima_Click(object sender, EventArgs e)
        {
            GravarCookieChameFacil(string.Empty, txtUltima.Text);

            SalvarMensagemChameFacil(txtUltima.Text);
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

        #endregion

    }
}