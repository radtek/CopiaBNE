using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CurriculoController : ApiController
    {
        private readonly Curriculo _curriculo;
        private readonly ILogger _logger;

        public CurriculoController(Curriculo curriculo, ILogger logger)
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