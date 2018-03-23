using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BNE.BLL.Custom.Email
{
    [Obsolete("Use EnviadorEmailPorFila().", true)]
    public class EnviadorEmailPorSmtp : ComponenteEnviadorEmail
    {
        #region Métodos

        #region VerificarEmail
        public static bool VerificarEmail(ref string from, string to)
        {
            bool emailValido = true;

            if (string.IsNullOrEmpty(from.Trim()))
            {
                from = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);
            }

            if (!Validacao.ValidarEmail(from))
            {
                emailValido = false;
            }

            if (string.IsNullOrEmpty(to))
            {
                emailValido = false;
            }
            else if (!Validacao.ValidarEmail(to))
            {
                emailValido = false;
            }

            return emailValido;
        }
        #endregion

        #region Enviar
        [Obsolete("Use EnviadorEmailPorFila().", true)]
        public override bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema,
            string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans)
        {
            if (VerificarEmail(ref emailRemetente, emailDestinatario))
            {
                return MailController.Send(emailDestinatario, emailRemetente, assunto, mensagem, new Dictionary<string, byte[]> { { nomeArquivoAnexo, arquivoAnexo } });
            }
            return false;
        }
        #endregion

        #endregion
    }
}
