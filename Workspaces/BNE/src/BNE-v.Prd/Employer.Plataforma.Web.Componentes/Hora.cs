using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.Hora.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:Hora runat=server Obrigatorio=false ></{0}:Hora>")]
    public class Hora : CompositeControl
    {
        #region Atributos

        private TextBox _txtValor;
        private CustomValidator _cvValor;
        private Boolean _renderUpLevel;
        private HiddenField _hfHoraMaxima;
        private HiddenField _hfHoraMinima;

        #endregion

        #region Propriedades

        #region Validation Group
        /// <summary>
        /// Grupo de validação do RequiredField Validator
        /// </summary>
        [
            Category("Employer - Hora"),
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
            Category("Employer - Hora"),
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
            Category("Employer - Hora"),
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
            Category("Employer - Hora"),
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

        #region Mensagem Erro Formato
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Formato"),
            Localizable(true)
        ]
        public string MensagemErroFormato
        {
            get
            {
                if (ViewState["MensagemErroFormato"] == null)
                    return "Formato Inválido";
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
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Obrigatorio"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorio
        {
            get
            {
                if (ViewState["MensagemErroObrigatorio"] == null)
                    return "Campo Obrigatório";
                return ViewState["MensagemErroObrigatorio"].ToString();
            }
            set
            {
                ViewState["MensagemErroObrigatorio"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Formato Summary
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Formato Summary"),
            Localizable(true)
        ]
        public string MensagemErroFormatoSummary
        {
            get
            {
                if (ViewState["MensagemErroFormatoSummary"] == null)
                    return String.Empty;
                return ViewState["MensagemErroFormatoSummary"].ToString();
            }
            set
            {
                ViewState["MensagemErroFormatoSummary"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Obrigatorio Summary
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Obrigatorio Summary"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorioSummary
        {
            get
            {
                if (ViewState["MensagemErroObrigatorioSummary"] == null)
                    return string.Empty;
                return ViewState["MensagemErroObrigatorioSummary"].ToString();
            }
            set
            {
                ViewState["MensagemErroObrigatorioSummary"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Intervalo Summary
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Intervalor Summary"),
            Localizable(true)
        ]
        public string MensagemErroIntervaloSummary
        {
            get
            {
                if (ViewState["MensagemErroIntervaloSummary"] == null)
                    return "Intervalo Hora inválido.";
                return ViewState["MensagemErroIntervaloSummary"].ToString();
            }
            set
            {
                ViewState["MensagemErroIntervaloSummary"] = value;
            }
        }

        #endregion

        #region Mensagem Erro Intervalo
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("Mensagem Erro Intervalor"),
            Localizable(true)
        ]
        public string MensagemErroIntervalo
        {
            get
            {
                if (ViewState["MensagemErroIntervalo"] == null)
                    return "Intervalo inválido.";
                return ViewState["MensagemErroIntervalo"].ToString();
            }
            set
            {
                ViewState["MensagemErroIntervalo"] = value;
            }
        }

        #endregion

        #region TextBox - CssClass
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [
            Category("Employer - Hora"),
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

        #region CustomValidator - CssClass
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [
            Category("Employer - Hora"),
            DisplayName("CssClass CustomValidator")
        ]
        public string CssClassCustom
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
            Category("Employer - Hora"),
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
        /// Propriedade para setar ou receber valor do Campo Hora.
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
                _txtValor.Text = AplicarMascara(value);
            }
        }

        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Hora"),
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
            Category("Employer - Hora"),
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
            Category("Employer - Hora"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
        }
        #endregion

        #region Limites
        #region HoraMaxima
        private int? HoraMaximaGet
        {
            get{
                if (ViewState["HoraMaxima"] == null)
                    return null;
                return (int) ViewState["HoraMaxima"];
            }
        }

        public int HoraMaxima
        {
            set
            {
                ViewState["HoraMaxima"] = value;
            }
        }
        #endregion

        #region HoraMinima
        private int? HoraMinimaGet
        {
            get
            {
                if (ViewState["HoraMinima"] == null)
                    return null;
                return (int)ViewState["HoraMinima"];
            }
        }

        public int HoraMinima
        {
            set
            {
                ViewState["HoraMinima"] = value;
            }
        }
        #endregion

        #region MinutoMaximo
        public int MinutoMaximo
        {
            set
            {
                ViewState["MinutoMaximo"] = value;
            }
        }

        private int? MinutoMaximoGet
        {
            get
            {
                if (ViewState["MinutoMaximo"] == null)
                    return null;
                return (int)ViewState["MinutoMaximo"];
            }
        }
        #endregion

        #region MinutoMinimo
        public int MinutoMinimo
        {
            set
            {
                ViewState["MinutoMinimo"] = value;
            }
        }

        private int? MinutoMinimoGet
        {
            get
            {
                if (ViewState["MinutoMinimo"] == null)
                    return null;
                return (int)ViewState["MinutoMinimo"];
            }
        }
        #endregion
        #endregion

        #endregion

        #region Construtor

        public Hora()
        {
            _txtValor = new TextBox();
            _cvValor = new CustomValidator();
            _hfHoraMaxima = new HiddenField();
            _hfHoraMinima = new HiddenField();
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
            Args args = new Args();
            args.NomeCampoValor = _txtValor.ClientID;
            List<string> nomeValidadores = new List<string>();
            nomeValidadores.Add(_cvValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            ////_txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 4); ");
            //_txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 4);");
            //_txtValor.Attributes.Add("OnFocus", "RemoverMascaraHora(this);");

            //if (!String.IsNullOrEmpty(OnFocusClient))
            //    _txtValor.Attributes["OnFocus"] += OnFocusClient + "(" + new JSONReflector(args) + ");";

            //_txtValor.Attributes.Add("OnBlur", "AplicarMascaraHora(this," + new JSONReflector(args) + ");");
            ////_txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            ////_txtValor.Attributes.Add("OnChange", "AplicarMascaraHora(this);");
            //if (!String.IsNullOrEmpty(ValorAlteradoClient))
            //    //_txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            //    if (ValorAlterado != null)
            //        _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
            //if (!String.IsNullOrEmpty(OnBlurClient))
            //    _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método de configuração das propriedades do campo
        /// </summary>
        private void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 5;
            _txtValor.MaxLength = 5;
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Método que seta as configurações da propriedades do Custom Validator do campo.
        /// </summary>
        protected void InicializarCustomValidator()
        {
            _cvValor.ID = "cvValor";
            _cvValor.ForeColor = Color.Empty;
            _cvValor.ControlToValidate = _txtValor.ID;
            _cvValor.ClientValidationFunction = "Validar";
            _cvValor.Display = ValidatorDisplay.Dynamic;
            _cvValor.ValidateEmptyText = true;
            _cvValor.ErrorMessage = "Erro";
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar os caracteres especiais do Cnpj
        /// </summary>
        /// <returns>Cnpj limpo</returns>
        public static string LimparMascara(string valor)
        {
            valor = valor.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(valor, @"\d{1,2}:\d{1,2}"))
            {
                string sPadrao = @"[:]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(valor, "");
            }
            return valor;
        }
        #endregion

        #region AplicarMascara
        private String AplicarMascara(String valor) {
            if (!String.IsNullOrEmpty(valor))
            {
                valor = valor.PadRight(4, '0');
                return String.Format("{0}:{1}", valor.Substring(0, 2), valor.Substring(2, 2));
            }
            return String.Empty;
        }
        #endregion

        #region ValidarFormato
        /// <summary>
        /// Validar o formato do valor desejado
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        private static bool ValidarFormato(string valor)
        {
            Int16 vlr;
            if (!valor.Length.Equals(0) && Int16.TryParse(valor, out vlr))
            {
                switch (valor.Length)
                {
                    case 1:
                        return true;
                    case 2:
                        return (vlr < 24);
                    case 3:
                        return (Int16.Parse(valor.Substring(1)) < 60);
                    case 4:
                        return ((Int16.Parse(valor.Substring(0,2)) < 24) && (Int16.Parse(valor.Substring(2)) < 60));
                    default:
                        return false;
                }
            }
            return false;
        }
        #endregion

        #region Validar
        /// <summary>
        /// Validar valor do campo TextBox atende as regras
        /// </summary>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool Validar(string valor)
        {
            valor = LimparMascara(valor);

            if (!ValidarFormato(valor))
                return false;

            return true;
        }
        #endregion
        
        #region InicializarLimites
        private void InicializarLimites() {
            _hfHoraMaxima.ID = "hfHoraMaxima";
            _hfHoraMinima.ID = "hfHoraMinima";

            _hfHoraMaxima.Value = String.Empty;
            _hfHoraMinima.Value = String.Empty;

            if(this.HoraMaximaGet.HasValue)
                _hfHoraMaxima.Value = String.Format("{0}:{1}", HoraMaximaGet.Value.ToString().PadLeft(2), MinutoMaximoGet.HasValue ? MinutoMaximoGet.Value.ToString().PadLeft(2) : "00");
            if(this.HoraMinimaGet.HasValue)
                _hfHoraMinima.Value = String.Format("{0}:{1}", HoraMinimaGet.Value.ToString().PadLeft(2), MinutoMinimoGet.HasValue ? MinutoMinimoGet.Value.ToString().PadLeft(2) : "00");
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
                writer.AddAttribute("Obrigatorio", Obrigatorio ? "1" : "");
                
                writer.AddAttribute("MensagemErroObrigatorio", MensagemErroObrigatorio);
                writer.AddAttribute("MensagemErroFormato", MensagemErroFormato);
                writer.AddAttribute("MensagemErroIntervalo", MensagemErroIntervalo);

                writer.AddAttribute("MensagemErroObrigatorioSummary", String.IsNullOrEmpty(MensagemErroObrigatorioSummary) ? "Campo Hora obrigatório." : MensagemErroObrigatorioSummary);
                writer.AddAttribute("MensagemErroFormatoSummary", String.IsNullOrEmpty(MensagemErroFormatoSummary) ? "Campo Hora inválido." : MensagemErroFormatoSummary);
                writer.AddAttribute("MensagemErroIntervaloSummary", String.IsNullOrEmpty(MensagemErroIntervaloSummary) ? "Campo Hora incorreto." : MensagemErroIntervaloSummary);
                
                
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

            if (!Page.ClientScript.IsClientScriptBlockRegistered("Hora.js"))
                Page.ClientScript.RegisterClientScriptInclude("Hora.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.Hora.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarLimites();
            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_cvValor); //CustomValidator
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor); //Campo Texto com Valor
            Controls.Add(_hfHoraMaxima);
            Controls.Add(_hfHoraMinima);
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
            Ajax.Utility.RegisterTypeForAjax(typeof(Hora));
        }
        #endregion
        #endregion
    }
}