using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CurriculoController : ApiController
    {
        private readonly Domain.Curriculo _curriculo;
        private readonly ILogger _logger;

        public CurriculoController(Domain.Curriculo curriculo, ILogger logger)
        {
            _curriculo = curriculo;
            _logger = logger;
        }

        [HttpGet]
        public ResponseMessageResult GetInformacoesCurriculo(int idVaga, string cpf)
        {
            try
            {
                var result = _curriculo.CarregarInformacoesCurriculo(idVaga, Convert.ToDecimal(cpf));
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, result));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - CarregarInformacoesCurriculo vaga(" + idVaga + ") cpf(" + cpf + ")");
                throw;
            }
        }        
    }
}