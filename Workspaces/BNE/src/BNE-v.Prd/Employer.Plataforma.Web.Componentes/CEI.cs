using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.CEI.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:CEI runat=server Obrigatorio=False ></{0}:CEI>")]
    public class CEI : CompositeControl
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
            Category("Employer - CEI"),
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
            Category("Employer - CEI"),
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
        /// ValidationGroup para o componente
        /// </summary>
        [
            Category("Employer - CEI"),
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
        /// Definir se o Campo é obrigatório
        /// </summary>
        [
            Category("Employer - CEI"),
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
        /// Mensagem de Erro do RequiredFieldValidator
        /// </summary>
        [
            Category("Employer - CEI"),
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

        #region CustomValidator - Mensagem de Erro
        /// <summary>
        /// Mensagem de Erro do CustomValidator
        /// </summary>
        [
            Category("Employer - CEI"),
            DisplayName("Mensagem Formato Incorreto"),
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

        #region TextBox - CssClass
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [Category("Employer - CEI")]
        [DisplayName("CssClass TextBox")]
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
        [Category("Employer - CEI")]
        [DisplayName("CssClass RequiredField")]
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

        #region CustomValidator - CssClass
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [
            Category("Employer - CEI"),
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
        /// Retornar ou setar o valor
        /// ##.###.#####/##
        /// </summary>
        [Browsable(false)]
        public string Valor
        {
            get
            {
                string valor = LimparMascara(_txtValor.Text);
                if (Validar(valor))
                    return valor;
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (Validar(value))
                    {
                        value = LimparMascara(value);
                        string[] vlr = new[]{
                            value.Substring(0,2),
                            value.Substring(2,3),
                            value.Substring(5,5),
                            value.Substring(10,2)};
                        value = string.Format("{0}.{1}.{2}/{3}", vlr);
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
            Category("Employer - CEI"),
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
        /// <summary>
        /// Construtor
        /// </summary>
        public CEI()
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

            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 12);");
            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 12);");

            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{2}.\\\\d{3}.\\\\d{5}/\\\\d{2}');");

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraCEI(this);");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraCEI(this);");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");"; 
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método que inicializa o componente TextBox Cei
        /// ##.###.###.###-#
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            //Configurando a TextBox
            _txtValor.ID = "txtValor";
            _txtValor.Columns = 15;
            _txtValor.MaxLength = 15;
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Método que inicializa o RequiredField CEI
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

        #region InicializarCustomValidator
        /// <summary>
        /// Método que inicializa o componente 
        /// CustonValidator CEI
        /// </summary>
        protected virtual void InicializarCustomValidator()
        {
            _cvValor.ID = "cvValor";
            _cvValor.ForeColor = Color.Empty;
            _cvValor.ControlToValidate = _txtValor.ID;
            _cvValor.ClientValidationFunction = "ValidarCEI";
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

            //##.###.#####/##
            return valor.PadLeft(12, '0').Insert(2, ".").Insert(6, ".").Insert(12, "/");
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar pontuação do campo TextBox
        /// </summary>
        /// <returns>Valor do campo TextBox sem pontuação</returns>
        public static string LimparMascara(string cei)
        {
            cei = cei.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(cei, @"\d{2}.\d{3}.\d{5}/\d{2}"))
            {
                const string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(cei, string.Empty);
            }
            return cei;
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
            if (valor.Length.Equals(12) && Int64.TryParse(valor, out vlr))
                return true;
            return false;
        }
        #endregion

        #region ValidarCalculo
        /// <summary>
        /// Calcula o digito verificador de um número
        /// de CEI.
        /// </summary>
        /// <param name="cei">string com o número do cei sem formatação</param>
        /// <returns>Digito Verificador</returns>
        private static bool ValidarCalculo(string cei)
        {
            //variáveis

            //Contantes para efeito de cálculo
            int[] ceiConstante = { 7, 4, 1, 8, 5, 2, 1, 6, 3, 7, 4 };

            //Calcula a soma dos digitos multiplicados pelo seu correspondente
            //no vetor de constantes para o calculo.
            int soma = 0;
            for (int i = 0; i < 11; i++)
                soma += Convert.ToInt32(cei[i].ToString()) * ceiConstante[i];

            //Com a variável soma calculada, é possível calcular os valores das
            //variáveis unidades e dezena.
            int unidade = soma % 10;
            int dezena = (soma % 100) - unidade;

            //Calculo do digito verificador.
            int digitoVerificador = 10 - (((unidade + (dezena / 10)) % 10));

            //Caso o digito calculado for maior que 9, o valor do dígito é 0. 
            if (digitoVerificador > 9)
                digitoVerificador = 0;

            return cei[11].Equals(Convert.ToChar(digitoVerificador.ToString()));
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

            if (!ValidarCalculo(valor))
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
        /// Especificar ao iniciar os javascripts a serem utilizados
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("CEI.js"))
                Page.ClientScript.RegisterClientScriptInclude("CEI.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.CEI.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            // Inicializando os componentes.
            InicializarTextBox();
            InicializarRequiredField();
            InicializarCustomValidator();

            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor); //RequiredFieldValidator
            pnlValidador.Controls.Add(_cvValor); //CustomValidator
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor); //Campo Texto (CEI)

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
            Ajax.Utility.RegisterTypeForAjax(typeof(CEI));
        }
        #endregion

        #endregion
    }
}