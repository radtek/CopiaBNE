using BNE.Componentes.Extensions;
using BNE.Componentes.Handlers;
using BNE.Componentes.Interface;
using BNE.Componentes.Util;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Componentes
{
    /// <summary>
    /// Componente que recupera os dados do cartão CNPJ do site da receita federal
    /// </summary>
    public class ControlCNPJReceitaFederal : CompositeControl, IRequiredField, IErrorMessage
    {

        #region Fields
        private Image _imgCaptcha = new Image { ID = "img_Captcha" };
        private ControlLabel _lblCNPJ = new ControlLabel { ID = "lblCnpj" };
        //private ControlCNPJ _txtCNPJ = new ControlCNPJ();
        private ControlCNPJ _txtCNPJ = new ControlCNPJ { ID = "txtCNPJ_fjkfjfkj" };
        private ControlLabel _lblCaptcha = new ControlLabel { ID = "lblCaptcha" };
        private ControlAlfaNumerico _txtCaptcha = new ControlAlfaNumerico { ID = "txtCaptcha" };
        private ControlButton _btnEnviar = new ControlButton { ID = "btnEnviar" };
        private ControlLabel _lblErrosCaptcha = new ControlLabel { ID = "lblErros" };

        private ControlButton _btnValidarReceita = new ControlButton { ID = "btnValidarReceita" };
        private UpdatePanel _upComponente = new UpdatePanel { ID = "upCnpjReceitaFederal", UpdateMode = UpdatePanelUpdateMode.Conditional };
        private UpdatePanel _upCnpj = new UpdatePanel { ID = "upTxbCnpjReceitaFederal" };
        private ControlPanel _pnlComponente = new ControlPanel { ID = "pnlComponente" };
        private ControlPanel _pnlCnpj = new ControlPanel { ID = "pnlCnpj" };
        private ControlPanel _pnlCaptcha = new ControlPanel { ID = "pnlCaptcha" };

        private ControlImageButton _btnFechar = new ControlImageButton { CausesValidation = false, ID = "btnFechar" };
        private ControlButton _btnRenovaCaptcha = new ControlButton { CausesValidation = false, ID = "btnRenovaCaptcha", ToolTip = "Renovar Captcha" };
        #endregion

        #region Eventos
        /// <summary>
        /// Handler de CNPJ encontrado
        /// </summary>
        /// <param name="sender">Objeto que enviou o evento</param>
        /// <param name="dadosCnpj">Os dados do CNPJ selecionado</param>
        public delegate void CnpjEncontradoHandler(object sender, DadosCNPJReceitaFederal dadosCnpj);
        /// <summary>
        /// Evento disparado quando um CNPJ é recuperado da receita federal
        /// </summary>
        public event CnpjEncontradoHandler CnpjEncontrado;
        /// <summary>
        /// Evento disparado quando o valor do campo CNPJ é alterado
        /// </summary>
        public event EventHandler ValorAlterado;
        /// <summary>
        /// Handler para problema de comunicacao
        /// </summary>
        /// <param name="sender">Objeto que enviou o evento</param>
        public delegate void ProblemaComunicacaoHandler(object sender, Exception ex);
        /// <summary>
        /// Evento disparado quando houve um problema na comunicação com o site da receita federal
        /// </summary>
        public event ProblemaComunicacaoHandler ProblemaComunicacao;
        #endregion

        #region Properties

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [Category("TextBox"), DisplayName("ReadOnly")]
        public bool ReadOnly
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.ReadOnly;
            }

            set
            {
                EnsureChildControls();
                _txtCNPJ.ReadOnly = value;
            }
        }
        #endregion

        private FormularioReceita DadosFormularioReceita
        {
            get
            {
                if (ViewState["FormularioReceita"] == null)
                    ViewState["FormularioReceita"] = new FormularioReceita(this.Page);

                return ViewState["FormularioReceita"] as FormularioReceita;
            }
        }

        #region DadosCnpjReceita
        /// <summary>
        /// Retorna os dados do CNPJ recuperados a partir da Receita Federal
        /// </summary>
        public DadosCNPJReceitaFederal DadosCnpjReceita
        {
            get;
            private set;
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Determina se o campo é ou não obritatório
        /// </summary>
        public Boolean Obrigatorio
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.Obrigatorio;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.Obrigatorio = value;
            }
        }
        #endregion

        #region ValidationGroup
        /// <summary>
        /// O grupo de validação a qual este controle pertence
        /// </summary>
        public String ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return this._txtCNPJ.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                this._txtCNPJ.ValidationGroup = value;
            }
        }
        #endregion

        #region numeroCNPJ
        /// <summary>
        /// O numero de cnpj formatado em casas decimais que foi selecionado. (Null caso o campo esteja vazio)
        /// </summary>
        public Decimal? numeroCNPJ
        {
            get
            {
                EnsureChildControls();
                return this._txtCNPJ.ValorDecimal;
            }
            set
            {
                EnsureChildControls();
                this._txtCNPJ.ValorDecimal = value;
            }
        }
        #endregion

        #region Styles

        #region CssClassPainelCaptcha
        /// <summary>
        /// Classe css do textbox do captcha
        /// </summary>
        public String CssClassPainelCaptcha
        {
            get
            {
                EnsureChildControls();
                return _pnlCaptcha.CssClass;
            }
            set
            {
                EnsureChildControls();
                _pnlCaptcha.CssClass = value;
            }
        }
        #endregion

        #region CssClassBotaoFechar
        /// <summary>
        /// Classe css da imagem de captcha
        /// </summary>
        public String CssClassBotaoFechar
        {
            get
            {
                return Convert.ToString(this.ViewState["CssClassBotaoFechar"]);
            }
            set
            {
                this.ViewState["CssClassBotaoFechar"] = value;
            }
        }
        #endregion

        #region CssClassImagemCaptcha
        public String CssClassImagemCaptcha
        {
            set
            {
                _imgCaptcha.CssClass = value;
            }
        }
        #endregion

        #region CssClassLinha
        /// <summary>
        /// Classe css para as linhas do controle
        /// </summary>
        public String CssClassLinha
        {
            get
            {
                return Convert.ToString(this.ViewState["CssClassLinha"]);
            }
            set
            {
                this.ViewState["CssClassLinha"] = value;
            }
        }
        #endregion

        #region CssClassContainerCampo
        /// <summary>
        /// Classe Css para o container dos campos
        /// </summary>
        public String CssClassContainerCampo
        {
            get
            {
                return Convert.ToString(this.ViewState["CssClassContainerCampo"]);
            }
            set
            {
                this.ViewState["CssClassContainerCampo"] = value;
            }
        }
        #endregion

        #region CssClassLabelCNPJ
        /// <summary>
        /// Classe css da label de CNPJ
        /// </summary>
        public String CssClassLabelCNPJ
        {
            get
            {
                EnsureChildControls();
                return this._lblCNPJ.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._lblCNPJ.CssClass = value;
            }
        }
        #endregion

        #region CssClassLabelCaptcha
        /// <summary>
        /// Classe css da label de captcha
        /// </summary>
        public String CssClassLabelCaptcha
        {
            get
            {
                EnsureChildControls();
                return this._lblCaptcha.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._lblCaptcha.CssClass = value;
            }
        }
        #endregion

        #region CssClassLabelErros
        /// <summary>
        /// Classe css da label de erros
        /// </summary>
        public String CssClassLabelErros
        {
            get
            {
                EnsureChildControls();
                return this._lblErrosCaptcha.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._lblErrosCaptcha.CssClass = value;
            }
        }
        #endregion

        #region CssClassBotaoValidarReceita
        /// <summary>
        /// Classe css do botão validar receita
        /// </summary>
        public String CssClassBotaoValidarReceita
        {
            get
            {
                EnsureChildControls();
                return _btnValidarReceita.CssClass;
            }
            set
            {
                EnsureChildControls();
                _btnValidarReceita.CssClass = value;
            }
        }
        #endregion

        #region CssClassBotaoEnviar
        /// <summary>
        /// Classe css do botão enviar
        /// </summary>
        public String CssClassBotaoEnviar
        {
            get
            {
                EnsureChildControls();
                return this._btnEnviar.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._btnEnviar.CssClass = value;
            }
        }

        /// <summary>
        /// Classe css do botão Renovar Captcha
        /// </summary>
        public String CssClassBotaoRenovarCaptcha
        {
            get
            {
                EnsureChildControls();
                return this._btnRenovaCaptcha.CssClass;
            }
            set
            {
                EnsureChildControls();
                this._btnRenovaCaptcha.CssClass = value;
            }
        }
        #endregion

        #region CssClassCaptcha
        /// <summary>
        /// Classe css da imagem de captcha
        /// </summary>
        public String CssClassCaptcha
        {
            get
            {
                return Convert.ToString(this.ViewState["CssClassCaptcha"]);
            }
            set
            {
                this.ViewState["CssClassCaptcha"] = value;
            }
        }
        #endregion

        #region CssClassTextBoxCaptcha
        public String CssClassTextBoxCaptcha
        {
            set
            {
                EnsureChildControls();
                _txtCaptcha.CssClassTextBox = value;
            }
        }
        #endregion

        #region CssClassTextBoxCpf
        public string CssClassTextBoxCpf
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.CssClassTextBox;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.CssClassTextBox = value;
            }
        }
        #endregion

        #endregion

        #region ExibirValidadorReceitaFederal
        /// <summary>
        /// Exibição da validação por captcha
        /// </summary>
        public Boolean ExibirValidadorReceitaFederal
        {
            get
            {
                if (this.ViewState["ExibirValidadorReceitaFederal"] == null)
                    return false;

                return (Boolean)this.ViewState["ExibirValidadorReceitaFederal"];
            }
            set
            {
                this.ViewState["ExibirValidadorReceitaFederal"] = value;
                MostrarValidadorReceitaFederal();
            }
        }
        #endregion

        #region ValidarReceitaFederal
        /// <summary>
        /// Exibir validação Receita Federal
        /// </summary>
        public Boolean ValidarReceitaFederal
        {
            get
            {
                if (this.ViewState["ValidarReceitaFederal"] == null)
                    return true;

                return (Boolean)this.ViewState["ValidarReceitaFederal"];
            }
            set
            {
                this.ViewState["ValidarReceitaFederal"] = value;
                MostrarValidadorReceitaFederal();
            }
        }
        #endregion

        #region Enabled
        public new bool Enabled
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.Enabled;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.Enabled = value;
            }
        }
        #endregion

        #region BtnFecharImageUrl
        public String BtnFecharImageUrl
        {
            private get
            {
                return _btnFechar.ImageUrl;
            }
            set
            {
                _btnFechar.ImageUrl = value;
            }
        }
        #endregion

        #region TabIndex
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.TabIndex;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.TabIndex = value;
            }
        }
        #endregion

        #region Mensagem Summary
        public string MensagemErroFormato
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.MensagemErroFormato;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.MensagemErroFormato = value;
            }
        }

        public string MensagemErroFormatoSummary
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.MensagemErroFormatoSummary;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.MensagemErroFormatoSummary = value;
            }
        }

        public string MensagemErroInvalidoSummary
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.MensagemErroInvalidoSummary;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.MensagemErroInvalidoSummary = value;
            }
        }

        public string MensagemErroObrigatorio
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.MensagemErroObrigatorio;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.MensagemErroObrigatorio = value;
            }
        }

        public string MensagemErroObrigatorioSummary
        {
            get
            {
                EnsureChildControls();
                return _txtCNPJ.MensagemErroObrigatorioSummary;
            }
            set
            {
                EnsureChildControls();
                _txtCNPJ.MensagemErroObrigatorioSummary = value;
            }
        }
        #endregion

        #endregion

        #region Metodos
        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            this.InicializarCaptcha();
            this.InicializarTxtCaptcha();
            this.InicializarLinkButton();
            this.InicializarLabels();
            this.InicializarPanels();

            MostrarValidadorReceitaFederal();

            this.Controls.Add(_upComponente);
            base.CreateChildControls();
        }
        #endregion

        #region CriarLinha
        /// <summary>
        /// Cria uma linha preenchida por controles
        /// </summary>
        /// <param name="lblCpf">A label de Cpf</param>
        /// <param name="txtCpf">A textbox de Cpf</param>
        /// <returns>A div contendo a linha criada</returns>
        private System.Web.UI.WebControls.Panel CriarLinha(WebControl txtCpf, WebControl btnValidarReceita, string id)
        {
            System.Web.UI.WebControls.Panel linha = new System.Web.UI.WebControls.Panel { ID = id };
            if (!String.IsNullOrEmpty(this.CssClassLinha))
                linha.Attributes["class"] = this.CssClassLinha;

            linha.Controls.Add(txtCpf);

            linha.Controls.Add(btnValidarReceita);

            return linha;
        }

        /// <summary>
        /// Cria uma linha de controles contendo um botão 
        /// </summary>
        /// <param name="lblCaptcha">O label do captcha</param>
        /// <param name="txtCaptcha">O textbox do captcha</param>
        /// <param name="btnEnviar">O botão enviar</param>
        /// <param name="imagem">A imagem do captcha</param>
        /// <returns>A div contendo a linha criada</returns>
        private System.Web.UI.WebControls.Panel CriarLinha(WebControl txtCaptcha, WebControl lblCaptcha, WebControl btnEnviar)
        {
            System.Web.UI.WebControls.Panel linha = new System.Web.UI.WebControls.Panel();
            if (!String.IsNullOrEmpty(this.CssClassLinha))
                linha.Attributes["class"] = this.CssClassLinha;

            System.Web.UI.WebControls.Panel ctlDescricao = new System.Web.UI.WebControls.Panel();
            ctlDescricao.Controls.Add(_lblErrosCaptcha);
            ctlDescricao.Controls.Add(lblCaptcha);
            ctlDescricao.Controls.Add(txtCaptcha);
            ctlDescricao.Controls.Add(btnEnviar);
            linha.Controls.Add(ctlDescricao);

            return linha;
        }

        /// <summary>
        /// Cria uma linha de controles contendo um botão 
        /// </summary>
        /// <param name="lblCaptcha">O label do captcha</param>
        /// <param name="txtCaptcha">O textbox do captcha</param>
        /// <param name="btnEnviar">O botão enviar</param>
        /// <param name="imagem">A imagem do captcha</param>
        /// <returns>A div contendo a linha criada</returns>
        private System.Web.UI.WebControls.Panel CriarLinha(WebControl imagem, string css)
        {
            System.Web.UI.WebControls.Panel linha = new System.Web.UI.WebControls.Panel();
            if (!String.IsNullOrEmpty(css))
                linha.Attributes["class"] = css;

            //HtmlGenericControl ctlImagem = new HtmlGenericControl("div");
            //ctlImagem.Controls.Add(imagem);
            linha.Controls.Add(imagem);

            return linha;
        }

        #endregion

        #region InicializarCaptcha
        /// <summary>
        /// Inicializa a imagem de captcha
        /// </summary>
        private void InicializarCaptcha()
        {

            //@"~/a.caprf?vr=" + this.ClientID.Replace("_", String.Empty).Replace("$", String.Empty) + "&v=" + DateTime.Now.ToString("hhMMss"); ;
            this._imgCaptcha.Width = Unit.Pixel(150);
            this._imgCaptcha.Height = Unit.Pixel(50);

        }
        #endregion

        #region InicializarTxtCaptcha
        /// <summary>
        /// Inicializa o textbox de captcha
        /// </summary>
        private void InicializarTxtCaptcha()
        {
            this._txtCaptcha.Columns = 9;
            this._txtCaptcha.Tamanho = 10;
            this._txtCaptcha.AutoPostBack = false;
            this._txtCaptcha.CausesValidation = false;
        }
        #endregion

        #region InicializarLinkButton
        /// <summary>
        /// Inicializa o botão de enviar
        /// </summary>
        private void InicializarLinkButton()
        {
            this._btnEnviar.Text = "Continuar";
            this._btnEnviar.CausesValidation = false;

            this._btnValidarReceita.Text = "Validar na Receita Federal";
            this._btnValidarReceita.CausesValidation = false;
        }
        #endregion

        #region InicializarLabels
        /// <summary>
        /// Inicializa todas as labels
        /// </summary>
        private void InicializarLabels()
        {
            this._lblCNPJ.Text = "CNPJ";

            this._lblCaptcha.Text = "Código";

            this._lblErrosCaptcha.Text = String.Empty;

            this._lblCNPJ.AssociatedControlID = this._txtCNPJ.ID;
            this._lblCaptcha.AssociatedControlID = this._txtCaptcha.ID;
        }
        #endregion

        #region InicializarUpdatePanel
        /// <summary>
        /// Inicializa todas as labels
        /// </summary>
        private void InicializarPanels()
        {
            //TextBox CNPJ            
            _upCnpj.UpdateMode = UpdatePanelUpdateMode.Conditional;

            _pnlCnpj.Controls.Add(CriarLinha(_txtCNPJ, _btnValidarReceita, "linhaCPJ"));
            _upCnpj.ContentTemplateContainer.Controls.Add(_pnlCnpj);

            _pnlComponente.Controls.Add(_upCnpj);

            if (!ExibirValidadorReceitaFederal)
                _pnlCaptcha.Visible = false;
            _pnlCaptcha.Controls.Add(CriarLinha(_btnFechar, this.CssClassBotaoFechar));

            _pnlCaptcha.Controls.Add(CriarLinha(_txtCaptcha, _lblCaptcha, _btnEnviar));

            var linha = CriarLinha(_imgCaptcha, this.CssClassLinha);
            _pnlCaptcha.Controls.Add(linha);
            linha.Controls.Add(_btnRenovaCaptcha);

            _pnlComponente.Controls.Add(_pnlCaptcha);
            _upComponente.ContentTemplateContainer.Controls.Add(_pnlComponente);
        }
        #endregion

        #region RevonaCaptcha
        private void RevonaCaptcha()
        {
            ViewState["FormularioReceita"] = null;
            InicializarCaptcha();
        }
        #endregion

        #region OnInit
        /// <summary>
        /// Fase de carregamento do componente
        /// </summary>
        /// <param name="e">Os argumentos do evento</param>
        protected override void OnInit(EventArgs e)
        {
            this._btnEnviar.Click += new EventHandler(_btnEnviar_Click);
            this._btnRenovaCaptcha.Click += new EventHandler(_btnRenovaCaptcha_Click);
            this._txtCNPJ.ValorAlterado += new EventHandler(_txtCNPJ_ValorAlterado);

            this._btnValidarReceita.Click += new EventHandler(_btnValidarReceita_Click);
            this._btnFechar.Click += new ImageClickEventHandler(_btnFechar_Click);
            base.OnInit(e);
        }
        #endregion

        #region StripHtml
        /// <summary>
        /// Quebra o HTML para posterior processamento
        /// </summary>
        /// <param name="html">O texto html geral</param>
        /// <returns>O texto html tratado</returns>
        private static string StripHtml(string html)
        {
            html = System.Text.RegularExpressions.Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]*>", string.Empty);
        }
        #endregion

        #region RetornarPagina
        /// <summary>
        /// Recupera a página html do site da Receita Federal
        /// </summary>
        /// <returns></returns>
        protected Boolean RetornarPagina()
        {
            // Declarações necessárias
            Stream requestStream = null;
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/valida.asp");

                request.Method = WebRequestMethods.Http.Post;

                request.Headers.Add("Cookie", DadosFormularioReceita.SessionId);

                //request.CookieContainer.Add(new Uri("http://www.receita.fazenda.gov.br"), new Cookie("flag", "1"));

                // Neste ponto, você está setando a propriedade ContentType da página 
                // para urlencoded para que o comando POST seja enviado corretamente
                //request.ContentLength = 2113;
                request.ContentType = "application/x-www-form-urlencoded";

                StringBuilder urlEncoded = new StringBuilder();

                // alocando o bytebuffer
                byte[] byteBuffer = null;

                urlEncoded.Append("origem=comprovante&");
                urlEncoded.AppendFormat("cnpj={0}&", Formatadores.SomenteNumeros(_txtCNPJ.Text));
                urlEncoded.AppendFormat("txtTexto_captcha_serpro_gov_br={0}&", _txtCaptcha.Text);
                urlEncoded.Append("submit1=Consultar&");
                urlEncoded.Append("search_type=cnpj");

                // codificando em UTF8 (evita que sejam mostrados códigos malucos em caracteres especiais
                byteBuffer = Encoding.UTF8.GetBytes(urlEncoded.ToString());

                request.ContentLength = byteBuffer.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(byteBuffer, 0, byteBuffer.Length);

                requestStream.Close();

                try
                {
                    response = request.GetResponse();
                }
                catch (Exception ex)
                {
                    if (ProblemaComunicacao != null)
                        ProblemaComunicacao(this, ex);
                }
                // Dados recebidos 

                if ((response as HttpWebResponse).StatusCode != HttpStatusCode.OK)
                {
                    _lblErrosCaptcha.Text = "Falha de Comunicação com o Site da Receita Federal";
                    return false;
                }

                Stream responseStream = response.GetResponseStream();

                // Codifica os caracteres especiais para que possam ser exibidos corretamente
                System.Text.Encoding encoding = System.Text.Encoding.Default;

                // Preenche o reader
                reader = new StreamReader(responseStream, encoding);

                Char[] charBuffer = new Char[256];
                int count = reader.Read(charBuffer, 0, charBuffer.Length);

                StringBuilder Dados = new StringBuilder();

                // Lê cada byte para preencher meu stringbuilder
                while (count > 0)
                {
                    Dados.Append(new String(charBuffer, 0, count));
                    count = reader.Read(charBuffer, 0, charBuffer.Length);
                }

                string q = StripHtml(Dados.ToString());

                // Fecha tudo
                if (requestStream != null)
                    requestStream.Close();
                if (response != null)
                    response.Close();
                if (reader != null)
                    reader.Close();

                if (q.IndexOf("Erro na Consulta", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    this._lblErrosCaptcha.Text = "Captcha Inválido";
                    return false;
                }

                if (q.IndexOf("Verifique se o mesmo foi digitado corretamente", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    this._lblErrosCaptcha.Text = "CNPJ Inválido ou Inexistente";
                    return false;
                }

                this.DadosCnpjReceita = new DadosCNPJReceitaFederal(q);

                if (CnpjEncontrado != null)
                {
                    CnpjEncontrado(this, this.DadosCnpjReceita);
                }

                return true;
            }
            catch
            {
                string erro = "Houve um problema de comunicação com a Receita Federal.";

                this._lblErrosCaptcha.Text = erro;
                return false;
            }
        }
        #endregion

        #region FocusOnCaptcha
        /// <summary>
        /// Colca o foco no campo captcha
        /// </summary>
        public void FocusOnCaptcha()
        {
            this._txtCaptcha.Focus();
        }
        #endregion

        #region MostrarValidadorReceitaFederal
        /// <summary>
        /// Define as regras para exibir o formulário total ou parcial
        /// </summary>
        private void MostrarValidadorReceitaFederal()
        {
            if (ValidarReceitaFederal && ExibirValidadorReceitaFederal)
            {
                _btnValidarReceita.Visible = true;
                _btnValidarReceita.Enabled = false;

                _btnFechar.Visible = true;
                _btnRenovaCaptcha.Visible = true;
                _txtCaptcha.Visible = true;
                _imgCaptcha.Visible = true;
                _btnEnviar.Visible = true;
                _lblCaptcha.Visible = true;
                _pnlCaptcha.Visible = true;
            }
            else if (!ValidarReceitaFederal && ExibirValidadorReceitaFederal)
            {
                _btnValidarReceita.Visible = false;

                _btnFechar.Visible = false;
                _btnRenovaCaptcha.Visible = true;
                _txtCaptcha.Visible = true;
                _imgCaptcha.Visible = true;
                _btnEnviar.Visible = true;
                _lblCaptcha.Visible = true;
                _pnlCaptcha.Visible = true;
            }
            else if (!ValidarReceitaFederal)
            {
                _btnValidarReceita.Visible = false;

                _btnFechar.Visible = false;
                _btnRenovaCaptcha.Visible = false;
                _txtCaptcha.Visible = false;
                _imgCaptcha.Visible = false;
                _btnEnviar.Visible = false;
                _lblCaptcha.Visible = false;
                _pnlCaptcha.Visible = false;
            }
            else if (ValidarReceitaFederal && !ExibirValidadorReceitaFederal)
            {
                _btnValidarReceita.Visible = true;

                _btnFechar.Visible = false;
                _btnRenovaCaptcha.Visible = false;
                _txtCaptcha.Visible = false;
                _imgCaptcha.Visible = false;
                _btnEnviar.Visible = false;
                _lblCaptcha.Visible = false;
                _pnlCaptcha.Visible = false;
            }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                if (this.ExibirValidadorReceitaFederal)
                    this._imgCaptcha.ImageUrl = DadosFormularioReceita.UrlImg;
            }
            catch (Exception ex)
            {
                if (ProblemaComunicacao != null)
                    ProblemaComunicacao(this, ex);

                string erro = "Não foi possível consultar o número de CNPJ na Receita Federal.";
                this._lblErrosCaptcha.Text = erro;
            }

            if (string.IsNullOrEmpty(_btnFechar.ImageUrl))
                _btnFechar.ImageUrl = Page.ClientScript.GetWebResourceUrl(
                    this.GetType(), "BNE.Componentes.Content.Imagens.botao_padrao_fechar.png");

            base.OnPreRender(e);
        }

        #region Limpar
        public void Limpar()
        {
            _txtCNPJ.Text = String.Empty;
            _txtCaptcha.Text = String.Empty;
            MostrarValidadorReceitaFederal();
        }
        #endregion

        #region OcultarCaptcha
        /// <summary>
        /// Método utilizado externamente para ocultar o captcha
        /// </summary>
        public void OcultarCaptcha()
        {
            ExibirValidadorReceitaFederal = false;
            MostrarValidadorReceitaFederal();
            _btnValidarReceita.Enabled = true;
            this._txtCaptcha.Text = String.Empty;
            _upComponente.Update();
        }
        #endregion

        #region Focus
        public override void Focus()
        {
            if (this._txtCNPJ.Enabled)
                this._txtCNPJ.Focus();
        }
        #endregion
        #endregion

        #region Eventos

        void _btnRenovaCaptcha_Click(object sender, EventArgs e)
        {
            this.RevonaCaptcha();

            MostrarValidadorReceitaFederal();
            this._txtCaptcha.Text = String.Empty;
            ScriptManager.GetCurrent(this.Page).SetFocus(this._txtCaptcha);
            _upComponente.Update();
        }

        #region _btnEnviar_Click
        /// <summary>
        /// Tratador de eventos do botão que envia os dados para o site da Receita Federal
        /// </summary>
        /// <param name="sender">O objeto que enviou o evento</param>
        /// <param name="e">Os parâmetros do evento</param>
        void _btnEnviar_Click(object sender, EventArgs e)
        {
            this._lblErrosCaptcha.Text = String.Empty;

            if (String.IsNullOrEmpty(this._txtCNPJ.Text))
            {
                this._lblErrosCaptcha.Text = "Informe um CNPJ";
                return;
            }

            if (String.IsNullOrEmpty(this._txtCaptcha.Text))
            {
                this._lblErrosCaptcha.Text = "Informe o Captcha";
                return;
            }

            if (!Validacao.ValidarCNPJ(_txtCNPJ.Text))
                return;

            if (RetornarPagina())
            {
                ExibirValidadorReceitaFederal = false;
                MostrarValidadorReceitaFederal();
                this._txtCaptcha.Text = String.Empty;
                _btnValidarReceita.Enabled = true;
                _upComponente.Update();
            }
            else
            {
                _btnRenovaCaptcha_Click(sender, e);
            }
        }
        #endregion

        #region _btnValidarReceita_Click
        void _btnValidarReceita_Click(object sender, EventArgs e)
        {
            this.RevonaCaptcha();

            ExibirValidadorReceitaFederal = true;
            MostrarValidadorReceitaFederal();
            ScriptManager.GetCurrent(this.Page).SetFocus(this._txtCaptcha);
            _upComponente.Update();
        }
        #endregion

        #region _txtCNPJ_ValorAlterado
        /// <summary>
        /// Tratador de evento de valor alterado da textbox de CNPJ
        /// </summary>
        /// <param name="sender">O objeto que enviou o evento</param>
        /// <param name="e">Os parâmetros do evento</param>
        void _txtCNPJ_ValorAlterado(object sender, EventArgs e)
        {
            if (this.ValorAlterado != null)
                ValorAlterado(this, e);
        }
        #endregion

        #region _btnFechar_Click
        void _btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            _lblErrosCaptcha.Text = String.Empty;
            _btnValidarReceita.Enabled = true;
            _txtCaptcha.Text = String.Empty;
            ExibirValidadorReceitaFederal = false;
            MostrarValidadorReceitaFederal();
        }
        #endregion

        #endregion

        #region Sub classes

        #region FormularioReceita
        /// <summary>
        /// Representa os dados do formulário html.
        /// É utilizado para manter os dados de captha, sessão e etc.
        /// </summary>
        [Serializable]
        private class FormularioReceita
        {
            private string _urlImg;
            private string _sessionId;
            [NonSerialized]
            private readonly Page _pagina;

            public FormularioReceita(Page pagina)
            {
                _pagina = pagina;
                RecuperarCaptcha();
            }

            public string SessionId
            {
                get { return _sessionId; }
            }

            public string UrlImg
            {
                get { return _urlImg; }
            }

            #region RecuperarCaptcha
            /// <summary>
            /// Retorna o valor dos cookies enviados na solicitação ao site da Receita Federal
            /// </summary>
            /// <returns>O valor do cookie de segurança</returns>
            private void RecuperarCaptcha()
            {
                var uri = new Uri("http://www.receita.fazenda.gov.br/pessoajuridica/Cnpj/cnpjreva/cnpjreva_solicitacao2.asp");

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    _sessionId = httpWebResponse.Headers["Set-Cookie"];
                }

                var uriCaptcha = new Uri("http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/captcha/gerarCaptcha.asp");
                var httpWebRequestCaptcha = (HttpWebRequest)WebRequest.Create(uriCaptcha);
                httpWebRequestCaptcha.Timeout = 5000;

                httpWebRequestCaptcha.Headers.Add("Pragma", "no-cache");
                httpWebRequestCaptcha.Headers.Add("Origin", "http://www.receita.fazenda.gov.br");
                httpWebRequestCaptcha.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
                httpWebRequestCaptcha.Headers.Add("Accept-Encoding", "gzip, deflate");
                httpWebRequestCaptcha.Headers.Add("Cookie", string.Format("flag=1; {0}", SessionId));
                httpWebRequestCaptcha.Host = "www.receita.fazenda.gov.br";
                httpWebRequestCaptcha.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:32.0) Gecko/20100101 Firefox/32.0";
                httpWebRequestCaptcha.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                httpWebRequestCaptcha.Referer = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/cnpjreva_solicitacao2.asp";
                httpWebRequestCaptcha.KeepAlive = true;

                using (var smM = new MemoryStream())
                using (var sm = httpWebRequestCaptcha.GetResponse().GetResponseStream())
                {
                    var b = new byte[32768];
                    int r;
                    while (sm != null && (r = sm.Read(b, 0, b.Length)) > 0)
                        smM.Write(b, 0, r);

                    _pagina.Session[HandlerCaptchaReceitaFederal.ChaveCaptchaReceitaFederal] = smM.ToArray();
                }

                _urlImg = string.Format("{0}.caprf", Guid.NewGuid());
            }
            #endregion

        }
        #endregion

        #region DadosCNPJReceitaFederal
        /// <summary>
        /// Representa os dados do cartão CNPJ extraídos do site da Receita Federal
        /// </summary>
        [Serializable]
        public class DadosCNPJReceitaFederal
        {
            #region Properties
            /// <summary>
            /// O número de CNPJ
            /// </summary>
            public decimal Cnpj { get; private set; }
            /// <summary>
            /// Data de abertura da empresa
            /// </summary>
            public DateTime DataAbertura { get; private set; }
            /// <summary>
            /// Razão Social da empresa
            /// </summary>
            public String RazaoSocial { get; private set; }
            /// <summary>
            /// Nome Fantasia da empresa
            /// </summary>
            public String NomeFantasia { get; private set; }
            /// <summary>
            /// CNAE principal da empresa
            /// </summary>
            public String CNAEPrincipal { get; private set; }
            /// <summary>
            /// CNAE secundário da empresa
            /// </summary>
            public String CNAESecundario { get; private set; }
            /// <summary>
            /// Natureza Jurídica
            /// </summary>
            public String NaturezaJuridica { get; private set; }
            /// <summary>
            /// Porte da empresa
            /// </summary>
            public String PorteEmpresa { get; private set; }
            /// <summary>
            /// Logradouro (Nome da Rua)
            /// </summary>
            public String Logradouro { get; private set; }
            /// <summary>
            /// Número 
            /// </summary>
            public String Numero { get; private set; }
            /// <summary>
            /// Complemento de logradouro
            /// </summary>
            public String Complemento { get; private set; }
            /// <summary>
            /// CEP
            /// </summary>
            public String CEP { get; private set; }
            /// <summary>
            /// Bairro
            /// </summary>
            public String Bairro { get; private set; }
            /// <summary>
            /// Municipio
            /// </summary>
            public String Municipio { get; private set; }
            /// <summary>
            /// Estado
            /// </summary>
            public String UF { get; private set; }
            /// <summary>
            /// Se a empresa está ou não ativa
            /// </summary>
            public Boolean Ativa { get; private set; }
            /// <summary>
            /// Motivo da situação
            /// </summary>
            public String MotivoSituacao { get; private set; }
            /// <summary>
            /// Usado para situações especiais
            /// </summary>
            public String SituacaoEspecial { get; private set; }
            #endregion

            #region Metodos
            #region StripString
            /// <summary>
            /// Trata string removento caracteres especiais
            /// </summary>
            /// <param name="s">String original</param>
            /// <returns>String tratada</returns>
            private String StripString(String s)
            {
                return s.Replace(".", String.Empty)
                    .Replace("/", String.Empty)
                    .Replace("-", String.Empty)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();
            }
            #endregion
            #region Inicio
            /// <summary>
            /// Recupera a posição inicial de um fragmento dentro de um dado em formato texto
            /// </summary>
            /// <param name="dados">O dado a ser consultado</param>
            /// <param name="fragmento">O fragmento a ser localizado</param>
            /// <returns>A posição inicial do fragmento</returns>
            private int Inicio(String dados, String fragmento)
            {
                return dados.IndexOf(fragmento) + fragmento.Length;
            }
            private int Inicio(String dados, String fragmento, int startIndex)
            {
                return dados.IndexOf(fragmento, startIndex) + fragmento.Length;
            }
            #endregion
            #endregion

            #region Ctor
            /// <summary>
            /// Construtor - Recebe uma string HTML e extrái os dados do Cartão CNPJ
            /// </summary>
            /// <param name="dados">A página de cartão CNPJ do site da Receita Federal</param>
            public DadosCNPJReceitaFederal(String dados)
            {
                int origem, destino = 0;
                String dummy = String.Empty;

                #region  CNPJ
                origem = Inicio(dados, "NÚMERO DE INSCRIÇÃO");
                destino = dados.IndexOf("MATRIZ", origem);
                if (destino == -1)
                    destino = dados.IndexOf("FILIAL", origem);

                this.Cnpj = Convert.ToDecimal(
                    this.StripString(dados.Substring(origem, destino - origem))
                    );
                #endregion

                #region Data de abertura
                origem = Inicio(dados, "DATA DE ABERTURA");
                destino = dados.IndexOf("NOME EMPRESARIAL", origem);

                dummy = dados.Substring(origem, destino - origem).Replace(" ", String.Empty)
                    .Replace(".", String.Empty)
                    .Replace("-", String.Empty)
                    .Replace("&nbsp;", String.Empty);

                this.DataAbertura = DateTime.Parse(dummy, new CultureInfo("pt-BR"));
                #endregion

                #region Razão Social
                origem = Inicio(dados, "NOME EMPRESARIAL");
                destino = dados.IndexOf("TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)", origem);

                dummy = dados.Substring(origem, destino - origem)
                            .Replace("&nbsp;", " ")
                            .Replace("\r", " ")
                            .Replace("\n", " ")
                            .Replace("\t", " ")
                            .Trim();

                this.RazaoSocial = dummy;
                #endregion

                #region Nome Fantasia
                origem = Inicio(dados, "TÍTULO DO ESTABELECIMENTO (NOME DE FANTASIA)");
                destino = dados.IndexOf("CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));
                this.NomeFantasia = String.IsNullOrEmpty(dummy.Replace("*", "")) ? "" : dummy;
                #endregion

                #region Atividade Economica Principal
                origem = Inicio(dados, "CÓDIGO E DESCRIÇÃO DA ATIVIDADE ECONÔMICA PRINCIPAL");
                destino = dados.IndexOf("CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS", origem);

                dummy = dados.Substring(origem, destino - origem)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();

                if (dummy.IndexOf("Não informada", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    this.CNAEPrincipal = String.Empty;
                }
                else
                {
                    String[] c = dummy.Split(" - ");
                    dummy = StripString(c[0]);
                    this.CNAEPrincipal = dummy;
                }
                #endregion

                #region Atividade Economica Secundária
                origem = Inicio(dados, "CÓDIGO E DESCRIÇÃO DAS ATIVIDADES ECONÔMICAS SECUNDÁRIAS");
                destino = dados.IndexOf("CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA", origem);

                dummy = dados.Substring(origem, destino - origem)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();

                if (dummy.IndexOf("Não informada", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    this.CNAESecundario = String.Empty;
                }
                else
                {
                    String[] c2 = dummy.Split(" - ");
                    dummy = StripString(c2[0]);
                    this.CNAESecundario = dummy;
                }
                #endregion

                #region Código e descrição da natureza jurídica
                origem = Inicio(dados, "CÓDIGO E DESCRIÇÃO DA NATUREZA JURÍDICA");
                destino = dados.IndexOf("PORTE DA EMPRESA", origem);

                dummy = dados.Substring(origem, destino - origem)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();

                String[] n = dummy.Split(" - ");
                dummy = StripString(n[0]);

                this.NaturezaJuridica = dummy;
                #endregion

                #region Porte da empresa
                origem = Inicio(dados, "PORTE DA EMPRESA");
                destino = dados.IndexOf("LOGRADOURO", origem);

                dummy = dados.Substring(origem, destino - origem)
                    .Replace("&nbsp;", " ")
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ")
                    .Trim();

                this.PorteEmpresa = dummy;
                #endregion

                #region Logradouro
                origem = Inicio(dados, "LOGRADOURO");
                destino = dados.IndexOf("COMPLEMENTO", origem);

                dummy = dados.Substring(origem, destino - origem);

                destino = dummy.IndexOf("NÚMERO");

                //dummy = StripString(dados.Substring(origem, destino - origem));

                this.Logradouro = dummy.Substring(0, destino).Replace("*", string.Empty);
                #endregion

                #region Número
                // Aproveita o destino calculado anteriormente como início
                origem = destino + "NÚMERO".Length;
                //destino = dados.IndexOf("COMPLEMENTO");
                //dummy = StripString(dados.Substring(origem, destino - origem).Replace("NÚMERO", String.Empty));

                this.Numero = dummy.Substring(origem, dummy.Length - origem).Replace("*", string.Empty);
                #endregion

                #region Complemento
                origem = Inicio(dados, "COMPLEMENTO");
                destino = dados.IndexOf("CEP", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));

                this.Complemento = dummy.Replace("*", string.Empty);
                #endregion

                #region Cep
                var origemAnterior = origem;
                origem = Inicio(dados, "CEP", origemAnterior);
                destino = dados.IndexOf("BAIRRO/DISTRITO", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));

                this.CEP = dummy.Replace("*", string.Empty);
                #endregion

                #region Bairro
                origem = Inicio(dados, "BAIRRO/DISTRITO");
                destino = dados.IndexOf("MUNICÍPIO", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));

                this.Bairro = dummy.Replace("*", string.Empty);
                #endregion

                #region Município
                origem = Inicio(dados, "MUNICÍPIO");
                destino = dados.IndexOf("UF", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));

                this.Municipio = dummy.Replace("*", string.Empty);
                #endregion

                #region UF
                origem = Inicio(dados, "UF", destino);
                destino = dados.IndexOf("DATA DA SITUAÇÃO CADASTRAL", origem);

                dummy = dados.Substring(origem, destino - origem);
                destino = destino - dummy.IndexOf("SITUAÇÃO CADASTRAL");


                dummy = StripString(dados.Substring(origem, destino - origem));

                this.UF = dummy.Replace("*", string.Empty);
                #endregion

                #region Empresa Ativa
                origem = destino;
                destino = dados.IndexOf("DATA DA SITUAÇÃO CADASTRAL");

                dummy = StripString(dados.Substring(origem, destino - origem));

                origem = dummy.IndexOf("SITUAÇÃO CADASTRAL");

                dummy = StripString(dummy.Substring(origem));

                dummy = StripString(dummy.Replace("SITUAÇÃO CADASTRAL", String.Empty));

                this.Ativa = dummy.Equals("ATIVA", StringComparison.OrdinalIgnoreCase);
                #endregion

                #region Motivo da situação
                origem = Inicio(dados, "MOTIVO DE SITUAÇÃO CADASTRAL");
                destino = dados.IndexOf("SITUAÇÃO ESPECIAL", origem);

                dummy = StripString(dados.Substring(origem, destino - origem)).Replace("*", string.Empty);

                this.MotivoSituacao = dummy;
                #endregion

                #region Situação Especial
                origem = Inicio(dados, "SITUAÇÃO ESPECIAL");
                destino = dados.IndexOf("DATA DA SITUAÇÃO ESPECIAL", origem);

                dummy = StripString(dados.Substring(origem, destino - origem));

                this.SituacaoEspecial = String.IsNullOrEmpty(dummy.Replace("*", "")) ? "" : dummy;
                #endregion
            }
            #endregion
        }
        #endregion

        #endregion

    }
}