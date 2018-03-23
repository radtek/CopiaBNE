using System;
using System.Configuration;

namespace BNE.Web
{
    public partial class Versao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblVersao.Text = ConfigurationManager.AppSettings["versao"];
            if (Server.GetLastError() != null)
                lblErro.Text = Server.GetLastError().StackTrace;
        }
    }
}
