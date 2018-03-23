using System;
using System.Web.UI;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;

namespace BNE.Web.UserControls.Modais
{
    public partial class EditarDadosPessoais : BaseUserControl
    {

        #region Propriedades

        #region IdPessoaFisica
        public int IdPessoaFisica
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Delegates

        public delegate void DelegateSucesso();
        public event DelegateSucesso Sucesso;

        #endregion

        #region btiFechar_Click
        protected void BtiFecharClick(object sender, ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region btnConfirmar_Click
        protected void BtnConfirmarClick(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                LimparCampos();
                FecharModal();

                if (Sucesso != null)
                    Sucesso();
                upEditarDadosPessoais.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica);
            PreencherCampos(objPessoaFisica);
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeEditarDadosPessoais.Hide();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeEditarDadosPessoais.Show();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(PessoaFisica objPessoaFisica)
        {
            lblCPFValor.Text = objPessoaFisica.NumeroCPF;
            txtNome.Valor = objPessoaFisica.NomePessoa;
            txtDataNascimento.Valor = objPessoaFisica.DataNascimento.ToShortDateString();
            txtTelefone.Fone = objPessoaFisica.NumeroCelular;
            txtTelefone.DDD = objPessoaFisica.NumeroDDDCelular;

            EnableCampos(true);
        }
        #endregion

        #region EnableCampos
        private void EnableCampos(bool enable)
        {
            txtDataNascimento.Enabled = enable;
            txtNome.Enabled = enable;
            txtTelefone.Enabled = enable;
            upEditarDadosPessoais.Update();
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica);
            objPessoaFisica.NomePessoa = txtNome.Valor;
            objPessoaFisica.DataNascimento = txtDataNascimento.ValorDatetime.Value;
            objPessoaFisica.NumeroCelular = txtTelefone.Fone;
            objPessoaFisica.NumeroDDDCelular = txtTelefone.DDD;

            objPessoaFisica.Save();
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtNome.Valor = string.Empty;
            txtTelefone.DDD = string.Empty;
            txtTelefone.Fone = string.Empty;
            txtDataNascimento.Valor = string.Empty;
        }
        #endregion

        #endregion

    }
}