using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Drawing;
using BNE.Componentes.Interface;
using BNE.Componentes;

namespace BNE.Componentes.Base
{
    /// <summary>
    /// Classe base para outros webcontros Validator
    /// </summary>
    public abstract class ControlBaseValidator : AjaxClientControlBase, ITextControl, IRequiredField, IErrorMessage
    {

        #region Propriedades

        #region PainelValidadorCss
        /// <summary>
        /// A classe de css do paínel dos validadores
        /// </summary>
        [Category("Validador")]
        public string PainelValidadorCss
        {
            get { return PanelValidador.CssClass; }
            set { PanelValidador.CssClass = value; }
        }
        #endregion

        #region ValidationGroup
        /// <summary>
        /// Grupo de validação do RequiredField Validator
        /// </summary>
        [Category("Validador"), DisplayName("Validation Group")]
        public virtual string ValidationGroup
        {
            get
            {
                EnsureChildControls();
                return ValidadorTexto.ValidationGroup;
            }
            set
            {
                EnsureChildControls();
                ValidadorTexto.ValidationGroup = value;
                CampoTexto.ValidationGroup = value;
            }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é obrigatório.
        /// </summary>
        [Category("Validador"), DisplayName("Obrigatório")]
        public virtual bool Obrigatorio
        {
            get
            {
                if (ViewState["Obrigatorio"] == null)
                    return false;
                return (bool)ViewState["Obrigatorio"];
            }
            set { ViewState["Obrigatorio"] = value; }
        }
        #endregion

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [Category("Validador"), DisplayName("SetFocusOnError")]
        public bool SetFocusOnError
        {
            get { return ValidadorTexto.SetFocusOnError; }
            set { ValidadorTexto.SetFocusOnError = value; }
        }
        #endregion

        #region Mensagens

        #region MensagemErroFormato
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Formato"), Localizable(true)]
        public string MensagemErroFormato
        {
            get
            {
                EnsureChildControls();
                if (ValidadorTexto.Text == null)
                    return Resources.ControlBaseValidatorMensagemErroFormato;
                return ValidadorTexto.Text;
            }
            set
            {
                EnsureChildControls();
                ValidadorTexto.Text = value;
            }
        }
        #endregion

        #region InicializarPainel
        /// <summary>
        /// Inicia o painel
        /// </summary>
        protected void InicializarPainel()
        {
            PanelValidador.ID = "pnlValidador";
            PanelValidador.Controls.Add(ValidadorTexto); //CustomValidator
            PanelValidador.EnableTheming = false;
            Controls.Add(PanelValidador);
        }
        #endregion

        #region MensagemErroObrigatorio
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Obrigatorio"), Localizable(true)]
        public string MensagemErroObrigatorio
        {
            get
            {
                if (ViewState["MensagemErroObrigatorio"] == null)
                    return Resources.ControlBaseValidatorMensagemErroObrigatorio;
                return (string)ViewState["MensagemErroObrigatorio"];
            }
            set { ViewState["MensagemErroObrigatorio"] = value; }
        }
        #endregion

        #region MensagemErroValorMinimo
        /// <summary>
        /// Mensagem de Erro Obrigatorio Valor Minimo
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Valor Minimo"), Localizable(true)]
        public string MensagemErroValorMinimo
        {
            get
            {
                if (ViewState["MensagemErroValorMinimo"] == null)
                    return Resources.ControlBaseValidatorMensagemErroValorMinimo;
                return ViewState["MensagemErroValorMinimo"].ToString();
            }
            set { ViewState["MensagemErroValorMinimo"] = value; }
        }
        #endregion

        #region MensagemErroValorMaximo
        /// <summary>
        /// Mensagem de Erro Obrigatorio Valor Máximo
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Valor Máximo"), Localizable(true)]
        public string MensagemErroValorMaximo
        {
            get
            {
                if (ViewState["MensagemErroValorMaximo"] == null)
                    return Resources.ControlBaseValidatorMensagemErroValorMaximo;
                return (string)ViewState["MensagemErroValorMaximo"];
            }
            set
            {
                ViewState["MensagemErroValorMaximo"] = value;
            }
        }
        #endregion


        #region MensagemErroFormatoSummary
        public String MensagemErroFormatoSummary
        {
            get
            {
                EnsureChildControls();
                return this.ValidadorTexto.ErrorMessage;
            }
            set
            {
                EnsureChildControls();
                this.ValidadorTexto.ErrorMessage = value;
            }
        }
        #endregion

        #region MensagemErroInvalidoSummary
        /// <summary>
        /// Sem funcionalidade. Obrigado a implementar por causa da Interface.
        /// </summary>
        public String MensagemErroInvalidoSummary
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }
        #endregion

        #region MensagemErroObrigatorioSummary
        public String MensagemErroObrigatorioSummary
        {
            get
            {
                return Convert.ToString(this.ViewState["MensagemErroObrigatorioSummary"]);
            }
            set
            {
                this.ViewState["MensagemErroObrigatorioSummary"] = value;
            }
        }
        #endregion

        #region MensagemErroValorMinimoSummary
        public String MensagemErroValorMinimoSummary
        {
            get
            {
                return Convert.ToString(this.ViewState["MensagemErroValorMinimoSummary"]);
            }
            set
            {
                this.ViewState["MensagemErroValorMinimoSummary"] = value;
            }
        }
        #endregion

        #region MensagemErroValorMaximoSummary
        public String MensagemErroValorMaximoSummary
        {
            get
            {
                return Convert.ToString(this.ViewState["MensagemErroValorMaximoSummary"]);
            }
            set
            {
                this.ViewState["MensagemErroValorMaximoSummary"] = value;
            }
        }
        #endregion

        #endregion

        #region  CssClass
        #region CssClassTextBox
        /// <summary>
        /// CssClass do TextBox
        /// </summary>
        [Category("TextBox"),
         DisplayName("CssClass TextBox")]
        public string CssClassTextBox
        {
            get
            {
                EnsureChildControls();
                return CampoTexto.CssClass;
            }
            set
            {
                EnsureChildControls();
                CampoTexto.CssClass = value;
            }
        }
        #endregion

        #region CssClassRegularExpression
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [Category("Validador"),
         DisplayName("CssClass RegularExpression")]
        public string CssClassRegularExpression
        {
            get { return ValidadorTexto.CssClass; }
            set { ValidadorTexto.CssClass = value; }
        }
        #endregion

        #endregion

        #region Text
        /// <summary>
        /// O texto atualmente no controle
        /// </summary>
        public virtual string Text
        {
            get
            {
                EnsureChildControls();
                return CampoTexto.Text;
            }
            set
            {
                EnsureChildControls();
                CampoTexto.Text = value;
            }
        }
        #endregion

        #region Abstratos

        /// <summary>
        /// O campo de texto do controle
        /// </summary>
        protected abstract TextBox CampoTexto { get; }
        /// <summary>
        /// O validador do campo texto
        /// </summary>
        protected abstract BaseValidator ValidadorTexto { get; }
        /// <summary>
        /// O painel dos validadores
        /// </summary>
        protected abstract System.Web.UI.WebControls.Panel PanelValidador { get; }
        #endregion

        #endregion

        #region Métodos

        #region Focus
        /// <inheritdoc />
        public override void Focus()
        {
            EnsureChildControls();

            if (!ControlAutoTabIndex.Focus(this.CampoTexto.ClientID))
                Sm.SetFocus(this.CampoTexto.ClientID);
        }
        #endregion

        #region InicializarValidator
        /// <summary>
        /// Método que seta as configurações da propriedades do Custom Validator do campo Data.
        /// </summary>
        protected void InicializarValidator()
        {
            ValidadorTexto.ID = "cvValor";
            ValidadorTexto.ForeColor = Color.Empty;
            ValidadorTexto.ControlToValidate = CampoTexto.ID;
            ValidadorTexto.Display = ValidatorDisplay.Dynamic;

            ValidadorTexto.EnableTheming = false;
            ValidadorTexto.EnableClientScript = true;

        }
        #endregion

        #region SetScriptDescriptors
        /// <summary>
        /// Registra as propriedades para o objeto em javascript
        /// </summary>
        /// <param name="descriptor"></param>
        protected override void SetScriptDescriptors(ScriptControlDescriptor descriptor)
        {
            base.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("Obrigatorio", this.Obrigatorio);
            descriptor.AddProperty("MensagemErroObrigatorio", this.MensagemErroObrigatorio);
            descriptor.AddProperty("MensagemErroFormato", this.MensagemErroFormato);
            descriptor.AddProperty("MensagemErroValorMinimo", this.MensagemErroValorMinimo);
            descriptor.AddProperty("MensagemErroValorMaximo", this.MensagemErroValorMaximo);

            //Summary
            descriptor.AddProperty("MensagemErroFormatoSummary", this.MensagemErroFormatoSummary);
            descriptor.AddProperty("MensagemErroObrigatorioSummary", this.MensagemErroObrigatorioSummary);
            descriptor.AddProperty("MensagemErroValorMinimoSummary", this.MensagemErroValorMinimoSummary);
            descriptor.AddProperty("MensagemErroValorMaximoSummary", this.MensagemErroValorMaximoSummary);
            descriptor.AddProperty("MensagemErroValorMaximoSummary", this.MensagemErroValorMaximoSummary);
        }
        #endregion

        #region SetScriptReferences
        /// <inheritdoc />        
        protected override void SetScriptReferences(IList<ScriptReference> references)
        {
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "BNE.Componentes";
            reference.Name = "BNE.Componentes.Content.js.ControlBaseValidator.js";
            references.Add(reference);
        }
        #endregion

        #endregion
    }
}
