using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// WebControl com Validador e TextBox com javascript de formatação e validação embutido.
    /// </summary>
    #pragma warning disable 1591
    public class ValorNumericoTextBox : ControlBaseTextBox, IScriptControl
    {
        public event ServerValidateEventHandler ValidacaoServidor;

        #region Atributos
        private CustomValidator _cvValor = new CustomValidator();
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        #endregion        

        #region Propriedades

        #region ValidadorTexto
        /// <summary>
        /// Sobrecar da propriedade para uso na clase ControlBaseTextBox
        /// </summary>
        protected override BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }
        #endregion

        #region Validador

        #region PanelValidador
        /// <summary>
        /// O panel dos validadores
        /// </summary>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion 

        #region CasasDecimais
        /// <summary>
        /// Define o número de dígitos após a virgula.
        /// </summary>
        [Category("Validador"), DisplayName("Número de Dígitos")]
        public int CasasDecimais
        {
            get
            {
                if (ViewState["CasasDecimais"] == null)
                    return 2;
                return (int)ViewState["CasasDecimais"];
            }
            set { ViewState["CasasDecimais"] = value; }
        }
        #endregion 

        #region ValorMinimo
        /// <summary>
        /// Define o valor mínimo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Valor Mínimo")]
        public decimal? ValorMinimo
        {
            get
            {
                if (ViewState["ValorMinimo"] == null)
                    return null;
                return (decimal?)ViewState["ValorMinimo"];
            }
            set { ViewState["ValorMinimo"] = value; }
        }
        #endregion 

        #region CampoValorMinimo
        /// <summary>
        /// Define um campo de valor mínimo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Campo Valor Mínimo")]
        public string CampoValorMinimo
        {
            get
            {
                if (ViewState["CampoValorMinimo"] == null)
                    return null;
                return (string)ViewState["CampoValorMinimo"];
            }
            set { ViewState["CampoValorMinimo"] = value; }
        }
        #endregion 

        #region ValorMaximo
        /// <summary>
        /// Define o valor máximo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Valor Máximo")]
        public decimal? ValorMaximo
        {
            get
            {
                if (ViewState["ValorMaximo"] == null)
                    return null;
                return (decimal?)ViewState["ValorMaximo"];
            }
            set { ViewState["ValorMaximo"] = value; }
        }
        #endregion 

        #region CampoValorMaximo
        /// <summary>
        /// Define um campo de valor Máximo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Campo Valor Máximo")]
        public string CampoValorMaximo
        {
            get
            {
                if (ViewState["CampoValorMaximo"] == null)
                    return null;
                return (string)ViewState["CampoValorMaximo"];
            }
            set { ViewState["CampoValorMaximo"] = value; }
        }
        #endregion 

        #region CssValidador
        /// <summary>
        /// Define um campo de valor Máximo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Css class validador")]
        public string CssValidador
        {
            get
            {
                return _cvValor.CssClass;
            }
            set { _cvValor.CssClass = value; }
        }
        #endregion 

        #endregion

        #region Data
        #region ValorDecimal
        /// <summary>
        /// Propriedade para setar ou receber valor do Campo Valor Decimal.
        /// </summary>
        [Category("Data"), Browsable(true), Bindable(true)]
        public decimal? ValorDecimal
        {
            get
            {
                NumberStyles style = NumberStyles.Currency;
                CultureInfo culture = CultureInfo.CurrentCulture;
                decimal vlr;
                if (decimal.TryParse(CampoTexto.Text, style, culture, out vlr))
                    return vlr;
                return null;
            }
            set
            {
                if (value.HasValue)
                    CampoTexto.Text = value.Value.ToString("N" + this.CasasDecimais.ToString());
                else
                    CampoTexto.Text = string.Empty;
            }
        }
        #endregion 

        #region ValorInteiro
        /// <summary>
        /// Propriedade para setar ou receber valor do Campo Valor Decimal.
        /// </summary>
        [Category("Data"), Browsable(true), Bindable(true)]
        public int? ValorInteiro
        {
            get
            {
                NumberStyles style = NumberStyles.Currency;
                CultureInfo culture = CultureInfo.CurrentCulture;
                int vlr;
                if (int.TryParse(CampoTexto.Text, style, culture, out vlr))
                    return vlr;
                return null;
            }
            set
            {
                if (value.HasValue)
                    CampoTexto.Text = value.Value.ToString();
                else
                    CampoTexto.Text = string.Empty;
            }
        }
        #endregion 

        #endregion

        #endregion

        #region Inicializar Controles

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();

            _cvValor.ValidateEmptyText = true;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ValorNumerico.ValidarTextBox";
        }
        #endregion 

        #endregion

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarPainel();
            //CampoTexto.Enabled = this.Enabled;
            Controls.Add(CampoTexto);
            CampoTexto.Columns = 15;

            base.CreateChildControls();
        }
        #endregion 

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
            base.OnInit(e);
        }
        #endregion 

        #region _cvValor_ServerValidate
        /// <summary>
        /// Evento disparado no server validate do custom validator
        /// </summary>
        /// <param name="source">O objeto que disparou o evento</param>
        /// <param name="args">Os argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio)
            {
                args.IsValid = this.ValorDecimal.HasValue &&
                (
                   (this.ValorMinimo.HasValue == false || this.ValorDecimal.Value >= this.ValorMinimo.Value) &&
                   (this.ValorMaximo.HasValue == false || this.ValorDecimal.Value <= this.ValorMaximo.Value)
                );
            }
            if (ValidacaoServidor != null)
                ValidacaoServidor(source, args);
        }
        #endregion 

        #endregion

        #region IScriptControl Members
        #region GetScriptDescriptors
        /// <summary>
        /// Registra as propriedades para o objeto em javascript
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ValorNumerico", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("CasasDecimais", this.CasasDecimais);
            descriptor.AddProperty("ValorMinimo", this.ValorMinimo);
            descriptor.AddProperty("ValorMaximo", this.ValorMaximo);
            descriptor.AddProperty("Tamanho", this.Tamanho);

            if (string.IsNullOrEmpty(CampoValorMinimo) == false)
            {
                Control objCtrl = this.Parent.FindControl(CampoValorMinimo);
                if (objCtrl is ValorNumericoTextBox)
                    descriptor.AddProperty("CampoValorMinimo", ((ValorNumericoTextBox)objCtrl).CampoTexto.ClientID);
                else
                    descriptor.AddProperty("CampoValorMinimo", this.Parent.FindControl(CampoValorMinimo).ClientID);
            }

            if (string.IsNullOrEmpty(CampoValorMaximo) == false)
            {
                Control objCtrl = this.Parent.FindControl(CampoValorMaximo);
                if (objCtrl is ValorNumericoTextBox)
                    descriptor.AddProperty("CampoValorMaximo", ((ValorNumericoTextBox)objCtrl).CampoTexto.ClientID);
                else
                    descriptor.AddProperty("CampoValorMaximo", this.Parent.FindControl(CampoValorMaximo).ClientID);
            }

            //Conficuração de internacionalização
            descriptor.AddProperty("NumberDecimalSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            descriptor.AddProperty("NumberGroupSeparator", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
            descriptor.AddProperty("CurrencySymbol", CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
            descriptor.AddProperty("NegativeSign", CultureInfo.CurrentCulture.NumberFormat.NegativeSign);
            descriptor.AddProperty("NumberGroupSizes", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSizes);
            

            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion 

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referências das bibiotecas em javascript
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ValorNumerico.js";
            references.Add(reference);


            return references;
        }
        #endregion 

        #endregion
    }
}
