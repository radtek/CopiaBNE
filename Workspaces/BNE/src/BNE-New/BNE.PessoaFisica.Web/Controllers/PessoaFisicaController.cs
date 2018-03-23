using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BNE.PessoaFisica.API;
using BNE.PessoaFisica.API.Models;
using BNE.PessoaFisica.Web.Attributes;
using log4net;
using Curriculo = BNE.PessoaFisica.Web.Models.Curriculo;
using PreCurriculo = BNE.PessoaFisica.Web.Models.PreCurriculo;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class PessoaFisicaController : Controller
    {
        private readonly IPessoaFisicaAPI _apiClient;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public PessoaFisicaController(IPessoaFisicaAPI apiClient, ILog logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _apiClient = apiClient;
        }

        public ActionResult Index()
        {
            var model = new PreCurriculo();

            return View(model);
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarCurriculo")]
        public async Task<PartialViewResult> CadastrarCurriculo(Curriculo model)
        {
            try
            {
                var command = _mapper.Map<Curriculo, SalvarCurriculoCommand>(model);
                var retorno = await _apiClient.Curriculo.CadastrarWithHttpMessagesAsync(command);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    return PartialView("_CadastroEtapa2", model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("PF WEB => PessoaFisica/CadastrarMiniCurriculo", ex);
            }
            //TODO: Charan => fazer tratamento de erros aqui
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarDadosPessoais")]
        public PartialViewResult CadastrarDadosPessoais(PreCurriculo model)
        {
            return PartialView("_CadastroEtapa3", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarFormacaoCursos")]
        public PartialViewResult CadastrarFormacaoCursos(PreCurriculo model)
        {
            return PartialView("_CadastroEtapa4", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarInformacoesExtras")]
        public PartialViewResult CadastrarInformacoesExtras(PreCurriculo model)
        {
            return PartialView("_CadastroEtapa5", model);
        }
    }
}