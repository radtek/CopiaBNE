using AllInTriggers.Base;
using AllInTriggers.Helper;
using AllInTriggers.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AllInTriggers
{
    public static class AllInTransaction
    {
        private const int TimeOutAllInRequest = 30000;

        private static string BaseUrlTransactionService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_TransacionService", @"http://transacional01.postmatic.com.br/api/");
            }
        }

        private static string LoginInTransactionService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_LoginInTransactionService", @"trbne");
            }
        }

        private static string PassInTransactionService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_PassInTransactionService", @"rV4Q38ytQ3");
            }
        }

        public static AllInRequestBase LoginTransactionCall()
        {
            return new AllInRequest(BaseUrlTransactionService)
            {
                ResourceRequest = "?method=get_token&output=json&username={seuLogin}&password={suaSenha}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuLogin", LoginInTransactionService),
                                    new KeyValuePair<string,string>("suaSenha", PassInTransactionService)
                                },
                Method = Method.GET,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest,
                ResultResponseAccessor = (resp) =>
                {
                    var jObj = JObject.Parse(resp.Content);
                    JToken value;
                    if (jObj.TryGetValue("token", out value))
                    {
                        return value.Value<string>();
                    }

                    return resp.Content;
                }
            };
        }

        public static AllInRequestBase EnviarHtmlCall(string token, string htmlName, string contentHtml)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NullReferenceException("token");

            if (string.IsNullOrWhiteSpace(htmlName))
                throw new ArgumentNullException("htmlId");

            contentHtml = contentHtml ?? string.Empty;

            return new AllInRequest(BaseUrlTransactionService)
            {
                ResourceRequest = "?method=cadastrar_html&output=json&token={seuToken}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                },
                Method = Method.POST,
                ResultResponseAccessor = (resp) => resp.Content,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            }.SetContentBodyAccessor(() =>
            {
                var json = JsonConvert.SerializeObject(new
                {
                    nm_html = htmlName,
                    html = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contentHtml)),
                });

                //var arg = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("dados", json), 
                //});

                //var t1 = arg.ReadAsStringAsync();
                //return t1.Result;
                return new KeyValuePair<string, string>("dados", json);
            });
        }

        public static AllInRequestBase EditarHtmlCall(string token, string htmlId, string contentHtml)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NullReferenceException("token");

            if (string.IsNullOrWhiteSpace(htmlId))
                throw new NullReferenceException("htmlId");

            contentHtml = contentHtml ?? string.Empty;

            return new AllInRequest(BaseUrlTransactionService)
            {
                ResourceRequest = "?method=alterar_html&output=json&token={seuToken}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                },
                Method = Method.POST,
                ResultResponseAccessor = (resp) => resp.Content,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            }.SetContentBodyAccessor(() =>
            {
                var json = JsonConvert.SerializeObject(new
                {
                    html_id = htmlId,
                    html = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contentHtml)),
                });

                return new KeyValuePair<string, string>("dados", json);
            });
        }

        public static AllInRequestBase BuscarHtmlCall(string token, string htmlId)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NullReferenceException("token");

            if (string.IsNullOrWhiteSpace(htmlId))
                throw new NullReferenceException("htmlId");

            return new AllInRequest(BaseUrlTransactionService)
            {
                ResourceRequest = "?method=getHtmlById&output=json&token={seuToken}&html_id={seuhtmlid}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                    new KeyValuePair<string,string>("seuhtmlid", htmlId)
                                },
                Method = Method.GET,
                ResultResponseAccessor = (resp) => resp.Content,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            };
        }

        public static AllInRequestBase EnviarEmailCall(string token, EnviaEmailTransacaoAllIn model)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NullReferenceException("token");
            if (model == null)
                throw new NullReferenceException("model");

            return new AllInRequest(BaseUrlTransactionService)
            {
                ResourceRequest = "?method=enviar_email&output=json&token={seuToken}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                },
                Method = Method.POST,
                ResultResponseAccessor = (resp) => resp.Content,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            }.SetContentBodyAccessor(() =>
                {
                    var json = JsonConvert.SerializeObject(new
                    {
                        nm_email = model.EmailEnvio,
                        html_id = model.HtmAllInlId,
                        nm_subject = model.Assunto,
                        nm_remetente = model.NomeRemente,
                        email_remetente = model.EmailRemente,
                        nm_reply = string.IsNullOrWhiteSpace(model.EmailResposta) ? model.EmailRemente : model.EmailResposta,
                        dt_envio = model.DataHoraEnvio.HasValue ? model.DataHoraEnvio.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"),
                        hr_envio = model.DataHoraEnvio.HasValue ? model.DataHoraEnvio.Value.ToString("HH:mm") : DateTime.Now.ToString("HH:mm"),
                        campos = (model.Campos ?? new string[0]).Aggregate((a, b) => a + "," + b),
                        valor = (model.Valores ?? new string[0]).Aggregate((a, b) => a + "," + b)
                    });

                    return new KeyValuePair<string, string>("dados", json);
                });
        }
    }
}
