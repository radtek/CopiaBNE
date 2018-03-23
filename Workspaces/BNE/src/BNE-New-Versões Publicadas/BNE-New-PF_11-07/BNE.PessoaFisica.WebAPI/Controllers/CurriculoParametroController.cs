using System.Web.Http;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Domain;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CurriculoParametroController : ApiController
    {
        private readonly CurriculoParametro _curriculoParametro;
        private readonly ILogger _logger;

        public CurriculoParametroController(CurriculoParametro curriculoParametro, ILogger logger)
        {
            _curriculoParametro = curriculoParametro;
            _logger = logger;
        }
    }
}