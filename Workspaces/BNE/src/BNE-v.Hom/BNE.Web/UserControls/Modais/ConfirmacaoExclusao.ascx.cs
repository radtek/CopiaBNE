using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ConfirmacaoExclusao : BaseUserControl
    {

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region delegate

        public delegate void delegateConfirmar();
        public event delegateConfirmar Confirmar;

        public delegate void delegateCancelar();
        public event delegateCancelar Cancelar;

        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnConfirmar_Click
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Confirmar != null)
                Confirmar();

            FecharModal();
        }
        #endregion

        #region btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Cancelar != null)
                Cancelar();

            FecharModal();
        }
        #endregion

        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            mpeConfirmacaoExclusao.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeConfirmacaoExclusao.Hide();
        }
        #endregion

        #region Inicializar
        public void Inicializar(string titulo, string descricao)
        {
            lblTitulo.Text = titulo;
            lblDescricao.Text = descricao;
            upConfirmacaoExclusao.Update();
        }
        #endregion

        #endregion


    }
}