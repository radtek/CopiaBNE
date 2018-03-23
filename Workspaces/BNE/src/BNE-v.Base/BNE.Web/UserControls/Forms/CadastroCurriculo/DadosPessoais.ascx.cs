using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.Services.Base.ProcessosAssincronos;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using BNE.Web.Resources;
using BNE.BLL.Security;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using System.Web.UI;
using BNE.BLL.Custom;
using System.Text;

namespace BNE.Web.UserControls.Forms.CadastroCurriculo
{
    public partial class DadosPessoais : BaseUserControl
    {

        #region Propriedades

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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

        #region IdExperienciaProfissional1 - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional1
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
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

        #region IdExperienciaProfissional2 - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional2
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional3 - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional3
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
            }
        }
        #endregion

        #region EstadoManutencao - Variavel5
        /// <summary>
        /// Propriedade que armazena e recupera um boolean indicando se o user controls está em estado de manutenção
        /// </summary>
        public bool EstadoManutencao
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

        #region EnumSituacaoCurriculo - Variavel6
        /// <summary>
        /// Propriedade que armazena e recupera um enum situação curriculo setado pelo usuário administrador no curriculo completo
        /// </summary>
        public Enumeradores.SituacaoCurriculo? EnumSituacaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (Enumeradores.SituacaoCurriculo)(ViewState[Chave.Temporaria.Variavel6.ToString()]);
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region MostrarDegustacaoCandidatura - Variavel7
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public bool? MostrarDegustacaoCandidatura
        {
            get
            {
                if (Session[Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString()] != null)
                    return Boolean.Parse(Session[Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    Session.Add(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString(), value);
                else
                    Session.Remove(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString());
            }
        }
        #endregion

        #region contadorExperienciaProfissional - variavel 8

        public int? contadorExperienciaProfissional
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel8.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel8.ToString());
            }
        }

        public static Dictionary<int, bool> GerenciarPanelsExperienciasProfissionais = new Dictionary<int, bool>();

        #endregion

        #region IdExperienciaProfissional6 - Variável 9
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional6
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel9.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel9.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel9.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel9.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional4 - Variável 10
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional4
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel10.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel10.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional5 - Variável 11
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional5
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel11.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel11.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel11.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel11.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissionalExcluir - Variável 12
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Experiência que será excluída.
        /// </summary>
        public int? IdExperienciaProfissionalExcluir
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel12.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel12.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel12.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel12.ToString());
            }
        }
        #endregion

        #region idPanelParaOcultar - Variável 13
        /// <summary>
        /// Propriedade que armazena e recupera o nome do panel com a experiência.
        /// </summary>
        public int? idPanelParaOcultar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel13.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel13.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel13.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel13.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional7 - Variável 14
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional7
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel14.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel14.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel14.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel14.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional8 - Variável 15
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional8
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel15.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel15.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel15.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel15.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional9 - Variável 16
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional9
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel16.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel16.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel16.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel16.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional10 - Variável 17
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdExperienciaProfissional10
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel17.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel17.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel17.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel17.ToString());
            }
        }
        #endregion

        #region DadosNavegador - Variável 18
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public string strDadosNavegador
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel18.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel18.ToString()].ToString();
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

        #region AlertaEmailExperienciaProfissionalEnviado - Salva na session por causa da navegação entre as paginas
        /// <summary>
        /// Propriedade que armazena um valor boolean para indicar se o email foi enviado.
        /// </summary>
        public bool? AlertaEmailExperienciaProfissionalEnviado
        {
            get
            {
                return Convert.ToBoolean(Session["AlertaEmailExperienciaProfissionalEnviado"]);
            }
            set
            {
                Session["AlertaEmailExperienciaProfissionalEnviado"] = value;
            }
        }
        #endregion

        #region AlertaSMSExperienciaProfissionalEnviado - Salva na session por causa da navegação entre as paginas
        /// <summary>
        /// Propriedade que armazena um valor boolean para indicar se o SMS foi enviado.
        /// </summary>
        public bool? AlertaSMSExperienciaProfissionalEnviado
        {
            get
            {
                return Convert.ToBoolean(Session["AlertaSMSExperienciaProfissionalEnviado"]);
            }
            set
            {
                Session["AlertaSMSExperienciaProfissionalEnviado"] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //DesabilitarBotaoAposClick(this.Page, btnSalvar);

            if (!Page.IsPostBack)
                Inicializar();

                System.Web.HttpBrowserCapabilities browser = Request.Browser;

                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("Type:{0} -", browser.Type);
                sb.AppendFormat("Name:{0} - ", browser.Browser);
                sb.AppendFormat("Version:{0} - ", browser.Version);
                sb.AppendFormat("Major:{0} - ", browser.MajorVersion);
                sb.AppendFormat("Minor:{0} - ", browser.MinorVersion);
                sb.AppendFormat("Platform:{0} - ", browser.Platform);
                sb.AppendFormat("Suporta cookies:{0} - ", browser.Cookies);
                sb.AppendFormat("Suporta JS:{0} - ", browser.EcmaScriptVersion.ToString());

                strDadosNavegador = sb.ToString();

                //Ajustando a expressao de validacao.
                //revMSN.ValidationExpression = Configuracao.regexEmail;

                if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                    InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "DadosPessoais");
                else
                    InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "DadosPessoais");

                Ajax.Utility.RegisterTypeForAjax(typeof(DadosPessoais));
            }
        #endregion

        protected void btnInvoke_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
            //lblText.Text = "Atualizado com sucesso!";

            if (Salvar())
            {
                Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisica.Value);

                Redirect("~/CadastroCurriculoFormacao.aspx");
            }
        }

        #region btnSalvar
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Salvar())
                {
                    Session.Add(Chave.Temporaria.Variavel1.ToString(), IdPessoaFisica.Value);

                    Redirect("~/CadastroCurriculoFormacao.aspx");
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion    

        private void OcultarDivsAvisoDtaDemissao()
        {
            divDtaDemissaoAviso4.Visible = false;
            divDtaDemissaoAviso5.Visible = false;
            divDtaDemissaoAviso6.Visible = false;
            divDtaDemissaoAviso7.Visible = false;
            divDtaDemissaoAviso8.Visible = false;
            divDtaDemissaoAviso9.Visible = false;
            divDtaDemissaoAviso10.Visible = false;
        }

        #region btnAddExperiencia
        /// <summary>
        /// Evento disparado no click do btnAddExperiencia
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnAddExperiencia_Click(object sender, EventArgs e)
        {
            OcultarDivsAvisoDtaDemissao();

            switch (contadorExperienciaProfissional)
            { 
                case 5:
                    pnExperienciaProfissional5.Visible = true;
                    lblTituloExperienciaProfissionalAdicional5.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao5.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao5_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao5_txtValor','divDtaDemissaoAviso5');";
                    divDtaDemissaoAviso5.Visible = false;
                    divDtaDemissaoAviso4.Visible = false;
                    break;
                case 6:
                    pnExperienciaProfissional6.Visible = true;
                    lblTituloExperienciaProfissionalAdicional6.Text = "Experiência Profissional";
                    lblAvisoApos6Experiencia.Text = "Para uma boa apresentação do seu currículo, os recrutadores terão acesso as últimas 5 experiências cadastradas no currículo, as demais experiências aparecerão no campo observações, no final de currículo.";
                    mpeAvisoApos6Experiencia.Show();
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao6.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao6_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao6_txtValor','divDtaDemissaoAviso6');";
                    divDtaDemissaoAviso6.Visible = false;
                    divDtaDemissaoAviso5.Visible = false;
                    divDtaDemissaoAviso4.Visible = false;
                    break;
                case 7:
                    pnExperienciaProfissional7.Visible = true;
                    lblTituloExperienciaProfissionalAdicional7.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao7.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao7_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao7_txtValor','divDtaDemissaoAviso7');";
                    divDtaDemissaoAviso7.Visible = false;
                    break;
                case 8:
                    pnExperienciaProfissional8.Visible = true;
                    lblTituloExperienciaProfissionalAdicional8.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao8.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao8_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao8_txtValor','divDtaDemissaoAviso8');";
                    divDtaDemissaoAviso8.Visible = false;
                    break;
                case 9:
                    pnExperienciaProfissional9.Visible = true;
                    lblTituloExperienciaProfissionalAdicional9.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao9.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao9_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao9_txtValor','divDtaDemissaoAviso9');";
                    divDtaDemissaoAviso9.Visible = false;
                    break;
                case 10:
                    pnExperienciaProfissional10.Visible = true;
                    lblTituloExperienciaProfissionalAdicional10.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao10.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao10_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao10_txtValor','divDtaDemissaoAviso10');";
                    divDtaDemissaoAviso10.Visible = false;
                    break;
                default:
                    pnExperienciaProfissional4.Visible = true;
                    lblTituloExperienciaProfissionalAdicional4.Text = "Experiência Profissional";
                    contadorExperienciaProfissional++;
                    ChecarGraficoQualidade();
                    txtDataDemissao4.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao4_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao4_txtValor','divDtaDemissaoAviso4');";
                    divDtaDemissaoAviso4.Visible = false;
                    break;
            }
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region Excluir Experiência Profissional

        #region btnExcluir Experiência Profissional 4
        protected void btnExcluirExp4_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional4 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional4.Value;
            idPanelParaOcultar = 4;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa4.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else
            {
                pnExperienciaProfissional4.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 5
        protected void btnExcluirExp5_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional5 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional5.Value;
            idPanelParaOcultar = 5;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa5.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional5.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 6
        protected void btnExcluirExp6_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional6 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional6.Value;
            idPanelParaOcultar = 6;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa6.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional6.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 7
        protected void btnExcluirExp7_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional7 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional7.Value;
            idPanelParaOcultar = 7;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa7.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional7.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 8
        protected void btnExcluirExp8_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional8 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional8;
            idPanelParaOcultar = 8;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa8.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional8.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 9
        protected void btnExcluirExp9_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional9 != null)
            {
            IdExperienciaProfissionalExcluir = IdExperienciaProfissional9;
            idPanelParaOcultar = 9;
            lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa9.Text);
            mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional9.Visible = false;
            }
        }
        #endregion

        #region btnExcluir Experiência Profissional 10
        protected void btnExcluirExp10_Click(object sender, EventArgs e)
        {
            if (IdExperienciaProfissional10 != null)
            {
                IdExperienciaProfissionalExcluir = IdExperienciaProfissional10;
                idPanelParaOcultar = 10;
                lblConfirmacaoExclusao.Text = string.Format("Tem certeza que deseja excluir a experiência {0}?", txtEmpresa10.Text);
                mpeConfirmacaoExclusao.Show();
            }
            else {
                pnExperienciaProfissional10.Visible = false;
            }
        }
        #endregion

        #endregion

        #region Modal

        #region btnExcluirModalExclusao_Click
        /// <summary>
        /// Executa os procedimentos de exclusão do usuário selecionado e fecha a modal de confirmação de exclusão.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcluirModalExclusao_Click(object sender, EventArgs e)
        {
            ExcluirExperienciaProfissional(IdExperienciaProfissionalExcluir, idPanelParaOcultar);
            mpeConfirmacaoExclusao.Hide();
        }
        #endregion

        #region btnCancelarModalExclusao
        /// <summary>
        /// Cancela a excluão do usuário e fecha a modal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarModalExclusao_Click(object sender, EventArgs e)
        {
            mpeConfirmacaoExclusao.Hide();
            btnSalvar.Enabled = true;
        }
        #endregion

        #region btnFinalizar_Click
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #endregion

        #region btnCancelar
        /// <summary>
        /// Evento disparado no click do btnCancelar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
        #endregion

        #region txtNumeroRG_ValorAlterado
        protected void txtNumeroRG_ValorAlterado(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumeroRG.Valor))
            {
                txtOrgaoEmissorRG.Enabled = true;
                txtOrgaoEmissorRG.Focus();
            }
            else
            {
                txtOrgaoEmissorRG.Valor = string.Empty;
                txtOrgaoEmissorRG.Enabled = true;
                txtOrgaoEmissorRG.Enabled = false;
                ddlEstadoCivil.Focus();
            }
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
            if (EstadoManutencao)
            {
                pnlBotoes.Visible = false;
                pnlAbas.Visible = false;
                litTitulo.Text = "Dados Pessoais e Profissionais";
            }

            UIHelper.CarregarDropDownList(ddlEstadoCivil, EstadoCivil.Listar(), new ListItem("Selecione", "0"));
            //Carregar drop down da experiencia profissional
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa1, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa2, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa3, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa4, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa5, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa6, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa7, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa8, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa9, AreaBNE.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlAtividadeEmpresa10, AreaBNE.Listar(), new ListItem("Selecione", "0"));

            
            txtFuncaoExercida1.Attributes["OnChange"] += "txtFuncaoExercida1_TextChanged(this);";
            txtFuncaoExercida2.Attributes["OnChange"] += "txtFuncaoExercida2_TextChanged(this);";
            txtFuncaoExercida3.Attributes["OnChange"] += "txtFuncaoExercida3_TextChanged(this);";
            txtFuncaoExercida4.Attributes["OnChange"] += "txtFuncaoExercida4_TextChanged(this);";
            txtFuncaoExercida5.Attributes["OnChange"] += "txtFuncaoExercida5_TextChanged(this);";
            txtFuncaoExercida6.Attributes["OnChange"] += "txtFuncaoExercida6_TextChanged(this);";
            txtFuncaoExercida7.Attributes["OnChange"] += "txtFuncaoExercida7_TextChanged(this);";
            txtFuncaoExercida8.Attributes["OnChange"] += "txtFuncaoExercida8_TextChanged(this);";
            txtFuncaoExercida9.Attributes["OnChange"] += "txtFuncaoExercida9_TextChanged(this);";
            txtFuncaoExercida10.Attributes["OnChange"] += "txtFuncaoExercida10_TextChanged(this);";

            CarregarParametros();
           
            txtUltimoSalario.MensagemErroIntervalo = string.Format("Sua pretensão salarial deve ser maior que o Salário Mínimo Nacional R$ {0}", txtUltimoSalario.ValorMinimo);

            btlGestao.Visible = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            LimparCampos();
            PreencherCampos();

            VerificarObrigatoriedadeCamposExperiencia();

            ucEndereco.Inicializar(btnSalvar.ValidationGroup);

            var dataMinima = new DateTime(1900, 01, 01);
            var dataMaxima = new DateTime(3000, 12, 31);

            txtDataAdmissao1.DataMinima = txtDataAdmissao2.DataMinima = txtDataAdmissao3.DataMinima = txtDataAdmissao4.DataMinima = txtDataAdmissao5.DataMinima = txtDataAdmissao6.DataMinima = txtDataAdmissao7.DataMinima = txtDataAdmissao8.DataMinima = txtDataAdmissao9.DataMinima = txtDataAdmissao10.DataMinima = dataMinima;
            txtDataDemissao1.DataMinima = txtDataDemissao2.DataMinima = txtDataDemissao3.DataMinima = txtDataDemissao4.DataMinima = txtDataDemissao5.DataMinima = txtDataDemissao6.DataMinima = txtDataDemissao7.DataMinima = txtDataDemissao8.DataMinima = txtDataDemissao9.DataMinima = txtDataDemissao10.DataMinima = dataMinima;
            txtDataAdmissao1.DataMaxima = txtDataAdmissao2.DataMaxima = txtDataAdmissao3.DataMaxima = txtDataAdmissao4.DataMaxima = txtDataAdmissao5.DataMaxima = txtDataAdmissao6.DataMaxima = txtDataAdmissao7.DataMaxima = txtDataAdmissao8.DataMaxima = txtDataAdmissao9.DataMaxima = txtDataAdmissao10.DataMaxima = dataMaxima;
            txtDataDemissao1.DataMaxima = txtDataDemissao2.DataMaxima = txtDataDemissao3.DataMaxima = txtDataDemissao4.DataMaxima = txtDataDemissao5.DataMaxima = txtDataDemissao6.DataMaxima = txtDataDemissao7.DataMaxima = txtDataDemissao8.DataMaxima = txtDataDemissao9.DataMaxima = txtDataDemissao10.DataMaxima = dataMaxima;

            if (MostrarDegustacaoCandidatura.HasValue && MostrarDegustacaoCandidatura.Value)
                AbrirModalDegustacaoCandidatura();

            UIHelper.ValidateFocus(btnSalvar);
            UIHelper.ValidateFocus(btlDadosComplementares);
            UIHelper.ValidateFocus(btlFormacaoCursos);
            UIHelper.ValidateFocus(btlMiniCurriculo);

            //validar data demissão
            divDtaDemissaoAviso1.Visible = false;
            divDtaDemissaoAviso2.Visible = false;
            divDtaDemissaoAviso3.Visible = false;
            divDtaDemissaoAviso4.Visible = false;
            divDtaDemissaoAviso5.Visible = false;
            divDtaDemissaoAviso6.Visible = false;
            divDtaDemissaoAviso7.Visible = false;
            divDtaDemissaoAviso8.Visible = false;
            divDtaDemissaoAviso9.Visible = false;
            divDtaDemissaoAviso10.Visible = false;

            txtDataDemissao1.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao1_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao1_txtValor','divDtaDemissaoAviso1');";
            txtDataDemissao2.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao2_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao2_txtValor','divDtaDemissaoAviso2');";
            txtDataDemissao3.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao3_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao3_txtValor','divDtaDemissaoAviso3');";
            txtDataDemissao4.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao4_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao4_txtValor','divDtaDemissaoAviso4');";
            txtDataDemissao5.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao5_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao5_txtValor','divDtaDemissaoAviso5');";
            txtDataDemissao6.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao6_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao6_txtValor','divDtaDemissaoAviso6');";
            txtDataDemissao7.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao7_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao7_txtValor','divDtaDemissaoAviso7');";
            txtDataDemissao8.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao8_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao8_txtValor','divDtaDemissaoAviso8');";
            txtDataDemissao9.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao9_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao9_txtValor','divDtaDemissaoAviso9');";
            txtDataDemissao10.OnBlurClient += "ValidarDataDemissao('cphConteudo_ucDadosPessoais_txtDataAdmissao10_txtValor','cphConteudo_ucDadosPessoais_txtDataDemissao10_txtValor','divDtaDemissaoAviso10');";

        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCampos()
        {
            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                txtNumeroRG.Valor = objPessoaFisica.NumeroRG;
                txtOrgaoEmissorRG.Valor = objPessoaFisica.NomeOrgaoEmissor;

                if (objPessoaFisica.EstadoCivil != null)
                    ddlEstadoCivil.SelectedValue = objPessoaFisica.EstadoCivil.IdEstadoCivil.ToString(CultureInfo.CurrentCulture);

                if (objPessoaFisica.Endereco != null)
                {
                    objPessoaFisica.Endereco.CompleteObject();
                    objPessoaFisica.Endereco.Cidade.CompleteObject();

                    string numeroCEP = objPessoaFisica.Endereco.NumeroCEP;
                    string descricaoLogradouro = objPessoaFisica.Endereco.DescricaoLogradouro;
                    string numeroEndereco = objPessoaFisica.Endereco.NumeroEndereco;
                    string descricaoComplemento = objPessoaFisica.Endereco.DescricaoComplemento;
                    string nomeBairro = objPessoaFisica.Endereco.DescricaoBairro;
                    string nomeCidade = objPessoaFisica.Endereco.Cidade.NomeCidade;
                    string siglaEstado = objPessoaFisica.Endereco.Cidade.Estado.SiglaEstado;

                    ucEndereco.PreencherCampos(numeroCEP, descricaoLogradouro, numeroEndereco, descricaoComplemento, nomeBairro, nomeCidade, siglaEstado);
                }

                txtCelular.DDD = objPessoaFisica.NumeroDDDCelular;
                txtCelular.Fone = objPessoaFisica.NumeroCelular;
                txtTelefoneResidencial.DDD = objPessoaFisica.NumeroDDDTelefone;
                txtTelefoneResidencial.Fone = objPessoaFisica.NumeroTelefone;

                PessoaFisicaComplemento objPessoaFisicaComplemento;
                if (PessoaFisicaComplemento.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
                {
                    Contato objContatoTelefone;
                    if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
                    {
                        txtTelefoneRecado.DDD = objContatoTelefone.NumeroDDDTelefone;
                        txtTelefoneRecado.Fone = objContatoTelefone.NumeroTelefone;
                        txtTelefoneRecadoFalarCom.Text = objContatoTelefone.NomeContato;
                    }
                    Contato objContatoCelular;
                    if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisicaComplemento.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null))
                    {
                        txtCelularRecado.DDD = objContatoCelular.NumeroDDDCelular;
                        txtCelularRecado.Fone = objContatoCelular.NumeroCelular;
                        txtCelularRecadoFalarCom.Text = objContatoCelular.NomeContato;
                    }
                }

                //Horario e Salário
                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                    txtUltimoSalario.Valor = objCurriculo.ValorUltimoSalario;

                //Experiencia
                List<int> listExp = objPessoaFisica.RecuperarExperienciaProfissional(null);

                if (listExp.Count >= 1)
                {
                    IdExperienciaProfissional1 = listExp[0];
                    PreencherCamposExp1();
                }

                if (listExp.Count >= 2)
                {
                    //preenche campos segunda experiencia
                    IdExperienciaProfissional2 = listExp[1];
                    PreencherCamposExp2();
                }

                if (listExp.Count >= 3)
                {
                    IdExperienciaProfissional3 = listExp[2];
                    PreencherCamposExp3();
                }
                if (listExp.Count >= 4)
                {
                    IdExperienciaProfissional4 = listExp[3];
                    PreencherCamposExp4();
                }
                if (listExp.Count >= 5)
                {
                    IdExperienciaProfissional5 = listExp[4];
                    PreencherCamposExp5();
                }
                if (listExp.Count >= 6)
                {
                    IdExperienciaProfissional6 = listExp[5];
                    PreencherCamposExp6();
                }
                if (listExp.Count >= 7)
                {
                    IdExperienciaProfissional7 = listExp[6];
                    PreencherCamposExp7();
                }
                if (listExp.Count >= 8)
                {
                    IdExperienciaProfissional8 = listExp[7];
                    PreencherCamposExp8();
                }
                if (listExp.Count >= 9)
                {
                    IdExperienciaProfissional9 = listExp[8];
                    PreencherCamposExp9();
                }
                if (listExp.Count >= 10)
                {
                    IdExperienciaProfissional10 = listExp[9];
                    PreencherCamposExp10();
                }

                contadorExperienciaProfissional = listExp.Count < 4 ? 4 : listExp.Count+1;

            }
        }
        #endregion

        #region PreencherCamposExp1
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp1()
        {
            try
            {
            if (IdExperienciaProfissional1.HasValue)
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional1.Value);
                txtEmpresa1.Text = objExperienciaProfissional.RazaoSocial;

                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa1.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);

                txtDataAdmissao1.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                if (objExperienciaProfissional.DataDemissao.HasValue)
                    txtDataDemissao1.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida1.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas1.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida1.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if(objExperienciaProfissional.VlrSalario.Value >0)
                txtUltimoSalario.Valor = objExperienciaProfissional.VlrSalario;
               
                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                txtAtividadeExercida1.Valor = objExperienciaProfissional.DescricaoAtividade;

                if(!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade1);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade1);

            }
        }
            catch (Exception ex)
            {
                string customMessage = "BNE.WEB - Experiencia Profissional - erro ao carregar a experiência";
                EL.GerenciadorException.GravarExcecao(ex, customMessage);
            }
        }
        #endregion

        #region PreencherCamposExp2
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp2()
        {
            if (IdExperienciaProfissional2.HasValue)
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional2.Value);
                txtEmpresa2.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa2.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao2.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                if (objExperienciaProfissional.DataDemissao.HasValue)
                    txtDataDemissao2.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida2.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas2.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida2.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario2.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida2.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade2);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade2);

            }
        }
        #endregion

        #region PreencherCamposExp3
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp3()
        {
            if (IdExperienciaProfissional3.HasValue)
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional3.Value);
                txtEmpresa3.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa3.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao3.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao3.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida3.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas3.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida3.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario3.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida3.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade3);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade3);
            }
        }
        #endregion

        #region PreencherCamposExp4
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp4()
        {
            if (IdExperienciaProfissional4.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional4.Visible = true;

                //atualizar contato
                contadorExperienciaProfissional = 5;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional4.Value);
                txtEmpresa4.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa4.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao4.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao4.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida4.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas4.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida4.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario4.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida4.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade4);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade4);
            }
        }
        #endregion

        #region PreencherCamposExp5
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp5()
        {
            if (IdExperienciaProfissional5.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional5.Visible = true;

                //atualizar contato
                contadorExperienciaProfissional = 6;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional5.Value);
                txtEmpresa5.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa5.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao5.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao5.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida5.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas5.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida5.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario5.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida5.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade5);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade5);
            }
            }
        #endregion

        #region PreencherCamposExp6
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp6()
        {
            if (IdExperienciaProfissional6.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional6.Visible = true;

                //incrementar contador de paineis Experiência Profissional
                contadorExperienciaProfissional = 7;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional6.Value);
                txtEmpresa6.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa6.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao6.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao6.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida6.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas6.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida6.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario6.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida6.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade6);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade6);
            }
        }
        #endregion

        #region PreencherCamposExp7
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp7()
        {
            if (IdExperienciaProfissional7.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional7.Visible = true;

                //incrementar contador de paineis Experiência Profissional
                contadorExperienciaProfissional = 8;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional7.Value);
                txtEmpresa7.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa7.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao7.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao7.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida7.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas7.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida7.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario7.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida7.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade7);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade7);
            }
        }
        #endregion

        #region PreencherCamposExp8
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp8()
        {
            if (IdExperienciaProfissional8.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional8.Visible = true;

                //incrementar contador de paineis Experiência Profissional
                contadorExperienciaProfissional = 9;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional8.Value);
                txtEmpresa8.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa8.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao8.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao8.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida8.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas8.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida8.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario8.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida8.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade8);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade8);
            }
        }
        #endregion

        #region PreencherCamposExp9
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp9()
        {
            if (IdExperienciaProfissional9.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional9.Visible = true;

                //incrementar contador de paineis Experiência Profissional
                contadorExperienciaProfissional = 10;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional9.Value);
                txtEmpresa9.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa9.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao9.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao9.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida9.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas9.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida9.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario9.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida9.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade9);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade9);
            }
        }
        #endregion

        #region PreencherCamposExp10
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        public void PreencherCamposExp10()
        {
            if (IdExperienciaProfissional10.HasValue)
            {
                //Mostrar o painel
                pnExperienciaProfissional10.Visible = true;

                //incrementar contador de paineis Experiência Profissional
                contadorExperienciaProfissional = 11;

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional10.Value);
                txtEmpresa10.Text = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                    ddlAtividadeEmpresa10.SelectedValue = objExperienciaProfissional.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                txtDataAdmissao10.Valor = objExperienciaProfissional.DataAdmissao.ToString(Configuracao.FormatoData);

                //txtDataDemissao3.DataMinima = txtDataAdmissao3.ValorDatetime.Value;

                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    txtDataDemissao10.Valor = objExperienciaProfissional.DataDemissao.Value.ToString(Configuracao.FormatoData);
                    //txtDataAdmissao3.DataMaxima = txtDataDemissao3.ValorDatetime.Value;
                }

                if (objExperienciaProfissional.Funcao != null)
                {
                    objExperienciaProfissional.Funcao.CompleteObject();
                    txtFuncaoExercida10.Text = objExperienciaProfissional.Funcao.DescricaoFuncao;
                    txtSugestaoTarefas10.Text = objExperienciaProfissional.Funcao.DescricaoJob;
                }
                else
                    txtFuncaoExercida10.Text = objExperienciaProfissional.DescricaoFuncaoExercida;

                if (objExperienciaProfissional.VlrSalario.Value > 0)
                    txtUltimoSalario10.Valor = objExperienciaProfissional.VlrSalario;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    txtAtividadeExercida10.Valor = objExperienciaProfissional.DescricaoAtividade;

                if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    MostrarGraficoQualidadeExperienciaProfissional(objExperienciaProfissional.DescricaoAtividade.Length, ltGraficoQualidade10);
                else
                    MostrarGraficoQualidadeExperienciaProfissional(0, ltGraficoQualidade10);
            }
        }
        #endregion

        #region ChecarGraficoQualidade
        private void ChecarGraficoQualidade()
        {
            if (!string.IsNullOrEmpty(txtAtividadeExercida4.Valor)) {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida4.Valor.Length, ltGraficoQualidade4);
            } if (!string.IsNullOrEmpty(txtAtividadeExercida5.Valor)) {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida5.Valor.Length, ltGraficoQualidade5);
            }
             if (!string.IsNullOrEmpty(txtAtividadeExercida6.Valor))
            {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida6.Valor.Length, ltGraficoQualidade6);
            }
             if (!string.IsNullOrEmpty(txtAtividadeExercida7.Valor))
            {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida7.Valor.Length, ltGraficoQualidade7);
            }
             if (!string.IsNullOrEmpty(txtAtividadeExercida8.Valor))
            {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida8.Valor.Length, ltGraficoQualidade8);
            }
             if (!string.IsNullOrEmpty(txtAtividadeExercida9.Valor))
            {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida9.Valor.Length, ltGraficoQualidade9);
            }
             if (!string.IsNullOrEmpty(txtAtividadeExercida10.Valor))
            {
                MostrarGraficoQualidadeExperienciaProfissional(txtAtividadeExercida10.Valor.Length, ltGraficoQualidade10);
            }
        }
        #endregion

        #region MostrarGraficoQualidadeExperienciaProfissional
        public void MostrarGraficoQualidadeExperienciaProfissional(int valor, Literal ltdestino)
        {
            if (valor > 0 && valor < 70)
            {
                ltdestino.Text = "<div class='icon icon-fraco icon-size'></br><span class='labelIcon'>FRACO</span></div>";
            }
            else if (valor > 70 && valor <= 140)
            {
                ltdestino.Text = "<div class='icon icon-regular icon-size'></br><span class='labelIcon'>REGULAR</label></div>";
            }
            else
            {
                ltdestino.Text = "<div class='icon icon-otimo icon-size'></br><span class='labelIcon'>ÓTIMO</span></div>";
            }
        }

        #endregion

        #region AbrirModalDegustacaoCandidatura
        private void AbrirModalDegustacaoCandidatura()
        {
            ucModalDegustacaoCandidatura.Inicializar(true, true, false, null);
            MostrarDegustacaoCandidatura = null;
        }
        #endregion

        #region Excluir Experiencia Profissional
        public void ExcluirExperienciaProfissional(int? idExperiencia, int? idPanelOcultar)
        {
            try
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperiencia.Value);
                objExperienciaProfissional.FlagInativo = true;
                objExperienciaProfissional.Save();

                base.ExibirMensagemConfirmacao("Confirmação de Exclusão", "Experiência excluída com sucesso!", false);

                switch (idPanelParaOcultar)
                {
                    case 4:
                        pnExperienciaProfissional4.Visible = false;
                        LimparCamposExp4();
                        break;
                    case 5:
                        pnExperienciaProfissional5.Visible = false;
                        LimparCamposExp5();
                        break;
                    case 6:
                        pnExperienciaProfissional6.Visible = false;
                        LimparCamposExp6();
                        break;
                    case 7:
                        pnExperienciaProfissional7.Visible = false;
                        LimparCamposExp7();
                        break;
                    case 8:
                        pnExperienciaProfissional8.Visible = false;
                        LimparCamposExp8();
                        break;
                    case 9:
                        pnExperienciaProfissional9.Visible = false;
                        LimparCamposExp9();
                        break;
                    case 10:
                        pnExperienciaProfissional10.Visible = false;
                        LimparCamposExp10();
                        break;
                }    
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        private void LimparCampos()
        {
            //Painel Dados
            txtNumeroRG.Valor = String.Empty;
            txtOrgaoEmissorRG.Valor = String.Empty;
            ucEndereco.LimparCampos();
            txtCelular.DDD = String.Empty;
            txtCelular.Fone = String.Empty;
            txtTelefoneResidencial.DDD = String.Empty;
            txtTelefoneResidencial.Fone = String.Empty;
            txtTelefoneRecado.DDD = String.Empty;
            txtTelefoneRecado.Fone = String.Empty;
            txtTelefoneRecadoFalarCom.Text = String.Empty;
            txtCelularRecado.DDD = String.Empty;
            txtCelularRecado.Fone = String.Empty;
            txtCelularRecadoFalarCom.Text = String.Empty;

            //Horario e Salario
            txtUltimoSalario.Valor = null;

            //Experiencia
            LimparCamposExp1();
            LimparCamposExp2();
            LimparCamposExp3();
            LimparCamposExp4();
            LimparCamposExp5();
            LimparCamposExp6();
        }
        #endregion

        #region LimparCamposExp1
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp1()
        {
            txtEmpresa1.Text = String.Empty;
            ddlAtividadeEmpresa1.SelectedIndex = 0;
            txtDataAdmissao1.Valor = String.Empty;
            txtDataDemissao1.Valor = String.Empty;
            txtFuncaoExercida1.Text = String.Empty;
            txtAtividadeExercida1.Valor = String.Empty;
            txtSugestaoTarefas1.Text = String.Empty;
            ltGraficoQualidade1.Text = "";
            txtUltimoSalario5.Valor = null;
            //upExperienciaProfissional1.Update();
        }
        #endregion

        #region LimparCamposExp2
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp2()
        {
            txtEmpresa2.Text = String.Empty;
            ddlAtividadeEmpresa2.SelectedIndex = 0;
            txtDataAdmissao2.Valor = String.Empty;
            txtDataDemissao2.Valor = String.Empty;
            txtFuncaoExercida2.Text = String.Empty;
            txtAtividadeExercida2.Valor = String.Empty;
            txtSugestaoTarefas2.Text = String.Empty;
            ltGraficoQualidade2.Text = "";
            txtUltimoSalario5.Valor = null;
        }
        #endregion

        #region LimparCamposExp3
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp3()
        {
            txtEmpresa3.Text = String.Empty;
            ddlAtividadeEmpresa3.SelectedIndex = 0;
            txtDataAdmissao3.Valor = String.Empty;
            txtDataDemissao3.Valor = String.Empty;
            txtFuncaoExercida3.Text = String.Empty;
            txtAtividadeExercida3.Valor = String.Empty;
            txtSugestaoTarefas3.Text = String.Empty;
            ltGraficoQualidade3.Text = "";
            txtUltimoSalario5.Valor = null;
            //upExperienciaProfissional3.Update();
        }
        #endregion

        #region LimparCamposExp4
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp4()
        {
            txtEmpresa4.Text = String.Empty;
            ddlAtividadeEmpresa4.SelectedIndex = 0;
            txtDataAdmissao4.Valor = String.Empty;
            txtDataDemissao4.Valor = String.Empty;
            txtFuncaoExercida4.Text = String.Empty;
            txtAtividadeExercida4.Valor = String.Empty;
            txtSugestaoTarefas4.Text = String.Empty;
            txtUltimoSalario4.Valor = null;
            ltGraficoQualidade4.Text = "";
            IdExperienciaProfissional4 = null;
        }
        #endregion

        #region LimparCamposExp5
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp5()
        {
            txtEmpresa5.Text = String.Empty;
            ddlAtividadeEmpresa5.SelectedIndex = 0;
            txtDataAdmissao5.Valor = String.Empty;
            txtDataDemissao5.Valor = String.Empty;
            txtFuncaoExercida5.Text = String.Empty;
            txtAtividadeExercida5.Valor = String.Empty;
            txtSugestaoTarefas5.Text = String.Empty;
            txtUltimoSalario5.Valor = null;
            ltGraficoQualidade5.Text = "";
            IdExperienciaProfissional5 = null;
        }
        #endregion

        #region LimparCamposExp6
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp6()
        {
            txtEmpresa6.Text = String.Empty;
            ddlAtividadeEmpresa6.SelectedIndex = 0;
            txtDataAdmissao6.Valor = String.Empty;
            txtDataDemissao6.Valor = String.Empty;
            txtFuncaoExercida6.Text = String.Empty;
            txtAtividadeExercida6.Valor = String.Empty;
            txtSugestaoTarefas6.Text = String.Empty;
            txtUltimoSalario6.Valor = null;
            ltGraficoQualidade6.Text = "";
            IdExperienciaProfissional6 = null;
        }
        #endregion

        #region LimparCamposExp7
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp7()
        {
            txtEmpresa7.Text = String.Empty;
            ddlAtividadeEmpresa7.SelectedIndex = 0;
            txtDataAdmissao7.Valor = String.Empty;
            txtDataDemissao7.Valor = String.Empty;
            txtFuncaoExercida7.Text = String.Empty;
            txtAtividadeExercida7.Valor = String.Empty;
            txtSugestaoTarefas7.Text = String.Empty;
            txtUltimoSalario7.Valor = null;
            ltGraficoQualidade7.Text = "";
            IdExperienciaProfissional7 = null;
        }
        #endregion

        #region LimparCamposExp8
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp8()
        {
            txtEmpresa8.Text = String.Empty;
            ddlAtividadeEmpresa8.SelectedIndex = 0;
            txtDataAdmissao8.Valor = String.Empty;
            txtDataDemissao8.Valor = String.Empty;
            txtFuncaoExercida8.Text = String.Empty;
            txtAtividadeExercida8.Valor = String.Empty;
            txtSugestaoTarefas8.Text = String.Empty;
            txtUltimoSalario8.Valor = null;
            ltGraficoQualidade8.Text = "";
            IdExperienciaProfissional8 = null;
        }
        #endregion

        #region LimparCamposExp9
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp9()
        {
            txtEmpresa9.Text = String.Empty;
            ddlAtividadeEmpresa9.SelectedIndex = 0;
            txtDataAdmissao9.Valor = String.Empty;
            txtDataDemissao9.Valor = String.Empty;
            txtFuncaoExercida9.Text = String.Empty;
            txtAtividadeExercida9.Valor = String.Empty;
            txtSugestaoTarefas9.Text = String.Empty;
            txtUltimoSalario9.Valor = null;
            ltGraficoQualidade9.Text = "";
            IdExperienciaProfissional9 = null;
        }
        #endregion

        #region LimparCamposExp10
        /// <summary>
        /// Método utilizado para limpar todos os campos do formulário
        /// </summary>
        public void LimparCamposExp10()
        {
            txtEmpresa10.Text = String.Empty;
            ddlAtividadeEmpresa10.SelectedIndex = 0;
            txtDataAdmissao10.Valor = String.Empty;
            txtDataDemissao10.Valor = String.Empty;
            txtFuncaoExercida10.Text = String.Empty;
            txtAtividadeExercida10.Valor = String.Empty;
            txtSugestaoTarefas10.Text = String.Empty;
            txtUltimoSalario10.Valor = null;
            ltGraficoQualidade10.Text = "";
            IdExperienciaProfissional10 = null;
        }
        #endregion

        

        #region Salvar
        /// <summary>
        /// Método utilizado para salvar um cadastro
        /// </summary>
        public bool Salvar()
        {
            if (txtUltimoSalario.Valor.HasValue)
            {
                var salarioMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional));

                if (txtUltimoSalario.Valor < salarioMinimo)
                {
                    ExibirMensagem(string.Format("Sua pretensão salarial deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo), TipoMensagem.Erro);
                    return false;
                }
            }

            //validar data de demissão das experiências
            if (!ValidarDataDemissaoExperiencias())
            {
                ExibirMensagem("A data de demissão não pode ser menor que a data de admissão, confira seus dados.", TipoMensagem.Erro);
                return false;
            }


            PessoaFisica objPessoaFisica;
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            var listRedeSocial = new List<PessoaFisicaRedeSocial>();

            Curriculo objCurriculo;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objPessoaFisicaComplemento))
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento();

                if (objPessoaFisica.Endereco != null)
                    objPessoaFisica.Endereco.CompleteObject();

                if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                    objCurriculo = new Curriculo();
            }
            else
            {
                objPessoaFisica = new PessoaFisica
                    {
                        Endereco = new Endereco()
                    };
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                objCurriculo = new Curriculo();
            }

            objPessoaFisica.NumeroRG = txtNumeroRG.Valor;
            objPessoaFisica.NomeOrgaoEmissor = txtOrgaoEmissorRG.Valor;

            if (!ddlEstadoCivil.SelectedValue.Equals("0"))
                objPessoaFisica.EstadoCivil = new EstadoCivil(Convert.ToInt32(ddlEstadoCivil.SelectedValue));

            //Limpando a localização, caso o CEP previamente informado seja diferente do 
            if (!string.IsNullOrEmpty(objPessoaFisica.Endereco.NumeroCEP) && !objPessoaFisica.Endereco.NumeroCEP.Equals(ucEndereco.CEP))
                objCurriculo.DescricaoLocalizacao = null;

            //Endereco
            objPessoaFisica.Endereco.NumeroCEP = ucEndereco.CEP;
            objPessoaFisica.Endereco.DescricaoLogradouro = ucEndereco.Logradouro;
            objPessoaFisica.Endereco.NumeroEndereco = ucEndereco.Numero;
            objPessoaFisica.Endereco.DescricaoComplemento = ucEndereco.Complemento;
            objPessoaFisica.Endereco.DescricaoBairro = ucEndereco.Bairro;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(ucEndereco.Cidade, out objCidade))
                objPessoaFisica.Endereco.Cidade = objCidade;

            objPessoaFisica.Cidade = objCidade;

            objPessoaFisica.NumeroDDDCelular = txtCelular.DDD;
            objPessoaFisica.NumeroCelular = txtCelular.Fone;
            objPessoaFisica.NumeroDDDTelefone = txtTelefoneResidencial.DDD;
            objPessoaFisica.NumeroTelefone = txtTelefoneResidencial.Fone;

            Contato objContatoTelefone;
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, null))
            {
                if (!String.IsNullOrEmpty(txtTelefoneRecado.Fone) && !String.IsNullOrEmpty(txtTelefoneRecado.DDD))
                {
                    objContatoTelefone.NumeroDDDTelefone = txtTelefoneRecado.DDD;
                    objContatoTelefone.NumeroTelefone = txtTelefoneRecado.Fone;
                    objContatoTelefone.NomeContato = txtTelefoneRecadoFalarCom.Text;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo);
                }
                else
                {
                    objContatoTelefone.NumeroDDDTelefone = string.Empty;
                    objContatoTelefone.NumeroTelefone = string.Empty;
                    objContatoTelefone.NomeContato = string.Empty;
                    objContatoTelefone.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo);
                }
            }
            else
            {
                objContatoTelefone = new Contato
                    {
                        NumeroDDDTelefone = txtTelefoneRecado.DDD,
                        NumeroTelefone = txtTelefoneRecado.Fone,
                        NomeContato = txtTelefoneRecadoFalarCom.Text,
                        TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoFixo)
                    };
            }

            Contato objContatoCelular;
            if (Contato.CarregarPorPessoaFisicaTipoContato(objPessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, null))
            {
                if (!String.IsNullOrEmpty(txtCelularRecado.Fone))
                {
                    objContatoCelular.NumeroDDDCelular = txtCelularRecado.DDD;
                    objContatoCelular.NumeroCelular = txtCelularRecado.Fone;
                    objContatoCelular.NomeContato = txtCelularRecadoFalarCom.Text;
                    objContatoCelular.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular);
                }
                else
                {
                    objContatoCelular.NumeroDDDCelular = string.Empty;
                    objContatoCelular.NumeroCelular = string.Empty;
                    objContatoCelular.NomeContato = string.Empty;
                    objContatoCelular.TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular);
                }
            }
            else
            {
                objContatoCelular = new Contato
                    {
                        NumeroDDDCelular = txtCelularRecado.DDD,
                        NumeroCelular = txtCelularRecado.Fone,
                        NomeContato = txtCelularRecadoFalarCom.Text,
                        TipoContato_ = new TipoContato((int)Enumeradores.TipoContato.RecadoCelular)
                    };
            }

            objCurriculo.ValorUltimoSalario = txtUltimoSalario.Valor;

            #region ExperienciaProfissional

            ExperienciaProfissional objExperienciaProfissional1 = SalvarExperienciasProfissionais(txtEmpresa1.Text, ddlAtividadeEmpresa1.SelectedValue, txtDataAdmissao1.ValorDatetime, txtDataDemissao1.ValorDatetime, txtFuncaoExercida1.Text, txtAtividadeExercida1.Valor, IdExperienciaProfissional1,txtUltimoSalario.Valor);
            ExperienciaProfissional objExperienciaProfissional2 = SalvarExperienciasProfissionais(txtEmpresa2.Text, ddlAtividadeEmpresa2.SelectedValue, txtDataAdmissao2.ValorDatetime, txtDataDemissao2.ValorDatetime, txtFuncaoExercida2.Text, txtAtividadeExercida2.Valor, IdExperienciaProfissional2, txtUltimoSalario2.Valor);
            ExperienciaProfissional objExperienciaProfissional3 = SalvarExperienciasProfissionais(txtEmpresa3.Text, ddlAtividadeEmpresa3.SelectedValue, txtDataAdmissao3.ValorDatetime, txtDataDemissao3.ValorDatetime, txtFuncaoExercida3.Text, txtAtividadeExercida3.Valor, IdExperienciaProfissional3, txtUltimoSalario3.Valor);
            ExperienciaProfissional objExperienciaProfissional4 = SalvarExperienciasProfissionais(txtEmpresa4.Text, ddlAtividadeEmpresa4.SelectedValue, txtDataAdmissao4.ValorDatetime, txtDataDemissao4.ValorDatetime, txtFuncaoExercida4.Text, txtAtividadeExercida4.Valor, IdExperienciaProfissional4, txtUltimoSalario4.Valor);
            ExperienciaProfissional objExperienciaProfissional5 = SalvarExperienciasProfissionais(txtEmpresa5.Text, ddlAtividadeEmpresa5.SelectedValue, txtDataAdmissao5.ValorDatetime, txtDataDemissao5.ValorDatetime, txtFuncaoExercida5.Text, txtAtividadeExercida5.Valor, IdExperienciaProfissional5, txtUltimoSalario5.Valor);
            ExperienciaProfissional objExperienciaProfissional6 = SalvarExperienciasProfissionais(txtEmpresa6.Text, ddlAtividadeEmpresa6.SelectedValue, txtDataAdmissao6.ValorDatetime, txtDataDemissao6.ValorDatetime, txtFuncaoExercida6.Text, txtAtividadeExercida6.Valor, IdExperienciaProfissional6, txtUltimoSalario6.Valor);
            ExperienciaProfissional objExperienciaProfissional7 = SalvarExperienciasProfissionais(txtEmpresa7.Text, ddlAtividadeEmpresa7.SelectedValue, txtDataAdmissao7.ValorDatetime, txtDataDemissao7.ValorDatetime, txtFuncaoExercida7.Text, txtAtividadeExercida7.Valor, IdExperienciaProfissional7, txtUltimoSalario7.Valor);
            ExperienciaProfissional objExperienciaProfissional8 = SalvarExperienciasProfissionais(txtEmpresa8.Text, ddlAtividadeEmpresa8.SelectedValue, txtDataAdmissao8.ValorDatetime, txtDataDemissao8.ValorDatetime, txtFuncaoExercida8.Text, txtAtividadeExercida8.Valor, IdExperienciaProfissional8, txtUltimoSalario8.Valor);
            ExperienciaProfissional objExperienciaProfissional9 = SalvarExperienciasProfissionais(txtEmpresa9.Text, ddlAtividadeEmpresa9.SelectedValue, txtDataAdmissao9.ValorDatetime, txtDataDemissao9.ValorDatetime, txtFuncaoExercida9.Text, txtAtividadeExercida9.Valor, IdExperienciaProfissional9, txtUltimoSalario9.Valor);
            ExperienciaProfissional objExperienciaProfissional10 = SalvarExperienciasProfissionais(txtEmpresa10.Text, ddlAtividadeEmpresa10.SelectedValue, txtDataAdmissao10.ValorDatetime, txtDataDemissao10.ValorDatetime, txtFuncaoExercida10.Text, txtAtividadeExercida10.Valor, IdExperienciaProfissional10, txtUltimoSalario10.Valor);

            #endregion

            objCurriculo.SalvarDadosPessoais(objPessoaFisica, objPessoaFisicaComplemento, objContatoTelefone, objContatoCelular, listRedeSocial, objExperienciaProfissional1, objExperienciaProfissional2, objExperienciaProfissional3, objExperienciaProfissional4, objExperienciaProfissional5, objExperienciaProfissional6, objExperienciaProfissional7, objExperienciaProfissional8, objExperienciaProfissional9, objExperienciaProfissional10, EnumSituacaoCurriculo);

            IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

            #region Publicação Currículo
            if(!objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals(Enumeradores.SituacaoCurriculo.Bloqueado.GetHashCode()))
                {
                    try
                {
                    var enfileiraPublicacao = Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnfileiraPublicacaoAutomaticaCurriculo));
                    if (enfileiraPublicacao)
                    {
                        var parametros = new ParametroExecucaoCollection
                                    {
                                        {"idCurriculo", "Curriculo", objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture), objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture)}
                                    };

                        ProcessoAssincrono.IniciarAtividade(
                            TipoAtividade.PublicacaoCurriculo,
                            PluginsCompatibilidade.CarregarPorMetadata("PublicacaoCurriculo", "PluginSaidaEmailSMS"),
                            parametros,
                            null,
                            null,
                            null,
                            null,
                            DateTime.Now);
                    }

                    //checar se o curriculo tem as experiencias profissionais vazias
                    //se não preencheu nenhuma experiencia profissional vai enviar um email e um sms para o candidato.
                    if (objExperienciaProfissional1 == null && objExperienciaProfissional2 == null && objExperienciaProfissional3 == null && objExperienciaProfissional4 == null
                        && objExperienciaProfissional5 == null && objExperienciaProfissional6 == null && objExperienciaProfissional7 == null && objExperienciaProfissional8 == null
                        && objExperienciaProfissional9 == null && objExperienciaProfissional10 == null)
                    {
                        //Envia e-mail de aviso para o candidato
                        UsuarioFilialPerfil objPerfilDestinatario = null;
                        UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objPerfilDestinatario);

                        int? idUsuarioFilialPerfil = null;

                        if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                            idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoCandidato.Value;

                        string assunto;
                        string template = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.AlertaEmailExperienciaProfissional);
                        string templateSMS = CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.AlertaExperienciaProfissional);
                        string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);

                        string listaVagas, salaVip, vip, cadastroCurriculo, quemMeViu, pesquisaVagas, loginCandidato, cadastroExperiencias;
                        BNE.BLL.Curriculo.RetornarHashLogarCurriculo(objCurriculo.IdCurriculo, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                        //parametros de estatisticas
                        Estatistica _estatisticas = Estatistica.RecuperarEstatistica();

                        template = template.Replace("{Sala_Vip}", salaVip);
                        template = template.Replace("{vip}", vip);
                        template = template.Replace("{Quem_Me_Viu}", quemMeViu);
                        template = template.Replace("{Cadastro_Curriculo}", cadastroCurriculo);
                        template = template.Replace("{Pesquisa_Vagas}", pesquisaVagas);
                        template = template.Replace("{login_candidato}", salaVip);

                        template = template.Replace("{Quantidade_empresas}", objPessoaFisica.NomeCompleto);
                        template = template.Replace("{Quantidade_vagas}", objPessoaFisica.NomeCompleto);
                        template = template.Replace("{login_candidato}", vip);

                        template = template.Replace("{Quantidade_empresas}", _estatisticas.QuantidadeEmpresa.ToString());
                        template = template.Replace("{Quantidade_vagas}", _estatisticas.QuantidadeVaga.ToString());

                        template = template.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);

                        MensagemSistema objMensagem = new MensagemSistema();

                        objMensagem.DescricaoMensagemSistema = template;

                        //checar se ja foi enviado alerta de email para a sessão atual do candidato
                        if (AlertaEmailExperienciaProfissionalEnviado == null || !AlertaEmailExperienciaProfissionalEnviado.Value)
                        {
                            if (BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                            {
                                //Enviar E-mail para o candidato
                                AlertaEmailExperienciaProfissionalEnviado =
                                    MensagemCS.SalvarEmail(objCurriculo, idUsuarioFilialPerfil.HasValue ? new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value) : null, objPerfilDestinatario, null, "Atualizar experiências Profissionais", objMensagem.DescricaoMensagemSistema, emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);
                            }
                        }
                        
                        //checar se ja foi enviado alerta de SMS para a sessão atual do candidato
                        if (AlertaSMSExperienciaProfissionalEnviado == null || !AlertaSMSExperienciaProfissionalEnviado.Value)
                        {
                            string idUFPRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EnvioAlertaSMSExperienciaProfissional);
                            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaSMS = new List<BLL.DTO.PessoaFisicaEnvioSMSTanque>();

                            //Enviar SMS para o candidato
                            StringBuilder smsMensagem = new StringBuilder();
                            smsMensagem.AppendFormat("{0}, {1} ", objPessoaFisica.PrimeiroNome, templateSMS);

                            //popular lista SMS
                            var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                            {
                                dddCelular = objPessoaFisica.NumeroDDDCelular.ToString(),
                                numeroCelular = objPessoaFisica.NumeroCelular.ToString(),
                                nomePessoa = objPessoaFisica.NomePessoa,
                                mensagem = smsMensagem.ToString(),
                                idDestinatario = objCurriculo.IdCurriculo
                            };

                            listaSMS.Add(objUsuarioEnvioSMS);
                            //fim do popular lista SMS

                            //enviar SMS pelo tanque
                            BNE.BLL.Mensagem.EnvioSMSTanque(idUFPRemetente, listaSMS, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                }
            #endregion

            return true;
        }
        #endregion

        #region SalvarExperienciasProfissionais
        private ExperienciaProfissional SalvarExperienciasProfissionais(string txtEmpresa, string ddlAtividadeEmpresa, DateTime? txtDataAdmissao, DateTime? txtDataDemissao, string txtFuncaoExercida, string txtAtividadeExercida, int? idExperienciaProfessional, decimal? txtSalario)
        {
            ExperienciaProfissional objExperienciaProfissional;
            if (!String.IsNullOrEmpty(txtEmpresa))
            {
                if (idExperienciaProfessional.HasValue)
                    objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                else
                    objExperienciaProfissional = new ExperienciaProfissional();

                objExperienciaProfissional.RazaoSocial = txtEmpresa;
                objExperienciaProfissional.AreaBNE = new AreaBNE(Convert.ToInt32(ddlAtividadeEmpresa));

                if(txtDataAdmissao.HasValue)
                objExperienciaProfissional.DataAdmissao = txtDataAdmissao.Value;
                
                objExperienciaProfissional.DataDemissao = txtDataDemissao;
                Funcao objFuncao;
                if (Funcao.CarregarPorDescricao(txtFuncaoExercida, out objFuncao))
                {
                    objExperienciaProfissional.Funcao = objFuncao;
                    objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                }
                else
                {
                    objExperienciaProfissional.Funcao = null;
                    objExperienciaProfissional.DescricaoFuncaoExercida = txtFuncaoExercida;
                }

                objExperienciaProfissional.DescricaoAtividade = txtAtividadeExercida;

                objExperienciaProfissional.VlrSalario = (txtSalario.HasValue ? txtSalario : null);
                objExperienciaProfissional.DescricaoNavegador = strDadosNavegador;
            }
            else if (idExperienciaProfessional.HasValue)
            {
                objExperienciaProfissional = ExperienciaProfissional.LoadObject(idExperienciaProfessional.Value);
                objExperienciaProfissional.FlagInativo = true;
            }
            else
                objExperienciaProfissional = null;

            return objExperienciaProfissional;
        }
        #endregion

        #region PegarDadosNavegador

        private string PegarDadosNavegador()
        {
            System.Web.HttpBrowserCapabilities browser = Request.Browser;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Type:{0} -", browser.Type);
            sb.AppendFormat("Name:{0} - ", browser.Browser);
            sb.AppendFormat("Version:{0} - ", browser.Version);
            sb.AppendFormat("Major:{0} - ", browser.MajorVersion);
            sb.AppendFormat("Minor:{0} - ", browser.MinorVersion);
            sb.AppendFormat("Platform:{0} - ", browser.Platform);
            sb.AppendFormat("Suporta cookies:{0} - ", browser.Cookies);
            sb.AppendFormat("Suporta JS:{0} - ", browser.EcmaScriptVersion.ToString());

            return sb.ToString();
        }

        #endregion

        #region ValidarDataDemissaoExperiencias

        /// <summary>
        /// Valida a data de demissão não deixando ser menor que a data de admissão.
        /// </summary>
        /// <returns></returns>
        private bool ValidarDataDemissaoExperiencias()
        {
            bool bolDtaValida = true;
            bolDtaValida = (txtDataAdmissao1.ValorDatetime != null ? (txtDataAdmissao1.ValorDatetime > txtDataDemissao1.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao2.ValorDatetime != null ? (txtDataAdmissao2.ValorDatetime > txtDataDemissao2.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao3.ValorDatetime != null ? (txtDataAdmissao3.ValorDatetime > txtDataDemissao3.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao4.ValorDatetime != null ? (txtDataAdmissao4.ValorDatetime > txtDataDemissao4.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao5.ValorDatetime != null ? (txtDataAdmissao5.ValorDatetime > txtDataDemissao5.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao6.ValorDatetime != null ? (txtDataAdmissao6.ValorDatetime > txtDataDemissao6.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao7.ValorDatetime != null ? (txtDataAdmissao7.ValorDatetime > txtDataDemissao7.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao8.ValorDatetime != null ? (txtDataAdmissao8.ValorDatetime > txtDataDemissao8.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao9.ValorDatetime != null ? (txtDataAdmissao9.ValorDatetime > txtDataDemissao9.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);
            bolDtaValida = (txtDataAdmissao10.ValorDatetime != null ? (txtDataAdmissao10.ValorDatetime > txtDataDemissao10.ValorDatetime ? false : (bolDtaValida ? true : false)) : bolDtaValida);

            return bolDtaValida;
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
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.IntervaloTempoAutoComplete,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                        Enumeradores.Parametro.SalarioMinimoNacional
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceFuncaoExercida1.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida1.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida1.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida2.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida2.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida2.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida3.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida3.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida3.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida4.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida4.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida4.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida5.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida5.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida5.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida6.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida6.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida6.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida7.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida7.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida7.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida8.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida8.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida8.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida9.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida9.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida9.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                aceFuncaoExercida10.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida10.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida10.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                txtUltimoSalario.ValorMinimo = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.SalarioMinimoNacional]);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlMiniCurriculo
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlMiniCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlFormacaoCursos_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlFormacaoCursos_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoFormacao.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosComplementares_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosComplementares_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoComplementar.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlRevisaoDados_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlRevisaoDados_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoRevisao.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlGestao_Click
        /// <summary>
        /// habilitar aba Gestao
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlGestao_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect("~/CadastroCurriculoGestao.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region txtEmpresa1_TextChanged
        protected void txtEmpresa1_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa2_TextChanged
        protected void txtEmpresa2_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa3_TextChanged
        protected void txtEmpresa3_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa4_TextChanged
        protected void txtEmpresa4_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa5_TextChanged
        protected void txtEmpresa5_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa6_TextChanged
        protected void txtEmpresa6_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa7_TextChanged
        protected void txtEmpresa7_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa8_TextChanged
        protected void txtEmpresa8_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa9_TextChanged
        protected void txtEmpresa9_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa10_TextChanged
        protected void txtEmpresa10_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida1_TextChanged
        protected void txtFuncaoExercida1_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas1.Text = RecuperarJobFuncao(txtFuncaoExercida1.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida2_TextChanged
        protected void txtFuncaoExercida2_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas2.Text = RecuperarJobFuncao(txtFuncaoExercida2.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida3_TextChanged
        protected void txtFuncaoExercida3_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas3.Text = RecuperarJobFuncao(txtFuncaoExercida3.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida4_TextChanged
        protected void txtFuncaoExercida4_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas4.Text = RecuperarJobFuncao(txtFuncaoExercida4.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida5_TextChanged
        protected void txtFuncaoExercida5_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas5.Text = RecuperarJobFuncao(txtFuncaoExercida5.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida6_TextChanged
        protected void txtFuncaoExercida6_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas6.Text = RecuperarJobFuncao(txtFuncaoExercida6.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida7_TextChanged
        protected void txtFuncaoExercida7_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas7.Text = RecuperarJobFuncao(txtFuncaoExercida7.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida8_TextChanged
        protected void txtFuncaoExercida8_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas8.Text = RecuperarJobFuncao(txtFuncaoExercida8.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida9_TextChanged
        protected void txtFuncaoExercida9_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas9.Text = RecuperarJobFuncao(txtFuncaoExercida9.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtFuncaoExercida10_TextChanged
        protected void txtFuncaoExercida10_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas10.Text = RecuperarJobFuncao(txtFuncaoExercida10.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region ddlAtividadeEmpresa1_SelectedIndexChanged
        protected void ddlAtividadeEmpresa1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region ddlAtividadeEmpresa2_SelectedIndexChanged
        protected void ddlAtividadeEmpresa2_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa3_SelectedIndexChanged
        protected void ddlAtividadeEmpresa3_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa4_SelectedIndexChanged
        protected void ddlAtividadeEmpresa4_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa5_SelectedIndexChanged
        protected void ddlAtividadeEmpresa5_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa6_SelectedIndexChanged
        protected void ddlAtividadeEmpresa6_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa7_SelectedIndexChanged
        protected void ddlAtividadeEmpresa7_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa8_SelectedIndexChanged
        protected void ddlAtividadeEmpresa8_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa9_SelectedIndexChanged
        protected void ddlAtividadeEmpresa9_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region ddlAtividadeEmpresa10_SelectedIndexChanged
        protected void ddlAtividadeEmpresa10_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();

        }
        #endregion

        #region VerificarObrigatoriedadeCamposExperiencia
        private void VerificarObrigatoriedadeCamposExperiencia()
        {
            bool empresa1 = false;
            bool empresa2 = false;
            bool empresa3 = false;
            bool empresa4 = false;
            bool empresa5 = false;
            bool empresa6 = false;
            bool empresa7 = false;
            bool empresa8 = false;
            bool empresa9 = false;
            bool empresa10 = false;

            if (!String.IsNullOrEmpty(txtEmpresa1.Text) || Convert.ToInt32(ddlAtividadeEmpresa1.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao1.Valor) || !String.IsNullOrEmpty(txtDataDemissao1.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida1.Text))
                empresa1 = true;
            if (!String.IsNullOrEmpty(txtEmpresa2.Text) || Convert.ToInt32(ddlAtividadeEmpresa2.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao2.Valor) || !String.IsNullOrEmpty(txtDataDemissao2.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida2.Text))
                empresa2 = true;
            if (!String.IsNullOrEmpty(txtEmpresa3.Text) || Convert.ToInt32(ddlAtividadeEmpresa3.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao3.Valor) || !String.IsNullOrEmpty(txtDataDemissao3.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida3.Text))
                empresa3 = true;

            if (!String.IsNullOrEmpty(txtEmpresa4.Text) || Convert.ToInt32(ddlAtividadeEmpresa4.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao4.Valor) || !String.IsNullOrEmpty(txtDataDemissao4.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida4.Text))
                empresa4 = true;

            if (!String.IsNullOrEmpty(txtEmpresa5.Text) || Convert.ToInt32(ddlAtividadeEmpresa5.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao5.Valor) || !String.IsNullOrEmpty(txtDataDemissao5.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida5.Text))
                empresa5 = true;

            if (!String.IsNullOrEmpty(txtEmpresa6.Text) || Convert.ToInt32(ddlAtividadeEmpresa6.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao6.Valor) || !String.IsNullOrEmpty(txtDataDemissao6.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida6.Text))
                empresa6 = true;

            if (!String.IsNullOrEmpty(txtEmpresa7.Text) || Convert.ToInt32(ddlAtividadeEmpresa7.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao7.Valor) || !String.IsNullOrEmpty(txtDataDemissao7.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida7.Text))
                empresa7 = true;

            if (!String.IsNullOrEmpty(txtEmpresa8.Text) || Convert.ToInt32(ddlAtividadeEmpresa8.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao8.Valor) || !String.IsNullOrEmpty(txtDataDemissao8.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida8.Text))
                empresa8 = true;

            if (!String.IsNullOrEmpty(txtEmpresa9.Text) || Convert.ToInt32(ddlAtividadeEmpresa9.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao9.Valor) || !String.IsNullOrEmpty(txtDataDemissao9.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida9.Text))
                empresa9 = true;

            if (!String.IsNullOrEmpty(txtEmpresa10.Text) || Convert.ToInt32(ddlAtividadeEmpresa10.SelectedValue) >= 1 || !String.IsNullOrEmpty(txtDataAdmissao10.Valor) || !String.IsNullOrEmpty(txtDataDemissao10.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida10.Text))
                empresa10 = true;

            rfvEmpresa1.Enabled = empresa1;
            cvAtividadeEmpresa1.Enabled = empresa1;
            txtDataAdmissao1.Obrigatorio = empresa1;
            rfvFuncaoExercida1.Enabled = empresa1;
            txtAtividadeExercida1.Obrigatorio = empresa1;
            txtUltimoSalario.Obrigatorio = empresa1;

            rfvEmpresa2.Enabled = empresa2;
            cvAtividadeEmpresa2.Enabled = empresa2;
            txtDataAdmissao2.Obrigatorio = empresa2;
            rfvFuncaoExercida2.Enabled = empresa2;
            txtAtividadeExercida2.Obrigatorio = empresa2;
            //txtUltimoSalario2.Obrigatorio = empresa2;

            rfvEmpresa3.Enabled = empresa3;
            cvAtividadeEmpresa3.Enabled = empresa3;
            txtDataAdmissao3.Obrigatorio = empresa3;
            rfvFuncaoExercida3.Enabled = empresa3;
            txtAtividadeExercida3.Obrigatorio = empresa3;
            //txtUltimoSalario3.Obrigatorio = empresa3;

            rfvEmpresa4.Enabled = empresa4;
            cvAtividadeEmpresa4.Enabled = empresa4;
            txtDataAdmissao4.Obrigatorio = empresa4;
            rfvFuncaoExercida4.Enabled = empresa4;
            txtAtividadeExercida4.Obrigatorio = empresa4;
            //txtUltimoSalario4.Obrigatorio = empresa4;

            rfvEmpresa5.Enabled = empresa5;
            cvAtividadeEmpresa5.Enabled = empresa5;
            txtDataAdmissao5.Obrigatorio = empresa5;
            rfvFuncaoExercida5.Enabled = empresa5;
            txtAtividadeExercida5.Obrigatorio = empresa5;
            //txtUltimoSalario5.Obrigatorio = empresa5;

            rfvEmpresa6.Enabled = empresa6;
            cvAtividadeEmpresa6.Enabled = empresa6;
            txtDataAdmissao6.Obrigatorio = empresa6;
            rfvFuncaoExercida6.Enabled = empresa6;
            txtAtividadeExercida6.Obrigatorio = empresa6;

            rfvEmpresa7.Enabled = empresa7;
            cvAtividadeEmpresa7.Enabled = empresa7;
            txtDataAdmissao7.Obrigatorio = empresa7;
            rfvFuncaoExercida7.Enabled = empresa7;
            txtAtividadeExercida7.Obrigatorio = empresa7;

            rfvEmpresa8.Enabled = empresa8;
            cvAtividadeEmpresa8.Enabled = empresa8;
            txtDataAdmissao8.Obrigatorio = empresa8;
            rfvFuncaoExercida8.Enabled = empresa8;
            txtAtividadeExercida8.Obrigatorio = empresa8;

            rfvEmpresa9.Enabled = empresa9;
            cvAtividadeEmpresa9.Enabled = empresa9;
            txtDataAdmissao9.Obrigatorio = empresa9;
            rfvFuncaoExercida9.Enabled = empresa9;
            txtAtividadeExercida9.Obrigatorio = empresa9;

            rfvEmpresa10.Enabled = empresa10;
            cvAtividadeEmpresa10.Enabled = empresa10;
            txtDataAdmissao10.Obrigatorio = empresa10;
            rfvFuncaoExercida10.Enabled = empresa10;
            txtAtividadeExercida10.Obrigatorio = empresa10;

            //txtUltimoSalario.Obrigatorio = empresa1 || empresa2 || empresa3 || empresa4 || empresa5;
            txtUltimoSalario.Obrigatorio = empresa1;

            upExperienciaProfissional1.Update();
            upExperienciaProfissional2.Update();
            upExperienciaProfissional3.Update();

            ScriptManager.RegisterStartupScript(upExperienciaProfissional1, upExperienciaProfissional1.GetType(), "txtAtividadeExercida1_KeyUp", "javaScript:ChecarGraficoQualidadeDasAtividadesExercidas();", true);            

        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeConfirmacaoExclusao.Hide();
            mpeAvisoApos6Experiencia.Hide();
        }
        #endregion

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

        #region RecuperarJobFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valor)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
            {
                if (!string.IsNullOrEmpty(objFuncao.DescricaoJob))
                    return objFuncao.DescricaoJob;
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

        #region ValidarVariosEnderecosPorCEP
        /// <summary>
        /// Validar se o cep informado contém mais de um endereço associado ao mesmo.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarVariosEnderecosPorCEP(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                var servico = new wsCEP.cepws();

                ServiceAuth.GerarHashAcessoWS(servico);

                var objCep = new wsCEP.CEP
                    {
                        Cep = valor.Replace("-", string.Empty).Trim()
                    };

                int qtdeCepEncontrados = 0;
                try
                {
                    if (servico.RecuperarQuantidadeEnderecosPorCEP(objCep, ref qtdeCepEncontrados))
                    {
                        if (qtdeCepEncontrados > 1)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
            return false;
        }
        #endregion

        #region ValidarEstadoCivil
        /// <summary>
        /// Validar Estado Civil
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string ValidarEstadoCivil(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor)) //Selecione
                return String.Empty;

            if (valor.Equals("0"))
                return String.Empty;

            return valor;
        }
        #endregion

        #endregion

    }
}