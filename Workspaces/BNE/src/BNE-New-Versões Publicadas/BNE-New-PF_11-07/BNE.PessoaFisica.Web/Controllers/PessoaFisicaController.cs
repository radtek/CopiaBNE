using BNE.PessoaFisica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BNE.Logger.Interface;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class PessoaFisicaController : Controller
    {

        private readonly ILogger _logger;

        public PessoaFisicaController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: PessoaFisica
        public ActionResult Index()
        {
            #region Login e dados do candidato

            var user = Auth.NET45.BNEAutenticacao.User();

            if (user != null)
            {
                //Carregar dados da pessoa logada

            }

            #endregion


            var model = new Models.PessoaFisica();

            return View(model);
        }


        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarMiniCurriculo")]
        public PartialViewResult CadastrarMiniCurriculo(Models.PessoaFisica model)
        {
            try
            {


                var service = new Core.Helpers.HttpService();
                var serializer = new JavaScriptSerializer();

                var payload = serializer.Serialize(model);

                Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
                var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarMini", payload);

                if (retorno.IsSuccessStatusCode)
                {
                    var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);
                    //TODO Gieyson: 
                    /*
                     * Não estou logando o nome ainda por não estar sendo retornado na api, 
                     * isso para não ter que recuperar ele a 
                     * todo momento da candidatura no projeto velho
                     */
                    //Auth.NET45.BNEAutenticacao.LogarCandidato(content.nome.Value.ToString(), Convert.ToDecimal(content.cpf), Convert.ToDateTime(content.datanascimento));

                    return PartialView("_CadastroEtapa2", model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PF WEB => PessoaFisica/CadastrarMiniCurriculo");
            }
            //TODO: Charan => fazer tratamento de erros aqui
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarDadosPessoais")]
        public PartialViewResult CadastrarDadosPessoais(Models.PessoaFisica model)
        {
            return PartialView("_CadastroEtapa3", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarFormacaoCursos")]
        public PartialViewResult CadastrarFormacaoCursos(Models.PessoaFisica model)
        {
            return PartialView("_CadastroEtapa4", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "CadastrarInformacoesExtras")]
        public PartialViewResult CadastrarInformacoesExtras(Models.PessoaFisica model)
        {
            return PartialView("_CadastroEtapa5", model);
        }
    }
}