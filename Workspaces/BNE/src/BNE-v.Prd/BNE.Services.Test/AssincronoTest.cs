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
        public void PublicacaoVaga()
        {

            var idVaga = 1883050;
            var parametros = new ParametroExecucaoCollection
            {
                {"idVaga", "idVaga", idVaga.ToString(), idVaga.ToString()},

            };

            var plugin = new BNE.Services.Plugins.PluginsEntrada.PublicacaoVaga();


            plugin.DoExecute(parametros, null);
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

        [TestMethod]
        public void EnvioCandidatoVagaPerfil()
        {
            var idVaga = 1662878;
            var parametros = new ParametroExecucaoCollection
            {
                {"idVaga", "idVaga", idVaga.ToString(), idVaga.ToString()},

            };

            var plugin = new BNE.Services.Plugins.PluginsEntrada.EnvioCandidatoVagaPerfil();


            //  plugin.Execute(parametros);

        }

        [TestMethod]
        public void TesteSMSQuemMeViu()
        {
            var parametros = new ParametroExecucaoCollection
            {
                {"idMensagemCS", "idMensagemCS", "10443006", "10443006"},
                { "idUsuarioTanque","idUsuarioTanque", "QuemMeViu","QuemMeViu"},
                {"idCurriculo", "idCurriculo", "8094302", "8094302"},
                { "numeroDDD","numeroDDD", "41","41"},
                { "numeroCelular","numeroCelular", "999440287","999440287"},
                {"nomePessoa","nomePessoa","Elton Teste","Elton Teste" },
                {"Mensagem","Mensagem","Olá Teste, a Rh Center viu o seu curriculo agora mesmo. Saiba mais pelo Quem me Viu em http://www.bne.com.br","Olá Teste, a Rh Center viu o seu curriculo agora mesmo. Saiba mais pelo Quem me Viu em http://www.bne.com.br" }
            };

            var plugin = new BNE.Services.Plugins.PluginsEntrada.EnvioSMSTanque();

            var retorno = plugin.ExecuteTask(parametros,null);

            var pluginSaida = new BNE.Services.Plugins.PluginSaida.PluginSaidaEmailSMSTanque();

            pluginSaida.ExecuteTask(retorno, null);

            //ProcessoAssincrono.IniciarAtividade(BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioSMSTanque, parametros);
        }

        [TestMethod]
        public void IntegracaoCandidaturaSine()
        {
            var parametros = new ParametroExecucaoCollection
            {
                {"IdVaga", "IdVaga", "1475437", "1475437"},
                {"IdCurriculo", "IdCurriculo", "8094302", "8094302"},
            };

            var plugin = new BNE.Services.Plugins.PluginsEntrada.IntegrarCandidaturaSine();

            var retorno = plugin.ExecuteTask(parametros, null);

            var pluginSaida = new BNE.Services.Plugins.PluginSaida.PluginSaidaIntegrarCandidaturaSine();

            pluginSaida.ExecuteTask(retorno, null);
        }

    }
}
