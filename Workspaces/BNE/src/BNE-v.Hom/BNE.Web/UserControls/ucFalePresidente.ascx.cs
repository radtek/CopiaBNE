using System;
using System.Collections.Generic;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class FalePresidente : BaseUserControl
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            //Ajustando a expressao de validacao.
            txtEmail.ExpressaoValidacao = Configuracao.regexEmail;
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.EmailMensagens, 
                        Enumeradores.Parametro.EmailFalePresidente
                    };
                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                string templateAssunto;
                string templateMensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.FalePresidente, out templateAssunto);

                var parametrosMensagem = new
                    {
                        DataEnvio = DateTime.Today.ToShortDateString(),
                        Nome = txtNome.Valor,
                        Email = txtEmail.Valor,
                        CPF = txtCPF.Valor,
                        Mensagem = txtMensagem.Valor,
                        Assunto = txtAssunto.Valor
                    };

                string assunto = parametrosMensagem.ToString(templateAssunto);
                string mensagem = parametrosMensagem.ToString(templateMensagem);

                string para = valoresParametros[Enumeradores.Parametro.EmailFalePresidente];
                string de = txtEmail.Valor;

                int? idUsuarioFilialPerfil = null;
                
                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoCandidato.Value;

                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                    idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, idUsuarioFilialPerfil.HasValue ? new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value) : null, null, assunto, mensagem,Enumeradores.CartaEmail.FalePresidente, de, para);

                string msgAviso = MensagemAviso._203018;

                if (txtAssunto.Valor == "Quero excluir meu currículo")
                {
                    msgAviso = MensagemAviso._505706;
                }

                ucModalConfirmacao.PreencherCampos("Confirmação de Envio", msgAviso, false);
                ucModalConfirmacao.MostrarModal();
                
                LimparCampos();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            //Bug: Voltar do Fale com o Presidente deve voltar para a Home - Bug 5829
            Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        #region LimparCampos
        private void LimparCampos()
        {
            txtNome.Valor = String.Empty;
            txtEmail.Valor = String.Empty;
            txtCPF.Valor = String.Empty;
            txtAssunto.Valor = String.Empty;
            txtAssunto.Enabled = true;
            txtMensagem.Valor = String.Empty;
        }
        #endregion

        #region PreecherCampos
        private void PreecherCampos()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                txtNome.Valor = objPessoaFisica.NomePessoa;
                txtEmail.Valor = objPessoaFisica.EmailPessoa;
                txtCPF.Valor = objPessoaFisica.NumeroCPF;
            }

            string assunto = string.Empty;
            assunto = (Request.QueryString["action"] != null ? "Quero excluir meu currículo" : "");

            if (assunto != "")
            {
                txtAssunto.Valor = assunto;
                txtAssunto.Enabled = false;
            }
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            LimparCampos();
            PreecherCampos();
        }
        #endregion

        #endregion

    }
}