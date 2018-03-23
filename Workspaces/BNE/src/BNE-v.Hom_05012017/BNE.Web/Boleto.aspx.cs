using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using BNE.BLL;

namespace BNE.Web
{
    public partial class Boleto : System.Web.UI.Page
    {
        #region IdPagamento
        private int IdPagamento
        {
            get
            {
                int _tempIdPagamento = 0;
                if (Page.RouteData.Values.Count() > 0)
                {
                    if (Page.RouteData.Values["Id"] != null)
                    {
                        if (int.TryParse(Page.RouteData.Values["Id"].ToString(), out _tempIdPagamento))
                        {
                            return _tempIdPagamento;
                        }
                    }
                }
                if (Request.QueryString["Id"] != null)
                {
                    if (int.TryParse(Request.QueryString["Id"].ToString(), out _tempIdPagamento))
                    {
                        return _tempIdPagamento;
                    }
                }
                return _tempIdPagamento;
            }
        }
        #endregion IdPagamento

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (IdPagamento <= 0) return;

                BoletoNet.Boleto boleto = BoletoBancario.ProcessarBoletoNovo(Pagamento.LoadObject(IdPagamento),null);
                boletoBancario.Boleto = boleto;
            }

        }
    }
}