using BNE.Logger.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class ConfirmarEmailController : Controller
    {
        private readonly ILogger _logger;

        public ConfirmarEmailController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: ConfirmarEmail
        public ActionResult Index()
        {
            Models.ConfirmarEmail model = new Models.ConfirmarEmail();
            try
            {
                Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);

                string strCodigo = Request.QueryString != null ? Request.QueryString["codigo"] : "";

                var service = new Core.Helpers.HttpService();

                if (strCodigo != "")
                {
                    Dictionary<string, string> parm = new Dictionary<string, string> { { "codigo", strCodigo } };

                    var result = service.Get(urlApi, "api/pessoafisica/CodigoConfirmacaoEmail", parm);

                    ViewBag.Resultado = result;
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Resultado = ex.Message;
                _logger.Error(ex, "Erro ao confirmar email");
                model.Resultado = ex.Message;
                return View("Erro", model);
            }
        }
    }
}