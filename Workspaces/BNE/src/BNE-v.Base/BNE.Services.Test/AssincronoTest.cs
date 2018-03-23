using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.Services.Test
{
    [TestClass]
    public class AssincronoTest
    {

        [TestMethod]
        public void AssincronoEnvioCandidatoVagaPerfilAtividade()
        {
            //var idCurriculo = "709029";
            var idVaga = "539554";

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idVaga",
                                DesParametro = "idVaga",
                                Valor = idVaga,
                                DesValor = "V00539554"
                            }
                        };

            ProcessoAssincrono.IniciarAtividade(
            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.EnvioCandidatoVagaPerfil,
            BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("EnvioCandidatoVagaPerfil", "PluginSaidaEmailSMSTanque"),
            parametros,
            null,
            null,
            null,
            null,
            DateTime.Now);
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

            ProcessoAssincrono.IniciarAtividade(
            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE,
            BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("GatilhosEmail", "PluginSaidaGatilhos"),
            parametros,
            null,
            null,
            null,
            null,
            DateTime.Now);
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

            ProcessoAssincrono.IniciarAtividade(
            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE,
            BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("GatilhosEmail", "PluginSaidaGatilhos"),
            parametros,
            null,
            null,
            null,
            null,
            DateTime.Now);
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

            ProcessoAssincrono.IniciarAtividade(
            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE,
            BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("GatilhosEmail", "PluginSaidaGatilhos"),
            parametros,
            null,
            null,
            null,
            null,
            DateTime.Now);
        }

        [TestMethod]
        public void InclusaoEmpresaTest()
        {
            var idFilial = "171369";

            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "idFilial",
                                DesParametro = "idFilial",
                                Valor = idFilial,
                                DesValor = idFilial
                            }
                        };

            ProcessoAssincrono.IniciarAtividade(
            BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.InclusaoEmpresa,
            BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("InclusaoEmpresa", "PluginSaidaEmailSMS"),
            parametros,
            null,
            null,
            null,
            null,
            DateTime.Now);
        }

    }
}
