using AllInMail.Core;
using BNE.Services.AsyncExecutor;
using BNE.Services.Base.ProcessosAssincronos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace BNE.Services.Test
{

    [TestClass]
    public class ServiceAllinTest
    {
        [TestMethod]
        public void TestQuemMeViu()
        {
            //var service = new CustomAllinEmailQuemMeViu();
            var service = new AllinEmailQuemMeViu();
            service.Do(true);
        }

        [TestMethod]
        public void ProcedureVagasTest()
        {

        }

        [TestMethod]
        public void AssincronoTest()
        {
            Controller.GetControlerCapabilities += () => new Capabilities();
            Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\tfs\tfs.employer.com.br\BNE\src\BNE-v.Prd\BNE.Services.Plugins\bin\Debug");
            Controller.InitializeController();
            Controller.StartController();

            new ManualResetEventSlim(false).Wait();
        }

        [TestMethod]
        public void GatilhosSimulacao()
        {
            try
            {
                Controller.GetControlerCapabilities += () => new Capabilities();
                Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\Users\lennonvidal\Job\BNE\src\BNE-v.Prd\BNE.Services.Plugins\bin\Debug");
                Controller.InitializeController();

                var queueListener = new CustomQueueListener(BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil);

                foreach (var item in GetRunningMessages())
                {
                    try
                    {
                        queueListener.SimulateNewMessage(item);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                }

                Thread.Sleep(5000);

                while (queueListener.CountWorker > 0)
                {
                    Thread.Sleep(5000);
            }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }


            new ManualResetEventSlim(false).Wait();
        }

        private IEnumerable<MensagemAtividade> GetRunningMessages()
        {
            foreach (var item in GetRunningMessageIds())
            {
                yield return new MensagemAtividade { IdfAtividade = item };
            }
        }

        private IEnumerable<int> GetRunningMessageIds()
        {
            var codes = @"6397330";

            foreach (var item in codes.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                int value;
                if (int.TryParse(item, out value))
                {
                    yield return value;
                }
            }
        }
        [TestMethod]
        public void CarregarModelosAlln()
        {
            var ciclo = new BLL.AllinCicloVida(1);
            ciclo.CompleteObject();

            var trans = new BLL.AllinTransacional(1);
            trans.CompleteObject();

        }
        [TestMethod]
        public void OrigemTest()
        {
            var c1 = new BLL.Curriculo(2455840);
            Assert.IsTrue(c1.OrigemBNE());
        }

        [TestMethod]
        public void GatilhoBNETest()
        {
            var c2 = new BLL.Curriculo(2455840);
            c2.CompleteObject();
            c2.PessoaFisica.CompleteObject();

            try
            {
                c2.GatilhoCadastroBNE();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
        [TestMethod]
        public void TransactionTest()
        {
            var callLogin = AllInTriggers.AllInTransaction.LoginTransactionCall();
            var loginResult = callLogin.Execute();

            var buscarHtml = AllInTriggers.AllInTransaction.BuscarHtmlCall(loginResult, "1");
            var htmlResult = buscarHtml.Execute();

            Assert.IsTrue(loginResult != null);
        }

        [TestMethod]
        public void InserirOuAtualizarItemListaTest()
        {
            var modelCiclo = PopularModeloAtualizar();

            var callLogin = AllInTriggers.AllInPainel.LoginTokenPainelCall();
            var loginResult = callLogin.Execute();

            var callCicloDeVida = AllInTriggers.AllInPainel.InserirOuAtualizarItemLista(loginResult, modelCiclo);

            var cicloResult = callCicloDeVida.Execute();

            Assert.IsTrue(cicloResult != null);
            Assert.IsTrue(cicloResult.Contains("Email inserido na base!"));
        }

        [TestMethod]
        public void CicloVidaTest()
        {
            //    var callLogin = AllInTriggers.AllInPainel.LoginTokenPainelCall();
            //    var loginResult = callLogin.Execute();

            //    Assert.IsTrue(loginResult != null);

            var modelCiclo = PopularCicloDeVida();
            modelCiclo.EmailEnvio = "patrick@bne.com.br";
            var callCicloDeVida = AllInTriggers.AllInPainel.NotificarCicloDeVidaCall(modelCiclo);

            var cicloResult = callCicloDeVida.Execute();

            Assert.IsTrue(cicloResult != null);
            Assert.IsTrue(cicloResult == "OK");
        }

        private AllInTriggers.Model.InserirOuAtualizarItemListaAllIn PopularModeloAtualizar()
        {
            const int curriculoId = 3353507;

            var bllModel = BLL.Curriculo.CarregarCurriculoExportacaoAllIn(curriculoId, null);
            var allinModel = AllInMail.Helper.ModelTranslator.TranslateToAllInCurriculoModel(bllModel);

            var conv = new AllInCurriculoDefConverter();

            var resValue = conv.Parse(allinModel);
            var defProperties = conv.GetDefiniedFields();
            var defValues = resValue.Split(';');

            Assert.IsTrue(defProperties.Length == defValues.Length);

            var obj = new AllInTriggers.Model.InserirOuAtualizarItemListaAllIn();
            obj.NomeLista = "BNE Prd";
            obj.TituloCampos = conv.GetDeclaration();
            obj.ValorCampos = resValue;

            return obj;
        }

        private AllInTriggers.Model.NotificaCicloDeVidaAllIn PopularCicloDeVida()
        {
            var obj = new AllInTriggers.Model.NotificaCicloDeVidaAllIn();
            obj.AceitaRepeticao = false;
            //obj.EmailEnvio = "no-reply@bne.com.br";
            obj.IdentificadorAllIn = "4788";
            obj.Evento = "Novo Cadastro";
            //obj.Evento = "Fake";

            var curriculoId = 709029;
            var bllModel = BLL.Curriculo.CarregarCurriculoExportacaoAllIn(curriculoId, null);
            obj.EmailEnvio = bllModel.Email;

            var allinModel = AllInMail.Helper.ModelTranslator.TranslateToAllInCurriculoModel(bllModel);

            var conv = new AllInCurriculoDefConverter();

            var resValue = conv.Parse(allinModel);
            var defProperties = conv.GetDefiniedFields();
            var defValues = resValue.Split(';');

            Assert.IsTrue(defProperties.Length == defValues.Length);

            var pairs = defProperties.Select((a, index) => new KeyValuePair<string, string>(a, defValues[index]));

            var pf = new BLL.PessoaFisica(bllModel.IdPessoaFisica);
            pf.CompleteObject();

            var urlValue = @"http://dev.bne.com.br/logar/" + BNE.BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(pf);
            var customPair = new[] { new KeyValuePair<string, string>("url", urlValue) };

            obj.CamposComValores = pairs.Concat(customPair).ToArray();


            return obj;
        }
    }

    public class CustomQueueListener : QueueListener
    {
        public CustomQueueListener(BLL.AsyncServices.Enumeradores.TipoAtividade atividade)
            : base(atividade)
        {

        }

        internal void SimulateNewMessage(MensagemAtividade item)
        {
            QueueReceive(item);
        }

        public int CountWorker
        {
            get
            {
                var field = typeof(QueueListener).GetField("_workers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                return (int)field.GetValue(this);
            }
        }
    }
}
