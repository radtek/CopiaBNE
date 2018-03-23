using System;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;

namespace BNE.Web
{
    public partial class VipTelaMagica : BasePage
    {
        #region Métodos

        #region ProcessoCompra
        private void ProcessoCompra()
        {
            Redirect(Page.GetRouteUrl(RouteCollection.EscolhaPlano.ToString(), null));
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Principal) Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, GetType().ToString());
        }
        #endregion

        #region btnContinuar_Click
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                ProcessoCompra();
            else
                ExibirLogin();
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            if (UrlDestino.Value == "~/SalaSelecionador.aspx")
            {
                Redirect("~/SalaSelecionador.aspx");
            }
            else
            {
                ProcessoCompra();
            }
        }
        #endregion

        #endregion
    }
}