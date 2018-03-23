using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web.Util;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Panel padrão
    /// </summary>
    public class Panel : System.Web.UI.WebControls.Panel
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
                return Convert.ToString(this.ViewState["Text"]);
            }
            set
            {
                this.ViewState["Text"] = value;
            }
        }
        #endregion
        #endregion

        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Panel()
        {
            if (String.IsNullOrEmpty(this.CssClass))
                this.CssClass = "EmployerPanel";
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

            //base.Render(writer);
        }
        #endregion
        #endregion
    }
}
