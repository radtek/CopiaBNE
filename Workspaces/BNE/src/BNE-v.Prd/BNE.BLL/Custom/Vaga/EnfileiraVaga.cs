using BNE.Services.Base.ProcessosAssincronos;
using System.Globalization;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.BLL.Custom.Vaga
{
    public class EnfileiraVaga
    {
        public static void EnfileiraByVagaIntegracao(VagaIntegracao objVagaIntegracaoImportada, Origem oOrigem, Integrador oIntegrador)
        {

            //Tratamento salario
            string salarioDe = "";
            if (objVagaIntegracaoImportada.Vaga.ValorSalarioDe != null && objVagaIntegracaoImportada.Vaga.ValorSalarioDe > 0)
                salarioDe = objVagaIntegracaoImportada.Vaga.ValorSalarioDe.Value.ToString(new CultureInfo("pt-BR"));

            string salarioPara = "";
            if (objVagaIntegracaoImportada.Vaga.ValorSalarioPara != null && objVagaIntegracaoImportada.Vaga.ValorSalarioPara > 0)
                salarioPara = objVagaIntegracaoImportada.Vaga.ValorSalarioPara.Value.ToString(new CultureInfo("pt-BR"));

            var dtaAbertura = objVagaIntegracaoImportada.Vaga.DataAbertura.Value.ToString("dd/MM/yyyy HH:mm:ss");
            var dtaPrazo = objVagaIntegracaoImportada.Vaga.DataPrazo.Value.ToString("dd/MM/yyyy HH:mm:ss");

            var parametros = new ParametroExecucaoCollection
                    {
                        //Campos Gerais
                        {"codigo","codigo", objVagaIntegracaoImportada.CodigoVagaIntegrador, objVagaIntegracaoImportada.CodigoVagaIntegrador},
                                        
                        //Campos VagaIntegracao
                        {"vagaIntegracao_vagaParaDeficiente","vagaIntegracao_vagaParaDeficiente", objVagaIntegracaoImportada.VagaParaDeficiente.ToString(), objVagaIntegracaoImportada.VagaParaDeficiente.ToString()},
                                        

                        //Campos Vaga
                        {"email","email",objVagaIntegracaoImportada.Vaga.EmailVaga,objVagaIntegracaoImportada.Vaga.EmailVaga},
                        {"desCidade","desCidade",objVagaIntegracaoImportada.CidadeImportada,objVagaIntegracaoImportada.CidadeImportada},
                        {"dataAbertura","dataAbertura",dtaAbertura,dtaAbertura},
                        {"dataPrazo","dataPrazo", dtaPrazo, dtaPrazo},
                        {"desFuncao","desFuncao",objVagaIntegracaoImportada.FuncaoImportada,objVagaIntegracaoImportada.FuncaoImportada},
                        {"desEscolaridade","desEscolaridade",objVagaIntegracaoImportada.EscolaridadeImportada,objVagaIntegracaoImportada.EscolaridadeImportada},
                        {"desDeficiencia","desDeficiencia",objVagaIntegracaoImportada.DeficienciaImportada,objVagaIntegracaoImportada.DeficienciaImportada},
                        {"descricao","descricao",objVagaIntegracaoImportada.Vaga.DescricaoAtribuicoes,objVagaIntegracaoImportada.Vaga.DescricaoAtribuicoes},
                        {"empresa","empresa",objVagaIntegracaoImportada.Vaga.NomeEmpresa,objVagaIntegracaoImportada.Vaga.NomeEmpresa},
                        {"disponibilidades","disponibilidades",objVagaIntegracaoImportada.Vaga.DescricaoDisponibilidades,objVagaIntegracaoImportada.Vaga.DescricaoDisponibilidades},
                        {"tiposVinculo","tiposVinculo",objVagaIntegracaoImportada.Vaga.DescricaoTiposVinculo,objVagaIntegracaoImportada.Vaga.DescricaoTiposVinculo},
                        {"qtdeVaga","qtdeVaga",objVagaIntegracaoImportada.Vaga.QuantidadeVaga.ToString(),objVagaIntegracaoImportada.Vaga.QuantidadeVaga.ToString()},
                        {"OrigemImportacao","OrigemImportacao", oOrigem.IdOrigem.ToString(),oOrigem.IdOrigem.ToString()},
                        {"Integrador","Integrador", oIntegrador.IdIntegrador.ToString(),oIntegrador.IdIntegrador.ToString()},
                        {"vlrSalarioDe","vlrSalarioDe", salarioDe , salarioDe},
                        {"vlrSalarioPara","vlrSalarioPara", salarioPara, salarioPara}
                    };

            ProcessoAssincrono.IniciarAtividade(TipoAtividade.IntegracaoVaga, parametros);


        }

        public static void EnfileiraParaInativar(string idVagaSine, Integrador oIntegrador, string oportunidade)
        {
            var parametros = new ParametroExecucaoCollection
                    {
                        {"idVagaSine","idVagaSine", idVagaSine, idVagaSine},
                        {"Integrador","Integrador", oIntegrador.IdIntegrador.ToString(),oIntegrador.IdIntegrador.ToString()},
                        {"Oportunidade","Oportunidade", oportunidade,oportunidade}
                    };

            ProcessoAssincrono.IniciarAtividade(TipoAtividade.InativacaoVaga, parametros);
        }
    }
}
