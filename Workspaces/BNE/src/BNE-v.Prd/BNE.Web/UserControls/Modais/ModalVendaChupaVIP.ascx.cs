using System;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalVendaChupaVIP : BaseUserControl
    {

        #region Inicializar
        public void Inicializar(TipoChuvaVIP enumTipoChuvaVIP)
        {
            pnlSMS.Visible = pnlVisualizacao.Visible = false;

            if (enumTipoChuvaVIP.Equals(TipoChuvaVIP.SMS))
                pnlSMS.Visible = true;
            else
                pnlVisualizacao.Visible = true;

            upChupaVIP.Update();

            mpeEmpresaChupaVIP.Show();
        }
        #endregion

        #region btlQueroComprarPlano_Click
        protected void btlQueroComprarPlano_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null));
        }
        #endregion

    }

    public enum TipoChuvaVIP
    {
        Visualizacao,
        SMS
    }
}