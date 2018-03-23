using System;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucModalLogin : BaseUserControl
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucLogin.Cancelar += ucLogin_Cancelar;
            ucLogin.Logar += ucLogin_Logar;
        }
        #endregion

        #region ucLogin_LogarModal
        void ucLogin_Logar(string urlDestino)
        {
            if (Logar != null)
                Logar(urlDestino);

            Esconder();
        }
        #endregion

        #region ucLogin_CancelarModal
        void ucLogin_Cancelar()
        {
            Esconder();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            ucLogin.Inicializar();
        }
        public void Inicializar(bool redirecionarParaVagas, bool redirecionarParaNovoVip, string redirectUrl)
        {
            ucLogin.Inicializar(redirecionarParaVagas, redirecionarParaNovoVip, redirectUrl);
        }
        #endregion

        #region Mostrar
        public void Mostrar()
        {
            mpeLogin.Show();
        }
        #endregion

        #region Esconder
        public void Esconder()
        {
            mpeLogin.Hide();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeLogin.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void logar(string urlDestino);
        public event logar Logar;
        public delegate void fechar();
        public event fechar Fechar;
        #endregion

    }
}