using System.Dynamic;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BNE.Mensagem.Domain.Test
{
    /// <summary>
    /// Summary description for TemplateEmailTest
    /// </summary>
    [TestClass]
    public class TemplateEmailTest : BaseTest
    {

        [TestMethod]
        public void RecuperarTemplate()
        {
            var sistemaDomain = AutofacContainer.Resolve<Domain.Sistema>();
            var templateEmailDomain = AutofacContainer.Resolve<Domain.TemplateEmail>();

            var sistema = sistemaDomain.RecuperarSistema("bne");
            var parametros = new
            {
                NomeEmpresa = "Gieyson Nome Empresa",
                NomeUsuario = "Gieyson"
            };

            var jsonstring = JsonConvert.SerializeObject(parametros);

            var expConverter = new ExpandoObjectConverter();
            dynamic objParametros = JsonConvert.DeserializeObject<ExpandoObject>(jsonstring, expConverter);

            var carta = templateEmailDomain.HTML(sistema, "CartaBoasVindasPJteste", objParametros);

            Assert.AreEqual(carta, "<html><body>UrlImagens: http://www.bne.com.br/imagens UrlSite http://www.bne.com.br Template PJ Nome da Empresa: Gieyson Nome Empresa Nome Ususuário: Gieyson</body></html>");
        }
    }
}
