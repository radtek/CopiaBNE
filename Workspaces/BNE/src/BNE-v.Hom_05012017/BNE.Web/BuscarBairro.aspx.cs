using System;
using System.Web.UI;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class BuscarBairro : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string key = Parametro.RecuperaValorParametro(Enumeradores.Parametro.KeyAPIGoogleMaps, null);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "GoogleMaps", "<script src=\"http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=" + key + "\" type=\"text/javascript\"></script>" , false);
        }
    }
}
