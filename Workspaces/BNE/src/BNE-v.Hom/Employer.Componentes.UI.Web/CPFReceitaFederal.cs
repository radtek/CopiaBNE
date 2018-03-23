//using System;
//using System.Text;
//using System.Web.UI.WebControls;
//using System.IO;
//using System.Net;
//using System.Text.RegularExpressions;
//using Employer.Componentes.UI.Web.Extensions;
//using System.Web.UI.HtmlControls;
//using System.Web.UI;
//using Employer.Componentes.UI.Web.Util;
//using System.Web;
//using Employer.Componentes.UI.Web.Handlers;
//using Employer.Componentes.UI.Web.Interface;

//namespace Employer.Componentes.UI.Web
//{
//    /// <summary>
//    /// Componente que recupera os dados do cartão CPF do site da receita federal
//    /// </summary>
//    public class CPFReceitaFederal : CompositeControl, IRequiredField, IMensagemErro
//    {
//        #region DadosCpfReceitaFederal
//        /// <summary>
//        /// Representa os dados do cartão Cpf extraídos do site da Receita Federal
//        /// </summary>
//        public class DadosCpfReceitaFederal
//        {
//            #region Properties
//            #region Cpf
//            /// <summary>
//            /// O número de Cpf
//            /// </summary>
//            public decimal Cpf { get; private set; }
//            #endregion

//            #region Nome
//            /// <summary>
//            /// O nome
//            /// </summary>
//            public String Nome { get; private set; }
//            #endregion

//            #region Cpf
//            /// <summary>
//            /// A situação
//            /// </summary>
//            public String Situacao { get; private set; }
//            #endregion
//            #endregion

//            #region Metodos
//            #region StripString
//            /// <summary>
//            /// Trata string removento caracteres especiais
//            /// </summary>
//            /// <param name="s">String original</param>
//            /// <returns>String tratada</returns>
//            private String StripString(String s)
//            {
//                return s.Replace(".", String.Empty)
//                    .Replace("/", String.Empty)
//                    .Replace("-", String.Empty)
//                    .Replace("&nbsp;", " ")
//                    .Replace("\r", " ")
//                    .Replace("\n", " ")
//                    .Replace("\t", " ")
//                    .Replace(":", " ")
//                    .Trim();
//            }
//            #endregion
//            #region Inicio
//            /// <summary>
//            /// Recupera a posição inicial de um fragmento dentro de um dado em formato texto
//            /// </summary>
//            /// <param name="dados">O dado a ser consultado</param>
//            /// <param name="fragmento">O fragmento a ser localizado</param>
//            /// <returns>A posição inicial do fragmento</returns>
//            private int Inicio(String dados, String fragmento)
//            {
//                return dados.IndexOf(fragmento) + fragmento.Length;
//            }
//            #endregion
//            #endregion

//            #region Ctor
//            /// <summary>
//            /// Construtor - Recebe uma string HTML e extrái os dados do Cartão Cpf
//            /// </summary>
//            /// <param name="dados">A página de cartão Cpf do site da Receita Federal</param>
//            public DadosCpfReceitaFederal(String dados)
//            {
//                int origem, destino = 0;
//                String dummy = String.Empty;

//                #region  Cpf
//                origem = Inicio(dados, "No do CPF:");
//                destino = dados.IndexOf("Nome da Pessoa Física:");

//                this.Cpf = Convert.ToDecimal(
//                    this.StripString(dados.Substring(origem, destino - origem))
//                    );
//                #endregion

//                #region  Nome
//                origem = Inicio(dados, "Nome da Pessoa Física:");
//                destino = dados.IndexOf("Data de Nascimento:");

//                this.Nome = this.StripString(dados.Substring(origem, destino - origem));
//                #endregion

//                #region  Situacao
//                origem = Inicio(dados, "Situação Cadastral:");
//                destino = dados.IndexOf("Data da Inscrição:");

//                this.Situacao = Convert.ToString(
//                    this.StripString(dados.Substring(origem, destino - origem))
//                    );
//                #endregion
//            }
//            #endregion
//        }
//        #endregion

//        #region Fields
//        private Image _imgCaptcha = new Image();
//        private ControlCPF _txtCpf = new ControlCPF();
//        private Label _lblCaptcha = new Label();
//        private Label _lblDtaNasc = new Label();
//        //private ControlAlfaNumerico _txtCaptcha = new ControlAlfaNumerico();
//        private TextBox _txtCaptcha = new TextBox();
//        private DataTextBox _txtDtaNas = new DataTextBox { ID = "txtDtaNas" };
//        private Button _btnEnviar = new Button();

//        private Label _lblErrosCaptcha = new Label();
//        private Label _lblErrosDta = new Label();
//        private Button _btnValidarReceita = new Button();
//        private UpdatePanel _upComponente = new UpdatePanel() { ID = "upCpfReceitaFederal", UpdateMode = UpdatePanelUpdateMode.Conditional };
//        private UpdatePanel _upCpf = new UpdatePanel();
//        private Panel _pnlComponente = new Panel();
//        private Panel _pnlCpf = new Panel();
//        private Panel _pnlLinhaData = new Panel() { ID = "pnlLinhaData" };
//        //private Panel _pnlBtnFechar = new Panel() { ID = "pnlBtnFechar" };
//        private Panel _pnlCaptcha = new Panel();
//        private String _mensagemCaptchaInvalido;
//        private ImageButton _btnFechar = new ImageButton() { CausesValidation = false, ID = "btnFechar" };
//        private Employer.Componentes.UI.Web.Button _btnRenovaCaptcha = new Employer.Componentes.UI.Web.Button() { CausesValidation = false, ID = "btnRenovaCaptcha", ToolTip = "Renovar Captcha" };
//        #endregion

//        #region Eventos
//        /// <summary>
//        /// Handler de Cpf encontrado
//        /// </summary>
//        /// <param name="sender">Objeto que enviou o evento</param>
//        /// <param name="dadosCpf">Os dados do Cpf selecionado</param>
//        public delegate void CpfEncontradoHandler(object sender, DadosCpfReceitaFederal dadosCpf);
//        /// <summary>
//        /// Evento disparado quando um Cpf é recuperado da receita federal
//        /// </summary>
//        public event CpfEncontradoHandler CpfEncontrado;
//        /// <summary>
//        /// Evento disparado quando o valor do campo Cpf é alterado
//        /// </summary>
//        public event EventHandler ValorAlterado;
//        #endregion

//        #region Properties

//        #region Primeiro
//        /// <summary>
//        /// Propriedade usada para informar se o controle é o primeiro campo na tela para o controle de AutoTabIndex se localizar melhor.<br/>
//        /// Valor padrão verdadeiro.
//        /// </summary>
//        public bool Primeiro
//        {
//            get { EnsureChildControls(); return _txtCpf.Primeiro; }
//            set { EnsureChildControls(); _txtCpf.Primeiro = value; }
//        }
//        #endregion

//        #region MensagemErroInvalidoSummary
//        /// <summary>
//        /// Sem funcionalidade. Obrigado a implementar por causa da Interface.
//        /// </summary>
//        public string MensagemErroInvalidoSummary
//        {
//            get
//            {
//                EnsureChildControls();
//                return _txtCpf.MensagemErroInvalidoSummary;
//            }
//            set
//            {
//                EnsureChildControls();
//                _txtCpf.MensagemErroInvalidoSummary = value;
//            }
//        }
//        #endregion

//        #region DadosFormularioReceita
//        private FormularioReceita DadosFormularioReceita
//        {
//            get
//            {
//                if (ViewState["FormularioReceita"] == null)
//                    ViewState["FormularioReceita"] = new FormularioReceita(this.Page);

//                return ViewState["FormularioReceita"] as FormularioReceita;
//            }
//        }
//        #endregion

//        #region ValidarReceitaFederal
//        /// <summary>
//        /// Exibir validação Receita Federal
//        /// </summary>
//        public Boolean ValidarReceitaFederal
//        {
//            get
//            {
//                if (this.ViewState["ValidarReceitaFederal"] == null)
//                    return false;

//                return (Boolean)this.ViewState["ValidarReceitaFederal"];
//            }
//            set
//            {
//                this.ViewState["ValidarReceitaFederal"] = value;
//                this.MostrarValidadorReceitaFederal();
//            }
//        }
//        #endregion

//        #region ExibirValidadorReceitaFederal
//        /// <summary>
//        /// Exibição da validação por captcha
//        /// </summary>
//        private Boolean ExibirValidadorReceitaFederal
//        {
//            get
//            {
//                if (this.ViewState["ExibirValidadorReceitaFederal"] == null)
//                    return false;

//                return (Boolean)this.ViewState["ExibirValidadorReceitaFederal"];
//            }
//            set
//            {
//                this.ViewState["ExibirValidadorReceitaFederal"] = value;
//            }
//        }
//        #endregion

//        #region DadosCpfReceita
//        /// <summary>
//        /// Retorna os dados do Cpf recuperados a partir da Receita Federal
//        /// </summary>
//        public DadosCpfReceitaFederal DadosCpfReceita
//        {
//            get;
//            private set;
//        }
//        #endregion

//        #region CssClassLinha
//        /// <summary>
//        /// Classe css para as linhas do controle
//        /// </summary>
//        public String CssClassLinha
//        {
//            get
//            {
//                return Convert.ToString(this.ViewState["CssClassLinha"]);
//            }
//            set
//            {
//                this.ViewState["CssClassLinha"] = value;
//            }
//        }
//        #endregion

//        #region DtaNascimento
//        /// <summary>
//        /// Data de nascimento a informar para validar o captcha
//        /// </summary>
//        public DateTime? DataDeNascimento
//        {
//            get { EnsureChildControls(); return _txtDtaNas.ValorDateTime; }
//            set { EnsureChildControls(); _txtDtaNas.ValorDateTime = value; }
//        }
//        #endregion

//        #region CssClassLinha
//        /// <summary>
//        /// Classe css para as linhas do controle
//        /// </summary>
//        public String CssClassLinhaDataNascimentoCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return _pnlLinhaData.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _pnlLinhaData.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassLabelCaptcha
//        /// <summary>
//        /// Classe css da label de captcha
//        /// </summary>
//        public String CssClassLabelCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return _lblCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _lblDtaNasc.CssClass =
//                _lblCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassBotaoEnviar
//        /// <summary>
//        /// Classe css do botão enviar
//        /// </summary>
//        public String CssClassBotaoEnviar
//        {
//            get
//            {
//                EnsureChildControls();
//                return _btnEnviar.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _btnEnviar.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassBotaoRenovarCaptcha
//        /// <summary>
//        /// Classe css do botão Renovar Captcha
//        /// </summary>
//        public String CssClassBotaoRenovarCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._btnRenovaCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._btnRenovaCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassBotaoValidarReceita
//        /// <summary>
//        /// Classe css do botão validar receita
//        /// </summary>
//        public String CssClassBotaoValidarReceita
//        {
//            get
//            {
//                EnsureChildControls();
//                return _btnValidarReceita.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _btnValidarReceita.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassImagem
//        /// <summary>
//        /// Classe css da imagem de captcha
//        /// </summary>
//        public String CssClassImagem
//        {
//            get
//            {
//                EnsureChildControls();
//                return _imgCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _imgCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassBotaoFechar
//        /// <summary>
//        /// Classe css da imagem de captcha
//        /// </summary>
//        public String CssClassBotaoFechar
//        {
//            get
//            {
//                EnsureChildControls();
//                return _btnFechar.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _btnFechar.CssClass = value;
//            }
//        }
//        #endregion
//        #region CssClassTextBoxCaptcha
//        /// <summary>
//        /// Classe css do textbox do captcha
//        /// </summary>
//        public String CssClassTextBoxCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return _txtCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _txtDtaNas.CssClass =
//                _txtCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassTextBoxCpf
//        /// <summary>
//        /// Classe css do textbox do captcha
//        /// </summary>
//        public String CssClassTextBoxCpf
//        {
//            get
//            {
//                EnsureChildControls();
//                return _txtCpf.CssClassTextBox;
//            }
//            set
//            {
//                EnsureChildControls();
//                _txtCpf.CssClassTextBox = value;
//            }
//        }
//        #endregion

//        #region CssClassPainelCaptcha
//        /// <summary>
//        /// Classe css do textbox do captcha
//        /// </summary>
//        public String CssClassPainelCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return _pnlCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _pnlCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassLabelErroCaptcha
//        /// <summary>
//        /// Classe css do textbox do captcha
//        /// </summary>
//        public String CssClassLabelErroCaptcha
//        {
//            get
//            {
//                EnsureChildControls();
//                return _lblErrosCaptcha.CssClass;
//            }
//            set
//            {
//                EnsureChildControls();
//                _lblErrosDta.CssClass =
//                _lblErrosCaptcha.CssClass = value;
//            }
//        }
//        #endregion

//        #region CssClassErroCpf
//        /// <summary>
//        /// Classe css do validador
//        /// </summary>
//        public String CssClassErroCpf
//        {
//            set
//            {
//                EnsureChildControls();
//                _txtCpf.CssMensagemErro = value;
//            }
//        }
//        #endregion

//        #region Obrigatorio
//        /// <summary>
//        /// Determina se o campo é ou não obritatório
//        /// </summary>
//        public Boolean Obrigatorio
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.Obrigatorio;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.Obrigatorio = value;
//            }
//        }
//        #endregion

//        #region ValidationGroup
//        /// <summary>
//        /// O grupo de validação a qual este controle pertence
//        /// </summary>
//        public String ValidationGroup
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.ValidationGroup;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.ValidationGroup = value;
//            }
//        }
//        #endregion

//        #region NumeroCpf
//        /// <summary>
//        /// O numero de Cpf formatado em casas decimais que foi selecionado. (Null caso o campo esteja vazio)
//        /// </summary>
//        public Decimal? NumeroCpf
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.ValorDecimal;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.ValorDecimal = value;
//                if (value != null)
//                {
//                    this._txtCpf.Validar();
//                }
//            }
//        }
//        #endregion

//        #region MensagemInvalido
//        /// <summary>
//        /// Mensagem exibida na validação do CPF
//        /// </summary>
//        public String MensagemInvalido
//        {
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.MensagemErroFormato = value;
//            }

//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.MensagemErroFormato;
//            }
//        }
//        #endregion

//        #region MensagemErroFormatoSummary
//        /// <summary>
//        /// Mensagem de erro de formato apresentada no sumário.
//        /// </summary>
//        public string MensagemErroFormatoSummary
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.MensagemErroFormatoSummary;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.MensagemErroFormatoSummary = value;
//            }
//        }
//        #endregion

//        #region MensagemErroObrigatorioSummary
//        /// <summary>
//        /// Mensagem de erro de campo obrigatório apresentada no sumário.
//        /// </summary>
//        public string MensagemErroObrigatorioSummary
//        {
//            get
//            {
//                EnsureChildControls();
//                return this._txtCpf.MensagemErroObrigatorioSummary;
//            }
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.MensagemErroObrigatorioSummary = value;
//            }
//        }
//        #endregion

//        #region MensagemObrigatorio
//        /// <summary>
//        /// Mensagem exibida na validação do CPF
//        /// </summary>
//        public String MensagemObrigatorio
//        {
//            set
//            {
//                EnsureChildControls();
//                this._txtCpf.MensagemErroObrigatorio = value;
//            }
//        }
//        #endregion

//        #region MensagemCaptchaInvalido
//        /// <summary>
//        /// Mensagem exibida na validação do captcha
//        /// </summary>
//        public String MensagemCaptchaInvalido
//        {
//            private get
//            {
//                return this._mensagemCaptchaInvalido;
//            }
//            set
//            {
//                this._mensagemCaptchaInvalido = value;
//            }
//        }
//        #endregion

//        #region BotaoFecharUrl
//        /// <summary>
//        /// Imagem do botão fechar da validação do captcha
//        /// </summary>
//        public String BotaoFecharUrl
//        {
//            set
//            {
//                EnsureChildControls();
//                _btnFechar.ImageUrl = value;
//            }
//        }
//        #endregion       

//        #region Enabled
//        /// <summary>
//        /// Habilita ou desabilita o campo
//        /// </summary>
//        public new bool Enabled
//        {
//            get
//            {
//                EnsureChildControls();
//                return _txtCpf.Enabled;
//            }
//            set
//            {
//                EnsureChildControls();
//                _txtCpf.Enabled = value;
//            }
//        }
//        #endregion

//        #region TabIndex
//        /// <inheritdoc/>
//        public override short TabIndex
//        {
//            get
//            {
//                EnsureChildControls();
//                return _txtCpf.TabIndex;
//            }
//            set
//            {
//                EnsureChildControls();
//                _txtCpf.TabIndex = value;
//            }
//        }
//        #endregion

//        #endregion

//        #region Metodos
//        #region CreateChildControls
//        /// <summary>
//        /// Cria os controles filhos
//        /// </summary>
//        protected override void CreateChildControls()
//        {
//            this.InicializarTxtCpf();
//            this.InicializarCaptcha();
//            this.InicializarTxtCaptcha();
//            this.InicializarLinkButton();
//            this.InicializarLabels();
//            this.InicializarPanels();

//            MostrarValidadorReceitaFederal();

//            this.Controls.Add(_upComponente);
//            base.CreateChildControls();
//        }
//        #endregion

//        #region CriarLinhaCaptcha
//        /// <summary>
//        /// Cria uma linha preenchida por controles
//        /// </summary>
//        /// <returns>A div contendo a linha criada</returns>
//        private HtmlGenericControl CriarLinha(WebControl txtCpf, WebControl btnValidarReceita)
//        {
//            HtmlGenericControl linha = new HtmlGenericControl("div");
//            if (!String.IsNullOrEmpty(this.CssClassLinha))
//                linha.Attributes["class"] = this.CssClassLinha;

//            linha.Controls.Add(txtCpf);

//            linha.Controls.Add(btnValidarReceita);

//            return linha;
//        }



//        /// <summary>
//        /// Cria uma linha de controles contendo um botão 
//        /// </summary>
//        /// <param name="lblCaptcha">O label do captcha</param>
//        /// <param name="txtCaptcha">O textbox do captcha</param>
//        /// <param name="btnEnviar">O botão enviar</param>
//        /// <returns>A div contendo a linha criada</returns>
//        private HtmlGenericControl CriarLinhaCaptcha(WebControl txtCaptcha, WebControl lblCaptcha, WebControl btnEnviar)
//        {
//            HtmlGenericControl linha = new HtmlGenericControl("div");
//            if (!String.IsNullOrEmpty(this.CssClassLinha))
//                linha.Attributes["class"] = this.CssClassLinha;

//            HtmlGenericControl ctlDescricao = new HtmlGenericControl("div");
//            ctlDescricao.Controls.Add(_lblErrosCaptcha);
//            ctlDescricao.Controls.Add(lblCaptcha);
//            ctlDescricao.Controls.Add(txtCaptcha);
//            ctlDescricao.Controls.Add(btnEnviar);
//            linha.Controls.Add(ctlDescricao);

//            return linha;
//        }

//        /// <summary>
//        /// Cria uma linha de controles contendo um botão 
//        /// </summary>
//        /// <returns>A div contendo a linha criada</returns>
//        private HtmlGenericControl CriarLinha(WebControl imagem, string css)
//        {
//            HtmlGenericControl linha = new HtmlGenericControl("div");
//            if (!String.IsNullOrEmpty(css))
//                linha.Attributes["class"] = css;

//            //HtmlGenericControl ctlImagem = new HtmlGenericControl("div");
//            //ctlImagem.Controls.Add(imagem);
//            linha.Controls.Add(imagem);

//            return linha;
//        }

//        #endregion

//        #region Inicializar

//        #region InicializarCaptcha
//        /// <summary>
//        /// Inicializa a imagem de captcha
//        /// </summary>
//        private void InicializarCaptcha()
//        {
//            this._imgCaptcha.ID = "img_Captcha";

//            //@"~/a.caprf?vr=" + this.ClientID.Replace("_", String.Empty).Replace("$", String.Empty) + "&v=" + DateTime.Now.ToString("hhMMss");
//            this._imgCaptcha.Width = Unit.Pixel(100);
//            this._imgCaptcha.Height = Unit.Pixel(50);
//        }
//        #endregion

//        #region InicializarTxtCpf
//        /// <summary>
//        /// Inicializa o textbox de Cpf
//        /// </summary>
//        private void InicializarTxtCpf()
//        {
//            this._txtCpf.ID = "txtCpf";
//            this._txtCpf.CausesValidation = false;
//            this._txtCpf.Width = 100;
//            //this._txtCpf.Tamanho = 14;
//            //if (ComponenteCpfHabilitado.HasValue && ComponenteCpfHabilitado.Value == false)
//            //    this._txtCpf.Enabled = false;
//        }
//        #endregion

//        #region InicializarTxtCaptcha
//        /// <summary>
//        /// Inicializa o textbox de captcha
//        /// </summary>
//        private void InicializarTxtCaptcha()
//        {
//            this._txtCaptcha.ID = "txtCaptcha";
//            this._txtCaptcha.Columns = 9;
//            //this._txtCaptcha.Tamanho = 10;
//            this._txtCaptcha.MaxLength = 10;
//            this._txtCaptcha.AutoPostBack = false;
//            this._txtCaptcha.CausesValidation = false;
//        }
//        #endregion

//        #region InicializarLinkButton
//        /// <summary>
//        /// Inicializa o botão de enviar
//        /// </summary>
//        private void InicializarLinkButton()
//        {
//            this._btnEnviar.ID = "btnConsultar";
//            this._btnEnviar.Text = "Consultar";
//            this._btnEnviar.CausesValidation = false;

//            this._btnValidarReceita.ID = "txtCpf_btnValidarReceita";
//            this._btnValidarReceita.Text = "Validar na Receita Federal";
//            this._btnValidarReceita.CausesValidation = false;
//            _btnValidarReceita.TabIndex = -1;
//        }
//        #endregion

//        #region InicializarLabels
//        /// <summary>
//        /// Inicializa todas as labels
//        /// </summary>
//        private void InicializarLabels()
//        {
//            this._lblCaptcha.ID = "lblCaptcha";
//            this._lblCaptcha.Text = "Captcha";

//            _lblDtaNasc.ID = "lblDtaNasc";
//            _lblDtaNasc.Text = "Data Nascimento";

//            this._lblErrosCaptcha.ID = "lblErrosCaptcha";
//            this._lblErrosCaptcha.Text = String.Empty;

//            _lblErrosDta.ID = "lblErrosDta";

//            _lblErrosDta.EnableViewState =
//            _lblErrosCaptcha.EnableViewState = false;

//            this._lblCaptcha.AssociatedControlID = this._txtCaptcha.ID;
//            _lblDtaNasc.AssociatedControlID = this._txtDtaNas.ID;
//        }
//        #endregion

//        #region InicializarUpdatePanel
//        /// <summary>
//        /// Inicializa todas as labels
//        /// </summary>
//        private void InicializarPanels()
//        {
//            //TextBox CPF
//            _upCpf.ID = "upTxbCpfReceitaFederal";
//            _upCpf.UpdateMode = UpdatePanelUpdateMode.Conditional;

//            _pnlCpf.Controls.Add(CriarLinha(_txtCpf, _btnValidarReceita));
//            _upCpf.ContentTemplateContainer.Controls.Add(_pnlCpf);

//            _pnlComponente.Controls.Add(_upCpf);

//            if (!ExibirValidadorReceitaFederal)
//                _pnlCaptcha.Visible = false;

//            _pnlCaptcha.Controls.Add(_btnFechar);

//            #region Linha Data
//            HtmlGenericControl ctlDescricao = new HtmlGenericControl("div");

//            ctlDescricao.Controls.Add(_lblErrosDta);
//            ctlDescricao.Controls.Add(_lblDtaNasc);
//            ctlDescricao.Controls.Add(_txtDtaNas);
//            _pnlLinhaData.Controls.Add(ctlDescricao);

//            _pnlCaptcha.Controls.Add(_pnlLinhaData);
//            #endregion

//            //_txtDtaNas
//            _pnlCaptcha.Controls.Add(CriarLinhaCaptcha(_txtCaptcha, _lblCaptcha, _btnEnviar));

//            var linha = CriarLinha(_imgCaptcha, this.CssClassLinha);
//            _pnlCaptcha.Controls.Add(linha);
//            linha.Controls.Add(_btnRenovaCaptcha);

//            _pnlComponente.Controls.Add(_pnlCaptcha);
//            _upComponente.ContentTemplateContainer.Controls.Add(_pnlComponente);
//        }
//        #endregion

//        #endregion

//        #region RevonaCaptcha
//        private void RevonaCaptcha()
//        {
//            ViewState["FormularioReceita"] = null;
//            InicializarCaptcha();

//            try
//            {
//                this._imgCaptcha.ImageUrl = DadosFormularioReceita.UrlImg;
//            }
//            catch (Exception ex)
//            {
//                string erro = "Não foi possível consultar o número de CPF na Receita Federal.";

//                try
//                {
//                    this._imgCaptcha.ImageUrl =
//                        Page.ClientScript.GetWebResourceUrl(
//                            typeof(CPFReceitaFederal), "Employer.Componentes.UI.Web.Content.Images.ReceitaFora.jpg");

//                    //TODO Implementar log de erro do BNE
//                    /*if (!isTimeOut(ex))
//                        LogError.WriteLog(ex, erro);*/
//                }
//                catch { }

//                this._lblErrosCaptcha.Text = erro;
//            }

//            if (string.IsNullOrEmpty(this._imgCaptcha.ImageUrl))
//            {
//                this._imgCaptcha.ImageUrl =
//                        Page.ClientScript.GetWebResourceUrl(
//                            typeof(CPFReceitaFederal), "Employer.Componentes.UI.Web.Content.Images.ReceitaFora.jpg");
//            }

//            if (string.IsNullOrEmpty(_btnFechar.ImageUrl))
//                _btnFechar.ImageUrl = Page.ClientScript.GetWebResourceUrl(
//                    this.GetType(), "Employer.Componentes.UI.Web.Content.Images.botao_padrao_fechar.png");

//            if (string.IsNullOrEmpty(_txtCpf.Text))
//                _btnValidarReceita.Style[HtmlTextWriterStyle.Display] = "none";
//            else
//                _btnValidarReceita.Style.Clear();

//            _upComponente.Update();
//        }
//        #endregion

//        #region OnInit
//        /// <inheritdoc/>
//        protected override void OnInit(EventArgs e)
//        {
//            this._btnEnviar.Click += new EventHandler(_btnEnviar_Click);
//            this._btnRenovaCaptcha.Click += new EventHandler(_btnRenovaCaptcha_Click);
//            this._txtCpf.ValorAlterado += new EventHandler(_txtCpf_ValorAlterado);
//            this._btnValidarReceita.Click += new EventHandler(_btnValidarReceita_Click);
//            this._btnFechar.Click += new ImageClickEventHandler(_btnFechar_Click);

//            base.OnInit(e);
//        }
//        #endregion

//        #region StripHtml
//        /// <summary>
//        /// Quebra o HTML para posterior processamento
//        /// </summary>
//        /// <param name="html">O texto html geral</param>
//        /// <returns>O texto html tratado</returns>
//        private static string StripHtml(string html)
//        {
//            html = System.Text.RegularExpressions.Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
//        }
//        #endregion

//        internal static bool isTimeOut(Exception ex)
//        {
//            return ex is System.Net.WebException && ((System.Net.WebException)ex).Status == WebExceptionStatus.Timeout;
//        }

//        #region RetornarPagina
//        /// <summary>
//        /// Recupera a página html do site da Receita Federal
//        /// </summary>
//        /// <returns></returns>
//        protected Boolean RetornarPagina()
//        {
//            // Declarações necessárias
//            //Stream requestStream = null;
//            WebResponse response = null;
//            //StreamReader reader = null;

//            try
//            {
//                WebRequest request =
//                    //WebRequest.Create("http://www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/ConsultaPublicaExibir.asp");//Antigo
//                    WebRequest.Create("https://www.receita.fazenda.gov.br/Aplicacoes/SSL/ATCTA/CPF/ConsultaPublicaExibir.asp");

//                request.Method = WebRequestMethods.Http.Post;
//                request.Timeout = 1000;
//                request.Headers.Add("Cookie", DadosFormularioReceita.SessionId);

//                // Neste ponto, você está setando a propriedade ContentType da página 
//                // para urlencoded para que o comando POST seja enviado corretamente
//                // request.ContentLength = 2055;
//                request.ContentType = "application/x-www-form-urlencoded";

//                StringBuilder urlEncoded = new StringBuilder();

//                // alocando o bytebuffer
//                byte[] byteBuffer = null;

//                //urlEncoded.Append("Enviar=Consultar&");

//                urlEncoded.AppendFormat("txtTexto_captcha_serpro_gov_br={0}&", this._txtCaptcha.Text);
//                urlEncoded.AppendFormat("tempTxtCPF={0}&", _txtCpf.Text);
//                urlEncoded.AppendFormat("tempTxtNascimento={0}&", HttpUtility.UrlEncode(_txtDtaNas.Text));
//                urlEncoded.AppendFormat("temptxtToken_captcha_serpro_gov_br=&temptxtTexto_captcha_serpro_gov_br={0}&", this._txtCaptcha.Text);

//                // codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais
//                byteBuffer = Encoding.UTF8.GetBytes(urlEncoded.ToString());

//                request.ContentLength = byteBuffer.Length;
//                string q;
//                StringBuilder Dados = new StringBuilder();

//                using (var requestStream = request.GetRequestStream())
//                {
//                    requestStream.Write(byteBuffer, 0, byteBuffer.Length);
//                    requestStream.Close();

//                    // Dados recebidos 
//                    response = request.GetResponse();

//                    if ((response as HttpWebResponse).StatusCode != HttpStatusCode.OK)
//                    {
//                        throw new Exception("Falha de Comunicação com o Site da Receita Federal");
//                    }

//                    using (Stream responseStream = response.GetResponseStream())
//                    {
//                        // Codifica os caracteres especiais para que possam ser exibidos corretamente
//                        System.Text.Encoding encoding = System.Text.Encoding.Default;

//                        // Preenche o reader
//                        using (var reader = new StreamReader(responseStream, encoding))
//                        {
//                            Char[] charBuffer = new Char[256];
//                            int count = reader.Read(charBuffer, 0, charBuffer.Length);

//                            // Lê cada byte para preencher meu stringbuilder
//                            while (count > 0)
//                            {
//                                Dados.Append(new String(charBuffer, 0, count));
//                                count = reader.Read(charBuffer, 0, charBuffer.Length);
//                            }

//                            q = StripHtml(Dados.ToString());

//                            // Fecha tudo                  
//                            if (response != null)
//                                response.Close();
//                        }
//                    }
//                }

//                if (q.IndexOf("Erro na Consulta", StringComparison.OrdinalIgnoreCase) > -1 || Dados.ToString().IndexOf("Favor informar os dados abaixo") > -1)
//                {
//                    this._lblErrosCaptcha.Text = String.IsNullOrEmpty(this.MensagemCaptchaInvalido) ? "Captcha Inválido" : MensagemCaptchaInvalido;
//                    return false;
//                }

//                if (q.IndexOf("Verifique se o mesmo foi digitado corretamente", StringComparison.OrdinalIgnoreCase) > -1)
//                {
//                    this._lblErrosCaptcha.Text = "Cpf Inválido ou Inexistente";
//                    return false;
//                }

//                if (Dados.ToString().IndexOf("não existe em nossa base de dados.") > -1)
//                {
//                    this._lblErrosCaptcha.Text = "CPF informado não existe na Receita Federal.";
//                    return false;
//                }

//                this.DadosCpfReceita = new DadosCpfReceitaFederal(q);

//                if (CpfEncontrado != null)
//                {
//                    CpfEncontrado(this, this.DadosCpfReceita);
//                }

//                return true;

//            }
//            catch (Exception ex)
//            {
//                string erro = "Não foi possível consultar o número de CPF na Receita Federal.";

//                //TODO Implementar log de erro do bne
//                /*
//                if (!isTimeOut(ex))
//                    LogError.WriteLog(ex, erro);*/

//                this._imgCaptcha.ImageUrl =
//                        Page.ClientScript.GetWebResourceUrl(
//                            typeof(CPFReceitaFederal), "Employer.Componentes.UI.Web.Content.Images.ReceitaFora.jpg");

//                this._lblErrosCaptcha.Text = erro;
//                return false;
//            }
//        }
//        #endregion

//        #region MostrarValidadorReceitaFederal
//        /// <summary>
//        /// Define as regras para exibir o formulário total ou parcial
//        /// </summary>
//        private void MostrarValidadorReceitaFederal()
//        {
//            if (ValidarReceitaFederal && ExibirValidadorReceitaFederal)
//            {
//                _btnValidarReceita.Enabled = false;

//                _btnFechar.Visible =
//                _btnRenovaCaptcha.Visible =
//                _txtCaptcha.Visible =
//                _imgCaptcha.Visible =
//                _btnEnviar.Visible =
//                _lblDtaNasc.Visible =
//                _lblCaptcha.Visible =
//                _pnlCaptcha.Visible = true;
//            }
//            else if (!ValidarReceitaFederal)
//            {
//                _btnValidarReceita.Visible = false;

//                _btnFechar.Visible = false;
//                _btnRenovaCaptcha.Visible = false;
//                _txtCaptcha.Visible = false;
//                _imgCaptcha.Visible = false;
//                _btnEnviar.Visible = false;
//                _lblDtaNasc.Visible =
//                _lblCaptcha.Visible = false;
//                _pnlCaptcha.Visible = false;
//            }
//            else if (ValidarReceitaFederal && !ExibirValidadorReceitaFederal)
//            {
//                _btnValidarReceita.Visible = true;

//                _btnFechar.Visible = false;
//                _btnRenovaCaptcha.Visible = false;
//                _txtCaptcha.Visible = false;
//                _imgCaptcha.Visible = false;
//                _btnEnviar.Visible = false;
//                _lblDtaNasc.Visible =
//                _lblCaptcha.Visible = false;
//                _pnlCaptcha.Visible = false;
//            }
//        }

//        #endregion

//        #region Limpar
//        /// <summary>
//        /// Limpa os campos
//        /// </summary>
//        public void Limpar()
//        {
//            _txtCpf.Text = String.Empty;
//            _txtCaptcha.Text = String.Empty;
//            ExibirValidadorReceitaFederal = false;
//            MostrarValidadorReceitaFederal();
//        }
//        #endregion

//        #region OcultarCaptcha
//        /// <summary>
//        /// Método utilizado externamente para ocultar o captcha
//        /// </summary>
//        public void OcultarCaptcha()
//        {
//            ExibirValidadorReceitaFederal = false;
//            MostrarValidadorReceitaFederal();
//            _btnValidarReceita.Enabled = true;
//            this._txtCaptcha.Text = String.Empty;
//            _upComponente.Update();
//        }
//        #endregion

//        /// <inheritdoc/>
//        public override void Focus()
//        {
//            if (this._txtCpf.Enabled)
//                this._txtCpf.Focus();
//        }
//        #endregion

//        #region Eventos

//        void _btnRenovaCaptcha_Click(object sender, EventArgs e)
//        {
//            this.RevonaCaptcha();

//            MostrarValidadorReceitaFederal();
//            this._txtCaptcha.Text = String.Empty;
//            ScriptManager.GetCurrent(this.Page).SetFocus(this._txtCaptcha);
//            _upComponente.Update();
//        }

//        #region _btnEnviar_Click
//        /// <summary>
//        /// Tratador de eventos do botão que envia os dados para o site da Receita Federal
//        /// </summary>
//        /// <param name="sender">O objeto que enviou o evento</param>
//        /// <param name="e">Os parâmetros do evento</param>
//        void _btnEnviar_Click(object sender, EventArgs e)
//        {
//            this._lblErrosCaptcha.Text = String.Empty;

//            if (String.IsNullOrEmpty(this._txtCpf.Text))
//            {
//                this._lblErrosCaptcha.Text = "Informe um Cpf";
//                return;
//            }

//            if (String.IsNullOrEmpty(this._txtCaptcha.Text))
//            {
//                this._lblErrosCaptcha.Text = "Informe o Captcha";
//                return;
//            }

//            if (!this._txtDtaNas.ValorDateTime.HasValue)
//            {
//                this._lblErrosDta.Text = "Informe a data de nascimento";
//                return;
//            }

//            if (!Validacao.ValidarCPF(_txtCpf.Text))
//            {
//                _txtCpf.Validar();
//                return;
//            }

//            if (RetornarPagina())
//            {
//                ExibirValidadorReceitaFederal = false;
//                MostrarValidadorReceitaFederal();
//                this._txtCaptcha.Text = String.Empty;
//                _btnValidarReceita.Enabled = true;
//                _upComponente.Update();
//            }
//            else
//            {
//                _btnRenovaCaptcha_Click(sender, e);
//            }
//        }
//        #endregion

//        #region _btnValidarReceita_Click
//        void _btnValidarReceita_Click(object sender, EventArgs e)
//        {
//            this.RevonaCaptcha();

//            ExibirValidadorReceitaFederal = true;
//            MostrarValidadorReceitaFederal();
//            ScriptManager.GetCurrent(this.Page).SetFocus(this._txtCaptcha);
//            _upComponente.Update();
//        }
//        #endregion

//        #region _txtCpf_ValorAlterado
//        /// <summary>
//        /// Tratador de evento de valor alterado da textbox de Cpf
//        /// </summary>
//        /// <param name="sender">O objeto que enviou o evento</param>
//        /// <param name="e">Os parâmetros do evento</param>
//        void _txtCpf_ValorAlterado(object sender, EventArgs e)
//        {
//            if (this.ValorAlterado != null)
//                ValorAlterado(this, e);
//        }
//        #endregion

//        #region _btnFechar_Click
//        void _btnFechar_Click(object sender, ImageClickEventArgs e)
//        {
//            _btnValidarReceita.Enabled = true;
//            _txtCaptcha.Text = String.Empty;
//            ExibirValidadorReceitaFederal = false;
//            MostrarValidadorReceitaFederal();
//        }
//        #endregion

//        #endregion

//        #region FormularioReceita
//        /// <summary>
//        /// Representa os dados do formulário html.
//        /// É utilizado para manter os dados de captha, sessão e etc.
//        /// </summary>
//        [Serializable]
//        private class FormularioReceita
//        {
//            private string _UrlImg;
//            //private string _Token;
//            private string _SessionId;
//            [NonSerialized]
//            private Page _pagina;

//            public string SessionId
//            {
//                get { return _SessionId; }
//            }

//            public string UrlImg
//            {
//                get { return _UrlImg; }
//            }

//            public FormularioReceita(Page pagina)
//            {
//                _pagina = pagina;

//                try
//                {
//                    LerDados();
//                    LerHtml();
//                }
//                catch (Exception ex)
//                {
//                    //TODO Implementar log de erro do bne
//                    /*
//                        if (!isTimeOut(ex))
//                            LogError.WriteLog(ex, "Erro de comunicação com a Receita Federal");
//                            */
//                    this._pagina.Session[HandlerCaptchaReceitaFederal.ChaveCaptchaReceitaFederalPF] = CatchaForaDoAr();
//                }
//            }

//            private byte[] CatchaForaDoAr()
//            {
//                using (MemoryStream ms = new MemoryStream())
//                using (var s = typeof(CPFReceitaFederal).Assembly.GetManifestResourceStream
//                    ("Employer.Componentes.UI.Web.Content.Images.ReceitaFora.jpg"))
//                {
//                    byte[] b = new byte[32768];
//                    int r;
//                    while ((r = s.Read(b, 0, b.Length)) > 0)
//                        ms.Write(b, 0, r);
//                    return ms.ToArray();
//                }
//            }

//            public void LerHtml()
//            {
//                this._pagina.Session[HandlerCaptchaReceitaFederal.ChaveCaptchaReceitaFederalPF] = PostCatcha();

//                _UrlImg = string.Format("{0}.caprf?tipo=pf", Guid.NewGuid());
//            }

//            private byte[] PostCatcha()
//            {
//                WebRequest request = WebRequest.Create("https://www.receita.fazenda.gov.br/Aplicacoes/SSL/ATCTA/CPF/captcha/gerarCaptcha.asp?data=" +

//                DateTime.Now.Ticks.ToString());

//                request.Method = WebRequestMethods.Http.Get;
//                request.Timeout = 1000;
//                request.Headers.Add("Cookie", SessionId);

//                // Dados recebidos 
//                using (var response = request.GetResponse())
//                {
//                    if ((response as HttpWebResponse).StatusCode != HttpStatusCode.OK)
//                        throw new Exception("Falha de Comunicação com o Site da Receita Federal");

//                    using (MemoryStream ms = new MemoryStream())
//                    using (Stream responseStream = response.GetResponseStream())
//                    {
//                        byte[] b = new byte[32768];
//                        int r;
//                        while ((r = responseStream.Read(b, 0, b.Length)) > 0)
//                            ms.Write(b, 0, r);
//                        return ms.ToArray();
//                    }
//                }
//            }

//            #region RetornarCookiesSolicitacao
//            private string LerStream(Stream receiveStream)
//            {
//                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

//                StringBuilder Dados = new StringBuilder();

//                using (StreamReader readStream = new StreamReader(receiveStream, encode))
//                {
//                    Char[] read = new Char[256];
//                    int count = readStream.Read(read, 0, 256);

//                    while (count > 0)
//                    {
//                        Dados.Append(new String(read, 0, count));
//                        count = readStream.Read(read, 0, 256);
//                    }

//                    return Dados.ToString();
//                }
//            }

//            /// <summary>
//            /// Retorna o valor dos cookies enviados na solicitação ao site da Receita Federal
//            /// </summary>
//            /// <returns>O valor do cookie de segurança</returns>
//            private String LerDados()
//            {
//                Uri uri = new Uri("https://www.receita.fazenda.gov.br/Aplicacoes/SSL/ATCTA/CPF/ConsultaPublica.asp?data=" +
//                    DateTime.Now.Ticks.ToString());

//                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
//                httpWebRequest.Timeout = 1000;

//                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

//                String cookies = String.Empty;

//                using (Stream receiveStream = httpWebResponse.GetResponseStream())
//                {
//                    _SessionId = httpWebResponse.Headers["Set-Cookie"].ToString();

//                    return LerStream(receiveStream);
//                }
//            }
//            #endregion
//        }
//        #endregion
//    }
//}
