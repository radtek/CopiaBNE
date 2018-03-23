using AllInTriggers.Base;
using AllInTriggers.Helper;
using AllInTriggers.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AllInTriggers
{
    public static class AllInPainel
    {
        private const int TimeOutAllInRequest = 30000;

        private static string BaseUrlPainelService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_PainelService", @"https://painel02.allinmail.com.br/allinapi/");
            }
        }

        private static string LoginInPainelService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_LoginInPainelService", @"wsbne@empregos");
            }
        }

        private static string PassInPainelService
        {
            get
            {
                return ConfigHelper.GetConfig("AllIn_PassInPainelService", @"9tWPD8uE");
            }
        }

        public static AllInRequestBase LoginTokenPainelCall()
        {
            return new AllInRequest(BaseUrlPainelService)
            {
                ResourceRequest = "?method=get_token&output=json&username={seuLogin}&password={suaSenha}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuLogin",  LoginInPainelService),
                                    new KeyValuePair<string,string>("suaSenha", PassInPainelService)
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

        public static AllInRequestBase EnviarCampanhaCall(string token, EnviaCampanhaPainelAllIn model)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new NullReferenceException("token");
            if (model == null)
                throw new NullReferenceException("model");

            return new AllInRequest(BaseUrlPainelService)
            {
                ResourceRequest = "?method=criar_envio&output=json&token={seutoken}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                },
                Method = Method.POST,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest,
                ResultResponseAccessor = (resp) => resp.Content
            }.SetContentBodyAccessor(() =>
            {
                var json = JsonConvert.SerializeObject(new
                {
                    nm_campanha = model.Campanha,
                    nm_subject = model.Assunto,
                    nm_remetente = model.NomeRemente,
                    email_remetente = model.EmailRemente,
                    nm_reply = string.IsNullOrWhiteSpace(model.EmailResposta) ? model.EmailRemente : model.EmailResposta,
                    nm_html = model.Html,
                    dt_inicio = model.DataHoraEnvio.HasValue ? model.DataHoraEnvio.Value.ToString("yyyy-MM-dd HH:mm") : DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    nm_lista = model.NomeLista,
                    nm_filtro = model.Filtro,
                    nm_categoria = model.Categoria,
                    fl_analytics = model.UtilizarAnalytics ? "1" : "0",
                });

                //var arg = new FormUrlEncodedContent(new[]
                //{
                //    new KeyValuePair<string, string>("dados_envio", json), 
                //});

                //var t1 = arg.ReadAsStringAsync();
                //return t1.Result;
                return new KeyValuePair<string, string>("dados_envio", json);
            });
        }

        public static AllInRequestBase NotificarCicloDeVidaCall(NotificaCicloDeVidaAllIn model)
        {
            if (model == null)
                throw new NullReferenceException("model");

            return new AllInRequest(BaseUrlPainelService)
            {
                ResourceRequest = "?method=lifeCycle",
                Method = Method.POST,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            }.SetContentBodyAccessor(() =>
            {
                var specific = ((IDictionary<string, object>)new ExpandoObject());
                (model.CamposComValores ?? new KeyValuePair<string, string>[0]).ForEach(a => specific.Add(a.Key, a.Value));

                var listToUpdate = ((IDictionary<string, object>)new ExpandoObject());
                (model.ListaParaAtualizacao ?? new KeyValuePair<string, string>[0]).ForEach(a => listToUpdate.Add(a.Key, a.Value));

                var json = JsonConvert.SerializeObject(new
                {
                    i = model.IdentificadorAllIn,
                    evento = model.Evento,
                    nm_email = model.EmailEnvio,
                    repetir = model.AceitaRepeticao ? "1" : "0",
                    vars = specific,
                    lista = listToUpdate
                });

                return new KeyValuePair<string, string>("dados", json);
            })
            .SetResultResponseAccesssor(res =>
            {
                try
                {
                    JToken jObj = JObject.Parse(res.Content);
                    while (jObj.HasValues)
                    {
                        jObj = jObj.Last;
                    }
                    return jObj.Value<string>();
                }
                catch
                {
                    return res.Content;
                }
            }); ;
        }

        public static AllInRequestBase InserirOuAtualizarItemLista(string token, InserirOuAtualizarItemListaAllIn model)
        {
            if (model == null)
                throw new NullReferenceException("model");

            return new AllInRequest(BaseUrlPainelService)
            {
                ResourceRequest = "?method=inserir_email_base&output=json&token={seuToken}",
                UrlSegment = new KeyValuePair<string, string>[] 
                                { 
                                    new KeyValuePair<string,string>("seuToken", token),
                                },
                Method = Method.POST,
                ManipulateRequest = (req) => req.Timeout = TimeOutAllInRequest
            }.SetContentBodyAccessor(() =>
            {
                var json = JsonConvert.SerializeObject(new
                {
                    nm_lista = model.NomeLista,
                    campos = model.TituloCampos,
                    valor = model.ValorCampos
                });

                return new KeyValuePair<string, string>("dados_email", json);
            })
          .SetResultResponseAccesssor(res =>
          {
              try
              {
                  JToken jObj = JObject.Parse(res.Content);
                  while (jObj.HasValues)
                  {
                      jObj = jObj.Last;
                  }
                  return jObj.Value<string>();
              }
              catch
              {
                  return res.Content;
              }
          }); ;
        }

    }
}
