using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BNE.Services.AsyncExecutor;
using BNE.Services.Base.ProcessosAssincronos;
using System.ComponentModel.Composition.Hosting;
using System.Threading;

namespace BNE.BLL.Test
{
    [TestClass]
    public class CampanhaTeste
    {
        [TestMethod]
        public void TestarEnvioNovaCampanha()
        {

        }

        [TestMethod]
        public void RecuperarLoteCurriculo()
        {
            var funcao = "Administrador";
            var cidade = "Curitiba/PR";
            var salario = 2000;
            var idFilial = 75302;
            var idUsuario = 742338;

            var objCampanhaRecrutamento = new BLL.CampanhaRecrutamento
            {
                QuantidadeRetorno = 50,
                TipoRetornoCampanhaRecrutamento = new BLL.TipoRetornoCampanhaRecrutamento(Convert.ToInt16(BLL.Enumeradores.TipoRetornoCampanhaRecrutamento.InscricaoVaga)),
                Vaga = new Vaga
                {
                    Cidade = new Cidade(3287),
                    Funcao = new Funcao(5219),
                    ValorSalarioDe = 1100,
                    ValorSalarioPara = 1100
                }
            };
          
            var objCampanha = new PrivateObject(typeof(BNE.Services.Plugins.PluginsEntrada.Campanha));
            Assert.AreEqual(250, ((List<DTO.Curriculo>)objCampanha.Invoke("RecuperarLoteCurriculos", objCampanhaRecrutamento, 250)).Count);
            Assert.AreEqual(500, ((List<DTO.Curriculo>)objCampanha.Invoke("RecuperarLoteCurriculos", objCampanhaRecrutamento, 500)).Count);
            Assert.AreNotEqual(10, ((List<DTO.Curriculo>)objCampanha.Invoke("RecuperarLoteCurriculos", objCampanhaRecrutamento, 500)).Count);
            Assert.AreEqual(100, ((List<DTO.Curriculo>)objCampanha.Invoke("RecuperarLoteCurriculos", objCampanhaRecrutamento, 100)).Count);
            Assert.AreEqual(60, ((List<DTO.Curriculo>)objCampanha.Invoke("RecuperarLoteCurriculos", objCampanhaRecrutamento, 60)).Count);
        }

        [TestMethod]
        public void QuantidadeCurriculosEnviados()
        {
            PrivateObject objCampanhaRecrutamento = new PrivateObject(typeof(CampanhaRecrutamento));
            Assert.AreEqual(objCampanhaRecrutamento.Invoke("QuantidadeCurriculosEnviados"), 0);
            Assert.AreNotEqual(objCampanhaRecrutamento.Invoke("QuantidadeCurriculosEnviados"), 10);
        }

        [TestMethod]
        public void CampanhaTest()
        {
            var funcao = "Auxiliar Administrativo";
            var cidade = "Curitiba/PR";
            var salario = 1100;
            var idFilial = 75302;
            var idUsuario = 742338;

            var objCampanhaRecrutamento = new BLL.CampanhaRecrutamento
            {
                QuantidadeRetorno = 50,
                TipoRetornoCampanhaRecrutamento = new BLL.TipoRetornoCampanhaRecrutamento(Convert.ToInt16(BLL.Enumeradores.TipoRetornoCampanhaRecrutamento.InscricaoVaga)),
            };

            try
            {
                objCampanhaRecrutamento.Salvar(funcao, cidade, salario, new BLL.Filial(idFilial), new BLL.UsuarioFilialPerfil(idUsuario));
            }
            catch (Exception)
            {
            }

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idCampanha",
                                DesParametro = "idCampanha",
                                Valor = objCampanhaRecrutamento.IdCampanhaRecrutamento.ToString(),
                                DesValor = objCampanhaRecrutamento.IdCampanhaRecrutamento.ToString()
                            }
                        };
            var objCampanha = new Services.Plugins.PluginsEntrada.Campanha();
            objCampanha.ExecuteTask(parametros, null);
        }

        [TestMethod]
        public void CampanhaNovoLoteTest()
        {
            var objCampanhaRecrutamento = CampanhaRecrutamento.LoadObject(25852);

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idCampanha",
                                DesParametro = "idCmapanha",
                                Valor = objCampanhaRecrutamento.IdCampanhaRecrutamento.ToString(),
                                DesValor = objCampanhaRecrutamento.IdCampanhaRecrutamento.ToString()
                            }
                        };
            var objCampanha = new Services.Plugins.PluginsEntrada.Campanha();

            objCampanha.ExecuteTask(parametros, null);
        }

        [TestMethod]
        public void RecuperarLotesCV()
        {

            List<BNE.Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque> listaMensagem = new List<Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque>(); ;

            //listaMensagem.Add(new Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque() { DDDCelular = "41", NumeroCelular = "88385693", NomePessoa = "Marty", Descricao = "Mensagem teste", IdCurriculo = 9999 });
            //listaMensagem.Add(new BNE.Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque() { DDDCelular = "11", NumeroCelular = "88385693", NomePessoa = "Marty", Descricao = "Mensagem teste", IdCurriculo = 9999 });
            //listaMensagem.Add(new BNE.Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque() { DDDCelular = "41", NumeroCelular = "99999999", NomePessoa = "Marty", Descricao = "Mensagem teste", IdCurriculo = 9999, IdUsuarioTanque = "QuemMeViu" });
            //listaMensagem.Add(new BNE.Services.Plugins.PluginResult.MensagemPlugin.MensagemSMSTanque() { DDDCelular = "41", NumeroCelular = "77777777", NomePessoa = "Marty", Descricao = "Mensagem teste", IdCurriculo = 9999, IdUsuarioTanque = "QuemMeViu" });
            
            //BNE.Services.Plugins.PluginSaida.PluginSaidaEmailSMSTanque.EnviarSMSTanque(listaMensagem);


            /*
            CampanhaRecrutamento o = new CampanhaRecrutamento();
            o = CampanhaRecrutamento.LoadObject(12);
            var url = PesquisaCurriculo.MontaURLSolrBuscaCVsCampanha(o);
            PesquisaCurriculo.BuscaCurriculosCampanha(url, 0, 20);*/
        }

        [TestMethod]
        public void CampanhaAssincronoTest()
        {
            var funcao = "Auxiliar Administrativo";
            var cidade = "Curitiba/PR";
            var salario = 1100;
            var idFilial = 75302;
            var idUsuario = 742338;

            var objCampanhaRecrutamento = new BLL.CampanhaRecrutamento
            {
                QuantidadeRetorno = 50,
                TipoRetornoCampanhaRecrutamento = new BLL.TipoRetornoCampanhaRecrutamento(Convert.ToInt16(BLL.Enumeradores.TipoRetornoCampanhaRecrutamento.InscricaoVaga)),
            };

            try
            {
                objCampanhaRecrutamento.Salvar(funcao, cidade, salario, new BLL.Filial(idFilial), new BLL.UsuarioFilialPerfil(idUsuario));
            }
            catch (Exception)
            {
            }

            Controller.GetControlerCapabilities += () => new Capabilities();
            Controller.GetPluginCatalog += () => new DirectoryCatalog(@"C:\tfs\tfs.employer.com.br\BNE\src\BNE-v.Prd\BNE.Services.Plugins\bin\Debug");
            Controller.InitializeController();
            Controller.StartController();

            new ManualResetEventSlim(false).Wait();

        }

            

        [TestMethod]
        public void TestarEnvioCandVagaPerfil()
        {
            Vaga vaga = new Vaga();
            vaga = Vaga.LoadObject(791483);


            var parametrosAtividade = new ParametroExecucaoCollection 
                                    {
                                        {"idVaga","Vaga", "791483", "791483"}
                                    };
            var objPlugin = new Services.Plugins.PluginsEntrada.EnvioCandidatoVagaPerfil();

            objPlugin.ExecuteTask(parametrosAtividade, null);


        }
        
    }
}
