using System;
using BNE.BLL;
using BNE.Web.Code;
using Resources;
using BNE.BLL.Custom;
using System.Text.RegularExpressions;
using BNE.BLL.Custom.Email;

namespace BNE.Web.UserControls
{
    public partial class IndicarAmigo : BaseUserControl
    {

        #region Eventos

        #region Delegates

        public delegate void MensagemEnviado();
        public event MensagemEnviado Sucesso;

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(IndicarAmigo));
        }

        #endregion

        #region btnEnviarIndicacaoAmigo_Click
        protected void btnEnviarIndicacaoAmigo_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                LimparCampos();
                FecharModal();

                if (Sucesso != null)
                    Sucesso();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnFechar_Click
        protected void btnFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            rfNomeIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            rfEmailIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            cvEmailIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            rfNomeAmigoIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            rfEmailAmigoIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            cvEmailAmigoIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            txtTelefoneCelularAmigoIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;
            btnEnviarIndicacaoAmigoIndiqueBNE.ValidationGroup = btnEnviarIndicacaoAmigoIndiqueBNE.ClientID;

            Inicializar();
            mpeIndicarAmigoModal.Show();
            upIndicarAmigo.Update();
        }
        #endregion

        #region FecharModal

        public void FecharModal()
        {
            mpeIndicarAmigoModal.Hide();
        }

        #endregion

        #region LimparCampos

        public void LimparCampos()
        {
            txtNomeAmigoIndiqueBNE.Text = txtEmailIndiqueBNE.Text = txtEmailAmigoIndiqueBNE.Text =
            txtNomeAmigoIndiqueBNE.Text = txtTelefoneCelularAmigoIndiqueBNE.Fone = txtTelefoneCelularAmigoIndiqueBNE.DDD = string.Empty;
        }

        #endregion

        #region Inicializar
        private void Inicializar()
        {
            lblTitulo.Text = MensagemAviso._23014;
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            string templateAssunto;
            string templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.IndiqueBNE, out templateAssunto);

            var parametros = new
                {
                    NomeOrigem = txtNomeIndiqueBNE.Text,
                    NomeDestino = txtNomeAmigoIndiqueBNE.Text
                };

            string descricaoMensagem = parametros.ToString(templateMensagem);
            string assunto = parametros.ToString(templateAssunto);

            EmailSenderFactory
                .Create(TipoEnviadorEmail.Fila)
                .Enviar(assunto, descricaoMensagem, txtEmailIndiqueBNE.Text, txtEmailAmigoIndiqueBNE.Text);
        }
        #endregion

        #endregion

        #region AjaxMethod

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarEmail(string email)
        {
            var objRegex = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

            //Valida o email
            if (objRegex.IsMatch(email))
                return true;

            return false;
        }

        #endregion

    }
}