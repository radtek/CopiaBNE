using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.AlfaNumerico.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:AlfaNumerico runat=server Obrigatorio=False ContemIntervalo=False Tipo=AlfaNumerico ></{0}:AlfaNumerico>")]
    public class AlfaNumerico : CompositeControl
    {
        #region TipoAlfaNumerico
        /// <summary>
        /// TipoAlfaNumerico
        /// </summary>
        public enum TipoAlfaNumerico
        {
            AlfaNumerico = 0,
            Letras = 1,
            LetraMaiuscula = 2,
            LetraMinuscula = 3,
            Numerico = 4,
            AlfaNumericoMaiusculo = 5,
            AlfaNumericoMinusculo = 6
        }
        #endregion

        #region Atributos

        private TextBox _txtValor;
        private TipoAlfaNumerico _tipo;
        private RequiredFieldValidator _rfValor;
        private RangeValidator _rvValor;
        private RegularExpressionValidator _reValor;
        private CustomValidator _cvValor;
        private Boolean _renderUpLevel;

        #endregion

        #region Propriedades

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("SetFocusOnError")
        ]
        public bool SetFocusOnError
        {
            get
            {
                return _reValor.SetFocusOnError;
            }
            set
            {
                _cvValor.SetFocusOnError = value;
                _reValor.SetFocusOnError = value;
                _rvValor.SetFocusOnError = value;
            }
        }

        #endregion

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("ReadOnly")
        ]
        public bool ReadOnly
        {
            get
            {
                return _txtValor.ReadOnly;
            }
            set
            {
                _txtValor.ReadOnly = value;
            }
        }

        #endregion

        #region TextMode
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("TextMode")
        ]
        public TextBoxMode TextMode
        {
            get
            {
                return _txtValor.TextMode;
            }
            set
            {
                _txtValor.TextMode = value;
            }
        }

        #endregion

        #region Validation Group
        /// <summary>
        /// ValidationGroup
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Validation Group")
        ]
        public string ValidationGroup
        {
            get
            {
                return _reValor.ValidationGroup;
            }
            set
            {
                _rfValor.ValidationGroup = value;
                _rvValor.ValidationGroup = value;
                _reValor.ValidationGroup = value;
                _cvValor.ValidationGroup = value;
            }
        }
        #endregion

        #region TextBox - Rows
        /// <summary>
        /// Define Width do campo
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Rows")
        ]
        public int Rows
        {
            get
            { return _txtValor.Rows; }
            set
            { _txtValor.Rows = value; }
        }
        #endregion

        #region Width
        public override Unit Width
        {
            get
            {
                
                return base.Width;
            }
            set
            {
                this._txtValor.Width = value;
                base.Width = value;
            }
        }
        #endregion 


        #region TextBox - Columns
        /// <summary>
        /// TextBox
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Columns")
        ]
        public int Columns
        {
            get { return _txtValor.Columns; }
            set
            {
                _txtValor.Columns = value;
            }
        }
        #endregion

        #region TextBox - MaxLength
        /// <summary>
        /// Define MaxLength do campo
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("MaxLength")
        ]
        public int MaxLength
        {
            get
            { return _txtValor.MaxLength; }
            set
            {
                _txtValor.MaxLength = value;
                if (Columns.Equals(0))
                    Columns = value;
            }
        }
        #endregion

        #region Tipo Campo
        /// <summary>
        /// Define o tipo do campo
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Tipo")
        ]
        public TipoAlfaNumerico Tipo
        {
            get
            { return _tipo; }
            set
            {
                _tipo = value;

                _rvValor.Type = _tipo.Equals(TipoAlfaNumerico.Numerico) ? ValidationDataType.Integer : ValidationDataType.String;
            }
        }
        #endregion

        #region Campo Obrigatorio
        /// <summary>
        /// Define se o Campo é obrigatório
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            {
                if (this.ViewState["Obrigatorio"] == null)
                    this.ViewState["Obrigatorio"] = false;

                return Convert.ToBoolean(this.ViewState["Obrigatorio"]);
            }
            set
            { this.ViewState["Obrigatorio"] = value; }
        }
        #endregion

        #region Campo Contém Intervalo
        /// <summary>
        /// Define se o Campo é obrigatório
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Contém Intervalo")
        ]
        public bool ContemIntervalo
        {
            get
            { return _rvValor.Enabled; }
            set
            { _rvValor.Enabled = value; }
        }
        #endregion

        #region Expressão de Validação
        /// <summary>
        /// Define s expressão para o campo
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Expressão de Validação")
        ]
        public string ExpressaoValidacao
        {
            get
            { return _reValor.ValidationExpression; }
            set
            { _reValor.ValidationExpression = value; }
        }
        #endregion

        #region Valor Mínimo
        /// <summary>
        /// Define um valor mínimo
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Valor Mínimo")
        ]
        public string ValorMinimo
        {
            get
            {
                return _rvValor.MinimumValue;
            }
            set
            {
                _rvValor.MinimumValue = value;
            }
        }
        #endregion

        #region Valor Máximo
        /// <summary>
        /// Define uma Valor máximo.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Valor Máximo")
        ]
        public string ValorMaximo
        {
            get
            {
                return _rvValor.MaximumValue;
            }
            set
            {
                _rvValor.MaximumValue = value;
            }
        }

        #endregion

        #region RequiredField - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RequiredFieldValidator
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Mensagem Campo Obrigatório"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorio
        {
            get
            {
                return _rfValor.ErrorMessage;
            }
            set
            {
                _rfValor.ErrorMessage = value;
            }
        }
        #endregion

        #region RangeValidator - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RangeValidator
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Mensagem Erro Intervalo"),
            Localizable(true)
        ]
        public string MensagemErroIntervalo
        {
            get
            {
                return _rvValor.ErrorMessage;
            }
            set
            {
                _rvValor.ErrorMessage = value;
            }
        }
        #endregion

        #region RegularExpression - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RegularExpression
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Mensagem Formato Incorreto"),
            Localizable(true)
        ]
        public string MensagemErroFormato
        {
            get
            {
                return _reValor.ErrorMessage;
            }
            set
            {
                _reValor.ErrorMessage = value;
            }
        }
        #endregion

        #region CustomValidator - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RegularExpression
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("Mensagem Valor Incorreto"),
            Localizable(true)
        ]
        public string MensagemErroValor
        {
            get
            {
                return _cvValor.ErrorMessage;
            }
            set
            {
                _cvValor.ErrorMessage = value;
            }
        }
        #endregion

        #region TextBox - CssClass
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("CssClass TextBox")
        ]
        public string CssClassTextBox
        {
            get
            {
                return _txtValor.CssClass;
            }
            set
            {
                _txtValor.CssClass = value;
            }
        }
        #endregion

        #region RequiredField - CssClass
        /// <summary>
        /// CssClass do RequiredField
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("CssClass RequiredField")
        ]
        public string CssClassRequiredField
        {
            get
            {
                return _rfValor.CssClass;
            }
            set
            {
                _rfValor.CssClass = value;
            }
        }
        #endregion

        #region RangeValidator - CssClass
        /// <summary>
        /// CssClass do RangeValidator
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("CssClass RangeValidator")
        ]
        public string CssClassRangeValidator
        {
            get
            {
                return _rvValor.CssClass;
            }
            set
            {
                _rvValor.CssClass = value;
            }
        }
        #endregion

        #region RegularExpression - CssClass
        /// <summary>
        /// CssClass do RegularExpression
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("CssClass RegularExpression")
        ]
        public string CssClassRegularExpression
        {
            get
            {
                return _reValor.CssClass;
            }
            set
            {
                _reValor.CssClass = value;
            }
        }
        #endregion

        #region CustomValidator - CssClass
        /// <summary>
        /// CssClass do RegularExpression
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("CssClass CustomValidator")
        ]
        public string CssClassCustomValidator
        {
            get
            {
                return _cvValor.CssClass;
            }
            set
            {
                _cvValor.CssClass = value;
            }
        }
        #endregion

        #region Valor
        /// <summary>
        /// Set do componente
        /// </summary>
        [Browsable(false)]
        public string Valor
        {
            get
            {
                return _txtValor.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _txtValor.Text = value;
                else
                {
                    value = value.Trim();
                    _txtValor.Text = value;
                }
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region OnFocusClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("OnFocusClient")
        ]
        public string OnFocusClient
        {
            get;
            set;
        }
        #endregion

        #region OnBlurClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
        }
        #endregion

        #region OnKeyUpClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("OnKeyUpClient")
        ]
        public string OnKeyUpClient
        {
            get;
            set;
        }
        #endregion

        #region ClientValidationFunction
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - AlfaNumerico"),
            DisplayName("ClientValidationFunction")
        ]
        public string ClientValidationFunction
        {
            get
            {
                return _cvValor.ClientValidationFunction;
            }
            set
            {
                _cvValor.ClientValidationFunction = value;
            }
        }
        #endregion

        #region Placeholder
        [DisplayName("Placeholder")]
        public string Placeholder
        {
            set => _txtValor.Attributes["placeholder"] = value;
        }
        #endregion

        #endregion

        #region Construtores

        public AlfaNumerico()
        {
            _txtValor = new TextBox();
            _rfValor = new RequiredFieldValidator();
            _rvValor = new RangeValidator();
            _reValor = new RegularExpressionValidator();
            _cvValor = new CustomValidator();
        }


        #endregion

        #region Métodos

        #region DetermineRenderUpLevel
        /// <summary>
        /// DetermineRenderUpLevel
        /// </summary>
        /// <returns></returns>
        protected virtual bool DetermineRenderUpLevel()
        {
            HttpBrowserCapabilities browser = Page.Request.Browser;
            if (browser.W3CDomVersion.Major >= 1
                && browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)
                return true;
            return false;
        }
        #endregion

        #region AplicarAcoesComponente
        /// <summary>
        /// Método que instância o objeto de argumentos e registra a função
        /// valor alterado para ser invocada no Client.
        /// </summary>
        private void AplicarAcoesComponente()
        {
            if (String.IsNullOrEmpty(_reValor.ValidationExpression.Trim()))
            {
                switch (_tipo)
                {
                    case TipoAlfaNumerico.Letras:
                    case TipoAlfaNumerico.LetraMaiuscula:
                    case TipoAlfaNumerico.LetraMinuscula:
                        _reValor.ValidationExpression = @"([A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç]){1,";
                        break;
                    case TipoAlfaNumerico.Numerico:
                        _reValor.ValidationExpression = @"\d{1,";
                        break;
                    case TipoAlfaNumerico.AlfaNumericoMaiusculo:
                    case TipoAlfaNumerico.AlfaNumericoMinusculo:
                        _reValor.ValidationExpression = @"([0-9,A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç]){1,";
                        break;
                    default:
                        _reValor.Enabled = false;
                        break;
                }
                if (!String.IsNullOrEmpty(_reValor.ValidationExpression))
                {
                    if (_txtValor.MaxLength > 0)
                        _reValor.ValidationExpression += _txtValor.MaxLength.ToString();
                    _reValor.ValidationExpression += "}";
                }
            }

            Args args = new Args();
            args.NomeCampoValor = _txtValor.ClientID;
            List<string> nomeValidadores = new List<string>();
            if (_rfValor.Enabled)
                nomeValidadores.Add(_rfValor.ClientID);
            if (_rvValor.Enabled)
                nomeValidadores.Add(_rvValor.ClientID);
            if (_reValor.Enabled)
                nomeValidadores.Add(_reValor.ClientID);
            if (_cvValor.Enabled && (!String.IsNullOrEmpty(_cvValor.ClientValidationFunction) || (ServerValidate != null)))
                nomeValidadores.Add(_cvValor.ClientID);

            args.Validadores = nomeValidadores.ToArray();

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, " + _txtValor.MaxLength + ");");
            if (!String.IsNullOrEmpty(OnKeyUpClient))
                _txtValor.Attributes["OnKeyUp"] += OnKeyUpClient + "(this);";
            _txtValor.Attributes.Add("OnKeyPress", "return ApenasTeclasAlfaNumerico(this, event," + _txtValor.MaxLength + ", " + _tipo.GetHashCode() + ");");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, null);");
            if (!String.IsNullOrEmpty(OnFocusClient))
                _txtValor.Attributes["OnFocus"] += OnFocusClient + "(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraAlfaNumerico(this," + _tipo.GetHashCode() + "," + _txtValor.MaxLength + ");");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraAlfaNumerico(this," + _tipo.GetHashCode() + "," + _txtValor.MaxLength + ");");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if ((ValorAlterado != null) || (ServerValidate != null))
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
            if (!String.IsNullOrEmpty(OnBlurClient))
                _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Inicializar TextBox AlfaNumerico
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Inicializar RequiredField AlfaNumerico
        /// </summary>
        protected virtual void InicializarRequiredField()
        {
            _rfValor.ID = "rfValor";
            _rfValor.ForeColor = Color.Empty;
            _rfValor.ControlToValidate = _txtValor.ID;
            _rfValor.Display = ValidatorDisplay.Dynamic;
            _rfValor.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRangeValidator
        /// <summary>
        /// Método de configuração das propriedades do Range Validator.
        /// </summary>
        private void InicializarRangeValidator()
        {
            _rvValor.ID = "rvValor";
            _rvValor.ForeColor = Color.Empty;
            _rvValor.ControlToValidate = _txtValor.ID;
            _rvValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarRegularExpression
        /// <summary>
        /// Método de configuração das propriedades da Regular Expression.
        /// </summary>
        private void InicializarRegularExpression()
        {
            _reValor.ID = "cvAlfaNumerico";
            _reValor.ForeColor = Color.Empty;
            _reValor.ControlToValidate = _txtValor.ID;
            _reValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Método de Inicialização do Custom Validator
        /// </summary>
        protected void InicializarCustomValidator()
        {
            _cvValor.ID = "cvValor";
            _cvValor.ForeColor = Color.Empty;
            _cvValor.ControlToValidate = _txtValor.ID;
            _cvValor.Display = ValidatorDisplay.Dynamic;

            if (ServerValidate != null)
                _cvValor.ServerValidate += cvValor_ServerValidate;
        }
        #endregion

        #endregion

        #region Eventos

        #region AddAttributesToRender
        /// <summary>
        /// AddAttributesToRender
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (_renderUpLevel)
            {
                writer.AddAttribute("MaxLength", MaxLength.ToString());
            }
        }
        #endregion

        #region ValorAlterado
        /// <summary>
        /// Evento ao alterar valor
        /// </summary>
        public event EventHandler ValorAlterado;
        #endregion

        #region ServerValidate
        /// <summary>
        /// Evento ao alterar valor
        /// </summary>
        public event ServerValidateEventHandler ServerValidate;
        #endregion

        #region OnInit
        /// <summary>
        /// Especificar ao iniciar os javascripts a serem utilizados
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("AlfaNumerico.js"))
                Page.ClientScript.RegisterClientScriptInclude("AlfaNumerico.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.AlfaNumerico.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }

        protected override void CreateChildControls()
        {
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarTextBox();
            InicializarRequiredField();
            InicializarRangeValidator();
            InicializarRegularExpression();
            InicializarCustomValidator();

            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";

            pnlValidador.Controls.Add(_rfValor); //RequiredField


            if (_rvValor.Enabled)
                pnlValidador.Controls.Add(_rvValor); //RangeValidator
            pnlValidador.Controls.Add(_reValor); //RegularExpression
            if (_cvValor.Enabled && (!String.IsNullOrEmpty(_cvValor.ClientValidationFunction) || (ServerValidate != null)))
                pnlValidador.Controls.Add(_cvValor); //CustomValidator
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor); //Campo Texto de Data

            AplicarAcoesComponente();
            base.CreateChildControls();
        }
        #endregion

        #region OnPreRender
        /// <summary>
        /// Rederizar os componentes
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnPreRender(EventArgs e)
        {
            _rfValor.Enabled = this.Obrigatorio;           

            _renderUpLevel = DetermineRenderUpLevel();
            base.OnPreRender(e);

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
                txtValor_TextChanged(_txtValor, null);
        }

        void txtValor_TextChanged(object sender, EventArgs e)
        {
            if (ServerValidate != null)
                _cvValor.Validate();

            if (ValorAlterado != null)
                ValorAlterado(_txtValor, null);
        }
        #endregion

        #region cvValor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        void cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ServerValidate(source, args);
        }
        #endregion
        #endregion
    }
}