using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ConfirmacaoCadastroEmpresa : BaseUserControl
    {

        #region Eventos

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();

            if (Fechar != null)
                Fechar();
        }
        #endregion

        #endregion

        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            mpeConfirmacaoCadastroEmpresa.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeConfirmacaoCadastroEmpresa.Hide();
        }
        #endregion

        #endregion

        #region Delegates

        public delegate void DelegateFechar();
        public event DelegateFechar Fechar;

        #endregion

    }
}