using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web.Util;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// TextBox padrão
    /// </summary>
    public class EmployerTextBox : System.Web.UI.WebControls.TextBox, IRequiredField
    {
        #region Private
        private RequiredFieldValidator valRequired = new RequiredFieldValidator(); // Validador de campo requerido
        #endregion

        #region Properties
        #region Required
        /// <summary>
        /// Define se o controle é ou não de preenchimento obrigatório
        /// </summary>
        public bool Obrigatorio
        {
            get
            {
                if (this.ViewState[Keys.General.Required.ToString()] == null)
                    return false;
                return Convert.ToBoolean(this.ViewState[Keys.General.Required.ToString()]);
            }
            set
            {
                this.ViewState[Keys.General.Required.ToString()] = value;
            }
        }
        #endregion

        #region ValidatorCssClass
        /// <summary>
        /// Css class do validado
        /// </summary>
        public String ValidatorCssClass
        {
            get { return this.valRequired.CssClass; }
            set { this.valRequired.CssClass = value; }
        }
        #endregion
        
        #region AllowClientBehavior
        /// <summary>
        /// Define se o controle ativará o comportamento Javascript
        /// </summary>
        public Boolean AllowClientBehavior
        {
            get
            {
                if (this.ViewState[Keys.General.AllowClientBehavior.ToString()] == null)
                    return true;
                return Convert.ToBoolean(this.ViewState[Keys.General.AllowClientBehavior.ToString()]);
            }
            set
            {
                this.ViewState[Keys.General.AllowClientBehavior.ToString()] = value;
            }
        }
        #endregion

        #region CssClassFocus
        /// <summary>
        /// Css class quando a textbox estiver focada
        /// </summary>
        public String CssClassFocus
        {
            get
            {
                if (this.ViewState[Keys.Stylesheet.CssClassFocus.ToString()] == null)
                    return Keys.Css.EmployerTextBoxFocused.ToString();

                return Convert.ToString(this.ViewState[Keys.Stylesheet.CssClassFocus.ToString()]);
            }

            set { this.ViewState[Keys.Stylesheet.CssClassFocus.ToString()] = value; }
        }
        #endregion
        
        #region CssClassError
        /// <summary>
        /// Css class quando a textbox estiver inválida
        /// </summary>
        public String CssClassError
        {
            get
            {
                if (this.ViewState[Keys.Stylesheet.CssClassError.ToString()] == null)
                    return Keys.Css.EmployerTextBoxError.ToString();

                return Convert.ToString(this.ViewState[Keys.Stylesheet.CssClassError.ToString()]);
            }

            set { this.ViewState[Keys.Stylesheet.CssClassError.ToString()] = value; }
        }
        #endregion

        #endregion

        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public EmployerTextBox()
        {
            if (String.IsNullOrEmpty(this.CssClass))
                this.CssClass = Keys.Css.EmployerTextBox.ToString();

            if (String.IsNullOrEmpty(this.ValidatorCssClass))
                this.ValidatorCssClass = Keys.Css.EmployerValidator.ToString();

            this.CausesValidation = true;            
        }
        #endregion

        #region Methods
        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            this.EnsureValidator();
            base.CreateChildControls();
        }
        #endregion
        
        #region EnsureValidator
        /// <summary>
        /// Cria os validadores
        /// </summary>
        protected void EnsureValidator()
        {
            this.valRequired.ID = this.ClientID + "_valReq";
            this.valRequired.ErrorMessage = Resources.RequiredErrorMessage;
            this.valRequired.Display = ValidatorDisplay.Dynamic;
            this.valRequired.Enabled = this.Obrigatorio;
            this.valRequired.EnableClientScript = true;
            this.valRequired.ControlToValidate = this.ClientID;
            this.Controls.Add(this.valRequired);
        }
        #endregion

        #region Render
        /// <summary>
        /// Renderização dos controles na tela
        /// </summary>
        /// <param name="writer">Stream onde o controle será renderizado</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.AllowClientBehavior)
            {
                String js = String.Empty;
                if (this.Obrigatorio)
                {
                    js = "if (document.getElementById('" + this.ClientID + "').value == '')";
                    js += "document.getElementById('" + this.ClientID + "').className= '" + this.CssClassError + "';";
                    js += "else document.getElementById('" + this.ClientID + "').className= '" + this.CssClassFocus + "';";
                    //this.Attributes["onfocus"] = js;
                    js = "if (document.getElementById('" + this.ClientID + "').value == '')";
                    js += "document.getElementById('" + this.ClientID + "').className= '" + this.CssClassError + "';";
                    js += "else document.getElementById('" + this.ClientID + "').className= '" + this.CssClass + "';";
                    this.Attributes["onblur"] = js;
                    js = "if (document.getElementById('" + this.ClientID + "').value == '')";
                    js += "document.getElementById('" + this.ClientID + "').className= '" + this.CssClassError + "';";
                    js += "else document.getElementById('" + this.ClientID + "').className= '" + this.CssClassFocus + "';";
                    this.Attributes["onkeyup"] = js;
                }
                else
                {                    
                    this.Attributes["onblur"] = "document.getElementById('" + this.ClientID + "').className= '" + this.CssClass + "'";
                }
                js = "document.getElementById('" + this.ClientID + "').className= '" + this.CssClassFocus + "'";
                this.Attributes["onfocus"] = js;
                
            }

            if (!String.IsNullOrEmpty(this.ValidationGroup))
                this.valRequired.ValidationGroup = this.ValidationGroup;

            // Verifica se o validador será habilitado
            if (this.Enabled)
                this.valRequired.Enabled = this.Obrigatorio;
            else
                this.valRequired.Enabled = false;

            // Deixa o controle envolto em uma span
            writer.RenderBeginTag(HtmlTextWriterTag.Span);            
            this.valRequired.RenderControl(writer);
            base.Render(writer);            
            writer.RenderEndTag();

        }
        #endregion
        #endregion       
    }
}
