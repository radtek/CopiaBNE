using BNE.PessoaFisica.Web.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BNE.Core.ExtensionsMethods;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class PreCurriculoController : Controller
    {
        // GET: PreCurriculo
        public ActionResult Index()
        {
            try
            {
                //logar candidato
                //Auth.NET45.BNEAutenticacao.LogarCandidato("Sophia Natália Gomes", Convert.ToDecimal("47196597214"), Convert.ToDateTime("1980-03-30"));
                //Auth.NET45.BNEAutenticacao.LogarCandidato("", Convert.ToDecimal("76049674345"), Convert.ToDateTime("1980-03-30"));

                #region Login e dados do candidato

                var user = Auth.NET45.BNEAutenticacao.User();

                if (user != null)
                {
                    // 0 - Cadastro do Pré-CV
                    // 1 - Pedir Experiencia
                    // 2 - Pedir Formação
                    // 3 - Candidatar
                    var pedir = CandidatoTemExperiencia(user.CPF.ToString(), user.DataNascimento);

                    if (pedir == "1")
                    {
                        @ViewBag.PedirExperiencia = true;
                        @ViewBag.PedirFormacao = false;
                        @ViewBag.EnviarCV = false;
                    }
                    else if (pedir == "2")
                    {
                        @ViewBag.PedirExperiencia = false;
                        @ViewBag.PedirFormacao = true;
                        @ViewBag.EnviarCV = false;
                    }
                    else if (pedir == "3")
                    {
                        @ViewBag.PedirExperiencia = false;
                        @ViewBag.PedirFormacao = false;
                        @ViewBag.EnviarCV = true;

                    }
                    else
                    {
                        @ViewBag.PedirExperiencia = false;
                        @ViewBag.PedirFormacao = false;
                        @ViewBag.EnviarCV = false;
                    }
                }
                else
                {
                    @ViewBag.PedirExperiencia = false;
                    @ViewBag.PedirFormacao = false;
                    @ViewBag.EnviarCV = false;
                }
                #endregion

                string strIdVaga = "PreCurriculo";
                string strIdPesquisa = "0";
                

                //Recuperar parametros da rota
                var parametrosUrlOrigem = Request.Path.Substring(1).Split('/');
                var urlOrigem = Request.UrlReferrer;

                if (parametrosUrlOrigem.Length > 3)
                {
                    strIdPesquisa = parametrosUrlOrigem[parametrosUrlOrigem.Length - 1];
                    strIdVaga = parametrosUrlOrigem[parametrosUrlOrigem.Length - 2];
                }
                else if (parametrosUrlOrigem.Length == 3)
                {
                    strIdVaga = parametrosUrlOrigem[parametrosUrlOrigem.Length - 1];
                }

                //Pegar idPesquisa do cookie
                HttpCookie cookie = Request.Cookies["CodigoPesquisa"];

                if (cookie != null)
                    strIdPesquisa = cookie.Value;

                //strIdVaga = strIdVaga == "PreCurriculo" ? "149314" : strIdVaga; //teste vaga estágio
                strIdVaga = strIdVaga == "PreCurriculo" ? "791512" : strIdVaga; //HOM 149314 76647 791662
                //strIdVaga = strIdVaga == "PreCurriculo" ? "1268497" : strIdVaga; // PRD vaga = 1268497, oportunidade = 1091616

                Models.Vaga vaga = CarregarVaga(strIdVaga);

                if (vaga != null)
                {
                    var eEstagio = vaga.DescricaoTipoVinculo != null ? vaga.DescricaoTipoVinculo.SingleOrDefault(p=>p.Contains("Estágio")) : null;
                    
                    if (vaga.SalarioDe == 0 && vaga.SalarioAte == 0 && eEstagio == null)
                    {
                        Models.ResultadoPesquisaSalarioBR faixaSalarial = null;

                        if (vaga.IdFuncao.HasValue)
                            faixaSalarial = CarregarMediaSalarioBR(vaga.IdFuncao.Value);

                        if (faixaSalarial != null)
                        {
                            vaga.faixaSalarial = string.Format(
                                "Média salarial do mercado é de {0} até {1}",
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Trainee.ToString("C"),
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Master.ToString("C"));
                        }
                    }
                    else
                    {
                        vaga.faixaSalarial = "combinar";
                    }
                
                    vaga.urlPesquisa = urlOrigem != null ? urlOrigem.ToString() : "";
                    vaga.IdPesquisa = Convert.ToInt32(strIdPesquisa);

                    //Checar se o candidato ja enviou o CV
                    ViewBag.JaEnviei = false;
                    
                    if(user != null)
                        ViewBag.JaEnviei = JaCandidatou(vaga.IdVaga, user.CPF);

                    //GetLinksPaginasSemelhantes
                    string[] urls = GetLinksPaginasSemelhantes(vaga.Funcao, vaga.Cidade, vaga.UF, vaga.DescricaoAreaBNEPesquisa);

                    vaga.LinkVagasFuncao = Helpers.SEO.LinkHelper.ObterLinkVagasFuncao(vaga.Funcao, urls[0]);
                    vaga.LinkVagasCidade = Helpers.SEO.LinkHelper.ObterLinkVagasCidade(vaga.Cidade, vaga.UF, urls[1]);
                    vaga.LinkVagasFuncaoCidade = Helpers.SEO.LinkHelper.ObterLinkVagasFuncaoCidade(vaga.Funcao, vaga.Cidade,vaga.UF, urls[2]);
                    vaga.LinkVagasArea = Helpers.SEO.LinkHelper.ObterLinkVagasArea(vaga.DescricaoAreaBNEPesquisa, urls[3]);

                    //Montar description SEO da vaga
                    var descriptionSeo = new
                    {
                        Funcao = vaga.Funcao,
                        Cidade = vaga.Cidade,
                        Salario = vaga.faixaSalarial,
                        CodigoVaga = vaga.CodigoVaga
                    };

                    ViewBag.Title = Helpers.SEO.SitemapHelper.MontarTituloVaga(vaga.Funcao, vaga.DescricaoAreaBNEPesquisa, 1, vaga.Cidade, vaga.UF, vaga.UF, vaga.IdVaga);
                    ViewBag.Description = descriptionSeo.ToString("Procurando emprego? Candidate-se para a vaga de {Funcao} em {Cidade}. {Salario}. {CodigoVaga}. Banco Nacional de Empregos | BNE.");
                }

                var model = new Models.PreCurriculo(vaga);

                if (user != null)
                {
                    model.CPF = user.CPF.ToString();
                    model.DataNascimento = user.DataNascimento;
                    model.Nome = user.Nome;
                }

                if (vaga.eEstagio)
                {
                    if (vaga.Funcao != "Estagiário")
                        vaga.Funcao = "Estágio para " + vaga.Funcao;
                }
                else if (vaga.eAprendiz)
                {
                    if (vaga.FlgArquivada || vaga.FlgInativo)
                        vaga.Funcao = "Oportunidade de " + vaga.Funcao;
                    else
                        vaga.Funcao = "Vaga de " + vaga.Funcao;
                }
                else if (vaga.eEfetivo)
                {
                    if (vaga.FlgArquivada || vaga.FlgInativo)
                        vaga.Funcao = "Oportunidade de " + vaga.Funcao;
                    else
                        vaga.Funcao = "Vaga de " + vaga.Funcao;                    
                }

                if(vaga.FlgArquivada || vaga.FlgInativo)
                    model.VagaOportunidade = @ViewBag.CssVagaOportunidade = "oportunidade";
                else
                    model.VagaOportunidade = @ViewBag.CssVagaOportunidade = "vaga";
                
                return View(model);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public PartialViewResult FazerLogin()
        {
            return PartialView("_FazerLogin");
        }

        public PartialViewResult Passo1()
        {
            return PartialView("_CadastroPasso1");
        }

        private string CandidatoTemExperiencia(string cpf, DateTime dataNascimento)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();
            Dictionary<string, string> header = new Dictionary<string, string>();

            //header.Add("sistema", ConfigurationManager.AppSettings["KeyGatewayPessoaFisica"]);

            //var token = Helpers.ApiGatewayToken.GerarToken();
            //header.Add("Authorization", "bearer " + token.access_token);
            

            parm.Add("Cpf", cpf);
            parm.Add("dataNascimento", dataNascimento.Date.ToString("yyyy-MM-dd"));


            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/PessoaFisica/GetCandidatoTemExperienciaeFormacao", header, parm);
            
            return retorno;
        }

        private Models.Vaga CarregarVaga(string idVaga)
        {
            Models.Vaga vaga = null;
            
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();

            parm.Add("CodigoVaga", idVaga);
            parm.Add("sistema", ConfigurationManager.AppSettings["KeyGatewayPessoaFisica"]);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            //var retorno = service.Get(urlApi, "bne/pessoafisica/public/Vaga", parm);
            var retorno = service.Get(urlApi, "api/pessoafisica/Vaga", parm);

            vaga = serializer.Deserialize<Models.Vaga>(retorno);

            return vaga;
        }

        private bool JaCandidatou(int idVaga, decimal cpf)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();

            parm.Add("codigoVaga", idVaga.ToString());
            parm.Add("cpf", cpf.ToString());

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/Vaga/GetJaEnviei", parm);

            bool result = serializer.Deserialize<bool>(retorno);

            return result;
        }

        public string[] GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();

            parm.Add("funcao", funcao.Replace("#","sharp"));
            parm.Add("cidade", cidade);
            parm.Add("siglaEstado", siglaEstado);
            parm.Add("areaBNE", areaBNE);
            
            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var result = service.Get(urlApi, "api/pessoafisica/pessoafisica/GetLinksPaginasSemelhantes", parm);

            var retorno = result.Replace("[\"", "").Replace("\"]", "").Replace("\"", "").Split(',');

            return retorno;
        }

        private void CarregarMinimoNacional()
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/Parametro/GetMinimoNacional", null);
            ViewBag.MinimoNacional = retorno;
        }

        private string CarregarNumeroCadidaturasDegustacao(string cpf, string dataNascimento)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();
            Dictionary<string, string> header = new Dictionary<string, string>();

            parm.Add("Cpf", cpf);
            parm.Add("dataNascimento", dataNascimento);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/PessoaFisica/GetCadidaturasDegustacao", parm);
            
            return retorno.Replace("\"","");
        }

        private Models.ResultadoPesquisaSalarioBR CarregarMediaSalarioBR(int idFuncao)
        {
            string urlSalarioBR = "http://salariobr.com/api/Funcoes/RetornarInformacoesFuncao?funcao=" + idFuncao + "&funcoes_sbr=false";
            Models.ResultadoPesquisaSalarioBR ResultadoSlbr = null;

            var serializer = new JavaScriptSerializer();

            var client = new HttpClient();
            var response = client.GetAsync(urlSalarioBR).Result;
            var retorno = response.Content.ReadAsStringAsync().Result;

            ResultadoSlbr = serializer.Deserialize<Models.ResultadoPesquisaSalarioBR>(retorno);

            return ResultadoSlbr;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cadastro")]
        public PartialViewResult Cadastro(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PreCurriculo/Cadastro", payload);

            var obj = retorno.Content.ReadAsStringAsync().Result;

            dynamic objCV = JsonConvert.DeserializeObject(obj);

            model.Id = (int)objCV.Id.Value;

            if (retorno.IsSuccessStatusCode)
                return PartialView("_FinalizarPreCurriculo", model);

            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FinalizarCadastro")]
        public PartialViewResult FinalizarCadastro(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            model.CPF = model.CPF.Replace(".", "").Replace("-", "");

            if(model.DataNascimento > DateTime.Now.AddYears(-13))
            {
                @ViewBag.ErrorDataNascimento = "Data de nascimento inválida.";
                return null;
            }

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PreCurriculo/CadastroMini", payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);
                //TODO Gieyson: 
                /*
                 * Não estou logando o nome ainda por não estar sendo retornado na api, 
                 * isso para não ter que recuperar ele a 
                 * todo momento da candidatura no projeto velho
                 */
                Auth.NET45.BNEAutenticacao.LogarCandidato(content.nome.Value.ToString(), Convert.ToDecimal(content.cpf), Convert.ToDateTime(content.datanascimento));

                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }

            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FazerLogin")]
        public PartialViewResult FazerLogin(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            model.CPF = model.CPF.Replace(".", "").Replace("-", "");

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PreCurriculo/Candidatar", payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);
                //TODO Gieyson: 
                /*
                 * Não estou logando o nome ainda por não estar sendo retornado na api, 
                 * isso para não ter que recuperar ele a 
                 * todo momento da candidatura no projeto velho
                 */
                Auth.NET45.BNEAutenticacao.LogarCandidato(content.nome.Value.ToString(), Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);

                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("dd/MM/yyyy"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }
            else
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                if (content == "Inativa")
                {
                    return PartialView("_erro", model);
                }

                model.NumeroCandidaturasGratis = "0";
                return PartialView("_confirmacao", model);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "EnviarCurriculo")]
        public PartialViewResult EnviarCurriculo(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            #region Login e dados do candidato
            var user = Auth.NET45.BNEAutenticacao.User();
            #endregion

            var payload = serializer.Serialize(model);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PreCurriculo/Candidatar", payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);
                //TODO Gieyson: 
                /*
                 * Não estou logando o nome ainda por não estar sendo retornado na api, 
                 * isso para não ter que recuperar ele a 
                 * todo momento da candidatura no projeto velho
                 */
                Auth.NET45.BNEAutenticacao.LogarCandidato(content.nome.Value.ToString(), Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);

                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }

            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "VoltarparaPesquisa")]
        public ActionResult VoltarParaPesquisa(Models.PreCurriculo model)
        {
            return Content(string.Format("<script>window.location = '{0}'</script>", model.UrlVoltarLogado));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AdicionarExperiencia")]
        public PartialViewResult AdicionarExperiencia(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            #region Login e dados do candidato
            //var user = Auth.NET45.BNEAutenticacao.User();
            model.ExperienciaProfissional.IdVaga = model.IdVaga;
            model.ExperienciaProfissional.UrlPesquisa = model.UrlPesquisa;
            #endregion

            var payload = serializer.Serialize(model.ExperienciaProfissional);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarExperienciaProfissional", null, payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);

                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }

            //TODO: Charan => implementar view de erro.
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Formacao")]
        public PartialViewResult CadastrarFormacao(Models.PreCurriculo model)
        {
            model.NumeroCandidaturasGratis = "0";

            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            #region Login e dados do candidato
            //var user = Auth.NET45.BNEAutenticacao.User();

            model.Formacao.Idvaga = model.IdVaga;
            model.Formacao.UrlPesquisa = model.UrlPesquisa;
            #endregion

            var payload = serializer.Serialize(model.Formacao);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarFormacao", null, payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);

                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }

            //TODO: Charan => implementar view de erro.
            return null;
        }
    }
}