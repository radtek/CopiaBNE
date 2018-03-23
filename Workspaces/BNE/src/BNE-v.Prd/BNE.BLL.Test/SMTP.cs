using BNE.Services.Base.ProcessosAssincronos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.BLL.Test
{
    [TestClass]
    public class SMTP
    {
        [TestMethod]
        public void EnvioSMTP()
        {
            var parametros = new ParametroExecucaoCollection()
            {
                new ParametroExecucao
                {
                    Parametro = "idMensagem",
                    DesParametro = "idMensagem",
                    Valor = "7",
                    DesValor = "7"
                }
            };

            //ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.EnvioEmailSMTPCloud, parametros);
        }
        
        [TestMethod]
        public void MensagemCSTeste()
        {
            string emailRemetente = "gieyson@bne.com.br";
            string emailDestinatario = "grstelmak@gmail.com";
            string assunto = "Assunto";
            string mensagem = "Mensagem";

            MensagemCS.SalvarEmail(null, null, null, null, assunto, mensagem, null, emailRemetente, emailDestinatario, null, null, null);
        }
    }
}
