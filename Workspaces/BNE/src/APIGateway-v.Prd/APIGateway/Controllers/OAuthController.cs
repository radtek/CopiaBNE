using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BNE.Core.ExtensionsMethods;
using Newtonsoft.Json;
using System.Web;

namespace APIGateway.Controllers
{
    public class OAuthController : ApisController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="DataNascimento"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Token()
        {
            string SecretKey = Request.GetRouteData().Route.DataTokens["SecretKey"].ToString();
            string UrlSuffix = Request.GetRouteData().Route.DataTokens["UrlSuffix"].ToString();
            string BaseUrl = Request.GetRouteData().Route.DataTokens["BaseUrl"].ToString();
            string AuthenticationEndpoint = Request.GetRouteData().Route.DataTokens["AuthenticationEndpoint"].ToString();
            int Expiration = Convert.ToInt32(Request.GetRouteData().Route.DataTokens["Expiration"]);

            IEnumerable<string> values;
            if(!Request.Headers.TryGetValues("Sistema", out values))
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Chave de sistema não informada");                

            //Recuperando o Path relativo para executar a chamada
            var path = HttpContext.Current.Request.Url.AbsolutePath.Replace("/" + UrlSuffix + "/", "");
            path = Flurl.Url.Combine(BaseUrl, AuthenticationEndpoint) + HttpContext.Current.Request.Url.Query;

            HttpResponseMessage response = Run(path, Request);
            if (!response.IsSuccessStatusCode)
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Acesso não autorizado");

            JObject jObject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            List<JProperty> properties = jObject.Properties().Where(p => p.Value.Type != JTokenType.Null).ToList();

            DateTime expires_in = DateTime.Now.AddMinutes(Expiration);
            jObject = new JObject();
            jObject.Add("expires_in", expires_in);
            jObject.Add("sistema", values.First());
            jObject.Add("auth_data", new JObject(properties));
            
            string s = jObject.ToString(Formatting.None);
            var ret = new
            {
                access_token = s.Criptografa(SecretKey),
                token_type = "Bearer",
                expires_in = expires_in,
                scope = Request.RequestUri.Scheme + "://" + Flurl.Url.Combine(Request.RequestUri.Authority, UrlSuffix)
            };

            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        static HttpResponseMessage Run(string destino, HttpRequestMessage request)
        {
            HttpRequestMessage r = new HttpRequestMessage(request.Method, destino);

            //r.GetQueryNameValuePairs().add

            if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head)
                r.Content = request.Content;

            using (var client = new HttpClient())
            {
                return client.SendAsync(r).Result;
            }
        }
    }
}
