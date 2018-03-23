using Autofac;
using BNE.Mensagem.Domain.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.Mensagem.Domain.Test
{
    [TestClass]
    public class EmailTest : BaseTest
    {
        [TestMethod]
        public void EnviarEmail()
        {
            var emailDomain = AutofacContainer.Resolve<Domain.Email>();

            var retorno = emailDomain.EnviarEmail(new EnviarEmail
            {
                Assunto = "Teste do Gieyson",
                EmailRemetente = "gieyson@bne.com.br",
                EmailDestino = "grstelmak@gmail.com",
                GuidUsuarioDestino = "20adf87c-5a7b-4c84-b237-14dd0191dd85",
                GuidUsuarioRemetente = "76bc310b-017f-43b5-ad7a-2d699905d147",
                Parametros = new { NomeEmpresa = "Gieyson Corp", NomeUsuario = "Gieyson Stelmak" },
                Sistema = "bne",
                Template = "CartaBoasVindasPJ"
            });

            Assert.AreEqual(true, retorno);
        }
    }
}
