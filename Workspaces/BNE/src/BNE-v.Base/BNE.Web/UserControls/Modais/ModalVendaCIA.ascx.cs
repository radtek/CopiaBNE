using System;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalVendaCIA : BaseUserControl
    {

        #region btlQueroComprarPlano_Click
        protected void btlQueroComprarPlano_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
        }
        #endregion

        #region Inicializar
        public void Inicializar()
        {
            mpeVendaCIA.Show();
        }
        #endregion

    }
}