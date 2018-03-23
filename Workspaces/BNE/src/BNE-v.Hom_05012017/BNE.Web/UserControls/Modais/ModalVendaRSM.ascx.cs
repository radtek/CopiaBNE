using System;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalVendaRSM : BaseUserControl
    {

        #region btlQueroComprarPlano_Click
        protected void btlQueroComprarPlano_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
        }
        #endregion

        #region btlQueroComprarPlano_Click
        protected void btlNaoAgora_Click(object sender, EventArgs e)
        {
            mpeVendaRSM.Hide();
        }
        #endregion

        #region Inicializar
        public void Inicializar()
        {
            mpeVendaRSM.Show();
        }
        #endregion

        
    }
}