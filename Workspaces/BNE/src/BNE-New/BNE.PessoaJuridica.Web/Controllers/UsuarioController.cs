using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using log4net;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ILog _logger;

        public UsuarioController(ILog logger)
        {
            _logger = logger;
        }

        // GET: Usuario
        [HttpGet]
        public ActionResult Index(string numeroCNPJ, string email, string nome)
        {
            return View(new Models.CadastroUsuarioEmpresa { NumeroCNPJ = numeroCNPJ, EmailOriginal = email, Email = email, Nome = nome });
        }

        [HttpPost]
        public ActionResult Index(Models.CadastroUsuarioEmpresa model)
        {
            string ip;
            model.IP = Core.Common.Utils.RecuperarIP(out ip);

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
                _logger.Error(payload, ex);
                Employer.PlataformaLog.LogError.WriteLog(ex, payload);
            }

            ViewData["RunScripts"] = true;
            return PartialView("_Cadastro", model);
        }

    }
}