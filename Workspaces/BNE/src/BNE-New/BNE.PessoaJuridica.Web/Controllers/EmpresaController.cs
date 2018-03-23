using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BNE.Components.Web;
using BNE.Core.Common;
using BNE.Core.Helpers;
using BNE.PessoaJuridica.Web.Attributes;
using BNE.PessoaJuridica.Web.Models;
using BNE.PessoaJuridica.Web.wsCEP;
using Employer.PlataformaLog;
using log4net;
using Newtonsoft.Json;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly ILog _logger;

        public EmpresaController(ILog logger)
        {
            _logger = logger;
        }

        // GET: Empresa
        [HttpGet]
        public ActionResult Index()
        {
            return View(new CadastroEmpresa());
        }

        [HttpGet]
        [ActionName("IndexNomeEmailCNPJ")]
        public ActionResult Index(string nome, string email, string cnpj)
        {
            return View("Index", new CadastroEmpresa
            {
                NumeroCNPJ = cnpj,
                Usuario = new CadastroEmpresa.CadastroEmpresaUsuario { Nome = nome, Email = email }
            });
        }

        /// <summary>
        ///     Fluxo chamado quando o cadastro vir de uma empresa com vaga rápida
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("IndexEmail")]
        public ActionResult Index(string email)
        {
            ViewData["MostrarModalVagaRapida"] = true;

            return View("Index", new CadastroEmpresa
            {
                Usuario = new CadastroEmpresa.CadastroEmpresaUsuario { Email = Utils.FromBase64(email) }
            });
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Passo1")]
        public PartialViewResult Passo1(CadastroEmpresa model, string CaptchaSessionId)
        {
            var errorMessage = string.Empty;
            if (model.ReceitaOnline && !model.SolicitarEndereco) //Receita teoricamente está online
            {
                ControlCNPJReceitaFederal.DadosCNPJReceitaFederal.ErrorTypes error;
                var retornoReceita = new ControlCNPJReceitaFederal.DadosCNPJReceitaFederal().RecuperarDados(Utils.LimparMascaraCPFCNPJCEP(model.NumeroCNPJ), model.Captcha, CaptchaSessionId, out error);

                switch (error)
                {
                    case ControlCNPJReceitaFederal.DadosCNPJReceitaFederal.ErrorTypes.None:
                        if (!retornoReceita.Ativa)
                        {
                            errorMessage = "CNPJ não está ativo na Receita Federal!";
                        }
                        else
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
                        break;
                    case ControlCNPJReceitaFederal.DadosCNPJReceitaFederal.ErrorTypes.InvalidCaptcha:
                        errorMessage = "Captcha inválido!";
                        break;
                    case ControlCNPJReceitaFederal.DadosCNPJReceitaFederal.ErrorTypes.InvalidCnpj:
                        errorMessage = "CNPJ inválido!";
                        break;
                    case ControlCNPJReceitaFederal.DadosCNPJReceitaFederal.ErrorTypes.CommunicationProblem:
                        ModelState.Remove("SolicitarEndereco");
                        model.SolicitarEndereco = true;
                        return PartialView("_Passo1", model);
                }

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ViewData["Erro"] = errorMessage;
                    return PartialView("_Passo1", model);
                }
            }

            model.Usuario.NumeroComercial = model.NumeroComercial;

            return PartialView("_Passo2", model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Passo2")]
        public PartialViewResult Passo2(CadastroEmpresa model)
        {
            string ip;
            model.IP = Utils.RecuperarIP(out ip);

            var serializer = new JavaScriptSerializer();

            var payloadModel = (CadastroEmpresa)model.Clone();
            if (payloadModel.UsuariosAdicionais != null)
                payloadModel.UsuariosAdicionais = payloadModel.UsuariosAdicionais.Where(n => (n.Nome != null) && (n.Email != null)).ToList();

            var payload = serializer.Serialize(payloadModel);
            try
            {
                var service = new HttpService();

                var responseMessage = service.Post(new Uri(LinkController.EnderecoApiPessoaJuridica()), "/cadastro/cadastrar", payload);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = (dynamic)JsonConvert.DeserializeObject(responseMessage.Content.ReadAsStringAsync().Result);
                    if (Convert.ToBoolean(content.Click2Call))
                        service.Post(new Uri(LinkController.EnderecoApiPessoaJuridica()), "/cadastro/click2call", serializer.Serialize(new { Numero = content.NumeroTelefone.ToString(), Nome = content.Nome.ToString() }));
                    var linkController = new LinkController();
                    var linkPlanoFree = linkController.LogarBNE(content.LinkPlanoFree.ToString());
                    var linkplanopago = linkController.LogarBNE(content.LinkPlanoPago.ToString());
                    var link = linkController.LogarBNE(content.LinkAcesso.ToString());
                    var empresaJaCadastrada = Convert.ToBoolean(content.EmpresaJaCadastrada);
                    var valorPlanoPagoDe = Convert.ToDecimal(content.ValorPlanoPagoDe);
                    var valorPlanoPagoPara = Convert.ToDecimal(content.ValorPlanoPagoPara);
                    return PartialView("_Confirmacao", new CadastroEmpresaConfirmacao(linkPlanoFree, linkplanopago, link, model.ReceitaOnline, empresaJaCadastrada, valorPlanoPagoDe, valorPlanoPagoPara));
                }

                if (responseMessage.StatusCode == HttpStatusCode.BadRequest)
                    ViewData["Erro"] = responseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                _logger.Error(payload, ex);
                LogError.WriteLog(ex, payload);
            }

            return PartialView("_Passo2", model);
        }

        [HttpPost]
        [ActionName("CEP")]
        public ActionResult Cep(string cep)
        {
            var model = new CadastroEmpresa();
            try
            {
                var webservicecepclient = new CEPClient();
                var webservicecep = new CEP { Cep = cep };

                webservicecepclient.CompletarCEP(ref webservicecep);

                model.Logradouro = webservicecep.Logradouro;
                model.Complemento = webservicecep.Complemento;
                model.Bairro = webservicecep.Bairro;
                model.Cidade = Utils.FormatarCidade(webservicecep.Cidade, webservicecep.Estado);
            }
            catch (Exception ex)
            {
                _logger.Error(cep, ex);
                LogError.WriteLog(ex, cep);
            }
            return PartialView("_Endereco", model);
        }
    }
}