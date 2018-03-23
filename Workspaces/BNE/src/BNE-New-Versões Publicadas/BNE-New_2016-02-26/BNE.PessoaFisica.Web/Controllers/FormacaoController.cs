using BNE.PessoaFisica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class FormacaoController : Controller
    {
        // GET: Formacao
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Formacao")]
        public PartialViewResult CadastrarFormacao(Models.Formacao model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "bne/pessoafisica/CadastrarFormacao", null, payload);

            var obj = retorno.Content.ReadAsStringAsync().Result;

            dynamic objCV = JsonConvert.DeserializeObject(obj);

            model.Id = (int)objCV.Id.Value;

            return PartialView("_confirmacao", model);
        }
    }
}