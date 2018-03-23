using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using BNE.Componentes.Base;

namespace BNE.Componentes
{
    /// <summary>
    /// Controle alfa numérico
    /// </summary>
    public class ControlAlfaNumerico : ControlBaseTextBox
    {

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
        /// Expressão regular a ser aplicada
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
            get { return this.Tipo == TipoAlfaNumerico.Numeros; }
            set
            {
                if (value)
                    this.Tipo = TipoAlfaNumerico.Numeros;
                else
                {
                    if (this.Tipo == TipoAlfaNumerico.Numeros)
                    {
                        this.Tipo = TipoAlfaNumerico.Padrao;
                    }
                }
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

        #region AutoPostBack
        public Boolean AutoPostBack
        {
            set { base.AutoPostBack = value; }
        }
        #endregion

        #region Attributes
        public System.Web.UI.AttributeCollection Attributes
        {
            get { return base.CampoTexto.Attributes; }
        }
        #endregion

        #region Tipo
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
        public String CSSValidador
        {
            set
            {
                _cvValor.CssClass = value;
            }
        }
        #endregion

        #region ValorMaximo
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

        #endregion

        #region Metodos

        #region OnLoad
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarRegularExpressionValidador();
            InicializarPainel();
            this._pnlValidador.Controls.Add(_reValor);
            Controls.Add(CampoTexto);

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
            var descriptor = new ScriptControlDescriptor("BNE.Componentes.AlfaNumerico", this.ClientID);
            this.SetScriptDescriptors(descriptor);
            descriptor.AddProperty("SomenteNumeros", this.SomenteNumeros);
            descriptor.AddProperty("ExpressaoRegular", this.ExpressaoRegular);
            descriptor.AddProperty("MensagemErroFormato", this.MensagemErroFormato);
            descriptor.AddProperty("TipoAlfanumerico", this.Tipo.GetHashCode());

            //Range Validator
            descriptor.AddProperty("ValorMaximo", this.ValorMaximo);
            descriptor.AddProperty("ValorMinimo", this.ValorMinimo);
            descriptor.AddProperty("MensagemErroMinimo", String.Format("Valor deve ser maior que {0}", ValorMinimo));
            descriptor.AddProperty("MensagemErroMaximo", String.Format("Valor deve ser menor que {0}", ValorMaximo));
            descriptor.AddProperty("MensagemErroIntervalo", String.Format("Valor deve estar entre {0} e {1}", ValorMinimo, ValorMaximo));

            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referencias dos scripts
        /// </summary>
        /// <returns>Uma coleção das referências</returns>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "BNE.Componentes";
            reference.Name = "BNE.Componentes.Content.js.AlfaNumerico.js";
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
            _cvValor.ClientValidationFunction = "BNE.Componentes.AlfaNumerico.ValidarTextBox";
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
            _reValor.ErrorMessage = this.MensagemErroFormato;
            _reValor.EnableClientScript = false;
        }
        #endregion

        #region Validar
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
