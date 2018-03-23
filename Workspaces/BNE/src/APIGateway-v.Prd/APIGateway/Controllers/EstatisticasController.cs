using APIGateway.Domain.Estatisticas;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIGateway.Controllers
{
    public class EstatisticasController : ApiController
    {
        public class estatisticas
        {
            public List<string> labels = new List<string>();
            public List<string> series = new List<string>();
            public List<object> data = new List<object>();
        }

        private void DefineDefauls(string label, DateTime? inicio, DateTime? fim, out LabelType oLabelType, out DateTime oInicio, out DateTime oFim) 
        {
            if (!inicio.HasValue)
                inicio = DateTime.Now.Date.AddDays(-30);

            if (!fim.HasValue)
                fim = DateTime.Now.Date;

            if (fim < inicio)
                inicio = fim.Value.AddDays(-30);

            oInicio = inicio.Value.Date;
            oFim = fim.Value.Date;

            oLabelType = LabelType.nenhum;
            if (!String.IsNullOrEmpty(label))
            {
                oLabelType = (LabelType)Enum.Parse(typeof(LabelType), label);
            }
        }
        
        private estatisticas Sumarizar(List<ItemEstatistica> list, LabelType label, DateTime? inicio = null, DateTime? fim = null)
        {
            estatisticas est = new estatisticas();

            switch (label)
            {
                case LabelType.data:
                    est.series = list.Select(i => i.serie).Distinct().OrderBy(i => i).ToList();
                    foreach (var item in est.series)
                        est.data.Add(new List<decimal>());
                    for (var day = inicio.Value; day.Date <= fim; day = day.AddDays(1))
                    {
                        string sDay = day.ToString("dd/MM/yy");
                        est.labels.Add(sDay);
                        for (int i = 0; i < est.series.Count; i++)
                        {
                            if (est.data[i] == null)
                                est.data[i] = new List<int>();
                            var t = (List<decimal>)est.data[i];
                            t.Add(list.Where(j => j.date == day && j.serie == est.series[i]).Sum(j => j.data));
                            est.data[i] = t;
                        }
                    }
                    break;
                default:
                    est.labels = list.Select(i => i.serie).Distinct().OrderBy(i => i).ToList();
                    for (int i = 0; i < est.labels.Count; i++)
                    {
                        est.data.Add(list.Where(j => j.serie == est.labels[i]).First().data);
                    }
                    break;
            }
            return est;
        }
        
        [HttpGet]
        public HttpResponseMessage Get(string id, [FromUri]DateTime? inicio = null, [FromUri]DateTime? fim = null, string label = null, [FromUri]int[] idUsuario = null, [FromUri]string[] api = null, [FromUri]int[] endpoint = null)
        {
            switch (id.ToLower())
            {
                case "sistema": return Sistema(inicio, fim, label, idUsuario, api, endpoint);
                case "api": return Api(inicio, fim, label, idUsuario, api, endpoint);
                case "endpoint": return Api(inicio, fim, label, idUsuario, api, endpoint);
                default: return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Estatísticas para '{0}' inexistente", id));
            }
        }

        private HttpResponseMessage Sistema(DateTime? inicio = null, DateTime? fim = null, string label = null, int[] idUsuario = null, string[] api = null, int[] endpoint = null)
        {
            LabelType labelType;
            DateTime dataInicio;
            DateTime dataFim;
            DefineDefauls(label, inicio, fim, out labelType, out dataInicio, out dataFim);

            var l = Domain.Requisicao.NumeroRequisicoesSistema(idUsuario, api, endpoint, dataInicio, dataFim, labelType);

            return Request.CreateResponse(HttpStatusCode.OK, Sumarizar(l, labelType, dataInicio, dataFim));
        }

        private HttpResponseMessage Api(DateTime? inicio = null, DateTime? fim = null, string label = null, int[] idUsuario = null, string[] api = null, int[] endpoint = null)
        {
            LabelType labelType;
            DateTime dataInicio;
            DateTime dataFim;
            DefineDefauls(label, inicio, fim, out labelType, out dataInicio, out dataFim);

            var l = Domain.Requisicao.NumeroRequisicoesApi(idUsuario, api, endpoint, dataInicio, dataFim, labelType);

            return Request.CreateResponse(HttpStatusCode.OK, Sumarizar(l, labelType, dataInicio, dataFim));
        }

        private HttpResponseMessage Endpoint(DateTime? inicio = null, DateTime? fim = null, string label = null, int[] idUsuario = null, string[] api = null, int[] endpoint = null)
        {
            LabelType labelType;
            DateTime dataInicio;
            DateTime dataFim;
            DefineDefauls(label, inicio, fim, out labelType, out dataInicio, out dataFim);

            var l = Domain.Requisicao.NumeroRequisicoesEndpoint(idUsuario, api, endpoint, dataInicio, dataFim, labelType);

            return Request.CreateResponse(HttpStatusCode.OK, Sumarizar(l, labelType, dataInicio, dataFim));
        }

        
    }
}
