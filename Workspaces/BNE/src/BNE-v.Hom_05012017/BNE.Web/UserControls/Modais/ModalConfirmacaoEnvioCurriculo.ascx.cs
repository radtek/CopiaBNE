using System;
using BNE.BLL;
using BNE.BLL.Custom;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalConfirmacaoEnvioCurriculo : System.Web.UI.UserControl
    {

        #region Eventos
        
        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnOK_Click
        protected void btnOK_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar(string nome, string protocolo)
        {
            //litTitulo.Text = "Confirmação de Envio";
            pnlProtocoloCandidatura.Visible = true;
            litNome.Text = nome;
            litProtocolo.Text = protocolo;
            upProtocolo.Update();
            //upLitTitulo.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeModalConfirmacaoEnvioCurriculo.Show();
        }
        #endregion

        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeModalConfirmacaoEnvioCurriculo.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion

        #region Delegates
        public delegate void fechar();
        public event fechar Fechar;
        #endregion

    }
}