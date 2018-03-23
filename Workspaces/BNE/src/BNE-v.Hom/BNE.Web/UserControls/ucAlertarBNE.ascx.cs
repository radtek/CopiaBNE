using System;
using System.Text;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace BNE.Web.UserControls
{
    public partial class ucAlertarBNE : BaseUserControl
    {

        #region Propriedades

        #region IdCurriculoAlertarBNE - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdCurriculoAlertarBNE
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
            set
            {
                if (value == null)
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
                else
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Delegate

        public delegate void DelegateAlertaEnviado();
        public event DelegateAlertaEnviado AlertaEnviado;

        public delegate void DelagateFechar();
        public event DelagateFechar Fechar;

        #endregion

        #region Eventos

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
            LimparCampos();
            FecharModal();
            if (AlertaEnviado != null)
                AlertaEnviado();
        }
        #endregion

        #region btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            FecharModal();
        }
        #endregion

        #region btiFechar_Click
        protected void btlFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region Salvar
        private void Salvar()
        {
            if (IdCurriculoAlertarBNE.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                var curriculo = new Curriculo(IdCurriculoAlertarBNE.Value);
                curriculo.Denunciar(txtDescricao.Valor, UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtDescricao.Valor = string.Empty;
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeAlertarBNE.Hide();

            if (Fechar != null)
                Fechar();
        }
        #endregion

        #region AbrirModal
        public void AbrirModal()
        {
            mpeAlertarBNE.Show();
        }
        #endregion

        #endregion

    }
}