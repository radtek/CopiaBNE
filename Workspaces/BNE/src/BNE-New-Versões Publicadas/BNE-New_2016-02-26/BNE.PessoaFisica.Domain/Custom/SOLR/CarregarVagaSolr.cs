using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
namespace BNE.PessoaFisica.Domain.Custom.SOLR
{
    public class CarregarVagaSolr
    {
        public static Command.Vaga CarregarVaga(string idVaga, string urlSLOR)
        {
            Command.Vaga vaga = null;

            //string urlSLOR = urlSolr; //"http://10.114.113.205:8983/solr/VagaBNE/select?q=id%3A";

            urlSLOR += idVaga;
            urlSLOR += "&fl=Des_Funcao,Idf_Funcao,Des_Atribuicoes,Des_Beneficio,Des_Requisito,Vlr_Salario_De,Vlr_Salario_Para,id,Cod_Vaga,Nme_Empresa,Flg_Auditada,Flg_Vaga_Arquivada,Flg_Inativo,Qtd_Vaga,Idf_Origem,Des_Origem,Idf_Cidade,Nme_Cidade,Sig_Estado,Des_Tipo_Vinculo,Idf_Tipo_Vinculo,Des_Area_BNE,Dta_Abertura,Des_Deficiencia,Flg_Deficiencia&wt=json&indent=true";

            WebRequest request = WebRequest.Create(urlSLOR);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            ResultadoBuscaVagaSolr resultado = (ResultadoBuscaVagaSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagaSolr));


            if (resultado.response.docs.Count > 0)
            {
                var document = resultado.response.docs[0];

                vaga = new Command.Vaga();

                vaga.IdVaga = document.id;
                vaga.IdCidade = document.Idf_Cidade;
                vaga.Funcao = document.Des_Funcao;
                vaga.SalarioDe = document.Vlr_Salario_De;
                vaga.SalarioAte = document.Vlr_Salario_Para;
                vaga.CodigoVaga = document.Cod_Vaga;
                vaga.DataAnuncio = document.Dta_Abertura;
                vaga.Atribuicoes = document.Des_Atribuicoes;
                vaga.Beneficios = document.Des_Beneficio;
                vaga.Requisitos = document.Des_Requisito;
                vaga.IdFuncao = document.Idf_Funcao;
                vaga.Cidade = document.Nme_Cidade;
                vaga.UF = document.Sig_Estado;
                vaga.FlgAuditada = document.Flg_Auditada;
                vaga.FlgArquivada = document.Flg_Vaga_Arquivada;
                vaga.FlgInativo = document.Flg_Inativo;
                vaga.NomeEmpresa = document.Nme_Empresa;
                vaga.Descricao = document.Des_Funcao;
                vaga.DescricaoTipoVinculo = document.Des_Tipo_Vinculo;
                vaga.DescricaoAreaBNEPesquisa = document.Des_Area_BNE;
                vaga.FlagDeficiencia = document.Flg_Deficiencia;

                if(document.Idf_Tipo_Vinculo != null)
                {
                    foreach (var item in document.Idf_Tipo_Vinculo)
                    {
                        switch (item)
                        {
                            case 4:
                                vaga.IdTipoVinculo = 4;
                                vaga.eEstagio = true;
                                vaga.eEfetivo = false;
                                break;
                            case 1:
                                vaga.IdTipoVinculo = 1;
                                vaga.eAprendiz = true;
                                break;
                            default:
                                vaga.IdTipoVinculo = 3;
                                vaga.eEfetivo = true;
                                break;
                        }
                    }
                }
                else
                {
                    vaga.IdTipoVinculo = 3;
                    vaga.eEfetivo = true;
                }

            }
            else
            {
                Mapper.Models.Vaga.Vaga vagaSQL = new Mapper.ToOld.PessoaFisica().CarregarVagaSQL(int.Parse(idVaga));

                if(vagaSQL != null)
                {
                    vaga = new Command.Vaga();

                    vaga.IdVaga = vagaSQL.IdVaga;
                    vaga.IdFuncao = vagaSQL.IdFuncao;
                    vaga.IdCidade = vagaSQL.IdCidade;

                    vaga.SalarioDe = vagaSQL.SalarioDe;
                    vaga.SalarioAte = vagaSQL.SalarioAte;
                    vaga.Cidade = vagaSQL.Cidade;
                    vaga.UF = vagaSQL.UF;
                    vaga.CodigoVaga = vagaSQL.CodigoVaga;
                    vaga.Atribuicoes = vagaSQL.Atribuicoes;
                    vaga.Beneficios = vagaSQL.Beneficios;
                    vaga.Requisitos = vagaSQL.Requisitos;
                    vaga.DataAnuncio = vagaSQL.DataAnuncio;
                    vaga.FlgAuditada = vagaSQL.FlgAuditada;
                    vaga.FlgArquivada = vagaSQL.FlgArquivada;
                    vaga.FlgInativo = vagaSQL.FlgInativo;
                    vaga.NomeEmpresa = vagaSQL.NomeEmpresa;
                    vaga.Descricao = vagaSQL.Descricao;
                    vaga.DescricaoTipoVinculo = vagaSQL.DescricaoTipoVinculo;
                    vaga.DescricaoAreaBNEPesquisa = vagaSQL.DescricaoAreaBNEPesquisa;
                    vaga.Funcao = vagaSQL.Funcao;
                    vaga.FlagDeficiencia = vagaSQL.FlgDeficiencia != null ? vagaSQL.FlgDeficiencia.Value : false;
                    vaga.DescricaoDeficiencia = vagaSQL.DescricaoDeficiencia;

                    vaga.eAprendiz = vagaSQL.eAprendiz;
                    vaga.eEfetivo = vagaSQL.eEfetivo;
                    vaga.eEstagio = vagaSQL.eEstagio;

                }
            }

            return vaga;
        }
    }
}
