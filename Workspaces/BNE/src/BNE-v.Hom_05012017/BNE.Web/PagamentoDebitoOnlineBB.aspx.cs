using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class PagamentoBB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            idConv.Value = Request.QueryString["idConv"];
            refTran.Value = Request.QueryString["refTran"];
            valor.Value = Request.QueryString["valor"];
            dtVenc.Value = Request.QueryString["dtVenc"];
            tpPagamento.Value = Request.QueryString["tpPagamento"];
            urlRetorno.Value = Request.QueryString["urlRetorno"];
            urlInforma.Value = Request.QueryString["urlInforma"];
        }
    }
}