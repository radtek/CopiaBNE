using System;
using BNE.BLL;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Dados : BaseUserControl
    {

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            PreencherCampos();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            lblNomeUsuarioInternoValor.Text = new PessoaFisica(base.IdPessoaFisicaLogada.Value).PrimeiroNome;

            CarregarSaudacao();
        }
        #endregion

        #region CarregarSaudacao
        public void CarregarSaudacao()
        {
            if (DateTime.Now.Hour < 12)
                lblSaudacao.Text = "Bom Dia, ";
            else if (DateTime.Now.Hour < 18)
                lblSaudacao.Text = "Boa Tarde, ";
            else
                lblSaudacao.Text = "Boa Noite, ";
        }
        #endregion

        #endregion
    }
}