using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.PlacaVeiculo.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]


namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:PlacaVeiculo runat=server Obrigatorio=False ></{0}:PlacaVeiculo>")]
    public class PlacaVeiculo : CompositeControl
    {
        #region Atributos

        private TextBox _txtValor;
        private RegularExpressionValidator _reValor;
        private RequiredFieldValidator _rfValor;

        #endregion

        #region Propriedades

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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

        #region Validation Group
        /// <summary>
        /// ValidationGroup do Ccomponente
        /// </summary>
        [
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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

        #region TextBox - CssClass
        /// <summary>
        /// CSS do txtValor
        /// </summary>
        [
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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
            Category("Employer - PlacaVeiculo"),
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

        #region Valor
        /// <summary>
        /// Valor do campo Veiculo
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
                _txtValor.Text = value;
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - PlacaVeiculo"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Construtor

        public PlacaVeiculo()
        {
            _txtValor = new TextBox();
            _reValor = new RegularExpressionValidator();
            _rfValor = new RequiredFieldValidator();
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

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 7); ");
            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumerosPlacaVeiculo(this, event);");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '^[A-Z]{3}(-)\\\\d{4}$');");

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraPlacaVeiculo(this);");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraPlacaVeiculo(this);");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Inicializar TextBox PlacaVeiculo
        /// </summary>
        private void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 8;
            _txtValor.MaxLength = 8;
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Inicializar RequiredField PlacaVeiculo
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
        /// Inicializar RegularExpression PlacaVeiculo
        /// </summary>
        private void InicializarRegularExpression()
        {
            _reValor.ID = "reValor";
            _reValor.ForeColor = Color.Empty;
            _reValor.ControlToValidate = _txtValor.ID;
            _reValor.ValidationExpression = @"^[A-Z]{3}(-)\d{4}$";
            _reValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region Validar
        /// <summary>
        /// Validar o formato do valor desejado
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool Validar(string valor)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[A-Z]{3}(-)\d{4}$"))
                return true;
            return false;
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
            if (!Page.ClientScript.IsClientScriptBlockRegistered("PlacaVeiculo.js"))
                Page.ClientScript.RegisterClientScriptInclude("PlacaVeiculo.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.PlacaVeiculo.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }

        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarRequiredField();
            InicializarRegularExpression();

            //Adicionar Controles no WebForm            
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor); //RequiredField
            pnlValidador.Controls.Add(_reValor); //RegularExpression
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor);

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

        #endregion
    }
}