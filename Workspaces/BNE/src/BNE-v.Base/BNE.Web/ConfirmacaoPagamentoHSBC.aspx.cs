using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using System.Net;
using System.Text;
using System.Globalization;
using System.IO;

namespace BNE.Web
{
    public partial class ConfirmacaoPagamentoHSBC : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.Params["codigoPedidoLoja"]))
            {
                String erro = string.Empty;

                int idPlanoAdquirido = Convert.ToInt32(Request.Params["codigoPedidoLoja"]);
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(objPlanoAdquirido.UsuarioFilialPerfil.IdUsuarioFilialPerfil);
                Plano objPlano = Plano.LoadObject(objPlanoAdquirido.Plano.IdPlano);

                PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                BLL.Pagamento objPagamento = objListPagamentosPorParcela.Select(p => p).FirstOrDefault(p => (p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == (int)BNE.BLL.Enumeradores.TipoPagamento.CartaoCredito) && p.FlagInativo == false);

                

                
            }
        }
    }
}