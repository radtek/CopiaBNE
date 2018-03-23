using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CurriculoParametroController : ApiController
    {
        private readonly Domain.CurriculoParametro _curriculoParametro;
        private readonly ILogger _logger;

        public CurriculoParametroController(Domain.CurriculoParametro curriculoParametro, ILogger logger)
        {
            _curriculoParametro = curriculoParametro;
            _logger = logger;
        }

    }
}
