using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.Telefone.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:Telefone runat=server Obrigatorio=False Formato=DDD ></{0}:Telefone>")]
    public class Telefone : CompositeControl
    {
        #region FormatoTelefone
        /// <summary>
        /// Tipo do formato do telefone
        /// </summary>
        public enum FormatoTelefone
        {
            DDI,
            DDD,
            Numero
        }

        #endregion

        #region TipoTelefone
        /// <summary>
        /// Tipo do telefone
        /// </summary>
        public enum TipoTelefone
        {
            Outros,
            Fixo,
            Celular,
            FixoCelular
        }
        #endregion

        #region Atributos

        private TextBox _txtDDI;
        private RegularExpressionValidator _reDDI;
        private RequiredFieldValidator _rfDDI;
        private TextBox _txtDDD;
        private RegularExpressionValidator _reDDD;
        private RequiredFieldValidator _rfDDD;
        private TextBox _txtFone;
        private RegularExpressionValidator _reFone;
        private RequiredFieldValidator _rfFone;

        private FormatoTelefone _formato;
        private bool _permiteDDI;
        private bool _permiteDDD;

        private TipoTelefone _tipo;

        #endregion

        #region Propriedades

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("ReadOnly")
        ]
        public bool ReadOnly
        {
            get
            {
                return _txtFone.ReadOnly;
            }
            set
            {
                _txtDDI.ReadOnly = value;
                _txtDDD.ReadOnly = value;
                _txtFone.ReadOnly = value;
            }
        }

        #endregion

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("SetFocusOnError")
        ]
        public bool SetFocusOnError
        {
            get
            {
                return _reFone.SetFocusOnError;
            }
            set
            {
                _reDDI.SetFocusOnError = value;
                _reDDD.SetFocusOnError = value;
                _reFone.SetFocusOnError = value;
            }
        }

        #endregion

        #region Validation Group
        /// <summary>
        /// ValidationGroup para o componente
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Validation Group")
        ]
        public string ValidationGroup
        {
            get
            {
                return _reFone.ValidationGroup;
            }
            set
            {
                _reDDI.ValidationGroup = value;
                _reDDD.ValidationGroup = value;
                _reFone.ValidationGroup = value;

                _rfDDI.ValidationGroup = value;
                _rfDDD.ValidationGroup = value;
                _rfFone.ValidationGroup = value;
            }
        }
        #endregion

        #region Campo Obrigatorio
        /// <summary>
        /// Define se o(s) Campo(s) é obrigatório
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            {
                return _rfFone.Enabled;
            }
            set
            {
                _rfDDI.Enabled = value;
                _rfDDD.Enabled = value;
                _rfFone.Enabled = value;
            }
        }
        #endregion

        #region RequiredField - Mensagem de Erro DDI
        /// <summary>
        /// Mensagem de Erro do RequiredFieldValidator DDI
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Campo Obrigatório DDI"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorioDDI
        {
            get
            {
                return _rfDDI.ErrorMessage;
            }
            set
            {
                _rfDDI.ErrorMessage = value;
            }
        }
        #endregion

        #region RegularExpression - Mensagem de Erro DDI
        /// <summary>
        /// Mensagem de Erro do RegularExpression DDI
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Formato Incorreto DDI"),
            Localizable(true)
        ]
        public string MensagemErroFormatoDDI
        {
            get
            {
                return _reDDI.ErrorMessage;
            }
            set
            {
                _reDDI.ErrorMessage = value;
            }
        }
        #endregion

        #region RequiredField - Mensagem de Erro DDD
        /// <summary>
        /// Mensagem de Erro do RequiredFieldValidator DDD
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Campo Obrigatório DDD"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorioDDD
        {
            get
            {
                return _rfDDD.ErrorMessage;
            }
            set
            {
                _rfDDD.ErrorMessage = value;
            }
        }
        #endregion

        #region RegularExpression - Mensagem de Erro DDD
        /// <summary>
        /// Mensagem de Erro do RegularExpression DDD
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Formato Incorreto DDD"),
            Localizable(true)
        ]
        public string MensagemErroFormatoDDD
        {
            get
            {
                return _reDDD.ErrorMessage;
            }
            set
            {
                _reDDD.ErrorMessage = value;
            }
        }
        #endregion

        #region RequiredField - Mensagem de Erro Fone
        /// <summary>
        /// Mensagem de Erro do RequiredFieldValidator Fone
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Campo Obrigatório Fone"),
            Localizable(true)
        ]
        public string MensagemErroObrigatorioFone
        {
            get
            {
                return _rfFone.ErrorMessage;
            }
            set
            {
                _rfFone.ErrorMessage = value;
            }
        }
        #endregion

        #region RegularExpression - Mensagem de Erro Fone
        /// <summary>
        /// Mensagem de Erro do RegularExpression Fone
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Mensagem Formato Incorreto Fone"),
            Localizable(true)
        ]
        public string MensagemErroFormatoFone
        {
            get
            {
                return _reFone.ErrorMessage;
            }
            set
            {
                _reFone.ErrorMessage = value;
            }
        }
        #endregion

        #region TextBox - CssClass DDI
        /// <summary>
        /// CssClass do TextBox DDI
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass TextBox DDI")
        ]
        public string CssClassTextBoxDDI
        {
            get
            {
                return _txtDDI.CssClass;
            }
            set
            {
                _txtDDI.CssClass = value;
            }
        }
        #endregion

        #region TextBox - CssClass DDD
        /// <summary>
        /// CssClass do TextBox DDD
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass TextBox DDD")
        ]
        public string CssClassTextBoxDDD
        {
            get
            {
                return _txtDDD.CssClass;
            }
            set
            {
                _txtDDD.CssClass = value;
            }
        }
        #endregion

        #region TextBox - CssClass Fone
        /// <summary>
        /// CssClass do TextBox Fone
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass TextBox Fone")
        ]
        public string CssClassTextBoxFone
        {
            get
            {
                return _txtFone.CssClass;
            }
            set
            {
                _txtFone.CssClass = value;
            }
        }
        #endregion

        #region RequiredField - CssClass DDI
        /// <summary>
        /// CssClass do RequiredField DDI
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RequiredField DDI")
        ]
        public string CssClassRequiredFieldDDI
        {
            get
            {
                return _rfDDI.CssClass;
            }
            set
            {
                _rfDDI.CssClass = value;
            }
        }
        #endregion

        #region RequiredField - CssClass DDD
        /// <summary>
        /// CssClass do RequiredField DDD
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RequiredField DDD")
        ]
        public string CssClassRequiredFieldDDD
        {
            get
            {
                return _rfDDD.CssClass;
            }
            set
            {
                _rfDDD.CssClass = value;
            }
        }
        #endregion

        #region RequiredField - CssClass Fone
        /// <summary>
        /// CssClass do RequiredField Fone
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RequiredField Fone")
        ]
        public string CssClassRequiredFieldFone
        {
            get
            {
                return _rfFone.CssClass;
            }
            set
            {
                _rfFone.CssClass = value;
            }
        }
        #endregion

        #region RegularExpression - CssClass DDI
        /// <summary>
        /// CssClass do RegularExpression DDI
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RegularExpression DDI")
        ]
        public string CssClassRegularExpressionDDI
        {
            get
            {
                return _reDDI.CssClass;
            }
            set
            {
                _reDDI.CssClass = value;
            }
        }
        #endregion

        #region RegularExpression - CssClass DDD
        /// <summary>
        /// CssClass do RegularExpression DDD
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RegularExpression DDD")
        ]
        public string CssClassRegularExpressionDDD
        {
            get
            {
                return _reDDD.CssClass;
            }
            set
            {
                _reDDD.CssClass = value;
            }
        }
        #endregion

        #region RegularExpression - CssClass Fone
        /// <summary>
        /// CssClass do RegularExpression Fone
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("CssClass RegularExpression Fone")
        ]
        public string CssClassRegularExpressionFone
        {
            get
            {
                return _reFone.CssClass;
            }
            set
            {
                _reFone.CssClass = value;
            }
        }
        #endregion

        #region Formato
        /// <summary>
        /// Retorna ou seta o formato digitado no campo
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Formato")
        ]
        public FormatoTelefone Formato
        {
            get
            {
                return _formato;
            }
            set
            {
                _formato = value;

                switch (_formato)
                {
                    case FormatoTelefone.DDI:
                        _permiteDDI = true;
                        _permiteDDD = true;
                        break;
                    case FormatoTelefone.DDD:
                        _permiteDDI = false;
                        _permiteDDD = true;
                        break;
                    case FormatoTelefone.Numero:
                        _permiteDDI = false;
                        _permiteDDD = false;
                        break;
                    default:
                        _permiteDDI = false;
                        _permiteDDD = false;
                        break;
                }
            }
        }
        #endregion

        #region Tipo
        /// <summary>
        /// Retorna ou seta o tipo do telefone no campo
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("Tipo")
        ]
        public TipoTelefone Tipo
        {
            get
            {
                return _tipo;
            }
            set
            {
                _tipo = value;
            }
        }
        #endregion

        #region DDI
        /// <summary>
        /// Retorna ou seta o valor DDI
        /// </summary>
        [Browsable(false)]
        public string DDI
        {
            get
            {
                if (ValidarFormatoDDI(_txtDDI.Text))
                    return _txtDDI.Text;
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (ValidarFormatoDDI(value))
                    {
                        int i;
                        for (i = 0; i < value.Length; i++)
                        {
                            if (!value[i].Equals('0'))
                                break;
                        }
                        value = value.Substring(i);
                    }
                }
                _txtDDI.Text = value;
            }
        }
        #endregion

        #region DDD
        /// <summary>
        /// Retorna ou seta o valor DDD
        /// </summary>
        [Browsable(false)]
        public string DDD
        {
            get
            {
                if (ValidarFormatoDDD(_txtDDD.Text))
                    return _txtDDD.Text;
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (ValidarFormatoDDD(value))
                    {
                        int i;
                        for (i = 0; i < value.Length; i++)
                        {
                            if (!value[i].Equals('0'))
                                break;
                        }
                        value = value.Substring(i);
                    }
                }
                _txtDDD.Text = value;
            }
        }
        #endregion

        #region Fone
        /// <summary>
        /// Retorna ou seta o valor Fone
        /// </summary>
        [Browsable(false)]
        public string Fone
        {
            get
            {
                if (ValidarFormatoFone(_txtFone.Text))
                    return LimparMascaraFone(_txtFone.Text);
                return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (ValidarFormatoFone(value))
                    {
                        value = LimparMascaraFone(value);
                        if (value.Length.Equals(8))
                            value = string.Format("{0}-{1}", value.Substring(0, 4), value.Substring(4));
                        else if (value.Length.Equals(9))
                            value = string.Format("{0}-{1}", value.Substring(0, 5), value.Substring(5));
                        else
                            if (value.Length.Equals(11))
                            value = string.Format("{0}-{1}-{2}", value.Substring(0, 4), value.Substring(4, 3), value.Substring(7));
                    }
                }
                _txtFone.Text = value;
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region ValorDDDAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("ValorDDDAlteradoClient")
        ]
        public string ValorDDDAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region ValorDDIAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Telefone"),
            DisplayName("ValorDDIAlteradoClient")
        ]
        public string ValorDDIAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region PlaceholderFone
        [DisplayName("PlaceholderFone")]
        public string PlaceholderFone
        {
            set => _txtFone.Attributes["placeholder"] = value;
        }
        #endregion

        #region PlaceholderDDD
        [DisplayName("PlaceholderDDD")]
        public string PlaceholderDDD
        {
            set => _txtDDD.Attributes["placeholder"] = value;
        }
        #endregion

        #endregion

        #region Construtores
        /// <summary>
        /// Instânciar inicialmente suas variaveis
        /// </summary>
        public Telefone()
        {
            _txtDDI = new TextBox();
            _reDDI = new RegularExpressionValidator();
            _rfDDI = new RequiredFieldValidator();
            _txtDDD = new TextBox();
            _reDDD = new RegularExpressionValidator();
            _rfDDD = new RequiredFieldValidator();
            _txtFone = new TextBox();
            _reFone = new RegularExpressionValidator();
            _rfFone = new RequiredFieldValidator();
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
            List<string> nomeValidadores = new List<string>();

            ///DDI
            if (_permiteDDI)
            {
                args.NomeCampoValor = _txtDDI.ClientID;
                nomeValidadores.Add(_rfDDI.ClientID);
                nomeValidadores.Add(_reDDI.ClientID);
                args.Validadores = nomeValidadores.ToArray();

                _txtDDI.Attributes.Add("OnKeyPress", "return ApenasNumerosDD(this, event);");
                _txtDDI.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, getAreaCodeSize(this)); ");

                _txtDDI.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{1,3}');");

                _txtDDI.Attributes.Add("OnBlur", "AplicarMascaraDD(this);");
                _txtDDI.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

                _txtDDI.Attributes.Add("OnChange", "AplicarMascaraDD(this);");

                // Dispara client function ou faz postback normal
                if (!String.IsNullOrEmpty(ValorDDIAlteradoClient))
                    _txtDDD.Attributes["OnChange"] += "ClientFunction(this," + ValorDDIAlteradoClient + "," + new JSONReflector(args) + ");";


                if (ValorAlteradoDDI != null)
                    _txtDDI.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";

                _txtDDI.Attributes["OnBlur"] += String.Format("ConfigurarObrigatoriedade('{0}', {1});", this.ClientID, this.Obrigatorio ? "true" : "false");
            }

            ///DDD
            if (_permiteDDD)
            {
                args = new Args();
                args.NomeCampoValor = _txtDDD.ClientID;
                nomeValidadores = new List<string>();
                nomeValidadores.Add(_rfDDD.ClientID);
                nomeValidadores.Add(_reDDD.ClientID);
                args.Validadores = nomeValidadores.ToArray();

                _txtDDD.Attributes.Add("OnKeyPress", "return ApenasNumerosDD(this, event);");
                _txtDDD.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, getAreaCodeSize(this)); ");

                _txtDDD.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '\\\\d{1,3}');");

                _txtDDD.Attributes.Add("OnBlur", "AplicarMascaraDD(this);");

                if (this.Tipo == TipoTelefone.Celular || this.Tipo == TipoTelefone.FixoCelular)
                {
                    _txtDDD.Attributes["OnBlur"] += "ConfigurarValidador(this, '" + _tipo.GetHashCode() + "',\"" + _reFone.ClientID + "\");";
                }

                _txtDDD.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";
                _txtDDD.Attributes.Add("OnChange", "AplicarMascaraDD(this);");

                if (!String.IsNullOrEmpty(ValorDDDAlteradoClient))
                    _txtDDD.Attributes["OnChange"] += "ClientFunction(this," + ValorDDDAlteradoClient + "," + new JSONReflector(args) + ");";

                if (ValorAlteradoDDD != null)
                    _txtDDD.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";

                _txtDDD.Attributes["OnBlur"] += String.Format("ConfigurarObrigatoriedade('{0}', {1});", this.ClientID, this.Obrigatorio ? "true" : "false");
            }

            ///Fone
            args = new Args();
            args.NomeCampoValor = _txtFone.ClientID;
            nomeValidadores = new List<string>();
            nomeValidadores.Add(_rfFone.ClientID);
            nomeValidadores.Add(_reFone.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            _txtFone.Attributes.Add("OnKeyPress", "return ApenasNumerosFone(this, event, '" + _tipo.GetHashCode() + "', '" + _txtDDD.ClientID + "');");
            _txtFone.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, getPhoneSize(this, '" + _tipo.GetHashCode() + "', '" + _txtDDD.ClientID + "')); ");

            _txtFone.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '(\\\\d{4,5}-\\\\d{4})|(\\\\d{4}-\\\\d{3}-\\\\d{4})');");
            _txtFone.Attributes.Add("OnBlur", "AplicarMascaraFone(this);");
            _txtFone.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            _txtFone.Attributes.Add("OnChange", "AplicarMascaraFone(this);");
            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtFone.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
            if (ValorAlteradoFone != null)
                _txtFone.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";

            _txtFone.Attributes["OnBlur"] += String.Format("ConfigurarObrigatoriedade('{0}', {1});", this.ClientID, this.Obrigatorio ? "true" : "false");
        }
        #endregion

        #region InicializarTextBoxDDI
        /// <summary>
        /// Atribuir caracteristicas do campo TextBox DDI
        /// </summary>
        protected virtual void InicializarTextBoxDDI()
        {
            _txtDDI.ID = "txtDDI";
            _txtDDI.Columns = 3;
            _txtDDI.MaxLength = 3;
            _txtDDI.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarTextBoxDDD
        /// <summary>
        /// Atribuir caracteristicas do campo TextBox DDD
        /// </summary>
        protected virtual void InicializarTextBoxDDD()
        {
            _txtDDD.ID = "txtDDD";
            _txtDDD.Columns = 3;
            _txtDDD.MaxLength = 3;
            _txtDDD.AutoCompleteType = AutoCompleteType.Disabled;
            if (_tipo == TipoTelefone.Celular || _tipo == TipoTelefone.FixoCelular) //Tratamento para celular com 9 dígitos
            {
                _txtDDD.AutoPostBack = true;
                _txtDDD.TextChanged += _txtDDD_TextChanged;
            }
        }
        void _txtDDD_TextChanged(object sender, EventArgs e)
        {
            InicializarRegularExpressionFone();
            _txtFone.Focus();
        }
        #endregion

        #region InicializarTextBoxFone
        /// <summary>
        /// Atribuir caracteristicas do campo TextBox Fone
        /// ####-####
        /// 0800-##-####
        /// </summary>
        protected virtual void InicializarTextBoxFone()
        {
            _txtFone.ID = "txtFone";
            _txtFone.Columns = 12;
            _txtFone.MaxLength = 12;
            _txtFone.AutoCompleteType = AutoCompleteType.Disabled;
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Atribuir caracteristicas do campo TextBox
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            if (_permiteDDI)
                InicializarTextBoxDDI();
            if (_permiteDDD)
                InicializarTextBoxDDD();

            InicializarTextBoxFone();
        }
        #endregion

        #region InicializarRequiredFieldDDI
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField DDI
        /// </summary>
        protected virtual void InicializarRequiredFieldDDI()
        {
            _rfDDI.ID = "rfDDI";
            _rfDDI.ForeColor = Color.Empty;
            _rfDDI.ControlToValidate = _txtDDI.ID;
            _rfDDI.Display = ValidatorDisplay.Dynamic;
            _rfDDI.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRequiredFieldDDD
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField DDD
        /// </summary>
        protected virtual void InicializarRequiredFieldDDD()
        {
            _rfDDD.ID = "rfDDD";
            _rfDDD.ForeColor = Color.Empty;
            _rfDDD.ControlToValidate = _txtDDD.ID;
            _rfDDD.Display = ValidatorDisplay.Dynamic;
            _rfDDD.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRequiredFieldFone
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField Fone
        /// </summary>
        protected virtual void InicializarRequiredFieldFone()
        {
            _rfFone.ID = "rfFone";
            _rfFone.ForeColor = Color.Empty;
            _rfFone.ControlToValidate = _txtFone.ID;
            _rfFone.Display = ValidatorDisplay.Dynamic;
            _rfFone.ForeColor = Color.Empty;
            _rfFone.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField
        /// </summary>
        protected virtual void InicializarRequiredField()
        {
            if (_permiteDDI)
                InicializarRequiredFieldDDI();
            if (_permiteDDD)
                InicializarRequiredFieldDDD();

            InicializarRequiredFieldFone();
        }
        #endregion

        #region InicializarRegularExpressionDDI
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField DDI
        /// </summary>
        protected virtual void InicializarRegularExpressionDDI()
        {
            _reDDI.ID = "reDDI";
            _reDDI.ForeColor = Color.Empty;
            _reDDI.ControlToValidate = _txtDDI.ID;
            /// pattern 9 ou 99 ou 999
            _reDDI.ValidationExpression = @"^\d{1,3}$";
            _reDDI.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarRegularExpressionDDD
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField DDD
        /// </summary>
        protected virtual void InicializarRegularExpressionDDD()
        {
            _reDDD.ID = "reDDD";
            _reDDD.ForeColor = Color.Empty;
            _reDDD.ControlToValidate = _txtDDD.ID;
            /// patterns 999 ou 99 - impede 010 e 10
            _reDDD.ValidationExpression = "^[0][2-9][0]$|^[0][1-9][1-9]$|^[1-9][1-9]$|^[2-9][0]$";
            _reDDD.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarRegularExpressionFone
        /// <summary>
        /// Atribuir caracteristicas do campo RequiredField Fone
        /// </summary>
        protected virtual void InicializarRegularExpressionFone()
        {
            _reFone.ID = "reFone";
            _reFone.ForeColor = Color.Empty;
            _reFone.ControlToValidate = _txtFone.ID;

            _reFone.ValidationExpression = RecuperarExpressaoRegularFone();

            _reFone.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #region InicializarRegularExpression
        /// <summary>
        /// Atribuir caracteristicas do campo CustoValidator
        /// </summary>
        protected virtual void InicializarRegularExpression()
        {
            if (_permiteDDI)
                InicializarRegularExpressionDDI();
            if (_permiteDDD)
                InicializarRegularExpressionDDD();

            InicializarRegularExpressionFone();
        }
        #endregion

        #region LimparMascara
        /// <summary>
        /// Limpar pontuação do campo TextBox
        /// </summary>
        /// <returns>Valor do campo TextBox sem pontuação</returns>
        public static string LimparMascaraFone(string fone)
        {
            fone = fone.Trim();
            if (System.Text.RegularExpressions.Regex.IsMatch(fone, @"\d{4,5}-\d{4}") || System.Text.RegularExpressions.Regex.IsMatch(fone, @"\d{4}-\d{3}-\d{4}"))
            {
                string sPadrao = @"[.\\/-]";
                System.Text.RegularExpressions.Regex oRegEx = new System.Text.RegularExpressions.Regex(sPadrao);
                return oRegEx.Replace(fone, string.Empty);
            }
            return fone;
        }
        #endregion

        #region ValidarFormato DDI
        /// <summary>
        /// Validar o formato do valor desejado DDI
        /// </summary>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool ValidarFormatoDDI(string ddi)
        {
            ddi = ddi.Trim();
            Int16 vlr;
            if (!string.IsNullOrEmpty(ddi) && (ddi.Length <= 3) && Int16.TryParse(ddi, out vlr))
                return true;
            return false;
        }
        #endregion

        #region ValidarFormatoDDD
        /// <summary>
        /// Validar o formato do valor desejado DDD
        /// </summary>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public static bool ValidarFormatoDDD(string ddd)
        {
            ddd = ddd.Trim();
            Int16 vlr;
            if (!string.IsNullOrEmpty(ddd) && (ddd.Length <= 3) && Int16.TryParse(ddd, out vlr))
                return true;
            return false;
        }
        #endregion

        #region ValidarFormatoFone
        /// <summary>
        /// Validar o formato do valor desejado Fone
        /// </summary>
        /// <returns>'True' se valor é válido, caso contrário 'False'</returns>
        public bool ValidarFormatoFone(string fone)
        {
            System.Text.RegularExpressions.Regex objRegex = new System.Text.RegularExpressions.Regex(RecuperarExpressaoRegularFone());
            return objRegex.IsMatch(fone);
        }
        #endregion

        #region RecuperarExpressaoRegularFone
        private string RecuperarExpressaoRegularFone()
        {
            var listaDDDNoveDigitos = new List<string> { "11", "12", "13", "14", "15", "16", "17", "18", "19", "21", "22", "24", "27", "28", "31", "32", "33", "34", "35", "37", "38", "41", "42", "43", "44", "45", "46", "47", "48", "49", "51", "53", "54", "55", "61", "62", "63", "64", "65", "66", "67", "68", "69", "71", "73", "74", "75", "77", "79", "81", "82", "83", "84", "85", "86", "87", "88", "89", "91", "92", "93", "94", "95", "96", "97", "98", "99" };
            switch (_tipo)
            {
                case TipoTelefone.Outros:
                    return @"^[2-9]{1}\d{3}\-{0,1}\d{4}$|^[0][1-9]\d{2}\-\d{3}\-\d{4}$|^[0][1-9]\d{2}\d{7}$";
                case TipoTelefone.Fixo:
                    return @"^[2-5]{1}\d{3}\-{0,1}\d{4}$";
                case TipoTelefone.Celular:
                    {
                        if (listaDDDNoveDigitos.Contains(this.DDD.Trim()))
                            return @"^[9]\d{4}\-{0,1}\d{4}|[7]{1}\d{3}\-{0,1}\d{4}$";
                        else
                            return @"^[6-9]{1}\d{3}\-{0,1}\d{4}$";
                    }
                case TipoTelefone.FixoCelular:
                    {
                        if (listaDDDNoveDigitos.Contains(this.DDD.Trim()))
                            return @"^[2-5]{1}\d{3}\-{0,1}\d{4}|[9]\d{4}\-{0,1}\d{4}|[7]{1}\d{3}\-{0,1}\d{4}$";
                        else
                            return @"^[2-5]{1}\d{3}\-{0,1}\d{4}|[6-9]{1}\d{3}\-{0,1}\d{4}$";
                    }
                default:
                    return @"^[2-9]{1}\d{3}\-{0,1}\d{4}$|^[0][1-9]\d{2}\-\d{3}\-\d{4}$|^[0][1-9]\d{2}\d{7}$";
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region ValorAlteradoDDI
        /// <summary>
        /// Evento ao alterar valor DDI
        /// </summary>
        public event EventHandler ValorAlteradoDDI;
        #endregion

        #region ValorAlteradoDDD
        /// <summary>
        /// Evento ao alterar valor DDD
        /// </summary>
        public event EventHandler ValorAlteradoDDD;
        #endregion

        #region ValorAlteradoFone
        /// <summary>
        /// Evento ao alterar valor Fone
        /// </summary>
        public event EventHandler ValorAlteradoFone;
        #endregion

        #region OnInit
        /// <summary>
        /// Especificar ao iniciar os javascripts a serem utilizados
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Page.ClientScript.IsClientScriptBlockRegistered("Telefone.js"))
                Page.ClientScript.RegisterClientScriptInclude("Telefone.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.Telefone.js"));

            if (!Page.ClientScript.IsClientScriptBlockRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));
        }
        protected override void CreateChildControls()
        {
            // Métodos de Configuração das Propriedades dos Componentes
            InicializarTextBox();
            InicializarRequiredField();
            InicializarRegularExpression();

            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            //Adicionar Controles no WebForm
            if (_permiteDDI)
            {
                pnlValidador.Controls.Add(_rfDDI); //RequiredFieldValidator DDI
                pnlValidador.Controls.Add(_reDDI); //RegularExpression DDI
            }
            if (_permiteDDD)
            {
                pnlValidador.Controls.Add(_rfDDD); //RequiredFieldValidator DDD
                pnlValidador.Controls.Add(_reDDD); //RegularExpression DDD
            }
            pnlValidador.Controls.Add(_rfFone); //RequiredFieldValidator Fone
            pnlValidador.Controls.Add(_reFone); //RegularExpression Fone

            Controls.Add(pnlValidador);
            if (_permiteDDI)
                Controls.Add(_txtDDI); //Campo Texto DDI
            if (_permiteDDD)
                Controls.Add(_txtDDD); //Campo Texto DDD
            Controls.Add(_txtFone); //Campo Texto Fone

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

            // Atualiza o validador
            this.InicializarRegularExpressionFone();

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument))
            {
                if (eventArgument.Equals(_txtFone.ClientID))
                {
                    if (ValorAlteradoFone != null)
                        ValorAlteradoFone(_txtFone, null);
                }

                if (eventArgument.Equals(_txtDDD.ClientID))
                {
                    if (ValorAlteradoDDD != null)
                        ValorAlteradoDDD(_txtDDD, null);
                    // Dispara o validador de fone
                    _reFone.Validate();
                    _txtFone.Focus();
                }

                if (eventArgument.Equals(_txtDDI.ClientID))
                {
                    if (ValorAlteradoDDI != null)
                        ValorAlteradoDDI(_txtDDI, null);
                }
            }
        }
        #endregion

        #endregion
    }
}