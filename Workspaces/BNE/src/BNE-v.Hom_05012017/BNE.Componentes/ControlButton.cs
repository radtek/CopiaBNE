using System;
using BNE.Componentes.Util;

namespace BNE.Componentes
{
    /// <summary>
    /// Botão padrão
    /// </summary>
    public class ControlButton : System.Web.UI.WebControls.Button
    {
        #region Properties

        #region CssClassFocus
        /// <summary>
        /// Css class quando o mouse estiver em cima do botão
        /// </summary>
        public String CssClassOver
        {
            get
            {
                if (this.ViewState[Keys.Stylesheet.CssClassHover.ToString()] == null)
                    return String.Empty;

                return Convert.ToString(this.ViewState[Keys.Stylesheet.CssClassHover.ToString()]);
            }

            set { this.ViewState[Keys.Stylesheet.CssClassHover.ToString()] = value; }
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

        #endregion

        #region Construtor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public ControlButton()
        {
            if (String.IsNullOrEmpty(this.CssClass))
                this.CssClass = Keys.Css.EmployerButton.ToString();
            this.CausesValidation = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Renderização dos controles na tela
        /// </summary>
        /// <param name="writer">Stream onde o controle será renderizado</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.AllowClientBehavior && !String.IsNullOrEmpty(this.CssClassOver))
            {
                this.Attributes["onMouseOver"] = "document.getElementById('" + this.ClientID + "').className= '" + this.CssClassOver + "'";
                this.Attributes["onMouseOut"] = "document.getElementById('" + this.ClientID + "').className= '" + this.CssClass + "'";
            }

            base.Render(writer);
        }
        #endregion

    }
}
