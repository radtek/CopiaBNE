using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain.Estatisticas
{
    interface IEstatistica 
    {
        //IQueryable<Model.Requisicao> GetQueryable(IQueryable<Model.Requisicao> query, int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null)
        //{
        //    if (idsUsuario != null && idsUsuario.Count() > 0)
        //        query = query.Where(r => idsUsuario.Contains(r.Usuario.Id));

        //    if (idsUsuario != null && urlSuffixesApi.Count() > 0)
        //        query = query.Where(r => urlSuffixesApi.Contains(r.Endpoint.ApiUrlSuffix));

        //    if (idsUsuario != null && idsEndpoint.Count() > 0)
        //        query = query.Where(r => idsEndpoint.Contains(r.Endpoint.Id));

        //    if (inicio.HasValue && inicio > DateTime.MinValue)
        //        query = query.Where(r => r.DataRequisicao >= inicio);

        //    if (fim.HasValue && fim > DateTime.MinValue)
        //        query = query.Where(r => r.DataRequisicao <= fim);

        //    return query;
        //}

        List<ItemEstatistica> Get(int[] idsUsuario = null, string[] urlSuffixesApi = null, int[] idsEndpoint = null, DateTime? inicio = null, DateTime? fim = null, LabelType label = LabelType.nenhum);

    }
}
