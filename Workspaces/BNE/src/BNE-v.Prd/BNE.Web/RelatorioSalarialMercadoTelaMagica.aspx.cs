using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BNE.Web.Code;
//using BNE.BLL;

namespace BNE.Web
{
    public partial class RelatorioSalarialMercadoTelaMagica : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnComprar_Click(object sender, ImageClickEventArgs e)        
        {
            Redirect(Page.GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
        }
    }
}