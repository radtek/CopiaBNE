using BNE.PessoaFisica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class ExperienciaProfissionalController : Controller
    {
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AdicionarExperiencia")]
        public PartialViewResult Adicionar(Models.ExperienciaProfissional model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "bne/pessoafisica/PessoaFisica/CadastrarExperiencia", null, payload);

            var obj = retorno.Content.ReadAsStringAsync().Result;

            dynamic objCV = JsonConvert.DeserializeObject(obj);

            model.Id = (int)objCV.Id.Value;


            return PartialView("_confirmacao", model);
        }
    }
}