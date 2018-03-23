using System.ComponentModel.Composition.Hosting;
using System.Threading;
using BNE.Mensagem.AsyncServices.BLL;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BNE.Mensagem.AsyncExecutor.Test
{

    [TestClass]
    public class EnvioEmailTest
    {

        [TestMethod]
        public void EnviarEmail()
        {
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idEmail",
                                Valor = "45"
                            }
                        };

            AsyncServices.Base.ProcessosAssincronos.ProcessoAssincrono.IniciarAtividade(
            AsyncServices.BLL.Enumeradores.TipoAtividade.Email,
            new Model.Sistema { Id = 1, Nome = "bne" },
            new Model.TemplateEmail { Id = 7, Nome = "cartaboasvindaspjteste" },
            PluginsCompatibilidade.CarregarPorMetadata("EnvioEmail", "PluginSaidaEmail"),
            parametros,
            null);

            Controller.GetControlerCapabilities += () => new Capabilities();
            Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\tfs\tfs.employer.com.br\BNE\src\BNE-New\BNE.Mensagem.AsyncExecutor.Plugins\bin\Debug");
            Controller.InitializeController();
            Controller.StartController();

            var m = new ManualResetEventSlim(false);
            m.Wait();
        }

    }
}
