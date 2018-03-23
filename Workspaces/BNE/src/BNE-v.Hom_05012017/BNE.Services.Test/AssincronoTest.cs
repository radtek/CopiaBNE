using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BNE.Services.Base.ProcessosAssincronos;
using System.Threading;
using BNE.Services.AsyncExecutor;
using System.ComponentModel.Composition.Hosting;
using System.Data.SqlTypes;
using BNE.BLL.AsyncServices;

namespace BNE.Services.Test
{
    [TestClass]
    public class AssincronoTest
    {

        [TestMethod]
        public void AssincronoEnvioCandidatoVagaPerfilAtividade()
        {

            /*
             1123928 
            1123930
            1123931
            */
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idVaga",
                                DesParametro = "idVaga",
                                Valor = "1123931",
                                DesValor = "V01123931"
                            }
                        };

            ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil, parametros);

            Controller.GetControlerCapabilities += () => new Capabilities();
            Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\tfs\tfs.employer.com.br\BNE\src\BNE-v.Prd\BNE.Services.Plugins\bin\Debug");
            Controller.InitializeController();
            Controller.StartController();

            new ManualResetEventSlim(false).Wait();
        }



        [TestMethod]
        public void GatilhoCurriculoTest()
        {
            //var idCurriculo = "709029";
            var idCurriculo = "709031";

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BNE.BLL.Enumeradores.TipoGatilho.CadastroCurriculo).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = idCurriculo.ToString()    
                            },
                        };

            ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE, parametros);
        }

        [TestMethod]
        public void GatilhoCompraVipTest()
        {
            var idCurriculo = "709031";

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BNE.BLL.Enumeradores.TipoGatilho.CompraAprovadaVip).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = idCurriculo.ToString()
                            },
                        };

            ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE, parametros);
        }

        [TestMethod]
        public void GatilhoAcabouVipTest()
        {
            var idCurriculo = "709031";

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BNE.BLL.Enumeradores.TipoGatilho.AcabouVip).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = idCurriculo.ToString()    
                            },
                        };

            ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE, parametros);
        }

        [TestMethod]
        public void TesteEnvioTAG()
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
                {"assunto", "Assunto", assunto.ToString(), "Assunto" },
                {"mensagem", "Mensagem", mensagem.ToString(), "Mensagem" }
            };

            if (!tags.IsNull)
                parametros.Add("tags", "tags", tags.ToString(), "Tags");

            ProcessoAssincrono.IniciarAtividade(BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioEmailMailing, parametros);

            Controller.GetControlerCapabilities += () => new Capabilities();
            Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\TFS\defaultcollection\BNE\src\BNE-v.Prd\BNE.Services.Plugins\bin\Debug\");
            Controller.InitializeController();
            Controller.StartController();

            new ManualResetEventSlim(false).Wait();
        }

    }
}
