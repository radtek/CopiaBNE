using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using SolrNet.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WhatsJob.Domain
{
    public class Util
    {
        static Regex reNumberOfFrom = new Regex("^[0-9]+");
        static bool solrInit = false;

        //private void InitializeSolr<T>(String Handler)
        //{
        //    #region Configuração de Custom Handlers
        //    var container = ServiceLocator.Current as Container;
        //    if (container != null)
        //    {
        //        container.Remove<ISolrQueryExecuter<T>>();

        //        var instanceCidade = new SolrQueryExecuter<T>(container.GetInstance<ISolrAbstractResponseParser<T>>(), 
        //            connectionCidade, 
        //            container.GetInstance<ISolrQuerySerializer>(), 
        //            container.GetInstance<ISolrFacetQuerySerializer>(), 
        //            container.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<T>>())
        //        {
        //            Handler = Handler
        //        };

        //        container.Register<ISolrQueryExecuter<T>>(typeof(ISolrQueryExecuter<T>).FullName, c => instanceCidade);
        //    }
        //    #endregion
        //}

        /// <summary>
        /// Get the phone number of "from" string received from whatsapp server.
        /// </summary>
        /// <param name="from">"from" string received from whatsapp server</param>
        /// <returns>Phone Number</returns>
        public static string GetNumberOfFrom(String from)
        {
            if (!reNumberOfFrom.IsMatch(from))
                return null;

            return reNumberOfFrom.Match(from).Value;
        }

        public static List<DTO.VagaSine> GetVagasSine(string pesquisa)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.sine.com.br/api/v1.0/Job/List");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = client.GetAsync("?idFuncao=0&idCidade=0&pagina=1&pesquisa=" + System.Web.HttpUtility.UrlEncode(pesquisa)).Result;
                if (response.IsSuccessStatusCode)
                {
                    String json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<DTO.VagaSine>>(json);
                }
            }

            return null;
        }
        
        public static List<DTO.VagaSolrBNE> GetVagasBne(string pesquisa)
        {
            if (!solrInit)
            {
                //Startup.Init<Domain.DTO.VagaSolrBNE>("http://10.114.113.205:8983/solr/VagaBNE");
                //solrInit = true;
                //var executor = ServiceLocator.Current.GetInstance<ISolrQueryExecuter<Domain.DTO.VagaSolrBNE>>() as SolrQueryExecuter<Domain.DTO.VagaSolrBNE>;
                //executor.Handler = "/browse";

                Startup.Init<Domain.DTO.VagaSolrBNE>("http://10.114.113.205:8983/solr/VagaBNE");

                var container = ServiceLocator.Current as Container;
                if (container != null)
                {
                    container.Remove<ISolrQueryExecuter<Domain.DTO.VagaSolrBNE>>();

                    var instanceCidade = new SolrQueryExecuter<Domain.DTO.VagaSolrBNE>(container.GetInstance<ISolrAbstractResponseParser<Domain.DTO.VagaSolrBNE>>(), 
                        new SolrConnection("http://10.114.113.205:8983/solr/VagaBNE"), 
                        container.GetInstance<ISolrQuerySerializer>(), 
                        container.GetInstance<ISolrFacetQuerySerializer>(), 
                        container.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<Domain.DTO.VagaSolrBNE>>())
                    {
                        Handler = "/browse"
                    };

                    container.Register<ISolrQueryExecuter<Domain.DTO.VagaSolrBNE>>(typeof(ISolrQueryExecuter<Domain.DTO.VagaSolrBNE>).FullName, c => instanceCidade);
                    solrInit = true;
                }

                //var container = ServiceLocator.Current as SolrNet.Utils.Container;

                //container.Remove<ISolrQueryExecuter<Domain.DTO.VagaSolrBNE>>();

                //var instance = new SolrQueryExecuter<Domain.DTO.VagaSolrBNE>(container.GetInstance<ISolrAbstractResponseParser<Domain.DTO.VagaSolrBNE>>(),
                //    new SolrConnection("http://10.114.113.205:8983/solr/VagaBNE"),
                //    container.GetInstance<ISolrQuerySerializer>(),
                //    container.GetInstance<ISolrFacetQuerySerializer>(),
                //    container.GetInstance<ISolrMoreLikeThisHandlerQueryResultsParser<Domain.DTO.VagaSolrBNE>>());
                //instance.Handler = "/browse";
            }

            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<Domain.DTO.VagaSolrBNE>>();

            var qo = new QueryOptions();
            qo.Rows = 100;
            //qo.ExtraParams.


            var ret = solr.Query(new SolrQuery(pesquisa), qo);

            return ret;
        }

        public static string GetLinkVagasBNE(string cidade, string funcao)
        {
            if (!String.IsNullOrEmpty(cidade) && !String.IsNullOrEmpty(funcao))
                return String.Format("http://www.bne.com.br/vagas-de-emprego-para-{0}-em-{1}", NormalizeStringToUrl(funcao), NormalizeStringToUrl(cidade));

            if (!String.IsNullOrEmpty(cidade))
                return String.Format("http://www.bne.com.br/vagas-de-emprego-em-{0}", NormalizeStringToUrl(cidade));

            if (!String.IsNullOrEmpty(funcao))
                return String.Format("http://www.bne.com.br/vagas-de-emprego-para-{0}", NormalizeStringToUrl(funcao));

            return "http://www.bne.com.br/vagas-de-emprego";
        }

        public static string NormalizeStringToUrl(string s)
        {
            return Regex.Replace(RemoveAccents(s), "[ /]", "-").ToLower();
        }

        public static Regex GetAccentInsensitiveRegex(string input)
        {
            input = input.ToLower();

            string normalized = input.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();
            List<char> vowals = new List<char>() { 'a', 'e', 'i', 'o', 'u' };
            List<char> accentChars =
                new List<char>() { 
                    (char)768, //`
                    (char)769, //´
                    (char)770, //^
                    (char)771, //~
                    (char)776  //¨
                };

            foreach (char ch in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                {
                    if (vowals.Contains(ch))
                    {
                        builder.Append('[');
                        foreach (var accent in accentChars)
                        {
                            builder.Append(ch);
                            builder.Append(accent);
                        }
                        builder.Append(ch);
                        builder.Append(']');
                        continue;
                    }
                    if (ch == 'c')
                    {
                        builder.Append('[');
                        builder.Append(ch);
                        builder.Append(ch);
                        builder.Append((char)807); //ç
                        builder.Append(']');
                        continue;
                    }
                    builder.Append(ch);
                }
            }

            return new Regex(builder.ToString().Normalize(NormalizationForm.FormC), RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static string RemoveAccents(string text)
        {
            string s = text.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }
            return sb.ToString();
        }
    }
}
