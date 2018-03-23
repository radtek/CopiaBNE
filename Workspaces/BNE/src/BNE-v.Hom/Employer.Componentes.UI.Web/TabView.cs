using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Conteúdo da aba do TabNavigator
    /// </summary>
    public class TabView : View
    {

        #region Propriedades

        #region Titulo
        /// <summary>
        /// O título da aba
        /// </summary>
        public String Titulo
        {
            get { return Convert.ToString(this.ViewState["Titulo"]); }
            set { this.ViewState["Titulo"] = value; }
        }
        #endregion

        #region Enabled
        /// <summary>
        /// Define se a aba está ou não habilitada
        /// </summary>
        public Boolean Enabled
        {
            get
            {

                if (this.ViewState["Enabled"] == null)
                    this.ViewState["Enabled"] = true;

                return Convert.ToBoolean(this.ViewState["Enabled"]);
            }
            set { this.ViewState["Enabled"] = value; }
        }
        #endregion

        #region ShowButton
        /// <summary>
        /// Define se o botão da aba está ou não visível
        /// </summary>
        public  Boolean ShowButton
        {
            get
            {

                if (this.ViewState["ShowButton"] == null)
                    this.ViewState["ShowButton"] = true;

                return Convert.ToBoolean(this.ViewState["ShowButton"]);
            }
            set {
                this.ViewState["ShowButton"] = value;
            }
        }
        #endregion 

        #endregion 

    }
}
