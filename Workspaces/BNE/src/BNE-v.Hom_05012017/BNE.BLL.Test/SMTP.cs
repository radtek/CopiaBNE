using System;
using BNE.BLL.MailService.Providers;
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
        public void EnvioSMTP1()
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

            var plugin = new BNE.Services.Plugins.PluginsEntrada.EnvioEmailSMTP();
            plugin.ExecuteTask(parametros, null);
        }


        private static SendGrid _instance;

        private static SendGrid Instance
        {
            get { return _instance ?? (_instance = new SendGrid(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SendgridAPIKeyToken), Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail))); }
        }

        [TestMethod]
        public void SendGridTest()
        {
            string emailRemetente = "gieyson@bne.com.br";
            string emailDestinatario = "gieyson@bne.com.br";
            string assunto = "Assunto";
            string mensagem = "Mensagem";
            Instance.Send(emailRemetente, emailDestinatario, assunto, mensagem);
        }
    }
}
