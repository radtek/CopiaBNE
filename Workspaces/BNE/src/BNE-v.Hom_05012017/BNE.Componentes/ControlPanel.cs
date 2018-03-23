using System;
using System.Web.UI;
using BNE.Componentes.Util;

namespace BNE.Componentes
{
    /// <summary>
    /// Panel padrão
    /// </summary>
    public class ControlPanel : System.Web.UI.WebControls.Panel
    {

        #region Properties

        #region Text
        /// <summary>
        /// Título do panel
        /// </summary>
        public String Text
        {
            get
            {
                return Convert.ToString(this.ViewState[Keys.General.Text.ToString()]);
            }
            set
            {
                this.ViewState[Keys.General.Text.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Construtor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public ControlPanel()
        {
            if (String.IsNullOrEmpty(this.CssClass))
                this.CssClass = Keys.Css.EmployerPanel.ToString();
        }
        #endregion

        #region Methods

        #region Render
        /// <summary>
        /// Renderização dos controles na tela
        /// </summary>
        /// <param name="writer">Stream onde o controle será renderizado</param>
        protected override void Render(HtmlTextWriter writer)
        {            
            this.RenderBeginTag(writer);
            if (!String.IsNullOrEmpty(this.Text))
            {
                writer.WriteBeginTag("div");
                writer.Write(" class=\"PanelTitle\" >");
                writer.Write(this.Text);
                writer.WriteEndTag("div");
            }
            this.RenderChildren(writer);
            this.RenderEndTag(writer);
        }
        #endregion

        #endregion
    }
}
