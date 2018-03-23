using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.PIS.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:PIS runat=server Obrigatorio=False PermitePasep=True PermitePIS=True PermiteNIT=True PermiteSUS=True ></{0}:PIS>")]
    public class PIS : CompositeControl
    {
        #region Tipo Valor
        /// <summary>
        /// Tipo do formato do valor
        /// </summary>
        public enum TipoValor
        {
            Pasep,
            PIS,
            NIT,
            SUS
        }
        #endregion

        #region Atributos

        private TextBox _txtValor;
        private CustomValidator _cvValor;
        private RequiredFieldValidator _rfValor;
        private Label _lblValor;

        #endregion

        #region Propriedades

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - PIS"),
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
            Category("Employer - PIS"),
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

        #region Validation Group
        /// <summary>
        /// ValidationGroup para o componente
        /// </summary>
        [
            Category("Employer - PIS"),
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
            Category("Employer - PIS"),
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
            Category("Employer - PIS"),
            DisplayName("Mensagem Erro Obrigatório"),
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
            Category("Employer - PIS"),
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

        #region TextBox - CssClass
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [
            Category("Employer - PIS"),
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

        #region Label - CssClass
        /// <summary>
        /// CssClass do Label
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("CssClass Descricao")
        ]
        public string CssClassDescricao
        {
            get
            {
                return _lblValor.CssClass;
            }
            set
            {
                _lblValor.CssClass = value;
            }
        }
        #endregion

        #region RequiredField - CssClass
        /// <summary>
        /// CssClass do RequiredField
        /// </summary>
        [
            Category("Employer - PIS"),
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

        #region CustomValidator - CssClass
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [
            Category("Employer - PIS"),
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

        #region Exibir Desrição
        /// <summary>
        /// CssClass do Label
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("Exibir Desrição")
        ]
        public bool ExibirDescricao
        {
            get
            {
                return _lblValor.Enabled;
            }
            set
            {
                _lblValor.Enabled = value;
            }
        }
        #endregion

        #region PermitePasep
        /// <summary>
        /// Permitir Pasep
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("Permite Pasep")
        ]
        public bool PermitePasep { get; set; }
        #endregion

        #region PermitePIS
        /// <summary>
        /// Permitir PIS
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("Permite PIS")
        ]
        public bool PermitePIS { get; set; }
        #endregion

        #region PermiteNIT
        /// <summary>
        /// Permitir NIT
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("Permite NIT")
        ]
        public bool PermiteNIT { get; set; }
        #endregion

        #region PermiteSUS
        /// <summary>
        /// Permitir SUS
        /// </summary>
        [
            Category("Employer - PIS"),
            DisplayName("Permite SUS")
        ]
        public bool PermiteSUS { get; set; }
        #endregion

        #region Tipo
        /// <summary>
        /// Retornar o tipo digitado no campo (null se inválido)
        /// </summary>
        [Browsable(false)]
        public TipoValor? Tipo { get; set; }
        #endregion

        #region Valor
        /// <summary>
        /// Retornar ou setar o valor
        /// ###.#####.##-#
        /// </summary>
        [Browsable(false)]
        public string Valor
        {
            get
            {
                if (ValidarFormato(LimparMascara(_txtValor.Text)))
                    return LimparMascara(_txtValor.Text);
                return string.Empty;
            }
            set
            {
                _lblValor.Text = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (Validar(value))
                    {
                        value = LimparMascara(value);
                        value = string.Format("{0}.{1}.{2}-{3}", value.Substring(0, 3), value.Substring(3, 5), value.Substring(8, 2), value.Substring(10));
                        _lblValor.Text = RecuperarTipo(value);
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
            Category("Employer - PIS"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region Width
        public override Unit Width
        {
            get
            {
                return _txtValor.Width;
            }
            set
            {
                _txtValor.Width = value;
            }
        }
        #endregion
        #endregion

        #region Construtores
        /// <summary>
        /// Instânciar inicialmente suas variaveis
        /// </summary>
        public PIS()
        {
            _txtValor = new TextBox();
            _cvValor = new CustomValidator();
            _rfValor = new RequiredFieldValidator();
            _lblValor = new Label();
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

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, 11);");
            _txtValor.Attributes.Add("OnKeyPress", "return ApenasNumeros(this, event, 11);");
            _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{3}.\\\\d{5}.\\\\d{2}-\\\\d{1}');");

            _txtValor.Attributes.Add("OnBlur", "AplicarMascaraPIS(this,'" + _lblValor.ClientID + "');");
            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtValor.Attributes.Add("OnChange", "AplicarMascaraPIS(this,'" + _lblValor.ClientID + "');");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Atribuir caracteristicas do campo TextBox
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
        /// Atribuir caracteristicas do campo RequiredField
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
        /// Atribuir caracteristicas do campo CustomValidator
        /// </summary>
        protected virtual void InicializarCustomValidator()
        {
            _cvValor.ID = "cvValor";
            _cvValor.ForeColor = Color.Empty;
            _cvValor.ControlToValidate = _txtValor.ID;
            _cvValor.ClientValidationFunction = "ValidarPIS";
            _cvValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarLabel
        /// <summary>
        /// Inicializar label de descrição
        /// </summary>
        private void InicializarLabel()
        {
            _lblValor.ID = "lblValor";
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar pontuação do campo TextBox
        /// </summary>
        /// <returns>Valor do campo TextBox sem pontuação</returns>
        public static string LimparMascara(string valor)
        {
            valor = valor.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(valor, @"\d{3}.\d{5}.\d{2}-\d{1}"))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(valor, string.Empty);
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
        private static bool ValidarFormato(string valor)
        {
            Int64 vlr;
            if (valor.Length.Equals(11) && Int64.TryParse(valor, out vlr))
                return true;
            return false;
        }
        #endregion

        #region ValidarCalculo
        /// <summary>
        /// Validar o cálculo do valor desejado está correto 
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        private static bool ValidarCalculo(string valor)
        {
            UInt16[] aux = { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int result = 0;
            for (UInt16 i = 0; i < 10; i++)
                result += (Convert.ToInt32(valor[i].ToString()) * aux[i]);

            result = result % 11;
            if (result <= 1)
                result = 0;
            else
                result = 11 - result;

            return valor[10].Equals(Convert.ToChar(result.ToString()));
        }
        #endregion

        #region ValidarTipoPermitido
        /// <summary>
        /// Validar se o tipo do valor desejado está correto
        /// </summary>
        /// <param name="valor">Valor desejado a validar</param>
        /// <param name="tipo"></param>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        private static bool ValidarTipoPermitido(string valor, TipoValor tipo)
        {
            Int64 vlr = Convert.ToInt64(valor.Substring(0, 10));

            switch (tipo)
            {
                case TipoValor.Pasep:
                    if ((vlr >= 1000000001 && vlr <= 1022000000) || (vlr >= 1700000001 && vlr <= 1999999999))
                    {
                        return true;
                    }
                    return false;
                case TipoValor.PIS:
                    {
                        if ((vlr >= 1022000001 && vlr <= 1089999999) || (vlr >= 1200000001 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
                            return true;
                    }
                    return false;
                case TipoValor.NIT:
                    {
                        if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999))
                            return true;
                    }
                    return false;
                case TipoValor.SUS:
                    {
                        if (vlr >= 2000000000 && vlr <= 2999999999)
                            return true;
                    }
                    return false;
                default:
                    return false;
            }
        }
        #endregion

        #region
        /// <summary>
        /// Valida se o número está dentro dos intervalos permitidos
        /// </summary>
        /// <param name="valor">O Número</param>
        /// <returns>True se está entre os intervalos válidos</returns>
        private static bool ValidarIntervalo(string valor)
        {
            Int64 vlr = Convert.ToInt64(valor.Substring(0, 10));

            if ((vlr >= 1000000001 && vlr <= 1022000000) || (vlr >= 1700000001 && vlr <= 1999999999))
                return true;
            if ((vlr >= 1022000001 && vlr <= 1089999999) || (vlr >= 1200000001 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
                return true;
            if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999))
                return true;
            if (vlr >= 2000000000 && vlr <= 2999999999)
                return true;

            return false;
        }
        #endregion

        #region Validar
        /// <summary>
        /// Validar valor do campo TextBox atende as regras
        /// </summary>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool Validar(string valor)//, TipoValor tipo)
        {
            valor = LimparMascara(valor);

            if (!ValidarFormato(valor))
                return false;

            if (Convert.ToDecimal(valor) <= 1)
                return false;

            if (!ValidarCalculo(valor))
                return false;

            if (!ValidarIntervalo(valor))
                return false;

            return true;
        }
        #endregion

        #region RecuperarTipo
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarTipo(string valor)
        {
            valor = LimparMascara(valor);
            string tipo = null;
            if (ValidarFormato(valor))
            {
                if (ValidarCalculo(valor))
                {
                    Int64 vlr = Convert.ToInt64(valor.Substring(0, 10));

                    if ((vlr >= 1000000000 && vlr <= 1019999999) || (vlr >= 1700000000 && vlr <= 1909999999))
                        tipo = TipoValor.Pasep.ToString();
                    else if ((vlr >= 1020000000 && vlr <= 1089999999) || (vlr >= 1200000000 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
                        tipo = TipoValor.PIS.ToString();
                    else if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999) || (vlr >= 2670000000 && vlr <= 2679999999))
                        tipo = TipoValor.NIT.ToString();
                    //else if (vlr >= 2000000000 && vlr <= 2999999999)
                    else if ((vlr >= 2000000000 && vlr <= 2669999999) || (vlr >= 2680000000 && vlr <= 2999999999))
                        tipo = TipoValor.PIS.ToString();
                }
            }
            return tipo;
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

            if (!Page.ClientScript.IsClientScriptBlockRegistered("PIS.js"))
                Page.ClientScript.RegisterClientScriptInclude("PIS.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.PIS.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarLabel();
            InicializarTextBox();
            InicializarRequiredField();
            InicializarCustomValidator();

            //Adicionar Controles no WebForm
            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor); //RequiredFieldValidator
            pnlValidador.Controls.Add(_cvValor); //CustomValidator
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor); //Campo Texto
            Controls.Add(_lblValor);

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
            Ajax.Utility.RegisterTypeForAjax(typeof(PIS));
        }
        #endregion

        #endregion
    }
}