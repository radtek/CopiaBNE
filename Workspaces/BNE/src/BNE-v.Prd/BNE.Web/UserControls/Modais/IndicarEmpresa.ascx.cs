using System;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class IndicarEmpresa : BaseUserControl
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(IndicarEmpresa));
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                var objIndicacaoFilial = new IndicacaoFilial();

                //Endereco
                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidade.Text, out objCidade))
                    objIndicacaoFilial.Cidade = objCidade;

                objIndicacaoFilial.NomeEmpresa = txtNomeEmpresa.Valor;
                objIndicacaoFilial.Save();

                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);

                if (!String.IsNullOrEmpty(objPessoaFisica.EmailPessoa))
                {
                    string mensagem = String.Format(@"Indicação Empresa: {0} <br />Usuário: {1} <br />Indicou a Empresa: {2} <br />Da Cidade: {3}", DateTime.Today.ToShortDateString(), objPessoaFisica.NomePessoa, objIndicacaoFilial.NomeEmpresa, objIndicacaoFilial.Cidade.NomeCidade);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(null, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value), null, "Não achei empresa", mensagem,null, objPessoaFisica.EmailPessoa, Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailCIA));
                }

                if (Indicar != null)
                    Indicar();

                FecharModal();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region MostrarModal
        public void MostrarModal()
        {
            mpeIndicarEmpresa.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeIndicarEmpresa.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void fechar();
        public event fechar Fechar;

        public delegate void indicar();
        public event indicar Indicar;
        #endregion

    }
}