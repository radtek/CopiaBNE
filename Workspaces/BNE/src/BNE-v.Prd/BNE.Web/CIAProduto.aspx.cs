using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web
{
    public partial class CIAProduto : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (base.IdFilial.HasValue && (new Filial(base.IdFilial.Value).EmpresaBloqueada()))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._505705);
                    Redirect("Default.aspx");
                }
                //Verificar se tem plano com boleto que a primeira parcela ainda não venceu e não foi paga
                //Enviar para a tela de reimprimir boleto
                if (base.IdFilial.HasValue && BLL.Pagamento.PagouPrimeiroBoleto(base.IdFilial.Value))
                {
                    Redirect(GetRouteUrl(BLL.Enumeradores.RouteCollection.ReimprimirBoleto.ToString(), null));
                }

            }

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CIAProduto");
        }
    }
}