using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Este componente é uma gambiarra criada para colocar um ; no final de sua declaração. Em alguns casos dava erro.
    /// </summary>
    public class EmployerValidationSummary : ValidationSummary
    {
        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), this.ClientID, ";", true);
        }
    }
}
