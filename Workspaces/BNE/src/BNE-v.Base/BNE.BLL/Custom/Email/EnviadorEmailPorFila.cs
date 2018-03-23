using BNE.EL;
using System;
using System.Data.SqlClient;

namespace BNE.BLL.Custom.Email
{
    public class EnviadorEmailPorFila : ComponenteEnviadorEmail
    {
        #region Métodos

        #region VerificarEmail
        public static void VerificarEmail(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                const string mensagemErro = "É necessário informar um e-mail! Remetente em branco.";
                GerenciadorException.GravarExcecao(new Exception(mensagemErro));
            }

            if (!Validacao.ValidarEmail(from))
            {
                string mensagemErro = string.Format("É necessário informar um e-mail válido! Remetente inválido: {0}.", from);
                GerenciadorException.GravarExcecao(new Exception(mensagemErro));
            }

            if (string.IsNullOrEmpty(to))
            {
                const string mensagemErro = "É necessário informar um e-mail! Destinatário em branco.";
                GerenciadorException.GravarExcecao(new Exception(mensagemErro));
            }

            if (!Validacao.ValidarEmail(to))
            {
                string mensagemErro = string.Format("É necessário informar um e-mail válido! Destinatário inválido: {0}.", to);
                GerenciadorException.GravarExcecao(new Exception(mensagemErro));
            }
        }
        #endregion

        #region Enviar
        public override bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema,
            string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans)
        {
            VerificarEmail(emailRemetente, emailDestinatario);

            return MensagemCS.SalvarEmail(objCurriculo, objUfpRemetente, objUfpDestinatario, objMensagemSistema, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, trans);
        }
        #endregion

        #endregion
    }
}
