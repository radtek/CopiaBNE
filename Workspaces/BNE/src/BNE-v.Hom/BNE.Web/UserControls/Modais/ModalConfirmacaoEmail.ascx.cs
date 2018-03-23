using System;
using System.Web.UI;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalConfirmacaoEmail : BaseUserControl
    {

        #region btlVagasNoPerfil_Click
        protected void btlVagasNoPerfil_Click(object sender, EventArgs e)
        {
            OnVisualizarVagasNoPerfil();
        }
        #endregion

        #region btiFechar_OnClick
        protected void btiFechar_OnClick(object sender, ImageClickEventArgs e)
        {
            OnVisualizarVagasNoPerfil();
        }
        #endregion btiFechar_OnClick

        #region Inicializar
        public void Inicializar()
        {
            mpeConfirmacaoEmail.Show();
        }
        #endregion
        
        #region Delegates
        public event EventHandler VisualizarVagasNoPerfil;
        protected virtual void OnVisualizarVagasNoPerfil()
        {
            EventHandler handler = VisualizarVagasNoPerfil;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion Delegates

    }
}