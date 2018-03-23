using System;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalConfirmacao : BaseUserControl
    {

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region btiFechar_Click
        protected void btiFecha_Click(object sender, EventArgs e)
        {
            FecharModal();

            if (eventFechar != null)
                eventFechar();
        }
        #endregion

        #region btnConfirmarModal_Click
        protected void btnConfirmarModal_Click(object sender, EventArgs e)
        {
            FecharModal();

            if (ModalConfirmada != null)
                ModalConfirmada();
        }
        #endregion

        #region Delegates
        public delegate void delegateConfirmacao();
        public event delegateConfirmacao ModalConfirmada;
        public delegate void delegateCliqueAqui();
        public event delegateCliqueAqui CliqueAqui;
        public delegate void delegateFechar();
        public event delegateFechar eventFechar;
        public delegate void delegateVoltar();
        public event delegateVoltar eventVoltar;

        #endregion

        #region MostrarModal
        public void MostrarModal()
        {

            mpeModalSucesso.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeModalSucesso.Hide();
        }
        #endregion

        #region PreencherCampos(string strtitulo, string strMensagem){
        /// <summary>
        /// Alerta
        /// </summary>
        /// <param name="strTitulo"></param>
        /// <param name="strMensagem"></param>
        public void PreencherCampos(string strTitulo, string strMensagem)
        {
            divSucess.Visible = false; divAlert.Visible = true;
            pnlModalSucesso.CssClass = "alertModal";
            PreencherCampos(strTitulo, strMensagem, String.Empty, false, false);
    
        }
        #endregion
        #region PreencherCampos
        /// <summary>
        /// Confirmação
        /// </summary>
        /// <param name="strTitulo"></param>
        /// <param name="strMensagem"></param>
        /// <param name="visualizarCliqueAqui"></param>
        public void PreencherCampos(string strTitulo, string strMensagem, bool visualizarCliqueAqui)
        {
            PreencherCampos(strTitulo, strMensagem, String.Empty, visualizarCliqueAqui, false);
        }

        public void PreencherCampos(string strTitulo, string strMensagem, bool visualizarCliqueAqui, bool visualizarVoltar)
        {
            PreencherCampos(strTitulo, strMensagem, String.Empty, visualizarCliqueAqui, visualizarVoltar);
        }

        public void PreencherCampos(string strTitulo, string strMensagem, string strMensagemAuxiliar, bool visualizarCliqueAqui)
        {
            PreencherCampos(strTitulo, strMensagem, String.Empty, visualizarCliqueAqui, false);
        }

        public void PreencherCampos(string strTitulo, string strMensagem, string strMensagemAuxiliar, bool visualizarCliqueAqui, bool visualizarVoltar)
        {
            lblTitulo.Text = strTitulo;
            lblTexto.Text = strMensagem;
            btlCliqueAqui.Visible = visualizarCliqueAqui;
            btnVoltar.Visible = visualizarVoltar;
            if (!String.IsNullOrEmpty(strMensagemAuxiliar))
            {
                lblTextoAuxiliar.Text = String.Format("<br/>{0}", strMensagemAuxiliar);
                lblTextoAuxiliar.Visible = true;
            }
            upConteudo.Update();
        }

        public void PreencherCampos(string strTitulo, string strMensagem, bool visualizarCliqueAqui, string nomeBotao)
        {
            lblTitulo.Text = strTitulo;
            lblTexto.Text = strMensagem;
            btlCliqueAqui.Visible = visualizarCliqueAqui;
            upConteudo.Update();

        }
        #endregion

        #region btlCliqueAqui_Click
        protected void btlCliqueAqui_Click(object sender, EventArgs e)
        {
            if (CliqueAqui != null)
                CliqueAqui();
        }
        #endregion

        #region btnVoltar_Click

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (eventVoltar != null)
                eventVoltar();
        }

        #endregion
    }
}