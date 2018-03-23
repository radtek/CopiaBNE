using System.Data.SqlClient;

namespace BNE.BLL.Custom.Email
{
    public abstract class ComponenteEnviadorEmail
    {
        #region Método abstrato
        public abstract bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema, 
            string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans);
        #endregion

        #region Métodos concretos
        public bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, System.Data.SqlClient.SqlTransaction trans)
        {
            return Enviar(objCurriculo, objUfpRemetente, objUfpDestinatario, null, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, trans);
        }

        public bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, string assunto, string mensagem, string emailRemetente, string emailDestinatario, System.Data.SqlClient.SqlTransaction trans)
        {
            return Enviar(objCurriculo, objUfpRemetente, objUfpDestinatario, null, assunto, mensagem, emailRemetente, emailDestinatario, null, null, trans);
        }

        public bool Enviar(string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, System.Data.SqlClient.SqlTransaction trans)
        {
            return Enviar(null, null, null, null, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, trans);
        }

        public bool Enviar(string assunto, string mensagem, string emailRemetente, string emailDestinatario, System.Data.SqlClient.SqlTransaction trans)
        {
            return Enviar(null, null, null, null, assunto, mensagem, emailRemetente, emailDestinatario, null, null, trans);
        }

        public bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo)
        {
            return Enviar(objCurriculo, objUfpRemetente, objUfpDestinatario, null, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, null);
        }

        public bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, string assunto, string mensagem, string emailRemetente, string emailDestinatario)
        {
            return Enviar(objCurriculo, objUfpRemetente, objUfpDestinatario, null, assunto, mensagem, emailRemetente, emailDestinatario, null, null, null);
        }

        public bool Enviar(string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo)
        {
            return Enviar(null, null, null, null, assunto, mensagem, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, null);
        }

        public bool Enviar(string assunto, string mensagem, string emailRemetente, string emailDestinatario)
        {
            return Enviar(null, null, null, null, assunto, mensagem, emailRemetente, emailDestinatario, null, null, null);
        }
        #endregion
    }
}
