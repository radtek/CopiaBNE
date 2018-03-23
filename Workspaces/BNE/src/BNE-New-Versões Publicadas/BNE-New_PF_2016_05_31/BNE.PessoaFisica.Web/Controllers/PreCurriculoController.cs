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
using BNE.PessoaFisica.Web.Models;
using BNE.ExceptionLog.Interface;
using System.IO;
using System.Text;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class PreCurriculoController : Controller
    {
        private readonly ILogger _logger;

        public PreCurriculoController(ILogger logger)
        {
            _logger = logger;
        }
        public PreCurriculoController()
        {
        }
        // GET: PreCurriculo
        //nome e email vindos do link do jornal de vaga popup para cair preenchido os respectivos campos
        public ActionResult Index(string urlPesquisaVaga, string nome, string email, string pergunta)
        {
            Models.Vaga vaga = null;
            Models.InformacoesCurriculo informacoesCurriculo = new InformacoesCurriculo();

            informacoesCurriculo.EmpresaBloqueada = false;
            informacoesCurriculo.JaEnvioCvParaVaga = false;
            informacoesCurriculo.EstaNaRegiaoBH = false;
            informacoesCurriculo.EhVip = false;
            informacoesCurriculo.idCurriculo = 0;
            informacoesCurriculo.TemExperienciaProfissional = false;
            informacoesCurriculo.TemFormacao = false;

            try
            {
                @ViewBag.Erros = false;

                //logar candidato
                Auth.NET45.BNEAutenticacao.LogarCandidato("Sophia Natália Gomes", Convert.ToDecimal("47196597214"), Convert.ToDateTime("1980-03-30"));
                //Auth.NET45.BNEAutenticacao.LogarCandidato("Paulo", Convert.ToDecimal("64839520666"), Convert.ToDateTime("1990-08-29"));

                #region Login e dados do candidato

                var user = Auth.NET45.BNEAutenticacao.User();

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


                strIdVaga = strIdVaga == "PreCurriculo" ? "15185" : strIdVaga;
                //strIdVaga = strIdVaga == "PreCurriculo" ? "1008763" : strIdVaga;


                if (user != null)
                {
                    //carregar informações do usuario
                    informacoesCurriculo = CarregarInformacoesCurriculo(strIdVaga, user.CPF);

                    var pedir = "0";
                    // 0 - Cadastro do Pré-CV
                    // 1 - Pedir Experiencia
                    // 2 - Pedir Formação
                    // 3 - Candidatar
                    // 4 - Indicação de BH

                    //se o candidato é de BH e não é vip
                    //pedir indicações e dar uma candidatura
                    if (!informacoesCurriculo.EhVip && informacoesCurriculo.EstaNaRegiaoBH)
                    {
                        pedir = "4";
                        @ViewBag.PedirIndicacao = true;
                        @ViewBag.PedirExperiencia = false;
                        @ViewBag.PedirFormacao = false;
                        @ViewBag.EnviarCV = false;
                    }
                    else
                    {
                        pedir = informacoesCurriculo.TemFormacao ? "3" : "2";

                        if (!informacoesCurriculo.TemExperienciaProfissional)
                        {
                            @ViewBag.PedirExperiencia = true;
                            @ViewBag.PedirFormacao = false;
                            @ViewBag.EnviarCV = false;
                            @ViewBag.PedirIndicacao = false;
                        }
                        else if (!informacoesCurriculo.TemFormacao)
                        {
                            @ViewBag.PedirExperiencia = false;
                            @ViewBag.PedirFormacao = true;
                            @ViewBag.EnviarCV = false;
                            @ViewBag.PedirIndicacao = false;
                        }
                        else if (pedir == "3")
                        {
                            @ViewBag.PedirExperiencia = false;
                            @ViewBag.PedirFormacao = false;
                            @ViewBag.EnviarCV = true;
                            @ViewBag.PedirIndicacao = false;
                        }
                        else
                        {
                            @ViewBag.PedirExperiencia = false;
                            @ViewBag.PedirFormacao = false;
                            @ViewBag.EnviarCV = false;
                            @ViewBag.PedirIndicacao = false;
                        }
                    }
                }
                else
                {
                    @ViewBag.Nome = !String.IsNullOrEmpty(nome) ? nome.Replace("-", " ") : "";
                    @ViewBag.Email = !String.IsNullOrEmpty(email) ? email : "";
                    @ViewBag.PedirExperiencia = false;
                    @ViewBag.PedirFormacao = false;
                    @ViewBag.EnviarCV = false;
                    @ViewBag.PedirIndicacao = false;
                }
                #endregion



                #region Vaga
                vaga = CarregarVaga(strIdVaga);

                if (vaga != null)
                {

                    if (!String.IsNullOrEmpty(urlPesquisaVaga))
                        ViewBag.UrlPesquisa = String.Format("http://www.bne.com.br/vagas-de-emprego-para-{0}-em-{1}-{2}", vaga.Funcao, vaga.Cidade, vaga.UF).ToLower().Replace(" ", "-");

                    var eEstagio = vaga.DescricaoTipoVinculo != null ? vaga.DescricaoTipoVinculo.SingleOrDefault(p => p.Contains("Estágio")) : null;

                    //Vaga tem pergunta e veio do link de candidatura da SMS
                    ViewBag.pergunta = false;
                    if (!string.IsNullOrEmpty(pergunta))
                        ViewBag.pergunta = true;

                    if (vaga.SalarioDe == 0 && vaga.SalarioAte == 0 && eEstagio == null)
                    {
                        Models.ResultadoPesquisaSalarioBR faixaSalarial = null;

                        if (vaga.IdFuncao.HasValue)
                            faixaSalarial = CarregarMediaSalarioBR(vaga.IdFuncao.Value);

                        if (faixaSalarial != null && faixaSalarial.NomeFuncao != null && faixaSalarial.IdFuncaoSalarioBR > 0)
                        {
                            vaga.faixaSalarial = string.Format(
                                "Média salarial do mercado é de {0} até {1}",
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Trainee.ToString("C"),
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Master.ToString("C"));
                        }
                        else
                        {
                            vaga.faixaSalarial = "combinar";
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
                    ViewBag.Logado = false;
                    ViewBag.Bloqueada = false;
                    if (user != null)
                    {
                        ViewBag.JaEnviei = informacoesCurriculo.JaEnvioCvParaVaga; //JaCandidatou(vaga.IdVaga, user.CPF);
                        ViewBag.Logado = true;
                        if (vaga.FlgPremium)
                            vaga.PlanoPremium = Plano(user.CPF.ToString());

                        //Se a vaga é de empresa bloqueada pelo usuario
                        //if (EmpresaBloqueadaPeloUsuario(user.CPF, vaga.IdVaga))
                        if (informacoesCurriculo.EmpresaBloqueada)
                            ViewBag.Bloqueada = true;
                    }

                    //GetLinksPaginasSemelhantes
                    string[] urls = GetLinksPaginasSemelhantes(vaga.Funcao, vaga.Cidade, vaga.UF, vaga.DescricaoAreaBNEPesquisa);

                    vaga.LinkVagasFuncao = Helpers.SEO.LinkHelper.ObterLinkVagasFuncao(vaga.Funcao, urls[0]);
                    vaga.LinkVagasCidade = Helpers.SEO.LinkHelper.ObterLinkVagasCidade(vaga.Cidade, vaga.UF, urls[1]);
                    vaga.LinkVagasFuncaoCidade = Helpers.SEO.LinkHelper.ObterLinkVagasFuncaoCidade(vaga.Funcao, vaga.Cidade, vaga.UF, urls[2]);
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
                    ViewBag.VagaPremium = vaga.FlgPremium;

                    if (vaga.FlgPremium && user != null)
                        ViewBag.CandVip = CarregarNumeroCadidaturasDegustacao(user.CPF.ToString(), user.DataNascimento.ToString());
                    else
                        ViewBag.CandVip = "0";

                }

                var model = new Models.PreCurriculo(vaga);

                if (user != null)
                {
                    model.CPF = user.CPF.ToString();
                    model.DataNascimento = user.DataNascimento;
                    model.Nome = user.Nome;
                    model.EstaEmBH = informacoesCurriculo.EstaNaRegiaoBH;
                    model.EmpresaBloqueada = informacoesCurriculo.EmpresaBloqueada;
                    model.EhVIP = informacoesCurriculo.EhVip;
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

                if (vaga.FlgArquivada || vaga.FlgInativo)
                    model.VagaOportunidade = @ViewBag.CssVagaOportunidade = "oportunidade";
                else
                    model.VagaOportunidade = @ViewBag.CssVagaOportunidade = "vaga";
                #endregion

                return View(model);
            }
            catch (Exception ex)
            {

                var e = ex;
                StringBuilder sb = new StringBuilder();
                do
                {
                    sb.AppendLine("Exception:");
                    sb.Append(e.Message);
                    sb.AppendLine("Stack Trace:");
                    sb.Append(e.StackTrace);
                    e = e.InnerException;
                } while (e != null);

                System.IO.File.WriteAllText(Path.Combine(@"C:\erros\", Guid.NewGuid().ToString()), sb.ToString());

                _logger.Error(ex, "PF WEB PreCurriculo => index, vaga => " + vaga);
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

            parm.Add("Cpf", cpf);
            parm.Add("dataNascimento", dataNascimento.Date.ToString("yyyy-MM-dd"));


            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/PessoaFisica/GetCandidatoTemExperienciaeFormacao", header, parm);

            return retorno;
        }

        private Models.InformacoesCurriculo CarregarInformacoesCurriculo(string idVaga, decimal cpf)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();
            Dictionary<string, string> header = new Dictionary<string, string>();

            parm.Add("Cpf", cpf.ToString());
            parm.Add("idVaga", idVaga);


            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(urlApi, "api/pessoafisica/Curriculo/GetInformacoesCurriculo", header, parm);

            var result = serializer.Deserialize<Models.InformacoesCurriculo>(retorno);
            return result;
        }

        private Models.Vaga CarregarVaga(string idVaga)
        {
            Models.Vaga vaga = null;
            var retorno = "";
            try
            {
                var service = new Helpers.HttpService();
                var serializer = new JavaScriptSerializer();

                Dictionary<string, string> parm = new Dictionary<string, string>();

                parm.Add("CodigoVaga", idVaga);

                Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
                retorno = service.Get(urlApi, "api/pessoafisica/Vaga", parm);

                vaga = serializer.Deserialize<Models.Vaga>(retorno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PF WEB Carregar vaga => " + retorno);

                throw;
            }

            return vaga;
        }

        public string[] GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Dictionary<string, string> parm = new Dictionary<string, string>();

            parm.Add("funcao", funcao.Replace("#", "sharp"));
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

        private Models.PlanoPremium Plano(string cpf)
        {
            Models.PlanoPremium Plano = null;

            try
            {
                var service = new Helpers.HttpService();
                var serializer = new JavaScriptSerializer();
                Dictionary<string, string> parm = new Dictionary<string, string>();
                Dictionary<string, string> header = new Dictionary<string, string>();

                parm.Add("Cpf", cpf);

                Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
                var retorno = service.Get(urlApi, "api/pessoafisica/PessoaFisica/GetPlano", parm);

                Plano = serializer.Deserialize<Models.PlanoPremium>(retorno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PF WEB Carregar vaga");

                throw;
            }

            return Plano;
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


            return retorno.Replace("\"", "");
        }

        private Models.ResultadoPesquisaSalarioBR CarregarMediaSalarioBR(int idFuncao)
        {
            Models.ResultadoPesquisaSalarioBR ResultadoSlbr = null;

            try
            {
                string urlSalarioBR = "http://salariobr.com/api/Funcoes/RetornarInformacoesFuncao?funcao=" + idFuncao + "&funcoes_sbr=false";
                var serializer = new JavaScriptSerializer();

                var client = new HttpClient();
                var response = client.GetAsync(urlSalarioBR).Result;
                var retorno = response.Content.ReadAsStringAsync().Result;

                ResultadoSlbr = serializer.Deserialize<Models.ResultadoPesquisaSalarioBR>(retorno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PF WEB Carregar media salarial");

                throw;
            }

            return ResultadoSlbr;
        }

        private List<Models.Pergunta> CarregarPerguntas(int idVaga)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();
            List<Models.Pergunta> lista = null;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("idVaga", idVaga.ToString());
            Uri url = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Get(url, "api/pessoafisica/Vaga/GetPergunta", param);
            lista = serializer.Deserialize<List<Models.Pergunta>>(retorno);

            return lista;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cadastro")]
        public PartialViewResult Cadastro(Models.PreCurriculo model)
        {
            try
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
                {
                    try
                    {
                        #region Integrar com Allin
                        //Dictionary<string, string> parmIntegrador = new Dictionary<string, string>();

                        //parmIntegrador.Add("idPre", model.Id.ToString());

                        //var integrouAllin = service.Post(urlApi, "api/pessoafisica/IntegradorPreCurriculoAllin?idPreCurriculo=" + model.Id, null, parmIntegrador.ToString());
                        #endregion

                        return PartialView("_FinalizarPreCurriculo", model);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "PF WEB integra com Allin");

                        throw;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "PF WEB cadastro");

                throw;
            }

            return null;
        }

        public ActionResult CompraCandidatura(Models.PreCurriculo model)
        {
            var user = Auth.NET45.BNEAutenticacao.User();

            if (user == null)
                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome.ToString(), Convert.ToDecimal(model.CPF), Convert.ToDateTime(model.DataNascimento));
            return Content(string.Format("<script>window.location = '{0}'</script>", model.UrlVoltarLogado));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FinalizarCadastro")]
        public PartialViewResult FinalizarCadastro(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            model.CPF = model.CPF.Replace(".", "").Replace("-", "");

            if (model.DataNascimento > DateTime.Now.AddYears(-13))
            {
                @ViewBag.ErrorDataNascimento = "Data de nascimento inválida.";
                return null;
            }

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);

            #region Perguntas
            if (model.vagaTela.perguntas == null)
            {
                model.vagaTela.perguntas = CarregarPerguntas(model.IdVaga.Value);
                if (model.vagaTela.perguntas.Count > 0)
                {
                    model.Candidatar = false;
                    var pay = serializer.Serialize(model);
                    var ret = service.Post(urlApi, "api/pessoafisica/PreCurriculo/CadastroMini", pay);

                    if (ret.IsSuccessStatusCode)
                    {
                        var content = (dynamic)JsonConvert.DeserializeObject(ret.Content.ReadAsStringAsync().Result);
                        Auth.NET45.BNEAutenticacao.LogarCandidato(content.nome.Value.ToString(), Convert.ToDecimal(content.cpf), Convert.ToDateTime(content.datanascimento));

                    }
                    return PartialView("_Pergunta", model);
                }

            }


            #endregion

            model.Candidatar = true;//sem pergunta candidata direto
            var payload = serializer.Serialize(model);

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
                if (model.vagaTela.FlgPremium && model.vagaTela.PlanoPremium == null)
                    model.vagaTela.PlanoPremium = Plano(content.cpf.Value.ToString());
                //Salvou o curriculo mas não fez a candidatura pela vaga ser premium

                #region Integrar com Allin
                //Dictionary<string, string> parmIntegrador = new Dictionary<string, string>();
                //parmIntegrador.Add("idPre", model.Id.ToString());
                //var integrouAllin = service.Post(urlApi, "api/pessoafisica/IntegradorPreCurriculoAllin/RemovePreCurriculo?idPreCurriculoRemove=" + model.Id, null, parmIntegrador.ToString());
                #endregion

                if (!Convert.ToBoolean(content.candidatura))
                    return PartialView("_ConfirmacaoPreCurriculo", model);

                return PartialView("_confirmacao", model);
            }
            else
            {
                @ViewBag.Erros = "Não foi possível efetuar a sua candidatura!";
                return PartialView("_FinalizarPreCurriculo", model);
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FazerLogin")]
        public PartialViewResult FazerLogin(Models.PreCurriculo model)
        {
            decimal cpf = Convert.ToDecimal(model.CPF.Replace(".", "").Replace("-", ""));

            Auth.NET45.BNEAutenticacao.LogarCandidato("", cpf, model.DataNascimento.Value, true);
            var user = Auth.NET45.BNEAutenticacao.User();

            if (user == null)
            {
                model.NumeroCandidaturasGratis = "Erro";
                return PartialView("_confirmacao", model);
            }

            Models.InformacoesCurriculo informacoesCurriculo = CarregarInformacoesCurriculo(model.IdVaga.Value.ToString(), user.CPF);

            if (informacoesCurriculo.EstaNaRegiaoBH)
            {
                @ViewBag.Erros = false;
                return PartialView("_PedirIndicaoBH", model);
            }

            if (informacoesCurriculo.EmpresaBloqueada)
            {
                Auth.NET45.BNEAutenticacao.LogarCandidato("", cpf, model.DataNascimento.Value, true);
                return PartialView("_VagaDeEmpresaBloqueadaPeloUsuario", model);
            }

            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            model.CPF = model.CPF.Replace(".", "").Replace("-", "");
            model.Nome = "";

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
                model.Nome = content.nome.Value.ToString();
                if (model.vagaTela.FlgPremium && model.vagaTela.PlanoPremium == null)
                    model.vagaTela.PlanoPremium = Plano(content.cpf.Value.ToString());

                return PartialView("_confirmacao", model);
            }
            else
            {
                model.NumeroCandidaturasGratis = "Erro";
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

            #region CandidatoDeBHNaoVip
            if (!model.EhVIP && model.EstaEmBH)
            {
                @ViewBag.Erros = false;
                return PartialView("_PedirIndicaoBH", model);
            }
            #endregion

            #region Perguntas
            if (model.vagaTela.perguntas == null)
            {
                model.vagaTela.perguntas = CarregarPerguntas(model.IdVaga.Value);
                if (model.vagaTela.perguntas.Count > 0)
                {
                    return PartialView("_Pergunta", model);
                }
            }


            #endregion

            #region [Oportunidade]
            if (model.vagaTela.FlgArquivada && !model.Candidatar)
            {
                model.Candidatar = true;
                return PartialView("_EnviarCV", model);
            }

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
                if (model.vagaTela.FlgPremium && model.vagaTela.PlanoPremium == null)
                    model.vagaTela.PlanoPremium = Plano(content.cpf.Value.ToString());
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


            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);

            #region Perguntas
            if (model.vagaTela.perguntas == null)
            {
                model.vagaTela.perguntas = CarregarPerguntas(model.IdVaga.Value);
                if (model.vagaTela.perguntas.Count > 0)
                {
                    model.ExperienciaProfissional.Candidatar = false;
                    var pay = serializer.Serialize(model.ExperienciaProfissional);
                    var ret = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarExperienciaProfissional", pay);

                    if (ret.IsSuccessStatusCode)
                        return PartialView("_Pergunta", model);

                }
            }


            #endregion

            model.ExperienciaProfissional.Candidatar = true;//sem pergunta candidata direto
            var payload = serializer.Serialize(model.ExperienciaProfissional);

            var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarExperienciaProfissional", null, payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);
                if (model.vagaTela.FlgPremium && model.vagaTela.PlanoPremium == null)
                    model.vagaTela.PlanoPremium = Plano(content.cpf.Value.ToString());
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



            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);

            #region Perguntas

            if (model.vagaTela.perguntas == null)
            {
                model.vagaTela.perguntas = CarregarPerguntas(model.IdVaga.Value);
                if (model.vagaTela.perguntas.Count > 0)
                {
                    model.Formacao.Candidatar = false;
                    var pay = serializer.Serialize(model.Formacao);
                    var ret = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarFormacao", pay);

                    if (ret.IsSuccessStatusCode)
                        return PartialView("_Pergunta", model);

                }
            }
            #endregion

            model.Formacao.Candidatar = true;//sem pergunta candidata direto
            var payload = serializer.Serialize(model.Formacao);

            var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/CadastrarFormacao", null, payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(content.datanascimento.Value.ToString("yyyy-MM-dd")), true);
                if (model.vagaTela.FlgPremium && model.vagaTela.PlanoPremium == null)
                    model.vagaTela.PlanoPremium = Plano(content.cpf.Value.ToString());
                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), content.datanascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacao", model);
            }

            //TODO: Charan => implementar view de erro.
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "IndicarAmigos")]
        public PartialViewResult IndicarAmigos(Models.PreCurriculo model)
        {
            var service = new Helpers.HttpService();
            var serializer = new JavaScriptSerializer();

            Indicacao objIndicacao = new Indicacao();
            IList<AmigoIndicado> listaIndicados = new List<AmigoIndicado>();

            listaIndicados.Add(model.IndicadoUm);
            listaIndicados.Add(model.IndicadoDois);
            listaIndicados.Add(model.IndicadoTres);

            objIndicacao.IdVaga = model.IdVaga.Value;
            objIndicacao.CPF = model.CPF.Replace(".", "").Replace("-", "");
            objIndicacao.listaAmigos = listaIndicados;

            var payload = serializer.Serialize(objIndicacao);

            Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
            var retorno = service.Post(urlApi, "api/pessoafisica/PessoaFisica/IndicarAmigos", null, payload);

            if (retorno.IsSuccessStatusCode)
            {
                var content = (dynamic)JsonConvert.DeserializeObject(retorno.Content.ReadAsStringAsync().Result);

                Auth.NET45.BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(content.cpf.Value.ToString()), Convert.ToDateTime(model.DataNascimento.Value.ToString("yyyy-MM-dd")), true);
                model.NumeroCandidaturasGratis = CarregarNumeroCadidaturasDegustacao(content.cpf.Value.ToString(), model.DataNascimento.Value.ToString("yyyy-MM-dd"));
                model.UrlVoltarLogado = content.url;
                return PartialView("_confirmacaoIndicacao", model);
            }
            else
            {
                ViewBag.Erros = true;
                return PartialView("_PedirIndicaoBH", model);
            }
        }


    }
}