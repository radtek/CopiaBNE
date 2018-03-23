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


                if (string.IsNullOrEmpty(imgBoleto.ImageUrl))
                {
                    // pegar o ultimo pagamento não pago
                    Pagamento objPagamento = Pagamento.LoadObject(IdPagamento);

                    var objPagarme = new BLL.DTO.DTOBoletoPagarMe();
                    var dia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    if (objPagamento.DataVencimento.Value < dia)
                    {//Gera novo boleto com juros.
                        objPagamento.FlagJuros = true;
                        objPagarme = PagarMeOperacoes.GerarBoleto(objPagamento, null, true, DateTime.Now.AddDays(1));
                    }
                    else
                    {
                        objPagarme = PagarMeOperacoes.GerarBoleto(objPagamento);
                    }


                    List<BLL.DTO.DTOBoletoPagarMe> boletos = new List<BLL.DTO.DTOBoletoPagarMe>();
                    boletos.Add(objPagarme);

                    var htmlBoleto = BoletoBancario.GerarLayoutBoletoHTMLPagarMe(boletos);
                    byte[] img, boletobyte;
                    BoletoBancario.RetornarBoleto(htmlBoleto, out boletobyte, out img);
                    imgBoleto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(img);
                }
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Imprimir", "javascript:setTimeout('window.print()', 900);", true);
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.print();", true);
            }

        }
    }
}