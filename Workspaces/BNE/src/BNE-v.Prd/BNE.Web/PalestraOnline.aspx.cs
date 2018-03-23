using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class PalestraOnline : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarBarraBusca(TipoBuscaMaster.Vaga,false, GetType().ToString());
        }
    }
}