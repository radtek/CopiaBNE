using BNE.BLL;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTestaQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //PublicacaoVagaNaoAuditada();
            //while (true)
            //{
            //    //GatilhoEmail();
            //    EnvioSMSTanque();
            //}
            
        }

        public static void PublicacaoVagaNaoAuditada()
        {
            DataTable lista = Vaga.ListarVagasSalaAdministrador("", true);
            foreach (DataRow vaga in lista.Rows)
            {

                var idVaga = Convert.ToInt32(vaga["Idf_Vaga"]);
                var CodigoVaga = vaga["Cod_Vaga"].ToString();

                var parametros = new ParametroExecucaoCollection()
                        {
                            {"idVaga", "Vaga", idVaga.ToString(CultureInfo.InvariantCulture), CodigoVaga},
                            {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                        };

                ProcessoAssincrono.IniciarAtividade(
                BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.PublicacaoVaga,
                BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                parametros,
                null,
                null,
                null,
                null,
                DateTime.Now);

            }
        }

        public static void GatilhoEmail()
        {
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = "1"
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = "3049359"
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPessoaFisica",
                                DesParametro = "IdPessoaFisica",
                                Valor = "5325930"
                            }
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

        public static void EnvioSMSTanque()
        {
            BNE.BLL.MensagemCS.EnviaSMSTanque(new Curriculo(3637632), PessoaFisica.LoadObject(5919044), null, null, "Teste de envio de mensagem", "41", "88385693", BNE.BLL.Enumeradores.UsuarioSistemaTanque.QuemMeViu);
        }
    }
}
