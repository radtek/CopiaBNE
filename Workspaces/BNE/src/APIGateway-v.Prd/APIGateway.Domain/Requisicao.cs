using APIGateway.Data;
using APIGateway.Domain.Estatisticas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class Requisicao
    {
        public static void GravaRequisicao(Model.Endpoint endpoint, Model.SistemaCliente sistema, Model.Usuario usuario, double tempoExecucao, HttpRequestMessage request, string requestContent, HttpResponseMessage response)
        {
            //Se nenhum log está habilitado para o Endpoint, retorna
            if (!endpoint.LogErro && !endpoint.LogSucesso)
                return;

            //Se o status da requisição é de sucesso e o log de sucesso não está habilitado ou 
            //a o status da requisição é de erro e o log de erro não está habilitado, retorna
            if ((response.IsSuccessStatusCode && !endpoint.LogSucesso) || (!response.IsSuccessStatusCode && !endpoint.LogErro))
                return;

            //Executa Task assincrona para retornar a requisição rapidamente
            Task.Run(() =>
            {
                using (var _context = new APIGatewayContext())
                {
                    Model.Requisicao req = new Model.Requisicao
                    {
                        Id = Guid.NewGuid(),
                        DataRequisicao = DateTime.Now,
                        Endpoint = _context.Endpoint.Find(endpoint.Id),
                        Request = request.ToString(),
                        RequestContent = requestContent,
                        ResponseStatusCode = response.StatusCode,
                        SistemaCliente = _context.SistemaCliente.Find(sistema.Chave),
                        TempoExecucao = tempoExecucao,
                        Usuario = _context.Usuario.Find(usuario.Id),
                        Perfil = usuario.PerfilDeAcesso
                    };

                    if (endpoint.LogResponse)
                    {
                        try
                        {
                            req.Response = response.Content.ReadAsStringAsync().Result;
                        }
                        catch (Exception)
                        {
                            req.Response = "*no content*";
                        }
                    }

                    _context.Requisicao.Add(req);
                                        
                    _context.Commit();
                }
            });
        }

        private static IQueryable<Model.Requisicao> GetQueryable(IQueryable<Model.Requisicao> query, int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null)
        {
            if (idsUsuario != null && idsUsuario.Count() > 0)
                query = query.Where(r => idsUsuario.Contains(r.Usuario.Id));

            if (idsUsuario != null && urlSuffixesApi.Count() > 0)
                query = query.Where(r => urlSuffixesApi.Contains(r.Endpoint.ApiUrlSuffix));

            if (idsUsuario != null && idsEndpoint.Count() > 0)
                query = query.Where(r => idsEndpoint.Contains(r.Endpoint.Id));

            if (inicio.HasValue && inicio > DateTime.MinValue)
                query = query.Where(r => r.DataRequisicao >= inicio);

            if (fim.HasValue && fim > DateTime.MinValue)
                query = query.Where(r => r.DataRequisicao <= fim);

            return query;
        }

        public static List<ItemEstatistica> NumeroRequisicoesSistema(int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null, LabelType label = LabelType.nenhum)
        {
            using (var _ctx = new APIGatewayContext())
            {
                var query = GetQueryable(_ctx.Requisicao.Where(r => r.Usuario is Model.UsuarioSistemaCliente), idsUsuario, urlSuffixesApi, idsEndpoint, inicio, fim);

                switch (label)
                {
                    case LabelType.data:
                        return query.GroupBy(r => new { date = DbFunctions.TruncateTime(r.DataRequisicao), Usuario = r.Usuario as Model.UsuarioSistemaCliente })
                                    .OrderBy(g => g.Key.date)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            date = DbFunctions.TruncateTime(g.Key.date.Value).Value,
                                            serie = g.Key.Usuario.SistemaCliente.Nome,
                                            data = g.Count()
                                        }).ToList();
                    default:
                        return query.GroupBy(r => new { Usuario = r.Usuario as Model.UsuarioSistemaCliente })
                                    .OrderBy(g => g.Key.Usuario.Id)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            serie = g.Key.Usuario.SistemaCliente.Nome,
                                            data = g.Count()
                                        }).ToList();
                }
            }
        }

        public static List<ItemEstatistica> NumeroRequisicoesApi(int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null, LabelType label = LabelType.nenhum)
        {
            using (var _ctx = new APIGatewayContext())
            {
                var query = GetQueryable(_ctx.Requisicao, idsUsuario, urlSuffixesApi, idsEndpoint, inicio, fim);

                switch (label)
                {
                    case LabelType.data:
                        return query.GroupBy(r => new { date = DbFunctions.TruncateTime(r.DataRequisicao), r.Endpoint.ApiUrlSuffix })
                                    .OrderBy(g => g.Key.date)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            date = DbFunctions.TruncateTime(g.Key.date.Value).Value,
                                            serie = g.Key.ApiUrlSuffix,
                                            data = g.Count()
                                        }).ToList();
                    default:
                        return query.GroupBy(r => new { r.Endpoint.ApiUrlSuffix })
                                    .OrderBy(g => g.Key.ApiUrlSuffix)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            serie = g.Key.ApiUrlSuffix,
                                            data = g.Count()
                                        }).ToList();
                }
            }
        }

        public static List<ItemEstatistica> NumeroRequisicoesEndpoint(int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null, LabelType label = LabelType.nenhum)
        {
            using (var _ctx = new APIGatewayContext())
            {
                var query = GetQueryable(_ctx.Requisicao, idsUsuario, urlSuffixesApi, idsEndpoint, inicio, fim);

                switch (label)
                {
                    case LabelType.data:
                        return query.GroupBy(r => new { date = DbFunctions.TruncateTime(r.DataRequisicao), Endpoint = r.Endpoint.ApiUrlSuffix + "/" + r.Endpoint.RelativePath })
                                    .OrderBy(g => g.Key.date)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            date = DbFunctions.TruncateTime(g.Key.date.Value).Value,
                                            serie = g.Key.Endpoint.Replace("//", "/"),
                                            data = g.Count()
                                        }).ToList();
                    default:
                        return query.GroupBy(r => new { Endpoint = r.Endpoint.ApiUrlSuffix + "/" + r.Endpoint.RelativePath })
                                    .OrderBy(g => g.Key.Endpoint)
                                    .Select(g =>
                                        new ItemEstatistica
                                        {
                                            serie = g.Key.Endpoint.Replace("//", "/"),
                                            data = g.Count()
                                        }).ToList();
                }
            }
        }
    }
}
