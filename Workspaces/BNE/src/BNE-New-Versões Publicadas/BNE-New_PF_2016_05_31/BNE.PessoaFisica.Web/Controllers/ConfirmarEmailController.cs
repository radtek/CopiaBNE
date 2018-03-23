using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
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
            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            
            Models.ConfirmarEmail model = new Models.ConfirmarEmail();

            try
            {
                bool confirmadoComSucesso = false;

                string strCodigo = Request.QueryString != null ? Request.QueryString["codigo"] : "";
                string strIdCurriculo = Request.QueryString != null ? Request.QueryString["IdCV"] != null ? Request.QueryString["IdCV"] : "" : "";

                var service = new Helpers.HttpService();
                var serializer = new JavaScriptResult();

                if (strCodigo != "")
                {
                    Dictionary<string, string> parm = new Dictionary<string, string>();
                    parm.Add("codigo", strCodigo);

                    var result = service.Get(urlApi, "api/pessoafisica/CodigoConfirmacaoEmail", parm);
                    confirmadoComSucesso = result == "true";

                    ViewBag.Resultado = result;
                }

                return View("Index");

                //if (confirmadoComSucesso)
                //{
                    //TODO: Charan => implementar a Degustação de candidatura quando o projeto do mini estiver no novo BNE.

                    //#region IntegracaoAllin
                    //Dictionary<string, string> parmIntegrador = new Dictionary<string, string>();

                    //parmIntegrador.Add("IdCV", strIdCurriculo);

                    //var integrouAllin = service.Post(urlApi, "api/pessoafisica/IntegradorAllin?idCurriculo=" + strIdCurriculo, null, parmIntegrador.ToString());
                    //#endregion

                    //return View("Index");
                //}
                //else
                    //return View("Erro", model);
            }
            catch (Exception ex)
            {
                ViewBag.Resultado = ex.Message.ToString();
                _logger.Error(ex, "Erro ao confirmar email");
                model.Resultado = ex.Message.ToString();
                return View("Erro", model);
            }
                
            
        }
    }
}