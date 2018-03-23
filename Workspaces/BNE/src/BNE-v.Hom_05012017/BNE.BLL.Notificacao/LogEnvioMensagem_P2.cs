using System;
using System.Data;

namespace BNE.BLL.Notificacao
{
    public partial class LogEnvioMensagem
    {
        public static void Logar(string assunto, string emailRemetente, string emailDestinatario, int idCurriculo, string codigoVagas, int idCarta)
        {
            var objLogEnvioMensagem = new LogEnvioMensagem
            {
                DesAssunto = assunto,
                CartaEmail = idCarta,
                curriculo = idCurriculo,
                EmlDestinatario = emailDestinatario,
                EmlRemetente = emailRemetente,
                ObsMensagem = codigoVagas,
                DataCadastro = DateTime.Now
            };
            objLogEnvioMensagem.Save();
        }
    }
}
