using BNE.Components.Web;
using BNE.ExceptionLog.Interface;
using BNE.PessoaJuridica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class EmpresaController : Controller
    {

        private readonly ILogger _logger;

        public EmpresaController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: Empresa
        [HttpGet]
        public ActionResult Index()
        {
            return View(new Models.CadastroEmpresa { ReceitaOnline = ControlCNPJReceitaFederal.ReceitaOnline() });
        }

        [HttpGet]
        [ActionName("IndexNomeEmailCNPJ")]
        public ActionResult Index(string nome, string email, string cnpj)
        {
            return View("Index",new Models.CadastroEmpresa
            {
                NumeroCNPJ = cnpj,
                Usuario = new Models.CadastroEmpresa.CadastroEmpresaUsuario { Nome = nome, Email = email },
                ReceitaOnline = ControlCNPJReceitaFederal.ReceitaOnline()
            });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Passo1")]
        public PartialViewResult Passo1(Models.CadastroEmpresa model, string CaptchaSessionId)
        {
            string erro = string.Empty;
            if (model.ReceitaOnline)
            {
                var retornoReceita = ControlCNPJReceitaFederal.RetornarPagina(Core.Common.Utils.LimparMascaraCPFCNPJCEP(model.NumeroCNPJ), model.Captcha, CaptchaSessionId, out erro);
                if (retornoReceita != null)
                {
                    model.Logradouro = retornoReceita.Logradouro.Trim();
                    model.CEP = retornoReceita.CEP.Trim();
                    model.Numero = retornoReceita.Numero.Trim();
                    model.Complemento = retornoReceita.Complemento.Trim();
                    model.Bairro = retornoReceita.Bairro.Trim();
                    model.Cidade = Core.Common.Utils.FormatarCidade(retornoReceita.Municipio.Trim(), retornoReceita.UF.Trim());
                    model.RazaoSocial = retornoReceita.RazaoSocial.Trim();
                    model.NomeFantasia = retornoReceita.NomeFantasia.Trim();
                    model.CNAE = retornoReceita.CNAEPrincipal.Trim();
                    model.NaturezaJuridica = retornoReceita.NaturezaJuridica.Trim();
                    model.DataAbertura = retornoReceita.DataAbertura;
                    //model.Matriz = retornoReceita.Matriz;
                    model.SituacaoCadastral = retornoReceita.MotivoSituacao;
                    model.Usuario.NumeroComercial = model.NumeroComercial;

                    return PartialView("_Passo2", model);
                }
            }
            else
                ViewData["MostrarModalReceita"] = true;

            ViewData["Erro"] = erro;
            ViewData["RunScripts"] = true;

            return PartialView("_Passo1", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Passo2")]
        public PartialViewResult Passo2(Models.CadastroEmpresa model)
        {
            string ip;
            model.IP = Core.Common.Utils.RecuperarIP(out ip);
            _logger.Information(ip);

            var serializer = new JavaScriptSerializer();

            var payloadModel = (Models.CadastroEmpresa)model.Clone();
            if (payloadModel.UsuariosAdicionais != null)
                payloadModel.UsuariosAdicionais = payloadModel.UsuariosAdicionais.Where(n => n.Nome != null && n.Email != null).ToList();

            var payload = serializer.Serialize(payloadModel);
            try
            {
                var service = new Core.Helpers.HttpService();

                var responseMessage = service.Post(LinkController.EnderecoApiPessoaJuridica(), "/cadastro/cadastrar", null, payload);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = (dynamic)JsonConvert.DeserializeObject(responseMessage.Content.ReadAsStringAsync().Result);
                    if (Convert.ToBoolean(content.click2Call))
                        return PartialView("_Confirmacao", new Models.CadastroEmpresaConfirmacao { Link = new LinkController().LogarBNE(content.link.ToString()), Numero = content.numero.ToString(), Click2Call = true, Nome = content.nome.ToString() });

                    return PartialView("_Confirmacao", new Models.CadastroEmpresaConfirmacao { Link = new LinkController().LogarBNE(content.link.ToString()) });
                }

                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    ViewData["Erro"] = responseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Empty, payload);
                Employer.PlataformaLog.LogError.WriteLog(ex, payload);
            }

            return PartialView("_Passo2", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Click2Call")]
        public ActionResult Click2Call(Models.CadastroEmpresaConfirmacao model)
        {
            var service = new Core.Helpers.HttpService();

            var serializer = new JavaScriptSerializer();
            var payload = serializer.Serialize(model);
            try
            {
                service.Post(LinkController.EnderecoApiPessoaJuridica(), "/cadastro/click2call", null, payload);
                return PartialView("_Confirmacao", new Models.CadastroEmpresaConfirmacao { Link = model.Link, Click2Call = true });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Empty, payload);
                Employer.PlataformaLog.LogError.WriteLog(ex, payload);
            }

            return Json(new { url = model.Link });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Receita")]
        public PartialViewResult Receita(Models.CadastroEmpresaReceita model)
        {
            var service = new Core.Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            model.DataCadastro = DateTime.Now;

            var payload = serializer.Serialize(model);

            try
            {
                service.Post(LinkController.EnderecoApiPessoaJuridica(), "/cadastro/receita", null, payload);
                ViewData["Erro"] = "Solicitação enviada com sucesso!";
                return PartialView("_Passo1", new Models.CadastroEmpresa { NumeroCNPJ = model.CNPJ, NumeroComercial = model.Telefone });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Empty, payload);
                Employer.PlataformaLog.LogError.WriteLog(ex, payload);
            }

            return PartialView("_Receita", new Models.CadastroEmpresaReceita());
        }

    }
}