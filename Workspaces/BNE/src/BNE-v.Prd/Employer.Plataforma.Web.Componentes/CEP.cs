using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.CEP.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]


namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:CEP runat=server Obrigatorio=False ></{0}:CEP>")]
    public class CEP : CompositeControl
    {
        #region Atributos

        private TextBox _txtValor;
        private RegularExpressionValidator _reValor;
        private RequiredFieldValidator _rfValor;
        private Label _lblErro;

        #endregion

        #region Propriedades

        #region SetFocusOnError
        public Label LblErro
        {
            get
            {
                return _lblErro;
            }
        }
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - CEP"),
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
                _reValor.SetFocusOnError = value;
            }
        }

        #endregion

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - CEP"),
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

        #region Validation Group
        /// <summary>
        /// ValidationGroup do Ccomponente Data
        /// </summary>
        [
            Category("Employer - CEP"),
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
                _reValor.ValidationGroup = value;
                _rfValor.ValidationGroup = value;
            }
        }
        #endregion

        #region Campo Obrigatorio
        /// <summary>
        /// Define se o Campo Data é obrigatório
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            { return _rfValor.Enabled; }
            set
            { _rfValor.Enabled = value; }
        }
        #endregion

        #region RequiredField - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RequiredFieldValidator
        /// </summary>
        [
            Category("Employer - CEP"),
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

        #region RegularExpression - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do RegularExpression
        /// </summary>
        [
            Category("Employer - CEP"),
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

        #region LabelErro - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do LabelErro
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("Mensagem Erro WS"),
            Localizable(true)
        ]
        public string MensagemErroWS
        {
            get
            {
                return _lblErro.Text;
            }
            set
            {
                _lblErro.Text = value;
            }
        }
        #endregion

        #region DisplayMensagemErroWS - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do LabelErro
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("Mensagem Erro WS"),
            Localizable(true)
        ]
        public string DisplayMensagemErroWS
        {
            get
            {
                return _lblErro.Style["display"];
            }
            set
            {
                _lblErro.Style["display"] = value;
            }
        }
        #endregion

        #region TextBox - CssClass
        /// <summary>
        /// CSS do txtValor
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("CssClassTextBox")
        ]
        public string CssClassTextBox
        {
            set
            {
                _txtValor.CssClass = value;
            }
            get
            {
                return _txtValor.CssClass;
            }

        }
        #endregion

        #region RegularExpression - CssClass
        /// <summary>
        /// CSS do cvValor
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("CssClass RegularExpression")
        ]
        public string CssClassRegularExpression
        {
            set
            {
                _reValor.CssClass = value;
            }
            get
            {
                return _reValor.CssClass;
            }

        }
        #endregion

        #region RequiredField - CssClass
        /// <summary>
        /// CSS do rfValor
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("CssClass RequiredField")
        ]
        public string CssClassRequiredField
        {
            set
            {
                _rfValor.CssClass = value;
            }
            get
            {
                return _rfValor.CssClass;
            }

        }
        #endregion

        #region LabelErro - CssClass
        /// <summary>
        /// CSS do cvValor
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("CssClass LabelErro")
        ]
        public string CssClassLabelErro
        {
            set
            {
                _lblErro.CssClass = value;
            }
            get
            {
                return _lblErro.CssClass;
            }

        }
        #endregion

        #region Valor
        /// <summary>
        /// Valor do campo Cep
        /// </summary>
        [Browsable(false)]
        public string Valor
        {
            get
            {
                string valor = LimparMascara(_txtValor.Text);
                if (ValidarFormato(valor))
                    return valor;
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    value = LimparMascara(value);
                    if (ValidarFormato(value))
                    {
                        value = LimparMascara(value);
                        string[] vlr = new[]{
                            value.Substring(0,5),
                            value.Substring(5)};
                        value = string.Format("{0}-{1}", vlr);
                    }
                }
                _txtValor.Text = value;
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
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
            Category("Employer - CEP"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
        }
        #endregion

        #region OnChangeClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - CEP"),
            DisplayName("OnChangeClient")
        ]
        public string OnChangeClient
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Construtor

        public CEP()
        {
            _txtValor = new TextBox();
            _reValor = new RegularExpressionValidator();
            _rfValor = new RequiredFieldValidator();
            _lblErro = new Label();
        }

        #endregion

        #region Métodos

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
            if (_rfValor.Enabled)
                nomeValidadores.Add(_rfValor.ClientID);
            nomeValidadores.Add(_reValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 8); ");
            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 8);");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{5}-\\\\d{3}');");

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraCEP(this);");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraCEP(this);");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
            if (OnChangeClient != null)
                _txtValor.Attributes["OnChange"] += OnChangeClient + "(" + new JSONReflector(args) + ");";
            if (!String.IsNullOrEmpty(OnBlurClient))
                _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Inicializar TextBox CEP
        /// </summary>
        private void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 9;
            _txtValor.MaxLength = 9;
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Inicializar RequiredField CEP
        /// </summary>
        private void InicializarRequiredField()
        {
            _rfValor.ID = "rfValor";
            _rfValor.ForeColor = Color.Empty;
            _rfValor.ControlToValidate = _txtValor.ID;
            _rfValor.Display = ValidatorDisplay.Dynamic;
            _rfValor.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRegularExpression
        /// <summary>
        /// Inicializar RegularExpression CEP
        /// </summary>
        private void InicializarRegularExpression()
        {
            _reValor.ID = "_reValor";
            _reValor.ForeColor = Color.Empty;
            _reValor.ControlToValidate = _txtValor.ID;
            _reValor.ValidationExpression = @"(^\d{5}\-\d{3}$)|(^\d{8}$)";
            _reValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarLabelErro
        /// <summary>
        /// Inicializar Label de Erro
        /// </summary>
        private void InicializarLabelErro()
        {
            _lblErro.ID = "lblErro";
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Remove os caracteres especiais do CEP
        /// </summary>
        /// <param name="valor">CEP</param>
        /// <returns>Retorna somento os numeros do CEP</returns>
        public static string LimparMascara(string valor)
        {
            valor = valor.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(valor, @"\d{5}-\d{3}"))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(valor, "");
            }
            return valor;
        }
        #endregion

        #region ValidarFormato
        /// <summary>
        /// Validar o formato do valor desejado
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool ValidarFormato(string valor)
        {
            Int64 vlr;
            if (valor.Length.Equals(8) && Int64.TryParse(valor, out vlr))
                return true;
            return false;
        }
        #endregion

        #region Consultar
        /// <summary>
        /// Utilizada pelo javascript para validação
        /// </summary>
        /// <param name="valor">valor a validar</param>
        /// <returns>custom validator se necessário</returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string AjaxConsultar(string valor)
        {
            valor = LimparMascara(valor);
            if (string.IsNullOrEmpty(valor) || !ValidarFormato(valor))
                return string.Empty;
            try
            {
                //String uri = String.Empty;
                CEPWS.cepws wsCep = new CEPWS.cepws();

                //Object o = ConfigurationManager.
                //    GetSection("applicationSettings/Employer.Plataforma.Web.WebFoPag.Properties.Settings");

                //if (o != null)
                //{
                //    //String uri = (o as ClientSettingsSection).
                //    //    Settings.
                //    //    Get("Employer_Plataforma_Web_WebFoPag_CEPWS_cepws").Value.ValueXml.InnerText;

                //    IEnumerator e = (o as ClientSettingsSection).Settings.GetEnumerator();
                //    e.Reset();
                //    e.MoveNext();
                //    do
                //    {
                //        if ((e.Current as SettingElement).Value.ValueXml.InnerText.IndexOf("cepws.asmx", StringComparison.OrdinalIgnoreCase) > -1)
                //            uri = (e.Current as SettingElement).Value.ValueXml.InnerText;

                //    } while (e.MoveNext());
                //}

                //if (!String.IsNullOrEmpty(uri))
                //    wsCep.Url = uri;

                BNE.BLL.Security.ServiceAuth.GerarHashAcessoWS(wsCep);

                CEPWS.CEP objCep = new CEPWS.CEP();

                objCep.Cep = valor;

                if (ValidarFormato(objCep.Cep) && wsCep.CompletarCEP(ref objCep))
                {
                    var parametroCEP = new
                    {
                        CEP = objCep.Cep,
                        Estado = (objCep.Estado == null) ? String.Empty : objCep.Estado,
                        Cidade = (objCep.Cidade == null) ? String.Empty : objCep.Cidade,
                        Logradouro = (objCep.Logradouro == null) ? String.Empty : objCep.Logradouro,
                        TipoLogradouro = (objCep.TipoLogradouro == null) ? String.Empty : objCep.TipoLogradouro,
                        Bairro = (objCep.Bairro == null) ? String.Empty : objCep.Bairro
                    };

                    return Convert.ToString(new JSONReflector(parametroCEP));
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region ValorAlterado
        /// <summary>
        /// Evento ao alterar valor
        /// </summary>
        public event EventHandler ValorAlterado;
        #endregion

        #region OnInit
        /// <summary>
        /// Especificar ao iniciar os javascripts a serem utilizados
        /// </summary>
        /// <param name="e">Argumento do evento</param>        
        protected override void OnInit(EventArgs e)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("CEP.js"))
                Page.ClientScript.RegisterClientScriptInclude("CEP.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.CEP.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));

            _lblErro.Style["display"] = "none";
        }

        protected override void CreateChildControls()
        {
            InicializarLabelErro();
            InicializarTextBox();
            InicializarRequiredField();
            InicializarRegularExpression();

            //Adicionar Controles no WebForm            
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor); //RequiredField
            pnlValidador.Controls.Add(_reValor); //RegularExpression
            Controls.Add(pnlValidador);
            Controls.Add(_lblErro);
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
            Ajax.Utility.RegisterTypeForAjax(typeof(CEP));
        }
        #endregion

        #endregion
    }
}
