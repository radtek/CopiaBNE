using BNE.Components.Web;
using BNE.Logger.Interface;
using BNE.PessoaJuridica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BNE.Core.Common;

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
            return View("Index", new Models.CadastroEmpresa
            {
                NumeroCNPJ = cnpj,
                Usuario = new Models.CadastroEmpresa.CadastroEmpresaUsuario { Nome = nome, Email = email },
                ReceitaOnline = ControlCNPJReceitaFederal.ReceitaOnline()
            });
        }

        /// <summary>
        /// Fluxo chamado quando o cadastro vir de uma empresa com vaga rápida
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("IndexEmail")]
        public ActionResult Index(string email)
        {
            ViewData["MostrarModalVagaRapida"] = true;

            return View("Index", new Models.CadastroEmpresa
            {
                Usuario = new Models.CadastroEmpresa.CadastroEmpresaUsuario { Email = Utils.FromBase64(email) },
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
                var dadosCNPJReceitaFederal = new ControlCNPJReceitaFederal.DadosCNPJReceitaFederal(Utils.LimparMascaraCPFCNPJCEP(model.NumeroCNPJ), model.Captcha, CaptchaSessionId);
                var retornoReceita = dadosCNPJReceitaFederal.RecuperarDados(out erro);
                if (retornoReceita != null)
                {
                    model.Logradouro = retornoReceita.Logradouro.Trim();
                    model.CEP = retornoReceita.CEP.Trim();
                    model.Numero = retornoReceita.Numero.Trim();
                    model.Complemento = retornoReceita.Complemento.Trim();
                    model.Bairro = retornoReceita.Bairro.Trim();
                    model.Cidade = Utils.FormatarCidade(retornoReceita.Municipio.Trim(), retornoReceita.UF.Trim());
                    model.RazaoSocial = retornoReceita.RazaoSocial.Trim();
                    model.NomeFantasia = retornoReceita.NomeFantasia.Trim();
                    model.CNAE = retornoReceita.CNAEPrincipal.Trim();
                    model.NaturezaJuridica = retornoReceita.NaturezaJuridica.Trim();
                    model.DataAbertura = retornoReceita.DataAbertura;
                    model.SituacaoCadastral = retornoReceita.MotivoSituacao;
                }
            }

            model.Usuario.NumeroComercial = model.NumeroComercial;

            return PartialView("_Passo2", model);
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

                var responseMessage = service.Post(new Uri(LinkController.EnderecoApiPessoaJuridica()), "/cadastro/cadastrar", payload);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = (dynamic)JsonConvert.DeserializeObject(responseMessage.Content.ReadAsStringAsync().Result);
                    if (Convert.ToBoolean(content.click2Call))
                    {
                        service.Post(new Uri(LinkController.EnderecoApiPessoaJuridica()), "/cadastro/click2call", serializer.Serialize(new { Numero = content.numero.ToString(), Nome = content.nome.ToString() }));
                    }

                    return PartialView("_Confirmacao", new Models.CadastroEmpresaConfirmacao { LinkPlanoFree = new LinkController().LogarBNE(content.linkplanofree.ToString()), LinkPlanoPago = new LinkController().LogarBNE(content.linkplanopago.ToString()) });
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
    }
}