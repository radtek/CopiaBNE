using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using BNE.PessoaFisica.API;
using BNE.PessoaFisica.Web.Models;
using log4net;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class ConfirmarEmailController : Controller
    {
        private readonly API.IPessoaFisicaAPI _apiClient;
        private readonly ILog _logger;

        public ConfirmarEmailController(ILog logger, API.IPessoaFisicaAPI apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        // GET: ConfirmarEmail
        public async Task<ActionResult> Index()
        {
            var model = new ConfirmarEmail();
            try
            {
                var strCodigo = Request.QueryString != null ? Request.QueryString["codigo"] : "";

                if (string.IsNullOrWhiteSpace(strCodigo))
                {
                    ViewBag.Resultado = await _apiClient.CodigoConfirmacaoEmail.GetAsync(strCodigo);
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.Error("Erro ao confirmar email", ex);
                ViewBag.Resultado = ex.Message;
                model.Resultado = ex.Message;
                return View("Erro", model);
            }
        }
    }
}