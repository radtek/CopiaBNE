using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web;

[assembly: WebResource("Employer.Componentes.UI.Web.Content.Images.ico_cadastrar.png", "image/png")]

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Classe base para os controles de Auto Complete<br/>
    /// Este componente contem um validador, label, campo text, botão de busca botão de cadastro, campo de código e autocomplete<br/>
    /// </summary>
    public class ControlAutoCompleteBase : ControlBaseTextBox
    {
        #region Eventos
        /// <summary>
        /// Evento que ocorre após a criação dos controles
        /// </summary>
        public event EventHandler PosCreateChildControls;
        /// <summary>
        /// Evento para validar campo obrigatório
        /// </summary>
        public event ServerValidateEventHandler ValidarObrigatorio;
        /// <summary>
        /// Evento para realizar validação customizada
        /// </summary>
        public event ServerValidateEventHandler CustomServerValidate;
        /// <summary>
        /// Evento disparado ao informar o código do ítem a ser carregado
        /// </summary>
        public event EventHandler CarregarPorCodigo;

        /// <summary>
        /// Evento disparado ao clicar no botão de cadastro
        /// </summary>
        public event EventHandler CadastroClick;
        #endregion

        #region Atributos
        private CustomValidator _cvValor = new CustomValidator();
        private System.Web.UI.WebControls.Label _Label = new System.Web.UI.WebControls.Label();
        private ImageButton _BotaoCadastro = new ImageButton();
        private ImageButton _BotaoBusca = new ImageButton();
        private LinkButton _LinkBusca = new LinkButton();
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        private AutoCompleteExtender _AutoComplete = new AutoCompleteExtender();
        private HiddenField _hdnCodigo = new HiddenField();
        #endregion

        #region Propriedades

        #region ReadOnly
        /// <inheritdoc/>
        [Category("TextBox"), DisplayName("ReadOnly")]
        public new bool ReadOnly
        {
            get 
            {
                EnsureChildControls();
                return base.ReadOnly; 
            }
            set
            {
                EnsureChildControls();
                base.ReadOnly = value;

                this._BotaoCadastro.Enabled =
                this._LinkBusca.Enabled =
                this._BotaoBusca.Enabled = !value;                
            }
        }
        #endregion

        #region PanelValidador
        /// <summary>
        /// Panel contendo os validadores
        /// </summary>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion

        #region ValidadorTexto
        /// <summary>
        /// O validador da textbox
        /// </summary>
        protected override BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }
        #endregion

        #region Codigo
        /// <summary>
        /// O Código retornado pelo Auto Complete
        /// </summary>
        public string Codigo
        {
            get { return _hdnCodigo.Value; }
            set
            {
                _hdnCodigo.Value = value;
                if (this.CarregarPorCodigo != null)
                    this.CarregarPorCodigo(this, new EventArgs());
            }
        }
        #endregion

        #region IdModalAjax
        /// <summary>
        /// Client ID da Modal de consulta
        /// </summary>
        public string IdModalAjax
        {
            get
            {
                if (this.ViewState["IdModalAjax"] != null)
                    return (string)this.ViewState["IdModalAjax"];
                return string.Empty;
            }
            set { this.ViewState["IdModalAjax"] = value; }
        }
        #endregion

        #region IdModalEstado
        /// <summary>
        /// Propriedade para controlar o estado da modal.<br/>
        /// Se está aberta ou fechada.
        /// </summary>
        public string IdModalEstado
        {
            get
            {
                if (this.ViewState["IdModalEstado"] != null)
                    return (string)this.ViewState["IdModalEstado"];
                return string.Empty;
            }
            set { this.ViewState["IdModalEstado"] = value; }
        }
        #endregion

        #region CustomClientValidate
        /// <summary>
        /// Javascript disparado na validação do lado do cliente
        /// </summary>
        public string CustomClientValidate
        {
            get
            {
                if (this.ViewState["CustomClientValidate"] != null)
                    return (string)this.ViewState["CustomClientValidate"];
                return string.Empty;
            }
            set { this.ViewState["CustomClientValidate"] = value; }
        }
        #endregion

        #region Label
        /// <summary>
        /// Define se o label está ou não visível
        /// </summary>
        public bool LabelVisivel
        {
            get { return false; }
            set { _Label.Visible = false; }
        }

        /// <summary>
        /// Define o texto da label
        /// </summary>
        public string LabelTexto
        {
            get { return _Label.Text; }
            set { _Label.Text = value; }
        }

        /// <summary>
        /// Define o Css da label
        /// </summary>
        public string CssLabel
        {
            get { return _Label.CssClass; }
            set { _Label.CssClass = value; }
        }
        #endregion

        #region BotaoCadastro
        /// <summary>
        /// Retorna o botão de cadastro
        /// </summary>
        public ImageButton CadastroBotao
        {
            get { return _BotaoCadastro; }
        }

        /// <summary>
        /// Endereço da tela de cadastro.
        /// </summary>
        public string CadastroUrl
        {
            get { EnsureChildControls(); return ViewState["CadastroUrl"] as string; }
            set { EnsureChildControls(); ViewState["CadastroUrl"] = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroSessao
        {
            get { EnsureChildControls(); return ViewState["CadastroParametroSessao"] as string; }
            set { EnsureChildControls(); ViewState["CadastroParametroSessao"] = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroUrl
        {
            get { EnsureChildControls(); return ViewState["CadastroParametroUrl"] as string; }
            set { EnsureChildControls(); ViewState["CadastroParametroUrl"] = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroSessaoCodigo
        {
            get { EnsureChildControls(); return ViewState["CadastroParametroSessaoCodigo"] as string; }
            set { EnsureChildControls(); ViewState["CadastroParametroSessaoCodigo"] = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroUrlCodigo
        {
            get { EnsureChildControls(); return ViewState["CadastroParametroUrlCodigo"] as string; }
            set { EnsureChildControls(); ViewState["CadastroParametroUrlCodigo"] = value; }
        }
        #endregion

        #region BotaoBusca
        /// <summary>
        /// Caminho da imagem para o botão que ativa a modal de busca
        /// </summary>
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string ImagemBotaoBusca
        {
            get { return _BotaoBusca.ImageUrl; }
            set { _BotaoBusca.ImageUrl = value; }

        }

        /// <summary>
        /// Classe Css do botão que ativa a modal de busca
        /// </summary>
        public string CssBotaoBusca
        {
            get 
            {
                EnsureChildControls();
                return _BotaoBusca.CssClass; 
            }
            set 
            {
                EnsureChildControls();
                _BotaoBusca.CssClass = value; 
            }
        }

        /// <summary>
        /// Classe css do botão de link de busca
        /// </summary>
        public string CssLinkBusca
        {
            get
            {
                EnsureChildControls();
                return _LinkBusca.CssClass;
            }
            set
            {
                EnsureChildControls();
                _LinkBusca.CssClass = value;
            }
        }

        /// <summary>
        /// Client ID do botão que ativa a modal de busca
        /// </summary>
        public string BotaoBuscaID
        {
            get { return this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao ? _BotaoBusca.UniqueID : _LinkBusca.UniqueID; }
            set 
            {
                if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                    _BotaoBusca.ID = value;
                else
                    _LinkBusca.ID = value;
            }
        }
        /// <summary>
        /// Instância botão que ativa a modal de busca
        /// </summary>
        public System.Web.UI.WebControls.ImageButton BotaoBusca
        {
            get { return _BotaoBusca; }
        }

        /// <summary>
        /// Instância do LinkButton de busca
        /// </summary>
        public LinkButton LinkBusca
        {
            get { return _LinkBusca; }
        }

        /// <summary>
        /// Define a visibilidade do botão que ativa a modal de busca
        /// </summary>
        public Boolean BotaoBuscaVisivel
        {
            get 
            {
                EnsureChildControls();
                return this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao ? this._BotaoBusca.Visible : this._LinkBusca.Visible; 
            }
            set 
            {
                EnsureChildControls();
                if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                    this._BotaoBusca.Visible = value;
                else
                    this._LinkBusca.Visible = value;
            }
        }
        #endregion

        #region Autocomplete
        /// <summary>
        /// Css da listagem do Auto Complete
        /// </summary>
        public string AutoCompleteCompletionListCssClass
        {
            get { return _AutoComplete.CompletionListCssClass; }
            set { _AutoComplete.CompletionListCssClass = value; }
        }
        /// <summary>
        /// Css do ítem selecionado no Auto Complete
        /// </summary>
        public string AutoCompleteCompletionListHighlightedItemCssClass
        {
            get { return _AutoComplete.CompletionListHighlightedItemCssClass; }
            set { _AutoComplete.CompletionListHighlightedItemCssClass = value; }
        }
        /// <summary>
        /// Css de um ítem normal no Auto Complete
        /// </summary>
        public string AutoCompleteCompletionListItemCssClass
        {
            get { return _AutoComplete.CompletionListItemCssClass; }
            set { _AutoComplete.CompletionListItemCssClass = value; }
        }
        /// <summary>
        /// Método do webservice de Auto Complete a ser invocado
        /// </summary>
        public string AutoCompleteServiceMethod
        {
            get { return _AutoComplete.ServiceMethod; }
            set { _AutoComplete.ServiceMethod = value; }
        }
        /// <summary>
        /// Endereço do webservice de Auto Complete
        /// </summary>
        public string AutoCompleteServicePath
        {
            get { return _AutoComplete.ServicePath; }
            set { _AutoComplete.ServicePath = value; }
        }
        /// <summary>
        /// Quantidade mínima de caracteres a serem digitados no Auto Complete
        /// </summary>
        public int AutoCompleteMinimumPrefixLength
        {
            get { return _AutoComplete.MinimumPrefixLength; }
            set { _AutoComplete.MinimumPrefixLength = value; }
        }
        /// <summary>
        /// Quantidade de ítens retornados na listagem
        /// </summary>
        public int AutoCompleteCompletionSetCount
        {
            get { return _AutoComplete.CompletionSetCount; }
            set { _AutoComplete.CompletionSetCount = value; }
        }
        /// <summary>
        /// Se vai usar o context key
        /// </summary>
        public bool AutoCompleteUseContextKey
        {
            get { return _AutoComplete.UseContextKey; }
            set { _AutoComplete.UseContextKey = value; }
        }
        /// <summary>
        /// O context key da requisição
        /// </summary>
        public string AutoCompleteContextKey
        {
            get { return _AutoComplete.ContextKey; }
            set { _AutoComplete.ContextKey = value; }
        }
        /// <summary>
        /// O intervalo entre a requisição e a população dos ítens no Auto Complete
        /// </summary>
        public int AutoCompleteCompletionInterval
        {
            get { return _AutoComplete.CompletionInterval; }
            set { _AutoComplete.CompletionInterval = value; }
        }
        #endregion

        #region ValidarTextChanging
        /// <summary>
        /// Validar o input de entrada.
        /// </summary>
        public Boolean ValidarTextChanging{
            get{
                if (ViewState["ValidarTextChanging"] != null)
                    return (Boolean)ViewState["ValidarTextChanging"];
                return false;
            }
            set{
                ViewState["ValidarTextChanging"] = value;
            }
        }
        #endregion

        /// <inheritdoc />
        public override bool Enabled
        {
            get
            {
                EnsureChildControls();
                return base.Enabled;
            }
            set
            {
                EnsureChildControls();
                base.Enabled = value;
                
                _BotaoCadastro.Enabled=
                    this._LinkBusca.Enabled =
                this._BotaoBusca.Enabled = value;
            }
        }

        #region Tipo
        /// <summary>
        /// Restringe o tipo de texto a ser informado no campo.<br/>
        /// </summary>
        public TipoAlfaNumerico Tipo
        {
            get
            {
                if (this.ViewState["Tipo"] == null)
                    return TipoAlfaNumerico.Padrao;
                return (TipoAlfaNumerico)this.ViewState["Tipo"];
            }
            set
            {
                this.ViewState["Tipo"] = value;
            }
        }
        #endregion

        #region ClientOnFocus
        /// <summary>
        /// Função a ser chamada ao executar a função Focus.<br/>
        /// Ex: "functionName(parameter)"
        /// </summary>
        public String ClientOnFocus
        {
            private get
            {
                if (ViewState["ClientOnFocus"] == null)
                    return String.Empty;
                return (String)ViewState["ClientOnFocus"];
            }
            set
            {
                ViewState["ClientOnFocus"] = value;
            }
        }
        #endregion

        #region ClientOnBlur
        /// <summary>
        /// Função a ser chamada ao executar a função Blur.<br/>
        /// Ex: "functionName(parameter)"
        /// </summary>
        public String ClientOnBlur
        {
            private get
            {
                if (ViewState["ClientOnBlur"] == null)
                    return String.Empty;
                return (String)ViewState["ClientOnBlur"];
            }
            set
            {
                ViewState["ClientOnBlur"] = value;
            }
        }
        #endregion

        #region CampoTexto
        /// <inheritdoc/>
        public new TextBox CampoTexto
        {
            get
            {
                EnsureChildControls();
                return base.CampoTexto;
            }
        }
        #endregion

        #region MetodoCarregarAjax
        /// <summary>
        /// Método em ajax a ser invocado no evento blur após o ciclo de vida do componente para completar o campo com valores.<br/>
        /// O valor pode ser carregado antes mesmo do autocomplete encontrar algum valor.
        /// Formato do método a ser invocado <code>{ "texto": "texto", "contextKey": "contextKey" } </code>
        /// Retorna objeto em json <code>{ First : "Valor do campo texto", Second: "Código" }</code>
        /// </summary>
        public string MetodoCarregarAjax
        {
            get 
            {
                EnsureChildControls();
                return ViewState["MetodoCarregarAjax"] as string; 
            }
            set 
            {
                EnsureChildControls();
                ViewState["MetodoCarregarAjax"] = value; 
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            InicializarLabel();
            InicializarTextBox();
            InicializarCustomValidator();

            if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
            {
                InicializarPainel();
                Controls.Add(CampoTexto);
                InicializarBotoes();
            }
            else
            {
                Controls.Add(CampoTexto);
                InicializarBotoes();
                InicializarPainel();
            }
            
            InicializarAutoComplete();
            base.CreateChildControls();            
        }
        #endregion

        #region OnLoad
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (PosCreateChildControls != null)
                PosCreateChildControls(this, new EventArgs());
        }
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
            _BotaoCadastro.Click += new ImageClickEventHandler(_BotaoCadastro_Click);
            base.OnInit(e);
        }
        #endregion

        #region _BotaoCadastro_Click
        void _BotaoCadastro_Click(object sender, ImageClickEventArgs e)
        {
            if (CadastroClick != null)
                CadastroClick(sender, e);

            //var preparam = string.Empty;
            var valorUrl = string.Empty;
            var url = this.ResolveClientUrl(this.CadastroUrl);

            if (!string.IsNullOrEmpty(CadastroParametroUrl))
            {
                var preparam = url.Contains("?") ? "&" : "?";

                if (!string.IsNullOrEmpty(CadastroParametroUrl))
                {
                    valorUrl += string.Format("{0}{1}={2}",
                        preparam, CadastroParametroUrl, HttpUtility.UrlEncode(this.CampoTexto.Text));
                    preparam = "&";
                }

                if (!string.IsNullOrEmpty(CadastroParametroUrlCodigo))
                {
                    valorUrl += string.Format("{0}{1}={2}",
                        preparam, CadastroParametroUrlCodigo, HttpUtility.UrlEncode(this.Codigo));
                }
            }
            else
            {
                int iTemp;
                object codigo = int.TryParse(this.Codigo, out iTemp) ? iTemp : (object) this.Codigo;
                codigo = codigo is string && string.IsNullOrEmpty((string) codigo) ? new int?() : codigo;

                if (!string.IsNullOrEmpty(CadastroParametroSessao))
                    Page.Session[CadastroParametroSessao] = this.CampoTexto.Text;

                if (!string.IsNullOrEmpty(CadastroParametroSessaoCodigo))
                    Page.Session[CadastroParametroSessaoCodigo] = codigo;
            }

            Page.Response.Redirect(
                string.Format("{0}{1}", url, valorUrl), false);
        }
        #endregion

        #region _cvValor_ServerValidate
        /// <summary>
        /// Tratador do evento de Server Validate
        /// </summary>
        /// <param name="source">Objeto que disparou o evento</param>
        /// <param name="args">Argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.CustomServerValidate != null)
            {
                CustomServerValidate(source, args);
            }
            else if (this.Obrigatorio)
            {
                if (string.IsNullOrEmpty(this.Codigo))
                    args.IsValid = false;
                else if (ValidarObrigatorio != null)
                    ValidarObrigatorio(source, args);
            }
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            if (string.IsNullOrEmpty(_BotaoBusca.ImageUrl))
            {
                _BotaoBusca.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ControlBaseTextBox),
                "Employer.Componentes.UI.Web.Content.Images.icone_busca.png");
            }

            if (string.IsNullOrEmpty(_BotaoCadastro.ImageUrl))
            {
                _BotaoCadastro.ImageUrl = Page.ClientScript.GetWebResourceUrl(typeof(ControlAutoCompleteBase),
                "Employer.Componentes.UI.Web.Content.Images.ico_cadastrar.png");
            }

            base.OnPreRender(e);

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(this.CampoTexto.ClientID))
            {
                //this.Validar();
                // Seta como se a validaçao não tivesse sido disparada se código e texto estiverem vazios
                if (String.IsNullOrEmpty(this.Codigo) && String.IsNullOrEmpty(this.CampoTexto.Text))
                {
                    this.ValidadorTexto.ErrorMessage = String.Empty;
                    this.ValidadorTexto.IsValid = true;
                }
                else
                {
                    if (!this.ValidadorTexto.IsValid && String.IsNullOrEmpty(this.ValidadorTexto.ErrorMessage))
                        this.ValidadorTexto.ErrorMessage = MensagemErroFormato;
                }
            }

            _BotaoCadastro.Visible = !string.IsNullOrEmpty(this.CadastroUrl);
        }
        #endregion

        #endregion

        #region Inicializar Controles

        #region InicializarLabel
        /// <summary>
        /// Inicializa a label
        /// </summary>
        private void InicializarLabel()
        {
            _Label.EnableTheming = false;
            _Label.Visible = false;
            _Label.ID = "Label";
            Controls.Add(_Label);
        }
        #endregion

        #region InicializarBotoes
        /// <summary>
        /// Inicializa os botões
        /// </summary>
        private void InicializarBotoes()
        {
            _BotaoBusca.ID = "BotaoBusca";
            _BotaoBusca.EnableTheming = false;
            _BotaoBusca.CausesValidation = false;
            _BotaoBusca.TabIndex = -1;

            _LinkBusca.ID = "BotaoBusca";
            _LinkBusca.EnableTheming = false;
            _LinkBusca.CausesValidation = false;
            _LinkBusca.TabIndex = -1;

            if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                Controls.Add(_BotaoBusca);
            else
                Controls.Add(_LinkBusca);

            _BotaoCadastro.ID = "BotaoCadastro";
            _BotaoCadastro.EnableTheming = false;
            _BotaoCadastro.CausesValidation = false;
            _BotaoCadastro.TabIndex = -1;
            Controls.Add(_BotaoCadastro);
        }
        #endregion

        #region InicializarAutoComplete
        /// <summary>
        /// Inicializa o auto complete
        /// </summary>
        private void InicializarAutoComplete()
        {
            _AutoComplete.ID = "AutoComplete";
            //_AutoComplete.MinimumPrefixLength = 3;
            _AutoComplete.CompletionInterval = 300;
            //_AutoComplete.CompletionSetCount = 10;
            _AutoComplete.OnClientItemSelected = "Employer.Componentes.UI.Web.ControlAutoCompleteBase.ItemSelected";
            _AutoComplete.FirstRowSelected = true;

            _AutoComplete.TargetControlID = CampoTexto.ID;
            _AutoComplete.EnableTheming = false;

            Controls.Add(_AutoComplete);

            _hdnCodigo.ID = "hdnCodigo";
            Controls.Add(_hdnCodigo);
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador customizado
        /// </summary>
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();
            _cvValor.ValidateEmptyText = true;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ControlAutoCompleteBase.ValidarTextBox";
        }
        #endregion

        #endregion

        #region GetScriptDescriptors
        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ControlAutoCompleteBase", this.ClientID);

            base.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("IdModalAjax", this.IdModalAjax);
            descriptor.AddProperty("IdModalEstado", this.IdModalEstado);
            descriptor.AddProperty("CustomClientValidate", this.CustomClientValidate);
            descriptor.AddProperty("ValidarTextChanging", this.ValidarTextChanging);
            descriptor.AddProperty("TipoAlfanumerico", this.Tipo.GetHashCode());
            descriptor.AddProperty("ClientOnBlur", this.ClientOnBlur);
            descriptor.AddProperty("ClientOnFocus", this.ClientOnFocus);
            descriptor.AddProperty("CarregarOnAjaxBlur", this.MetodoCarregarAjax);
            //descriptor.AddProperty("CasasDecimais", this.CasasDecimais);
            //descriptor.AddProperty("ValorMinimo", this.ValorMinimo);
            //descriptor.AddProperty("ValorMaximo", this.ValorMaximo);

            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ControlAutoCompleteBase.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #region Validar
        /// <summary>
        /// Método para disparar o custom validator.
        /// </summary>
        public void Validar()
        {
            if (_cvValor != null && _cvValor.Visible)
                _cvValor.Validate();
        }

        #endregion

    }
}
