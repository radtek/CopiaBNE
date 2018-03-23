using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BNE.Logger.Interface;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILogger _logger;

        public UsuarioController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: Usuario
        [HttpGet]
        public ActionResult Index(string numeroCNPJ, string email)
        {
            return View(new Models.CadastroUsuarioEmpresa { NumeroCNPJ = numeroCNPJ, EmailOriginal = email, Email = email });
        }

        [HttpPost]
        public ActionResult Index(Models.CadastroUsuarioEmpresa model)
        {
            string ip;
            model.IP = Core.Common.Utils.RecuperarIP(out ip);
            _logger.Information(ip);

            var serializer = new JavaScriptSerializer();
            var payload = serializer.Serialize(model);
            try
            {
                var service = new Core.Helpers.HttpService();

                var retorno = service.Post(new Uri(LinkController.EnderecoApiPessoaJuridica()), "/cadastrousuario/cadastrar", payload);

                if (retorno.IsSuccessStatusCode)
                    return Json(new { url = new LinkController().LogarBNE(retorno.Content.ReadAsStringAsync().Result) });

                if (retorno.StatusCode == HttpStatusCode.BadRequest)
                    ViewData["Erro"] = retorno.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Empty, payload);
                Employer.PlataformaLog.LogError.WriteLog(ex, payload);
            }

            ViewData["RunScripts"] = true;
            return PartialView("_Cadastro", model);
        }

    }
}