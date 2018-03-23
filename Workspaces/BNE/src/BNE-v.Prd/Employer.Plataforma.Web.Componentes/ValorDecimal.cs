using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.ValorDecimal.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:ValorDecimal runat=server CasasDecimais=2 Obrigatorio=false></{0}:ValorDecimal>")]
    public class ValorDecimal : BaseCompositeControl
    {
        #region Atributos

        private TextBox _txtValor;
        private CustomValidator _cvValor;
        private int _casasDecimais;
        private Boolean _renderUpLevel;

        
        

        #endregion

        #region Propriedades

        #region Validation Group
        /// <summary>
        /// Grupo de validação do RequiredField Validator
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Validation Group")
        ]
        public string ValidationGroup
        {
            get
            {
                return _cvValor.ValidationGroup;
            }
            set
            {
                _cvValor.ValidationGroup = value;
            }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é obrigatório.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            {
                if (ViewState["Obrigatorio"] == null)
                    return false;
                return Convert.ToBoolean(ViewState["Obrigatorio"]);
            }
            set
            {
                ViewState["Obrigatorio"] = value;
            }
        }

        #endregion

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
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

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("SetFocusOnError")
        ]
        public bool SetFocusOnError
        {
            get
            {
                return _cvValor.SetFocusOnError;
            }
            set
            {
                _cvValor.SetFocusOnError = value;
            }
        }

        #endregion

        #region Número de Dígitos
        /// <summary>
        /// Define o número de dígitos após a virgula.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Número de Dígitos")
        ]
        public int CasasDecimais
        {
            get
            {
                return _casasDecimais;
            }
            set
            {
                _casasDecimais = value;
            }
        }
        #endregion

        #region Valor Mínimo
        /// <summary>
        /// Define o valor mínimo do campo.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Valor Mínimo")
        ]
        public decimal ValorMinimo
        {
            get
            {
                if (ViewState["ValorMinimo"] == null)
                    return decimal.Zero;
                return decimal.Parse(ViewState["ValorMinimo"].ToString());
            }
            set
            {
                ViewState["ValorMinimo"] = value.ToString();
                AtualizarTextBox();
            }
        }
        #endregion

        #region Valor Máximo
        /// <summary>
        /// Define o valor máximo do campo.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Valor Máximo")
        ]
        public decimal ValorMaximo
        {
            get
            {
                if (ViewState["ValorMaximo"] == null)
                    return decimal.MaxValue;
                return decimal.Parse(ViewState["ValorMaximo"].ToString());
            }
            set
            {
                ViewState["ValorMaximo"] = value.ToString();
                AtualizarTextBox();
            }
        }
        #endregion

        #region Mensagem Erro Formato
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Mensagem Erro Formato"),
            Localizable(true)
        ]
        public string MensagemErroFormato
        {
            get
            {
                if (ViewState["MensagemErroFormato"] == null)
                    return string.Empty;
                return ViewState["MensagemErroFormato"].ToString();
            }
            set
            {
                ViewState["MensagemErroFormato"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Obrigatorio
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Mensagem Erro Obrigatorio"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorio
        {
            get
            {
                if (ViewState["MensagemErroObrigatorio"] == null)
                    return string.Empty;
                return ViewState["MensagemErroObrigatorio"].ToString();
            }
            set
            {
                ViewState["MensagemErroObrigatorio"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Intervalo
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Mensagem Erro Intervalo"),
            Localizable(true)
        ]
        public string MensagemErroIntervalo
        {
            get
            {
                if (ViewState["MensagemErroIntervalo"] == null)
                    return string.Empty;
                return ViewState["MensagemErroIntervalo"].ToString();
            }
            set
            {
                ViewState["MensagemErroIntervalo"] = value;
            }
        }

        #endregion

        #region MensagemErroIntervaloInvalido
        /// <summary>
        /// Mensagem de erro quando inserido um intervalo inválido
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("Mensagem Erro Intervalo Inválido"),
            Localizable(true)
        ]
        public string MensagemErroIntervaloInvalido
        {
            get
            {
                if (ViewState["MensagemErroIntervaloInvalido"] == null)
                    return "O intervalo informado está incorreto. O valor final deve ser maior do que o valor inicial.";
                return Convert.ToString(ViewState["MensagemErroIntervaloInvalido"]);
            }
            set
            {
                ViewState["MensagemErroIntervaloInvalido"] = value;
            }
        }
        #endregion

        #region TextBox - CssClass
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
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

        #region RegularExpression - CssClass
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
            DisplayName("CssClass RegularExpression")
        ]
        public string CssClassRegularExpression
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

        #region TextBox - Columns
        /// <summary>
        /// TextBox
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
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

        #region Valor
        /// <summary>
        /// Propriedade para setar ou receber valor do Campo Valor Decimal.
        /// </summary>
        [Browsable(true), Bindable(true)]
        public decimal? Valor
        {
            get
            {
                NumberStyles style = NumberStyles.Currency;
                CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
                decimal vlr;
                if (decimal.TryParse(_txtValor.Text, style, culture, out vlr))
                    return vlr;
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
                    _txtValor.Text = RecuperarValor(value.Value.ToString("N" + _casasDecimais, culture), _casasDecimais);
                }
                else
                    _txtValor.Text = string.Empty;
            }
        }

        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Valor Decimal"),
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
            Category("Employer - Valor Decimal"),
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
            Category("Employer - Valor Decimal"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
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
        /// <summary>
        /// Construtor da Classe do Componente Valor Decimal.
        /// </summary>
        public ValorDecimal()
        {
            _txtValor = new TextBox();
            _cvValor = new CustomValidator();
        }
        #endregion

        #region Metodos

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

        #region AtualizarTextBox
        private void AtualizarTextBox()
        {
            string cultura = "pt-BR";

            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultura);
            int maxLength = RecuperarValor(ValorMaximo.ToString("N" + _casasDecimais, culture), _casasDecimais).Length;

            _txtValor.MaxLength = maxLength;
            if (_txtValor.Columns.Equals(0))
                _txtValor.Columns = maxLength;

            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumerosDecimal(this, event, " + _casasDecimais + ", " + maxLength + ", '" + cultura + "')");
            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, " + maxLength + "); ");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControleDecimal(this);");
        }
        #endregion

        #region AplicarAcoesComponente
        /// <summary>
        /// Método que instância o objeto de argumentos e registra a função
        /// valor alterado para ser invocada no Client.
        /// </summary>
        private void AplicarAcoesComponente()
        {
            Args args = new Args();
            args.NomeCampoValor = _txtValor.ClientID;
            List<string> nomeValidadores = new List<string>();
            nomeValidadores.Add(_cvValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            AtualizarTextBox();

            if (!String.IsNullOrEmpty(OnFocusClient))
                _txtValor.Attributes["OnFocus"] += OnFocusClient + "(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraDecimal(this," + _casasDecimais + ");");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraDecimal(this," + _casasDecimais + ");");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
            if (!String.IsNullOrEmpty(OnBlurClient))
                _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método de configuração das propriedades do campo Valor Decimal.
        /// </summary>
        private void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Método que seta as configurações da propriedades do Custom Validator do campo Data.
        /// </summary>
        protected void InicializarCustomValidator()
        {
            _cvValor.ID = "cvValor";
            _cvValor.ForeColor = Color.Empty;
            _cvValor.ControlToValidate = _txtValor.ID;
            _cvValor.ClientValidationFunction = "ValidarValorDecimal";
            _cvValor.Display = ValidatorDisplay.Dynamic;
            _cvValor.ValidateEmptyText = true;
            _cvValor.ErrorMessage = "Erro";
        }
        #endregion



        #region RecuperarValorDecimal
        public static string RecuperarValor(string valor, int casasDecimais)
        {
            NumberStyles style = NumberStyles.Currency;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
            decimal vlr;
            if (decimal.TryParse(valor, style, culture, out vlr))
            {
                return vlr.ToString("N" + casasDecimais, culture);
            }
            return null;
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
                CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
                writer.AddAttribute("Obrigatorio", Obrigatorio ? "1" : "");
                writer.AddAttribute("ValorMinimo", ValorMinimo.ToString("N" + _casasDecimais, culture));
                writer.AddAttribute("ValorMaximo", ValorMaximo.ToString("N" + _casasDecimais, culture));
                writer.AddAttribute("MensagemErroObrigatorio", MensagemErroObrigatorio);
                writer.AddAttribute("MensagemErroFormato", MensagemErroFormato);
                writer.AddAttribute("MensagemErroIntervalo", MensagemErroIntervalo);
                writer.AddAttribute("MensagemErroIntervaloInvalido", MensagemErroIntervaloInvalido);
            }
        }
        #endregion

        #region ValorAlterado
        /// <summary>
        /// Evento ao alterar valor
        /// </summary>
        public event EventHandler ValorAlterado;
        #endregion

        #region OnInit
        /// <summary>
        /// Evento de inicialização do componente.
        /// </summary>
        /// <param name="e">Argumentos do componente.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Define se registra ou não os JavaScripts referentes ao comportamento do controle
            // Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
            // muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
            // ATENÇÃO! Estes IFs a seguir não adiantam em nada, pois eles estão retornando sempre 'false'
            // o que acaba fazendo o controle registrar um .JS para cada instancia do controle.
            // Em caso de dúvida ou descrédito, coloque um Breakpoint aqui e verifique!
            // Eduardo Ordine
            if (!LoadScripts)
                return;

            string key = "ValorDecimal.js_v1_2";
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(key))
                Page.ClientScript.RegisterClientScriptInclude(key, Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.ValorDecimal.js"));

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarTextBox();
            InicializarCustomValidator();

            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_cvValor); //CustomValidator
            Controls.Add(pnlValidador);

            Controls.Add(_txtValor); //Campo Texto com Valor Decimal

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
            _renderUpLevel = DetermineRenderUpLevel();
            base.OnPreRender(e);
            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
                ValorAlterado(_txtValor, null);
        }
        #endregion

        #region OnLoad
        /// <summary>
        /// Evento executado quando o componente é carregado.
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Ajax.Utility.RegisterTypeForAjax(typeof(ValorDecimal));
        }
        #endregion
        #endregion

    }
}