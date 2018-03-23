using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class PaginaErro : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                base.InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "PaginaErro");
        }
    }
}