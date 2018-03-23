using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace BNE.BLL.Custom.Solr
{
    public class Vaga
    {

        #region EfetuarRequisicao
        public static ResultadoBuscaVagaSolr EfetuarRequisicao(String url)
        {
            Stream dataStream = null;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = true;
                ResultadoBuscaVagaSolr objRetorno = null;
                // Get the response.
                var response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);

                    objRetorno = (ResultadoBuscaVagaSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagaSolr));
                }
                return objRetorno;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, url);
            }
            finally
            {
                if (dataStream != null)
                    dataStream.Dispose();
            }
            return null;
        }
        #endregion

        public static int GetCount(Dictionary<string, string> parametros = null)
        {
            String urlSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlVagasSolr);
            urlSolr += "?wt=json"; //retorno JSON
            urlSolr += "&rows=0"; //Não busca vagas. Somente o total
            urlSolr += BuildQuery(parametros);

            ResultadoBuscaVagaSolr resultado = EfetuarRequisicao(urlSolr);

            return resultado.response.numFound;
        }

        public static Dictionary<string, int> GetFacets(String facetField, Dictionary<string, string> parametros = null, bool shortByCount = true, int limit = -1)
        {
            String urlSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlVagasSolr);
            urlSolr += "?wt=json"; //retorno JSON
            urlSolr += "&rows=0"; //Não busca vagas. Somente o total
            urlSolr += "&facet=true"; //Configurando Facets: ordenando pelo count do campo Idf_Funcao_Sinonimo
            urlSolr += "&fq=Flg_Oferece_Cursos:false";//não contar vagas que oferencem curso, elas só aparecem dentro do stc da empresa.
            urlSolr += "&facet.query=facet.sort%3D" + (shortByCount ? "count" : "index"); //Configurando Facets: ordenando pelo count do campo Idf_Funcao_Sinonimo
            urlSolr += "&facet.field=" + facetField; //Configurando Facets: ordenando pelo count do campo Idf_Funcao_Sinonimo
            urlSolr += "&facet.limit=" + limit; //Definindo limite do retorno
            urlSolr += BuildQuery(parametros);

            ResultadoBuscaVagaSolr resultado = EfetuarRequisicao(urlSolr);

            return TrataFacets(facetField, resultado);
        }

        private static string BuildQuery(Dictionary<string, string> parametros)
        {
            //Montando Query
            String query = String.Empty;
            if (parametros != null)
            {
                foreach (var p in parametros)
                {
                    query += !String.IsNullOrEmpty(query) ? " AND " : String.Empty;
                    query += String.Format("{0}:\"{1}\"", p.Key, p.Value);
                }
            }

            query = String.IsNullOrEmpty(query) ? "*:*" : query; //Se nenhuma pesquisa foi feita, 
            query = "&q=" + HttpUtility.UrlEncode(query);

            return query;
        }

        public static Dictionary<string, int> TrataFacets(string facetName, ResultadoBuscaVagaSolr resultado)
        {
            Dictionary<string, int> retorno = new Dictionary<string, int>();
            List<object> lstFacets;
            if (resultado.facet_counts == null ||
                resultado.facet_counts.facet_fields == null ||
                !resultado.facet_counts.facet_fields.TryGetValue(facetName, out lstFacets))
                throw new Exception(String.Format("Facet '{0}' não presente no resultado", facetName));

            for (int i = 0; i < lstFacets.Count; i = i + 1)
            {
                if (lstFacets[i] == null)
                    continue;

                //Descrição da função
                if (lstFacets[i].GetType() != typeof(string))
                    continue;

                var key = lstFacets[i].ToString();

                //Buscando count, presente na posição sucessora a descricao
                var j = i + 1;
                if (lstFacets.Count <= j && lstFacets[j].GetType() != typeof(int))
                    continue;

                retorno.Add(key, Convert.ToInt32(lstFacets[j]));
            }

            return retorno;
        }
        
    }
}
