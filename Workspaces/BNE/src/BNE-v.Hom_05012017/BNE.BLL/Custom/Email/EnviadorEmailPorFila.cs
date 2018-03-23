using System.Data.SqlClient;

namespace BNE.BLL.Custom.Email
{
    public class EnviadorEmailPorFila : ComponenteEnviadorEmail
    {
        #region Métodos

        #region Enviar
        public override bool Enviar(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema, string assunto, string mensagem, Enumeradores.CartaEmail? carta, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans)
        {
            return MensagemCS.SalvarEmail(objCurriculo, objUfpRemetente, objUfpDestinatario, objMensagemSistema, assunto,
                   mensagem, carta, emailRemetente, emailDestinatario, nomeArquivoAnexo, arquivoAnexo, trans);
        }
        #endregion

        #endregion
    }
}
