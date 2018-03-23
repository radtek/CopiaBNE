using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom.Solr
{
    public class BuscaVagaCandidato
    {
        #region EfetuarRequisicao
        //public static BuscaVagaCandidato EfetuarRequisicao(String url)
        //{
        //    Stream dataStream = null;

        //    try
        //    {
        //        var request = (HttpWebRequest)WebRequest.Create(url);
        //        request.KeepAlive = true;
        //        ResultadoBuscaVagaSolr objRetorno = null;
        //        // Get the response.
        //        var response = request.GetResponse();
        //        dataStream = response.GetResponseStream();

        //        if (dataStream != null)
        //        {
        //            var reader = new StreamReader(dataStream);

        //            objRetorno = (ResultadoBuscaVagaSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagaSolr));
        //        }
        //        return objRetorno;
        //    }
        //    catch (Exception ex)
        //    {
        //        EL.GerenciadorException.GravarExcecao(ex, url);
        //    }
        //    finally
        //    {
        //        if (dataStream != null)
        //            dataStream.Dispose();
        //    }
        //    return null;
        //}
        #endregion

        #region [MontaUrlSolrMinhasVagas]
        public static string MontaUrlSolrMinhasVagas(List<int> listaVaga)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrCandidatura));
            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start=0&rows=40000");

            sbFilters.AppendFormat("&fq=Idf_Vaga:({0})", string.Join(" OR ", listaVaga));
            sbFilters.AppendFormat("&fl=Dta_Visualizacao&fl=Idf_Vaga");
            return string.Concat(sbQuery, sbExtraParameters, sbFilters);
        }
        #endregion

        #region [MontaUrlSolr]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="IdfVaga"></param>
        /// <param name="Visualizados"></param>
        /// <returns></returns>
        public static string MontaUrlSolr(int currentIndex, int pageSize, int IdfVaga, bool? Visualizados, FiltroCurriculoVaga FiltrosCandidatos = null)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var SbFiltrosRange = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrCandidatura));
            //sbQuery.Append("?q={!boost=recip(ms(NOW,Dta_Cadastro),3.16e-11,1,1)}");

            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start={0}", ((currentIndex * pageSize))); //Primeiro registro
            sbExtraParameters.AppendFormat("&rows={0}", pageSize); //Número de registros retornados
            //sbExtraParameters.Append("&indent=true&facet=true");

            sbFilters.AppendFormat("&fq=Idf_Vaga:{0}", IdfVaga);
            if (Visualizados.HasValue && Visualizados.Value)//mostra apenas os visualizados na vaga
                sbFilters.Append($"&fq=Dta_Visualizacao:[*%20TO%20*]");
            else if (Visualizados.HasValue)
                sbFilters.Append($"&fq=!Dta_Visualizacao:[*%20TO%20*]");


            #region [Filtros candidatos na vaga]

            if (FiltrosCandidatos != null)
            {
                #region [Idade]

                if (FiltrosCandidatos.IdadeMinima.HasValue && FiltrosCandidatos.IdadeMaxima.HasValue)
                    SbFiltrosRange.Append($"v='Dta_Nascimento:[{DateTime.Today.AddYears(-FiltrosCandidatos.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z")} TO {DateTime.Today.AddYears(-FiltrosCandidatos.IdadeMinima.Value).ToString("yyyy-MM-ddT00:00:00.000Z")}] ");
                else
                {
                    if (FiltrosCandidatos.IdadeMinima.HasValue)
                        SbFiltrosRange.Append($"v='Dta_Nascimento:[* TO {DateTime.Today.AddYears(-FiltrosCandidatos.IdadeMinima.Value + 1).ToString("yyyy-MM-ddT00:00:00.000Z")}] ");

                    if (FiltrosCandidatos.IdadeMaxima.HasValue)
                        SbFiltrosRange.Append($"v='Dta_Nascimento:[{DateTime.Today.AddYears(-FiltrosCandidatos.IdadeMaxima.Value - 1).ToString("yyyy-MM-ddT00:00:00.000Z")} TO *] ");
                }
                #endregion

                #region [Salario]
                if (FiltrosCandidatos.SalarioMinimo.HasValue && FiltrosCandidatos.SalarioMaximo.HasValue)
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                    $"v='Vlr_Pretensao_Salarial:[{FiltrosCandidatos.SalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+{FiltrosCandidatos.SalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}] "
                                    : $" AND Vlr_Pretensao_Salarial:[{FiltrosCandidatos.SalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+{FiltrosCandidatos.SalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}] "
                                    );
                else
                {
                    if (FiltrosCandidatos.SalarioMinimo.HasValue)
                        SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                            $"v='Vlr_Pretensao_Salarial:[{FiltrosCandidatos.SalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+*] "
                            : $" AND Vlr_Pretensao_Salarial:[{FiltrosCandidatos.SalarioMinimo.Value.ToString("#.##", CultureInfo.InvariantCulture)}+TO+*] ");


                    if (FiltrosCandidatos.SalarioMaximo.HasValue)
                        SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                        $"v='Vlr_Pretensao_Salarial:[*+TO+{FiltrosCandidatos.SalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}] "
                                        : $" AND Vlr_Pretensao_Salarial:[*+TO+{FiltrosCandidatos.SalarioMaximo.Value.ToString("#.##", CultureInfo.InvariantCulture)}] ");
                }
                #endregion

                #region [Sexo]
                if (FiltrosCandidatos.Sexo.HasValue)
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                            $"v='Idf_Sexo:{FiltrosCandidatos.Sexo.Value} "
                                            : $" AND Idf_Sexo:{FiltrosCandidatos.Sexo.Value}");
                #endregion

                #region [Funcoes]
                var Funcoes = new List<int>();
                FiltrosCandidatos.ListFuncoes.ForEach(x => { Funcoes.Add(x.IdFuncao); });

                if (Funcoes.Count > 0)
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                                 $"v='Idf_Funcao:({string.Join(" OR ", Funcoes)} )"
                                                : $" AND Idf_Funcao:({string.Join(" OR ", Funcoes)}) ");
                #endregion

                #region [Cidades]
                var Cidade = new List<int>();
                FiltrosCandidatos.ListCidades.ForEach(x => { Cidade.Add(x.IdCidade); });

                if (Cidade.Count > 0)
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                            $"v='Idf_Cidade: ({string.Join(" OR ", Cidade)})"
                                            : $" AND Idf_Cidade: ({string.Join(" OR ", Cidade)})");
                #endregion

                #region [Formacao]

                var ListaFormacaoFiltro = new List<int>();
                FiltrosCandidatos.ListEscolaridade.ForEach(x => { ListaFormacaoFiltro.Add(x.IdEscolaridade); });

                if (ListaFormacaoFiltro.Count() > 0)
                    sbFilters.Append($"&fq={{!join%20from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv_formacao}}Idf_Escolaridade:({string.Join(" OR ", ListaFormacaoFiltro) })");

                #endregion

                #region [PalavraChave]
                if (!String.IsNullOrEmpty(FiltrosCandidatos.PalavraChave))
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?
                                         $" v='allsearch:{HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}"
                                        //OR Des_Bairro:{HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))} 
                                        //OR Eml_Pessoa: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))} 
                                        //OR Nme_Pessoa: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Logradouro: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Conhecimento: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Atividade_empresa: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Atividade: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Funcao_Exercida: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Raz_Social: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Disponibilidade: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Curso: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Grau_Escolaridade: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //OR Des_Fonte: {HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}
                                        //   "
                                        : $" AND allsearch:{HttpUtility.UrlEncode(FiltrosCandidatos.PalavraChave.Replace(",", " ").Replace(";", " "))}");

                #endregion

                #region [PCD]
                if (FiltrosCandidatos.PCD)
                {
                    SbFiltrosRange.Append(string.IsNullOrEmpty(SbFiltrosRange.ToString()) ?  $"v='Idf_Deficiencia: [1 TO 36] "
                                    : $" AND Idf_Deficiencia: [1 TO 36]");
                }
                #endregion

                #region [RespostasCorretasDasPerguntas]
                if (FiltrosCandidatos.RespostaCorreta)
                    sbFilters.Append($"&fq=!Flg_Resposta_Esperada:false&fq=Flg_Resposta_Esperada:true");
                #endregion

                if (!string.IsNullOrEmpty(SbFiltrosRange.ToString()))
                    sbFilters.Append($"&fq=({{!join%20from=Idf_Curriculo%20to=Idf_Curriculo%20fromIndex=cv {SbFiltrosRange}'}})");

             
            }
            #endregion

            sbFilters.Append("&sort=Perfil+desc%2CFlg_Vip+desc%2CDta_Cadastro+desc");

            return string.Concat(sbQuery, sbExtraParameters, sbFilters);
        }
        #endregion

        #region [MontaUrlSolrCandidatura]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="IdfVaga"></param>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static string MontaUrlSolrCandidatura(int currentIndex, int pageSize, int IdfVaga, int idCurriculo)
        {
            var sbFilters = new StringBuilder();
            var sbQuery = new StringBuilder();
            var SbFiltrosRange = new StringBuilder();
            var sbExtraParameters = new StringBuilder();

            sbQuery.Append(Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlSolrCandidatura));
            //sbQuery.Append("?q={!boost=recip(ms(NOW,Dta_Cadastro),3.16e-11,1,1)}");

            sbExtraParameters.Append("&wt=json"); //retorno JSON            
            sbExtraParameters.AppendFormat("&start={0}", ((currentIndex * pageSize))); //Primeiro registro
            sbExtraParameters.AppendFormat("&rows={0}", pageSize); //Número de registros retornados
            //sbExtraParameters.Append("&indent=true&facet=true");

            sbFilters.AppendFormat("&fq=Idf_Vaga:{0}&fq=Idf_Curriculo:{1}", IdfVaga, idCurriculo);

            return string.Concat(sbQuery, sbExtraParameters, sbFilters);
        }
        #endregion
    }
}
