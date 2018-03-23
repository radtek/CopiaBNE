using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.CNPJ.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:CNPJ runat=server Obrigatorio=False ></{0}:CNPJ>")]
    public class CNPJ : CompositeControl
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
            Category("Employer - CNPJ"),
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
            Category("Employer - CNPJ"),
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
        /// ValidationGroup do Componente CNPJ
        /// </summary>
        [
            Category("Employer - CNPJ"),
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
        /// Define se o Campo CNPJ é obrigatório
        /// </summary>
        [
            Category("Employer - CNPJ"),
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
            Category("Employer - CNPJ"),
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
        /// Mensagem de Erro de CNPJ Inválido.
        /// </summary>
        [
            Category("Employer - CNPJ"),
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
            Category("Employer - CNPJ"),
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
            Category("Employer - CNPJ"),
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
            Category("Employer - CNPJ"),
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
                if(Validar(_txtValor.Text))
                    return LimparMascara(_txtValor.Text);
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {//##.###.###/####-##
                    value = value.Trim();
                    if (Validar(value))
                    {
                        value = LimparMascara(value);
                        string[] vlr = new[] { 
                        value.Substring(0, 2), 
                        value.Substring(2, 3), 
                        value.Substring(5, 3), 
                        value.Substring(8, 4), 
                        value.Substring(12)};
                        value = string.Format("{0}.{1}.{2}/{3}-{4}", vlr);
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
            Category("Employer - CNPJ"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Construtores
        public CNPJ()
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

            nomeValidadores.Add(_rfValor.ClientID);
            nomeValidadores.Add(_cvValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 14);");
            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 14);");

            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{2}.\\\\d{3}.\\\\d{3}/\\\\d{4}-\\\\d{2}');");

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraCNPJ(this);");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraCNPJ(this);");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");"; 
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método de Inicialização do TextBox CNPJ
        /// ##.###.###/####-##
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 18;
            _txtValor.MaxLength = 18;
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
            _cvValor.ClientValidationFunction = "ValidarCNPJ";
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
            //##.###.###/####-##
            return valor.PadLeft(14, '0').Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar os caracteres especiais do Cnpj
        /// </summary>
        /// <returns>Cnpj limpo</returns>
        public static string LimparMascara(string cnpj)
        {
            cnpj = cnpj.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(cnpj, @"\d{2}.\d{3}.\d{3}/\d{4}-\d{2}"))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(cnpj, "");
            }
            return cnpj;
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
            if (valor.Length.Equals(14) && Int64.TryParse(valor, out vlr))
                return true;
            return false;
        }
        #endregion

        #region ValidarCalculo
        /// <summary>
        /// Verifica se o CNPJ informado é válido
        /// </summary>
        /// <param name="cnpj">CNPJ para validação</param>
        /// <returns>Retorna true caso o CNPJ seja válido</returns>
        private static bool ValidarCalculo(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            int i;
            string inx = cnpj.Substring(12, 2);
            cnpj = cnpj.Substring(0, 12);
            int s1 = 0;
            int s2 = 0;
            int m2 = 2;
            for (i = 11; i >= 0; i--)
            {
                string l = cnpj.Substring(i, 1);
                int v = Convert.ToInt16(l);
                int m1 = m2;
                m2 = m2 < 9 ? m2 + 1 : 2;
                s1 += v * m1;
                s2 += v * m2;
            }
            s1 %= 11;
            int d1 = s1 < 2 ? 0 : 11 - s1;
            s2 = (s2 + 2 * d1) % 11;
            int d2 = s2 < 2 ? 0 : 11 - s2;
            string dig = d1.ToString() + d2.ToString();

            return inx.Equals(dig);
        }
        #endregion

        #region Validar
        /// <summary>
        /// Validar valor do campo TextBox atende as regras
        /// </summary>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool Validar(string cnpj)
        {
            cnpj = LimparMascara(cnpj);

            if (!ValidarFormato(cnpj))
                return false;

            if (!ValidarCalculo(cnpj))
                return false;

            return true;
        }
        #endregion

        #region RetornarMatriz
        /// <summary>
        /// Retornar Matriz
        /// </summary>
        /// <returns></returns>
        public static string RetornarMatriz(string cnpj)
        {
            cnpj = LimparMascara(cnpj);

            if (!ValidarFormato(cnpj))
                return null;

            if (!ValidarCalculo(cnpj))
                return null;


            if (cnpj.Substring(8, 4).Equals("0001"))
                return null;

            cnpj = cnpj.Remove(8) + "0001";

            int s1 = 0;
            int s2 = 0;
            int m2 = 2;
            for (int i = 11; i >= 0; i--)
            {
                string l = cnpj.Substring(i, 1);
                int v = Convert.ToInt16(l);
                int m1 = m2;
                m2 = m2 < 9 ? m2 + 1 : 2;
                s1 += v * m1;
                s2 += v * m2;
            }
            s1 %= 11;
            int d1 = s1 < 2 ? 0 : 11 - s1;
            s2 = (s2 + 2 * d1) % 11;
            int d2 = s2 < 2 ? 0 : 11 - s2;
            string dig = d1.ToString() + d2.ToString();

            return cnpj + dig;
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

            if (!Page.ClientScript.IsClientScriptBlockRegistered("CNPJ.js"))
                Page.ClientScript.RegisterClientScriptInclude("CNPJ.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.CNPJ.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
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
            Ajax.Utility.RegisterTypeForAjax(typeof(CNPJ));
        }
        #endregion

        #endregion
    }
}