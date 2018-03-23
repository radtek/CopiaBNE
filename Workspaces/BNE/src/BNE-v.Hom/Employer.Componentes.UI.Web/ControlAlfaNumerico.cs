using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente TextBox com as seguintes funcionalidades:<br/>
    /// <list type="bullet">
    /// <item>Validador embutido</item>
    /// <item>Validação de intervalo de valor</item>
    /// <item>Validação por expressão regular</item>
    /// </list>
    /// </summary>
    public class ControlAlfaNumerico : ControlBaseTextBox
    {
        /// <summary>
        /// Evento disparado a realizar a validação
        /// </summary>
        public event ServerValidateEventHandler ValidacaoServidor;

        #region atributos
        private CustomValidator _cvValor = new CustomValidator();
        private RegularExpressionValidator _reValor = new RegularExpressionValidator();
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        #endregion

        #region Propriedades
        #region ValidadorTexto
        /// <summary>
        /// O Validador
        /// </summary>
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }
        #endregion

        #region PanelValidador
        /// <summary>
        /// O painel que contém o validador
        /// </summary>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion

        #region ExpressaoRegular
        /// <summary>
        /// Expressão regular a ser aplicada na validação
        /// </summary>
        public String ExpressaoRegular
        {
            get
            {
                return this._reValor.ValidationExpression;
            }
            set
            {
                this._reValor.ValidationExpression = value;
            }
        }
        #endregion
        
        #region ValidationGroup
        /// <inheritdoc />
        public override string ValidationGroup
        {
            get
            {
                return base.ValidationGroup;
            }
            set
            {
                this._reValor.ValidationGroup = value;
                base.ValidationGroup = value;
            }
        }
        #endregion

        #region SomenteNumeros
        /// <summary>
        /// Define se a textbox vai filtrar e aceitar somente números
        /// </summary>
        public Boolean SomenteNumeros
        {
            get
            {
                return this.Tipo == TipoAlfaNumerico.Numeros;
            }
            set
            {
                if (value)
                    this.Tipo = TipoAlfaNumerico.Numeros;
            }

        }
        #endregion

        #region TextMode
        /// <summary>
        /// O modo da caixa de texto
        /// </summary>
        public TextBoxMode TextMode
        {
            get
            {
                return this.CampoTexto.TextMode;
            }
            set
            {
                this.CampoTexto.TextMode = value;
            }
        }
        #endregion

        #region Rows
        /// <summary>
        /// A quantidade de linhas do controle quando no modo multiline
        /// </summary>
        public int Rows
        {
            get
            {
                return this.CampoTexto.Rows;
            }
            set
            {
                this.CampoTexto.Rows = value;
            }
        }
        #endregion

        #region Attributes
        /// <summary>
        /// Atributos do TextBox
        /// </summary>
        public new System.Web.UI.AttributeCollection Attributes
        {
            get { return base.CampoTexto.Attributes; }
        }
        #endregion

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

        #region CSSValidator
        /// <summary>
        /// Classe css do validador
        /// </summary>
        public String CSSValidador
        {
            set
            {
                _cvValor.CssClass = value;
            }
        }
        #endregion

        #region ValorMaximo
        /// <summary>
        /// Valor máximo usado na validação de dados
        /// </summary>
        public String ValorMaximo
        {
            get
            {
                return ViewState["ValorMaximo"] != null ? ViewState["ValorMaximo"].ToString() : String.Empty;
            }
            set
            {
                ViewState["ValorMaximo"] = value;
            }
        }
        #endregion

        #region ValorMinimo
        /// <summary>
        /// Valor mínimo usado na validação de dados
        /// </summary>
        public String ValorMinimo
        {
            get
            {
                return ViewState["ValorMinimo"] != null ? ViewState["ValorMinimo"].ToString() : String.Empty;
            }
            set
            {
                ViewState["ValorMinimo"] = value;
            }
        }
        #endregion

        #region MascaraMensagemErroIntervalo
        /// <summary>
        /// Mascará de mensagem de Erro de Intervalo usado no validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {} onde o primeiro valor é o mínimo e o segundo máximo.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve estar entre {0} e {1}"
        /// </summary>
        public string MascaraMensagemErroIntervalo
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroIntervalo"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = "Valor deve estar entre {0} e {1}";
                return msg;
            }
            set { ViewState["MascaraMensagemErroIntervalo"] = value; }
        }
        #endregion

        #region MascaraMensagemErroMaximo
        /// <summary>
        /// Mascará de mensagem de Erro de valor máximo usado no validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {}.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve ser maior que {0}"
        /// </summary>       
        public string MascaraMensagemErroMaximo
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroMaximo"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = "Valor deve ser menor que {0}";
                return msg;
            }
            set { ViewState["MascaraMensagemErroMaximo"] = value; }
        }
        #endregion

        #region MascaraMensagemErroMinimo
        /// <summary>
        /// Mascará de mensagem de Erro de valor mínimo usado no validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {}.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve ser menor que {0}"
        /// </summary>       
        public string MascaraMensagemErroMinimo
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroMinimo"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = "Valor deve ser maior que {0}";
                return msg;
            }
            set { ViewState["MascaraMensagemErroMinimo"] = value; }
        }
        #endregion

        #region MascaraMensagemErroIntervaloSummary
        /// <summary>
        /// Mascará de mensagem de Erro de Intervalo usado no sumário do validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {} onde o primeiro valor é o mínimo e o segundo máximo.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve estar entre {0} e {1}"
        /// </summary>
        public string MascaraMensagemErroIntervaloSummary
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroIntervaloSummary"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = this.MascaraMensagemErroIntervalo;
                return msg;
            }
            set { ViewState["MascaraMensagemErroIntervaloSummary"] = value; }
        }
        #endregion

        #region MascaraMensagemErroMaximoSummary
        /// <summary>
        /// Mascará de mensagem de Erro de valor máximo usado no sumário do validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {}.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve ser maior que {0}"
        /// </summary>       
        public string MascaraMensagemErroMaximoSummary
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroMaximoSummary"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = MascaraMensagemErroMaximo;
                return msg;
            }
            set { ViewState["MascaraMensagemErroMaximoSummary"] = value; }
        }
        #endregion

        #region MascaraMensagemErroMinimoSummary
        /// <summary>
        /// Mascará de mensagem de Erro de valor mínimo usado no sumário do validador do campo.<br/>
        /// Esta propriedade segue o padrão de formatação usando {}.<br/>
        /// Caso nenhum valor seja informado usará a mensagem padrão: "Valor deve ser menor que {0}"
        /// </summary>        
        public string MascaraMensagemErroMinimoSummary
        {
            get
            {
                var msg = ViewState["MascaraMensagemErroMinimoSummary"] as string;
                if (string.IsNullOrEmpty(msg))
                    msg = MascaraMensagemErroMinimo;
                return msg;
            }
            set { ViewState["MascaraMensagemErroMinimoSummary"] = value; }
        }
        #endregion

        #endregion

        #region Metodos

        #region OnLoad
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _reValor.ErrorMessage = this.MensagemErroFormato;
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
        }
        #endregion

        #region _cvValor_ServerValidate
        /// <summary>
        /// Dispara a validação server-side do validador
        /// </summary>
        /// <param name="source">O controle que disparou a ação</param>
        /// <param name="args">Os argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio)
            {
                args.IsValid = !String.IsNullOrEmpty(this.Text);

                if (args.IsValid)
                {
                    this.ValidadorTexto.ErrorMessage = String.Empty;
                    this.ValidadorTexto.IsValid = true;
                }
                else
                {
                    this.ValidadorTexto.ErrorMessage = MensagemErroObrigatorio;
                    this.ValidadorTexto.IsValid = false;
                }
            }
            if (ValidacaoServidor != null)
                ValidacaoServidor(source, args);
        }
        #endregion

        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            if (DesignMode)
            {
                Controls.Add(new Literal { Text = "Componente ControlAlfaNumerico" });
                return;
            }

            InicializarTextBox();
            InicializarCustomValidator();
            InicializarRegularExpressionValidador();

            if (ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
            {
                InicializarPainel();

                this._pnlValidador.Controls.Add(_reValor);
                Controls.Add(CampoTexto);
            }
            else
            {
                this._pnlValidador.Controls.Add(_reValor);
                Controls.Add(CampoTexto);

                InicializarPainel();
            }

            base.CreateChildControls();
        }
        #endregion

        #region GetScriptDescriptors
        /// <summary>
        /// Retorna os descritores de script
        /// </summary>
        /// <returns>Uma coleção dos descritores de script</returns>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.AlfaNumerico", this.ClientID);
            this.SetScriptDescriptors(descriptor);
            descriptor.AddProperty("SomenteNumeros", this.SomenteNumeros);
            descriptor.AddProperty("ExpressaoRegular", this.ExpressaoRegular);
            descriptor.AddProperty("MensagemErroFormato", this.MensagemErroFormato);
            descriptor.AddProperty("TipoAlfanumerico", this.Tipo.GetHashCode());

            //Range Validator
            descriptor.AddProperty("ValorMaximo", this.ValorMaximo);
            descriptor.AddProperty("ValorMinimo", this.ValorMinimo);

            descriptor.AddProperty("MensagemErroMinimo", String.Format(MascaraMensagemErroMinimo, ValorMinimo));
            descriptor.AddProperty("MensagemErroMaximo", String.Format(MascaraMensagemErroMaximo, ValorMaximo));
            descriptor.AddProperty("MensagemErroIntervalo", String.Format(MascaraMensagemErroIntervalo, ValorMinimo, ValorMaximo));

            descriptor.AddProperty("MensagemErroMinimoSummary", String.Format(MascaraMensagemErroMinimoSummary, ValorMinimo));
            descriptor.AddProperty("MensagemErroMaximoSummary", String.Format(MascaraMensagemErroMaximoSummary, ValorMaximo));
            descriptor.AddProperty("MensagemErroIntervaloSummary", String.Format(MascaraMensagemErroIntervaloSummary, ValorMinimo, ValorMaximo));

            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referências dos scripts
        /// </summary>
        /// <returns>Uma coleção das referências</returns>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.AlfaNumerico.js";
            references.Add(reference);


            return references;
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();
            ValidadorTexto.ErrorMessage = String.Empty;
            _cvValor.ValidateEmptyText = true;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.AlfaNumerico.ValidarTextBox";
        }
        #endregion

        #region InicializarRegularExpressionValidador
        /// <summary>
        /// Inicializa o validador de expressão regular
        /// </summary>
        private void InicializarRegularExpressionValidador()
        {
            _reValor.ID = "re_Valor";
            _reValor.Display = ValidatorDisplay.Dynamic;
            _reValor.ControlToValidate = this.CampoTexto.ID;
            _reValor.EnableClientScript = false;
        }
        #endregion

        #region Validar
        /// <summary>
        /// Invoca os validadores
        /// </summary>
        /// <returns>Verdadeiro caso um dos validador esteja válido.</returns>
        public Boolean Validar()
        {
            if (this._cvValor.Enabled)
                this._cvValor.Validate();
            if (this._reValor.Enabled)
                this._reValor.Validate();

            return (this._reValor.IsValid || this._cvValor.IsValid);
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            _reValor.Enabled = !String.IsNullOrEmpty(this.ExpressaoRegular);

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(this.CampoTexto.ClientID))
            {
                this.Validar();
            }

            base.OnPreRender(e);
        }
        #endregion
        #endregion
    }
}
