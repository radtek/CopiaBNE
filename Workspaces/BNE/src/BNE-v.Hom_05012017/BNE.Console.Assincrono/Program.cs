using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Console.Assincrono
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1; i++)
            {
                var idMensagem = 0;
                var de = "gieyson@bne.com.br";
                var para = "gieyson@bne.com.br";
                var assunto = "TAG v3 sendgrid api";
                var mensagem = "Oi";
                SqlString tags = "SendgridTag; Teste TAG";

                var parametros = new ParametroExecucaoCollection
                {
                    {"idMensagem", "Mensagem", idMensagem.ToString(), idMensagem.ToString()},
                    {"emailRemetente", "Remetente", de.ToString().Trim(), de.ToString()},
                    {"emailDestinatario", "Destinatário", para.ToString().Trim(), para.ToString()},
                    {"assunto", "Assunto", assunto.ToString(), "Assunto"},
                    {"mensagem", "Mensagem", mensagem.ToString(), "Mensagem"}
                };

                if (!tags.IsNull)
                    parametros.Add("tags", "tags", tags.ToString(), "Tags");

                ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioEmailMailing, parametros);
            }
        }
    }
}
