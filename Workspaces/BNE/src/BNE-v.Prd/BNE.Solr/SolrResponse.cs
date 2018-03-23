using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BNE.Solr
{
    public abstract class Solr
    {
        public ResponseHeader responseHeader { get; set; }
        public Facet facet_counts { get; set; }

        #region Requisicao
        public static T Requisicao<T>(String url)
        {
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;

            Stream dataStream = null;
            StreamReader reader = null;
            var objRetorno = default(T);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = true;
                // Get the response.
                var response = request.GetResponse();
                dataStream = response.GetResponseStream();

                if (dataStream != null)
                {
                    reader = new StreamReader(dataStream);

                    objRetorno = (T)new JsonSerializer().Deserialize(reader, typeof(T));
                }
                
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, url);
            }
            finally
            {
                if (dataStream != null)
                    dataStream.Dispose();

                if (reader != null)
                    reader.Dispose();
            }
            return objRetorno;
        }
        #endregion

    }

    public class SolrResponse<T> : Solr //<T>
    {
        public Response<T> response { get; set; }

        #region EfetuarRequisicao
        public static SolrResponse<T> EfetuarRequisicao(String url)
        {
            return Requisicao<SolrResponse<T>>(url);
        }
        #endregion
    }

    public class SolrGroupedResponse<TM, T> : Solr where TM : Grouped<T>
    {
        public TM grouped { get; set; }

        #region EfetuarRequisicao
        public static SolrGroupedResponse<TM,T> EfetuarRequisicao(String url)
        {
            return Requisicao<SolrGroupedResponse<TM, T>>(url);
        }
        #endregion

    }


    public class ResponseHeader
    {
        public int status { get; set; }
        public int QTime { get; set; }
        [JsonProperty(PropertyName = "params")]
        public Parameters param { get; set; }
    }

    public class Parameters
    {
        public string q { get; set; }
        public string start { get; set; }
        public string rows { get; set; }
    }

    public class Facet
    {
        public Dictionary<String, List<object>> facet_fields { get; set; }
        public Dictionary<String, int> facet_queries { get; set; }
    }

    public class Facet_Queries
    {
        public int Idf_Sexo2 { get; set; }
    }

    public abstract class Grouped<T>
    {
    }

    public class GroupedPorIdfCurriculo<T> : Grouped<T>
    {
        public GroupsList<T> Idf_Curriculo { get; set; }
    }

    public class GroupsList<T>
    {
        public int matches { get; set; }
        public List<Matches<T>> groups { get; set; }
    }

    public class Matches<T>
    {
        public int groupValue { get; set; }
        public Response<T> doclist { get; set; }
    }

    public class Response<T>
    {
        public int numFound { get; set; }
        public int start { get; set; }
        public decimal maxScore { get; set; }
        public List<T> docs { get; set; }
    }

}