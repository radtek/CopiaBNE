using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using BNE.PessoaFisica.Domain.ValueObjects;
using Newtonsoft.Json;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class VagaRepository : IVagaRepository
    {
        //Cache job for X minutes
        private const int CacheJobMinutes = 15;

        private readonly Cache.ICachingService _cache;

        private readonly Parametro _parametro;

        public VagaRepository(Parametro parametro, Cache.ICachingService cache)
        {
            _parametro = parametro;
            _cache = cache;
        }

        public Vaga RecuperarVaga(int idVaga)
        {
            var urlSolr = _parametro.RecuperarValor(Domain.Enumeradores.Parametro.UrlVagasSolr);

            urlSolr += idVaga;
            urlSolr += "&fl=Des_Funcao,Idf_Funcao,Des_Atribuicoes,Des_Beneficio,Des_Requisito,Vlr_Salario_De,Vlr_Salario_Para,id,Cod_Vaga,Nme_Empresa,Flg_Auditada,Flg_Vaga_Arquivada,Flg_Inativo,Qtd_Vaga,Idf_Origem,Des_Origem,Idf_Cidade,Nme_Cidade,Sig_Estado,Des_Tipo_Vinculo,Idf_Tipo_Vinculo,Des_Area_BNE,Dta_Abertura,Des_Deficiencia,Flg_Deficiencia,Flg_Vaga_Premium,Nme_Bairro,Idf_Deficiencia&wt=json&indent=true";

            var job = _cache.GetItem<Vaga>(urlSolr);
            if (job == null)
            {
                var request = WebRequest.Create(urlSolr);
                // Get the response.
                var response = request.GetResponse();
                // Get the stream containing content returned by the server.
                var dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);

                    var resultado = (ResultadoBuscaVagaSolr) new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagaSolr));


                    if (resultado.response.docs.Count > 0)
                    {
                        var document = resultado.response.docs[0];

                        job = new Vaga
                        {
                            IdVaga = document.id,
                            IdCidade = document.Idf_Cidade,
                            Funcao = document.Des_Funcao,
                            SalarioDe = document.Vlr_Salario_De,
                            SalarioAte = document.Vlr_Salario_Para,
                            CodigoVaga = document.Cod_Vaga,
                            DataAnuncio = document.Dta_Abertura,
                            Atribuicoes = document.Des_Atribuicoes,
                            Beneficios = document.Des_Beneficio,
                            Requisitos = document.Des_Requisito,
                            IdFuncao = document.Idf_Funcao,
                            Cidade = document.Nme_Cidade,
                            UF = document.Sig_Estado,
                            FlgAuditada = document.Flg_Auditada,
                            FlgArquivada = document.Flg_Vaga_Arquivada,
                            FlgInativo = document.Flg_Inativo,
                            NomeEmpresa = document.Nme_Empresa,
                            Descricao = document.Des_Funcao,
                            DescricaoTipoVinculo = document.Des_Tipo_Vinculo,
                            DescricaoAreaBNEPesquisa = document.Des_Area_BNE,
                            FlagDeficiencia = document.Flg_Deficiencia,
                            DescricaoDeficiencia = document.Des_Deficiencia,
                            Idf_Deficiencia = document.Idf_Deficiencia,
                            Bairro = document.Nme_Bairro != null ? Mapper.ToOld.PessoaFisica.AjustarString(document.Nme_Bairro) : string.Empty,
                            FlgPremium = !document.Flg_Vaga_Arquivada && document.Flg_Vaga_Premium,
                            FlgConfidencial = document.Flg_Confidencial
                        };


                        if (document.Idf_Tipo_Vinculo != null)
                        {
                            foreach (var item in document.Idf_Tipo_Vinculo)
                                switch (item)
                                {
                                    case 4:
                                        job.IdTipoVinculo = 4;
                                        job.eEstagio = true;
                                        job.eEfetivo = false;
                                        break;
                                    case 1:
                                        job.IdTipoVinculo = 1;
                                        job.eAprendiz = true;
                                        break;
                                    default:
                                        job.IdTipoVinculo = 3;
                                        job.eEfetivo = true;
                                        break;
                                }
                        }
                        else
                        {
                            job.IdTipoVinculo = 3;
                            job.eEfetivo = true;
                        }
                    }
                    else
                    {
                        var vagaSQL = new Mapper.ToOld.PessoaFisica().CarregarVagaSQL(idVaga);

                        if (vagaSQL != null)
                            job = new Vaga
                            {
                                IdVaga = vagaSQL.IdVaga,
                                IdFuncao = vagaSQL.IdFuncao,
                                IdCidade = vagaSQL.IdCidade,
                                SalarioDe = vagaSQL.SalarioDe,
                                SalarioAte = vagaSQL.SalarioAte,
                                Cidade = vagaSQL.Cidade,
                                UF = vagaSQL.UF,
                                CodigoVaga = vagaSQL.CodigoVaga,
                                Atribuicoes = vagaSQL.Atribuicoes,
                                Beneficios = vagaSQL.Beneficios,
                                Requisitos = vagaSQL.Requisitos,
                                DataAnuncio = vagaSQL.DataAnuncio,
                                FlgAuditada = vagaSQL.FlgAuditada,
                                FlgArquivada = vagaSQL.FlgArquivada,
                                FlgInativo = vagaSQL.FlgInativo,
                                FlgConfidencial = vagaSQL.FlgConfidencial,
                                NomeEmpresa = vagaSQL.NomeEmpresa,
                                Descricao = vagaSQL.Descricao,
                                DescricaoTipoVinculo = vagaSQL.DescricaoTipoVinculo,
                                DescricaoAreaBNEPesquisa = vagaSQL.DescricaoAreaBNEPesquisa,
                                Funcao = vagaSQL.Funcao,
                                DescricaoDeficiencia = vagaSQL.DescricaoDeficiencia,
                                Idf_Deficiencia = vagaSQL.Idf_Deficiencia,
                                eAprendiz = vagaSQL.eAprendiz,
                                eEfetivo = vagaSQL.eEfetivo,
                                eEstagio = vagaSQL.eEstagio,
                                FlgPremium = vagaSQL.FlgPremium,
                                Bairro = vagaSQL.Bairro
                            };
                    }
                }

                if (job != null)
                    job.LinkPaginasSemelhantes = GetLinksPaginasSemelhantes(job.Funcao.Replace("#", "sharp"), job.Cidade, job.UF, job.DescricaoAreaBNEPesquisa);

                _cache.AddItem(urlSolr, job, TimeSpan.FromMinutes(CacheJobMinutes));
            }

            return job;
        }

        public IEnumerable<string> GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            return new Mapper.ToOld.PessoaFisica().GetLinksPaginasSemelhantes(funcao, cidade, siglaEstado, areaBNE);
        }
    }
}