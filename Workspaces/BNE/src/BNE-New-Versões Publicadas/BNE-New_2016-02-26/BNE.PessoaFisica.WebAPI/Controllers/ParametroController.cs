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
    public class ParametroController : ApiController
    {
        private readonly Domain.Parametro _parametroDomain;

        public ParametroController(Domain.Parametro parametroDomain)
        {
            _parametroDomain = parametroDomain;
        }

        [DeflateCompression]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public string GetMinimoNacional()
        {
            var obj = _parametroDomain.RecuperarValor(Model.Enumeradores.Parametro.SalarioMinimoNacional);
            return obj;
        }
    }
}