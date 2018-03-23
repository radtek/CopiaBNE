using System;
using Employer.Componentes.UI.Web.Util;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Botão padrão com a função ter uma troca de css automático para os eventos onMouseOver e onMouseOut.
    /// </summary>
//    [Obsolete(@"Não existe necessidade real para usar este componente.<br/>
//Aconselho realisar troca estilos em onMouseOver e onMouseOut somente usando CSS.")]
    public class Button : System.Web.UI.WebControls.Button
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
                if (this.ViewState["CssClassHover"] == null)
                    return String.Empty;

                return Convert.ToString(this.ViewState["CssClassHover"]);
            }

            set { this.ViewState["CssClassHover"] = value; }
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
                if (this.ViewState["AllowClientBehavior"] == null)
                    return true;
                return Convert.ToBoolean(this.ViewState["AllowClientBehavior"]);
            }
            set
            {
                this.ViewState["AllowClientBehavior"] = value;
            }
        }
        #endregion
        #endregion

        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Button()
        {
            if (String.IsNullOrEmpty(this.CssClass))
                this.CssClass = "EmployerButton";
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
