using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class FalePresidente : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "FalePresidente");
        }
    }
}
