using BNE.PessoaFisica.WebAPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class RamoAtividadeController : ApiController
    {
        private readonly Global.Domain.RamoAtividade _ramoAtividadeDomain;

        public RamoAtividadeController(Global.Domain.RamoAtividade ramoAtividade)
        {
            _ramoAtividadeDomain = ramoAtividade;
        }

        //Deflate faz a compressão do retorno. Reduz até 80%.
        [DeflateCompression]
        //Faz o cache do método. TimeSpan em segundos.
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public IEnumerable<string> Get(string query, int limit)
        {
            var obj = _ramoAtividadeDomain.ListaRamoAtividades(query);
            return obj;
        }
    }
}
