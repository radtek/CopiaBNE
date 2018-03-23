using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.CPF.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:CPF runat=server Obrigatorio=False ></{0}:CPF>")]
    public class CPF : BaseCompositeControl
    {
        #region Atributos

        private TextBox _txtValor;
        private CustomValidator _cvValor;
        private RequiredFieldValidator _rfValor;
        #endregion

        #region Propriedades

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - CPF"),
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

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - CPF"),
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
        /// ValidationGroup do Componente CPF
        /// </summary>
        [
            Category("Employer - CPF"),
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
                _rfValor.ValidationGroup = value;
            }
        }
        #endregion

        #region Campo Obrigatorio
        /// <summary>
        /// Define se o Campo CPF é obrigatório
        /// </summary>
        [
            Category("Employer - CPF"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            {
                return _rfValor.Enabled;
            }
            set
            {
                _rfValor.Enabled = value;
            }
        }
        #endregion

        #region RequiredField - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro Campo Obrigatório
        /// </summary>
        [
            Category("Employer - CPF"),
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

        #region Custom Validator - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro de CPF Inválido.
        /// </summary>
        [
            Category("Employer - CPF"),
            DisplayName("Mensagem Erro Formato"),
            Localizable(true)
        ]
        public string MensagemErroFormato
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

        #region CustomValidator - CssClass
        /// <summary>
        /// Classe CSS do Componente CustomValidator
        /// </summary>
        [
            Category("Employer - CPF"),
            DisplayName("CssClass CustomValidator")
        ]
        public string CustomCssClass
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

        #region RequiredField - CssClass
        /// <summary>
        /// Classe CSS do Componente RequiredField
        /// </summary>
        [
            Category("Employer - CPF"),
            DisplayName("CssClass RequiredField")
        ]
        public string RequiredCssClass
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

        #region TextBox - CssClass
        /// <summary>
        /// Classe CSS do Componente TextBox
        /// </summary>
        [
            Category("Employer - CPF"),
            DisplayName("CssClass TextBox")
        ]
        public string TextBoxCssClass
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

        #region Valor
        /// <summary>
        /// Get/Set 
        /// </summary>
        public string Valor
        {
            get
            {
                if (Validar(_txtValor.Text))
                    return LimparMascara(_txtValor.Text);
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {//##.###.###-##
                    value = value.Trim();
                    if (Validar(value))
                    {
                        value = LimparMascara(value);
                        string[] vlr = new[] { 
                        value.Substring(0, 3), 
                        value.Substring(3, 3), 
                        value.Substring(6, 3), 
                        value.Substring(9)};
                        value = string.Format("{0}.{1}.{2}-{3}", vlr);
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
            Category("Employer - CPF"),
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
            Category("Employer - CPF"),
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
            Category("Employer - CPF"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Construtores
        public CPF()
        {
            _txtValor = new TextBox();
            _cvValor = new CustomValidator();
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
            nomeValidadores.Add(_cvValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 11); ");
            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 11);");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{3}.\\\\d{3}.\\\\d{3}-\\\\d{2}');");
            if (!String.IsNullOrEmpty(OnFocusClient))
                _txtValor.Attributes["OnFocus"] += OnFocusClient + "(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraCPF(this);");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraCPF(this);");

            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";

			if (ValorAlterado != null) {
				_txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";

                // Seta function para verificar situacao dos validadores associados ao campo.
                _txtValor.Attributes["OnChange"] += "ValidarSituacaoCampo(this," + new JSONReflector(args) + ");";
			}

			if (!String.IsNullOrEmpty(OnBlurClient))
                _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método de Inicialização do TextBox CPF
        /// ###.###.###-##
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 14;
            _txtValor.MaxLength = 14;
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Método de Inicialização do Required Field
        /// </summary>
        protected void InicializarRequiredField()
        {
            _rfValor.ID = "rfValor";
            _rfValor.ForeColor = Color.Empty;
            _rfValor.ControlToValidate = _txtValor.ID;
            _rfValor.Display = ValidatorDisplay.Dynamic;
            _rfValor.SetFocusOnError = false;
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
            _cvValor.ClientValidationFunction = "ValidarCPF";
            _cvValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region AplicarMascara
        /// <summary>
        /// Aplicar mascara
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string AplicarMascara(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;
            return valor.PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar os caracteres especiais do Cnpj
        /// </summary>
        /// <returns>Cnpj limpo</returns>
        public static string LimparMascara(string cpf)
        {
            cpf = cpf.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(cpf, @"\d{3}.\d{3}.\d{3}-\d{2}"))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(cpf, "");
            }
            return cpf;
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
            Int64 vlr;
            if (valor.Length.Equals(11) && Int64.TryParse(valor, out vlr))
            {
                for (int i = 1; i < 11; i++)
                {
                    if (!valor[0].Equals(valor[i]))
                        return true;
                }
                return false;
            }
            return false;
        }
        #endregion

        #region ValidarCalculo
        /// <summary>
        /// Verifica se o CPF informado é válido
        /// </summary>
        /// <param name="valor">CPF para validação</param>
        /// <returns>Retorna true caso o CPF seja válido</returns>
        private static bool ValidarCalculo(string valor)
        {
            int soma1 = 0;
            int soma2 = 0;
            for (int i = 0; i < 9; i++)
            {
                String dig_cpf = valor.Substring(i, 1);
                soma1 += int.Parse(dig_cpf) * (10 - i);
                soma2 += int.Parse(dig_cpf) * (11 - i);
            }
            int dv1 = (11 - (soma1 % 11));
            dv1 = dv1 >= 10 ? 0 : dv1;
            soma2 = soma2 + (dv1 * 2);
            int dv2 = (11 - (soma2 % 11));
            dv2 = dv2 >= 10 ? 0 : dv2;
            string dv = valor.Substring(9, 2);
            return dv.Equals(dv1.ToString() + dv2.ToString());
        }
        #endregion

        #region Validar
        public static bool Validar(string cpf)
        {
            cpf = LimparMascara(cpf);

            if (!ValidarFormato(cpf))
                return false;

            if (!ValidarCalculo(cpf))
                return false;

            return true;
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
        /// Registra o arquivo Javascript
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("CPF.js"))
                Page.ClientScript.RegisterClientScriptInclude("CPF.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.CPF.js"));

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarTextBox();
            InicializarRequiredField();
            InicializarCustomValidator();

            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor);
            pnlValidador.Controls.Add(_cvValor);
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
            //string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            //if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
            //    ValorAlterado(_txtValor, null);
        }
        #endregion

        #region OnLoad
        /// <summary>
        /// Evento executado quando o componente é carregado.
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnLoad(EventArgs e)
        {
            // Fix gambiarra
            if (ValorAlterado != null)
            {
                this._txtValor.TextChanged += new EventHandler(_txtValor_TextChanged);
            }

            base.OnLoad(e);
            Ajax.Utility.RegisterTypeForAjax(typeof(CPF));
        }
        #endregion

        #region _txtValor_TextChanged
        void _txtValor_TextChanged(object sender, EventArgs e)
        {
            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
                ValorAlterado(_txtValor, null);
        }
        #endregion
        #endregion
    }
}