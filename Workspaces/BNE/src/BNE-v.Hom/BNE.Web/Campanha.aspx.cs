using System;
using BNE.BLL.Custom;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class Campanha : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.Attributes["class"] += " nowayout";
        }

        protected void btnVagaRapidaCadastrarAgora_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["e"] != null)
            {
                Redirect($"http://{Helper.RecuperarURLAmbiente()}/cadastro-de-empresa-gratis/{Request.QueryString["e"]}");
            }
            Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.SouEmpresa.ToString(), null));
        }

        protected void btnVagaRapidaNaoQueroCadastrar_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.SouEmpresa.ToString(), null));
        }
    }
}