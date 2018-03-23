using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom.Email
{
    public class EnviadorEmailComVerificacao : EnviadorEmail
    {
        #region Construtores
        public EnviadorEmailComVerificacao(ComponenteEnviadorEmail componente)
            : base(componente) { }
        #endregion

        #region Métodos
        public override bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema, string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, System.Data.SqlClient.SqlTransaction trans)
        {
            if (Verificado(emailDestinatario))
                return Componente.Enviar(objCurriculo, objUfpRemetente, objUfpDestinatario, objMensagemSistema, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, trans);

            return false;
        }

        private bool Verificado(string emailDestinatario)
        {
            return emailDestinatario.Contains("@bne.com.br") || PessoaFisica.VerificarEmail(emailDestinatario);
        }

        #endregion
    }
}
