using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class teste : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(Request.Params["metodo"]))
            {
                Response.Write("Requisição recebida");
                Response.End();
            }

            if (Request.Params["metodo"] == "inclusaoPedido")
            {
                Response.Write((new Random()).Next().ToString());
                Response.End();
            }

            if (Request.Params["metodo"] == "inclusaoPedido")
            {
                Response.Write((new Random()).Next().ToString());
                Response.End();
            }
        }
    }
}