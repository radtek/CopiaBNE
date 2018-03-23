using API.Gateway.Auth;
using API.Gateway.DTO;
using API.Gateway.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;
using WebApiThrottle;
using API.Gateway.Extensions;

namespace API.Gateway.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProxyController : BaseApiController
    {
        [Authorize]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string VersionAPI, string api, string RemoteController, string RemoteAction)
        {
            return AnalysisAndExecute(VersionAPI, api, RemoteController, RemoteAction, "GET", null, Request.GetQueryNameValuePairs());
        }

        [Authorize]
        public HttpResponseMessage Post(string VersionAPI, string api, string RemoteController, string RemoteAction, [FromBody] object args)
        {
            return AnalysisAndExecute(VersionAPI, api, RemoteController, RemoteAction, "POST", args);
        }


        private HttpResponseMessage AnalysisAndExecute(string VersionAPI, string api, string RemoteController, string RemoteAction, string Method = "GET", object Args = null, IEnumerable<KeyValuePair<string, string>> Parms = null)
        {
            #region Obtem as claims do chave de acesso
            var claim_keys = CustomClaim.Build((ClaimsIdentity)User.Identity);
            decimal num_cpf = (decimal)claim_keys["Num_CPF"];
            DateTime dta_nascimento = (DateTime)claim_keys["Dta_Nascimento"];
            int idf_perfil = (int)claim_keys["Idf_Perfil"];
            int idf_perfil_usuario = (int)claim_keys["Idf_Perfil_Usuario"];
            DateTime dta_inicio_plano = (DateTime)claim_keys["Dta_Inicio_Plano"];
            string Idf_Cliente = claim_keys["Idf_Cliente"].ToString();
            #endregion

            Quota quota;
            long total_semanal = 0, total_mensal = 0, total = 0;
            using (var dbo = new APIGatewayContext())
            {
                #region Controle de Acesso e Quotas
                quota = (from qt in dbo.Quota.Include("Endpoint").AsNoTracking()
                         join ep in dbo.Endpoint.Include("WebApi").AsNoTracking() on qt.Idf_Endpoint equals ep.Idf_Endpoint
                         where ep.VersionAPI == VersionAPI && ep.Nme_Api == api && ep.Controller == RemoteController && ep.Action == RemoteAction
                         && ep.Flg_Inativo == false && qt.Idf_Perfil == idf_perfil
                         select qt).FirstOrDefault();

                if (quota != null)
                {
                    DateTime date = DateTime.Now;
                    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);

                    DateTime comeco_mes = (dta_inicio_plano > firstDayOfMonth) ? dta_inicio_plano : firstDayOfMonth;
                    DateTime comeco_sem = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);

                    total_semanal = (from r in dbo.Requisicao.AsNoTracking()
                                     where r.Idf_Usuario == idf_perfil_usuario && r.Idf_Endpoint == quota.Endpoint.Idf_Endpoint && r.Dta_Cadastro >= comeco_sem  && r.Codigo_Respsota >= 200 && r.Codigo_Respsota < 300
                                    select r).Count();

                    total_mensal = (from r in dbo.Requisicao.AsNoTracking()
                                    where r.Idf_Usuario == idf_perfil_usuario && r.Idf_Endpoint == quota.Endpoint.Idf_Endpoint && r.Dta_Cadastro >= comeco_mes && r.Codigo_Respsota >= 200 && r.Codigo_Respsota < 300
                                    select r).Count();

                    total =         (from r in dbo.Requisicao.AsNoTracking()
                                     where r.Idf_Usuario == idf_perfil_usuario && r.Idf_Endpoint == quota.Endpoint.Idf_Endpoint && r.Dta_Cadastro >= dta_inicio_plano && r.Codigo_Respsota >= 200 
                                        && r.Codigo_Respsota < 300 select r).Count();
                }
                else
                {
                    return Request.CreateResponse<string>(HttpStatusCode.Forbidden, string.Format("Endpoint informado não existe ou o perfil de usuário não tem acesso.", api));
                }


                if (total >= quota.Total_Limit)
                    return Request.CreateResponse<string>(HttpStatusCode.ServiceUnavailable,
                        string.Format("O Limite de {0} requisições foi atingido.", quota.Total_Limit));
                
                if(total_mensal >= quota.Per_Month)
                    return Request.CreateResponse<string>(HttpStatusCode.ServiceUnavailable,
                       string.Format("O Limite de {0} requisições por mês foi atingido.", quota.Per_Month));

                if (total_semanal >= quota.Per_Week)
                    return Request.CreateResponse<string>(HttpStatusCode.ServiceUnavailable,
                       string.Format("O Limite de {0} requisições por semana foi atingido.", quota.Per_Week));
                    
                #endregion

                #region Faz a requisição e salva a resposta 
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Requisicao req = new Requisicao();
                try
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    if (Parms != null)
                    {
                        foreach (var qItem in Parms)
                            if (!qItem.Key.Equals("key")) query[qItem.Key] = qItem.Value;
                    }

                    HttpResponseMessage exec = ExecuteRemote(claim_keys, VersionAPI, quota.Endpoint.WebApi, RemoteController, RemoteAction, Method, Args, query.ToString());

                    req.Idf_Usuario = idf_perfil_usuario;
                    req.Idf_Endpoint = quota.Endpoint.Idf_Endpoint;
                    req.Idf_Metodo = dbo.Metodo.Where(m => m.Des_Metodo == Method).FirstOrDefault().Idf_Metodo;

                    if(req.Idf_Metodo == 1)
                        req.Conteudo = query.ToString();
                    else
                        if (Args != null) req.Conteudo = (Args.GetType().Name == "JArray" || Args.GetType().Name == "JObject") ? Args.ToString() : "";

                    req.Codigo_Respsota = (int)exec.StatusCode;
                    req.Dta_Cadastro = DateTime.Now;
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    req.Tempo_Execucao = ts.TotalMilliseconds;
                    req.Idf_Cliente = new Guid(Idf_Cliente);
                    dbo.Requisicao.Add(req);
                    dbo.SaveChanges();

                    return exec;
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse<RemoteResult>(HttpStatusCode.InternalServerError, new RemoteResult(null, string.Format("Erro severo: {0}", ex.Message)));
                }
                #endregion
            }
        }


        private HttpResponseMessage ExecuteRemote(Dictionary<string, object> claims, string VersionAPI, WebApi objAPI, string RemoteController, string RemoteAction, string Method = "GET", object Args = null, string Parms = "")
        {
            RemoteResult result = null;

            #region Chamada a API de destino
            var httpHandler = new HttpClientHandler() { UseDefaultCredentials = true, UseProxy = true };

            using (var client = new HttpClient(httpHandler))
            {

                client.BaseAddress = new Uri(string.Format("http://{0}/", objAPI.Location));
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Add("Num_CPF", claims["Num_CPF"].ToString());
                client.DefaultRequestHeaders.Add("Idf_Filial", claims["Idf_Filial"].ToString());

                HttpResponseMessage response;

                try
                {
                    if (Method.Equals("POST"))
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        response = client.PostAsJsonAsync<object>(string.Format("{0}/{1}/{2}", VersionAPI, RemoteController, RemoteAction), Args).Result;
                    }
                    else
                    {
                        if (Parms.Equals(""))
                            response = client.GetAsync(string.Format("{0}/{1}/{2}", VersionAPI, RemoteController, RemoteAction)).Result;
                        else
                            response = client.GetAsync(string.Format("{0}/{1}/{2}?{3}", VersionAPI, RemoteController, RemoteAction, Parms)).Result;
                    }
                }
                catch (Exception ex)
                {
                    result = new RemoteResult(null, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
                    return Request.CreateResponse<RemoteResult>(HttpStatusCode.InternalServerError, result);
                }


                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                object objResult = (object)json_serializer.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                return Request.CreateResponse<object>(response.StatusCode, objResult);

            }
            #endregion
        }


    }
}
