using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.AsyncServices;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.BLL.Custom.Vaga
{
    public class EnfileiraVaga
    {
        public static void EnfileiraByVagaIntegracao(VagaIntegracao objVagaIntegracaoImportada, Origem oOrigem, Integrador oIntegrador)
        {
            var parametros = new ParametroExecucaoCollection
                    {
                        //Campos Gerais
                        {"codigo","codigo", objVagaIntegracaoImportada.CodigoVagaIntegrador, objVagaIntegracaoImportada.CodigoVagaIntegrador},
                                        
                        //Campos VagaIntegracao
                        {"vagaIntegracao_vagaParaDeficiente","vagaIntegracao_vagaParaDeficiente", objVagaIntegracaoImportada.VagaParaDeficiente.ToString(), objVagaIntegracaoImportada.VagaParaDeficiente.ToString()},
                                        
                        //Campos Vaga
                        {"email","email",objVagaIntegracaoImportada.Vaga.EmailVaga,objVagaIntegracaoImportada.Vaga.EmailVaga},
                        {"desCidade","desCidade",objVagaIntegracaoImportada.CidadeImportada,objVagaIntegracaoImportada.CidadeImportada},
                        {"dataAbertura","dataAbertura",objVagaIntegracaoImportada.Vaga.DataAbertura.ToString(),objVagaIntegracaoImportada.Vaga.DataAbertura.ToString()},
                        {"dataPrazo","dataPrazo",objVagaIntegracaoImportada.Vaga.DataPrazo.ToString(),objVagaIntegracaoImportada.Vaga.DataAbertura.ToString()},
                        {"desFuncao","desFuncao",objVagaIntegracaoImportada.FuncaoImportada,objVagaIntegracaoImportada.FuncaoImportada},
                        {"desEscolaridade","desEscolaridade",objVagaIntegracaoImportada.EscolaridadeImportada,objVagaIntegracaoImportada.EscolaridadeImportada},
                        {"desDeficiencia","desDeficiencia",objVagaIntegracaoImportada.DeficienciaImportada,objVagaIntegracaoImportada.DeficienciaImportada},
                        {"descricao","descricao",objVagaIntegracaoImportada.Vaga.DescricaoAtribuicoes,objVagaIntegracaoImportada.Vaga.DescricaoAtribuicoes},
                        {"empresa","empresa",objVagaIntegracaoImportada.Vaga.NomeEmpresa,objVagaIntegracaoImportada.Vaga.NomeEmpresa},
                        {"disponibilidades","disponibilidades",objVagaIntegracaoImportada.Vaga.DescricaoDisponibilidades,objVagaIntegracaoImportada.Vaga.DescricaoDisponibilidades},
                        {"tiposVinculo","tiposVinculo",objVagaIntegracaoImportada.Vaga.DescricaoTiposVinculo,objVagaIntegracaoImportada.Vaga.DescricaoTiposVinculo},
                        {"qtdeVaga","qtdeVaga",objVagaIntegracaoImportada.Vaga.QuantidadeVaga.ToString(),objVagaIntegracaoImportada.Vaga.QuantidadeVaga.ToString()},
                        {"OrigemImportacao","OrigemImportacao", oOrigem.IdOrigem.ToString(),oOrigem.IdOrigem.ToString()},
                        {"Integrador","Integrador", oIntegrador.IdIntegrador.ToString(),oIntegrador.IdIntegrador.ToString()}
                    };

            ProcessoAssincrono.IniciarAtividade(
                TipoAtividade.IntegracaoVaga,
                PluginsCompatibilidade.CarregarPorMetadata("IntegracaoVaga", "PluginSaidaPublicacaoVaga"),
                parametros,
                null,
                null,
                null,
                "",
                DateTime.Now);


        }

        public static void EnfileiraParaInativar(string idVagaSine, Integrador oIntegrador)
        {
            var parametros = new ParametroExecucaoCollection
                    {
                        {"idVagaSine","idVagaSine", idVagaSine, idVagaSine},
                        {"Integrador","Integrador", oIntegrador.IdIntegrador.ToString(),oIntegrador.IdIntegrador.ToString()}
                    };

            ProcessoAssincrono.IniciarAtividade(
                TipoAtividade.InativacaoVaga,
                PluginsCompatibilidade.CarregarPorMetadata("InativacaoVaga", "PluginSaidaPublicacaoVaga"),
                parametros,
                null,
                null,
                null,
                "",
                DateTime.Now);
        }
    }
}
