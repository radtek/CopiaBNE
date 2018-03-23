using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using AutoMapper;
using BNE.Auth.NET45;
using BNE.Core.ExtensionsMethods;
using BNE.PessoaFisica.API;
using BNE.PessoaFisica.API.Models;
using BNE.PessoaFisica.Web.Attributes;
using BNE.PessoaFisica.Web.Helpers.SEO;
using BNE.PessoaFisica.Web.Models;
using log4net;
using Newtonsoft.Json;
using CandidaturaDegustacao = BNE.PessoaFisica.API.Models.CandidaturaDegustacao;
using Curriculo = BNE.PessoaFisica.Web.Models.Curriculo;
using NavegacaoVaga = BNE.PessoaFisica.Web.Models.NavegacaoVaga;
using Pergunta = BNE.PessoaFisica.Web.Models.Pergunta;
using PreCurriculo = BNE.PessoaFisica.Web.Models.PreCurriculo;
using Vaga = BNE.PessoaFisica.Web.Models.Vaga;
using System.Dynamic;
using System.ComponentModel;

namespace BNE.PessoaFisica.Web.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class PreCurriculoController : Controller
    {
        private readonly IPessoaFisicaAPI _apiClient;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public PreCurriculoController(IPessoaFisicaAPI apiClient, ILog logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _apiClient = apiClient;
            ServicePointManager.DefaultConnectionLimit = 10000;
            ServicePointManager.Expect100Continue = false;
        }

        public async Task<ActionResult> Index(string urlPesquisaVaga, string nome, string email, string pergunta)
        {
            ViewBag.ExibirPossoAjudar = false;

            Vaga vaga = null;
            var informacoesCurriculo = new InformacaoCurriculo();
            var user = BNEAutenticacao.User();
            try
            {
                #region Login e dados do candidato
                var urlOrigem = Request.UrlReferrer;

                int? codigoPesquisa = null;
                var identificador = RouteData.Values["Identificador"] != null ? RouteData.Values["Identificador"].ToString() : "";

                int idVaga;
                if (identificador.Contains('&'))
                    idVaga = Convert.ToInt32(identificador.Substring(0, identificador.IndexOf('&')));
                else
                    idVaga = Convert.ToInt32(identificador);

                int idVagaNavegacao;
                if (Request.QueryString["sourcejob"] == null || !Int32.TryParse(Request.QueryString["sourcejob"], out idVagaNavegacao))
                {
                    idVagaNavegacao = idVaga;
                }

                int idVagaPaginacao = 0;
                if (RouteData.Values["CodigoPesquisa"] != null)
                {
                    codigoPesquisa = Convert.ToInt32(RouteData.Values["CodigoPesquisa"]);

                    if (Request.QueryString["jobindex"] == null || !Int32.TryParse(Request.QueryString["jobindex"], out idVagaPaginacao))
                    {
                        idVagaPaginacao = 0;
                    }
                }

                //Inicializa variaveis ViewBag
                ViewBag.Nome = !string.IsNullOrEmpty(nome) ? nome.Replace("-", " ") : "";
                ViewBag.Email = !string.IsNullOrEmpty(email) ? email : "";
                ViewBag.PedirExperiencia = false;
                ViewBag.PedirFormacao = false;
                ViewBag.EnviarCV = false;
                ViewBag.PedirIndicacao = false;

                if (user != null)
                {
                    //carregar informações do usuario
                    informacoesCurriculo = await CarregarInformacoesCurriculo(user.CPF, idVaga);

                    // 0 - Cadastro do Pré-CV
                    // 1 - Pedir Experiencia
                    // 2 - Pedir Formação
                    // 3 - Candidatar
                    // 4 - Indicação de BH

                    //se o candidato é de BH e não é vip
                    //pedir indicações e dar uma candidatura
                    if (informacoesCurriculo != null)
                    {
                        var pedir = "0";

                        if (!informacoesCurriculo.CurriculoVIP && informacoesCurriculo.EstaNaRegiaoBH && (informacoesCurriculo.SaldoCandidatura <= 0))
                        {
                            pedir = "4";
                            ViewBag.PedirIndicacao = true;
                            ViewBag.PedirExperiencia = false;
                            ViewBag.PedirFormacao = false;
                            ViewBag.EnviarCV = false;
                        }
                        else
                        {
                            pedir = informacoesCurriculo.TemFormacao ? "3" : "2";

                            if (!informacoesCurriculo.DisseQueNaoTemExperiencia && !informacoesCurriculo.TemExperienciaProfissional)
                            {
                                ViewBag.PedirExperiencia = true;
                                ViewBag.PedirFormacao = false;
                                ViewBag.EnviarCV = false;
                                ViewBag.PedirIndicacao = false;
                            }
                            else if (!informacoesCurriculo.TemFormacao)
                            {
                                ViewBag.PedirExperiencia = false;
                                ViewBag.PedirFormacao = true;
                                ViewBag.EnviarCV = false;
                                ViewBag.PedirIndicacao = false;
                            }
                            else if (pedir == "3")
                            {
                                ViewBag.PedirExperiencia = false;
                                ViewBag.PedirFormacao = false;
                                ViewBag.EnviarCV = true;
                                ViewBag.PedirIndicacao = false;
                            }
                            else
                            {
                                ViewBag.PedirExperiencia = false;
                                ViewBag.PedirFormacao = false;
                                ViewBag.EnviarCV = false;
                                ViewBag.PedirIndicacao = false;
                            }
                        }

                        //Exibir banner posso ajudar
                        ViewBag.ExibirPossoAjudar = informacoesCurriculo.CurriculoVIP;
                    }
                }
                #endregion

                #region Vaga
                vaga = await CarregarVaga(idVaga);

                if ((vaga != null) && !vaga.FlgInativo)
                {
                    if (string.IsNullOrWhiteSpace(urlPesquisaVaga))
                        ViewBag.UrlPesquisa = string.Format("http://www.bne.com.br/vagas-de-emprego-para-{0}-em-{1}-{2}", vaga.Funcao, vaga.Cidade, vaga.UF).ToLower().Replace(" ", "-");
                    else
                        ViewBag.UrlPesquisa = urlPesquisaVaga;

                    var eEstagio = vaga.DescricaoTipoVinculo != null ? vaga.DescricaoTipoVinculo.SingleOrDefault(p => p.Contains("Estágio")) : null;

                    //Vaga tem pergunta e veio do link de candidatura da SMS
                    ViewBag.pergunta = false;
                    if (!string.IsNullOrEmpty(pergunta))
                        ViewBag.pergunta = true;

                    if ((vaga.SalarioDe == 0) && (vaga.SalarioAte == 0) && (eEstagio == null))
                    {

                        ResultadoPesquisaSalarioBR faixaSalarial = null;

                        if (vaga.IdFuncao.HasValue)
                            faixaSalarial = await CarregarMediaSalarioBR(vaga.IdFuncao.Value);

                        if ((faixaSalarial != null) && (faixaSalarial.NomeFuncao != null) && (faixaSalarial.IdFuncaoSalarioBR > 0) && (faixaSalarial.DetalhesFuncao.SalarioPequena != null))
                        {
                            vaga.faixaSalarialDe = faixaSalarial.DetalhesFuncao.SalarioPequena.Trainee;
                            vaga.faixaSalarialAte = faixaSalarial.DetalhesFuncao.SalarioPequena.Master;
                            vaga.faixaSalarial = string.Format(
                                "Média salarial do mercado é de {0} até {1}",
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Trainee.ToString("C"),
                                faixaSalarial.DetalhesFuncao.SalarioPequena.Master.ToString("C"));
                        }
                        else
                            vaga.faixaSalarial = "combinar";
                    }
                    else
                    {
                        vaga.faixaSalarial = "combinar";
                    }

                    vaga.urlPesquisa = urlOrigem != null ? urlOrigem.ToString() : "";
                    vaga.IdPesquisa = codigoPesquisa.HasValue ? codigoPesquisa.Value : 0;

                    //Checar se o candidato ja enviou o CV
                    ViewBag.JaEnviei = false;
                    ViewBag.Logado = false;
                    ViewBag.Bloqueada = false;
                    ViewBag.IndicadoBNE = false;
                    if (user != null)
                    {
                        if (informacoesCurriculo != null)
                        {
                            ViewBag.JaEnviei = informacoesCurriculo.JaEnvioCvParaVaga;
                            ViewBag.IndicadoBNE = informacoesCurriculo.IndicadoBNE;
                        }
                        ViewBag.Logado = true;

                        if (vaga.FlgPremium)
                            vaga.PlanoPremium = await Plano(user.CPF);

                        ViewBag.Bloqueada = false;
                        if ((informacoesCurriculo != null) && informacoesCurriculo.EmpresaBloqueada)
                            ViewBag.Bloqueada = true;
                    }

                    vaga.LinkVagasFuncao = LinkHelper.ObterLinkVagasFuncao(vaga.Funcao, vaga.LinkPaginasSemelhantes[0]);
                    vaga.LinkVagasCidade = LinkHelper.ObterLinkVagasCidade(vaga.Cidade, vaga.UF, vaga.LinkPaginasSemelhantes[1]);
                    vaga.LinkVagasFuncaoCidade = LinkHelper.ObterLinkVagasFuncaoCidade(vaga.Funcao, vaga.Cidade, vaga.UF, vaga.LinkPaginasSemelhantes[2]);
                    vaga.LinkVagasArea = LinkHelper.ObterLinkVagasArea(vaga.DescricaoAreaBNEPesquisa, vaga.LinkPaginasSemelhantes[3]);

                    //Montar description SEO da vaga
                    var descriptionSeo = new
                    {
                        vaga.Funcao,
                        vaga.Cidade,
                        Salario = vaga.faixaSalarial,
                        vaga.CodigoVaga
                    };

                    ViewBag.Title = SitemapHelper.MontarTituloVaga(vaga.Funcao, vaga.eEstagio, vaga.DescricaoAreaBNEPesquisa, 1, vaga.Cidade, vaga.UF, vaga.UF, vaga.IdVaga);
                    ViewBag.Description = descriptionSeo.ToString("Procurando emprego? Candidate-se para a vaga de {Funcao} em {Cidade}. {Salario}. {CodigoVaga}. Banco Nacional de Empregos | BNE.");
                    ViewBag.VagaPremium = vaga.FlgPremium;

                    if (vaga.FlgPremium && (user != null))
                        ViewBag.CandVip = CarregarNumeroCadidaturasDegustacao(user.CPF, user.DataNascimento);
                    else
                        ViewBag.CandVip = "0";

                    vaga.Navegacao = await NavegacaoVaga(idVaga, codigoPesquisa, idVagaNavegacao, idVagaPaginacao > 0 ? idVagaPaginacao : (int?)null);
                }
                else
                {
                    //return View("_vagaNaoEncontrada");
                    _logger.Error(new Exception("Vaga não encontrada: " + idVaga));
                    return Redirect("http://www.bne.com.br");
                }

                var model = new Curriculo(vaga);

                if (user != null)
                {
                    model.CPF = user.CPF.ToString();
                    model.DataNascimento = user.DataNascimento;
                    model.Nome = user.Nome;
                    if(informacoesCurriculo !=null)
                            model.IdCurriculo = informacoesCurriculo.IdCurriculo;

                    if (informacoesCurriculo != null)
                    {
                        model.EstaEmBH = informacoesCurriculo.EstaNaRegiaoBH;
                        model.EmpresaBloqueada = informacoesCurriculo.EmpresaBloqueada;
                        model.CurriculoVIP = informacoesCurriculo.CurriculoVIP;
                    }
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
                    model.VagaOportunidade = ViewBag.CssVagaOportunidade = "oportunidade";
                else
                    model.VagaOportunidade = ViewBag.CssVagaOportunidade = "vaga";
                #endregion

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.Error("PF WEB PreCurriculo => index " + JsonConvert.SerializeObject(user) + JsonConvert.SerializeObject(vaga) + RouteData.Values["Identificador"], ex);
            }
            return null;
        }

        public PartialViewResult FazerLogin()
        {
            return PartialView("_FazerLogin");
        }

        public PartialViewResult Passo1()
        {
            return PartialView("_CadastroPasso1");
        }

        private async Task<InformacaoCurriculo> CarregarInformacoesCurriculo(decimal cpf, int idVaga)
        {
            try
            {
                var retorno = await _apiClient.Curriculo.GetInformacoesCurriculoWithHttpMessagesAsync(cpf, idVaga);

                if (retorno.Response.IsSuccessStatusCode)
                    return _mapper.Map<InformacaoCurriculoResponse, InformacaoCurriculo>(retorno.Body);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            return null;
        }

        private async Task<Vaga> CarregarVaga(int idVaga)
        {
            Vaga vaga = null;
            try
            {
                var retorno = await _apiClient.Vaga.GetWithHttpMessagesAsync(idVaga);
                if (retorno.Response.IsSuccessStatusCode)
                {
                    vaga = _mapper.Map<VagaResponse, Vaga>(retorno.Body);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            return vaga;
        }

        private async Task<NavegacaoVaga> NavegacaoVaga(int job, int? jobSearch, int? sourceJob, int? jobIndex)
        {
            NavegacaoVaga navegacaoVaga = null;
            try
            {
                var retorno = await _apiClient.NavegacaoVaga.GetWithHttpMessagesAsync(job, jobSearch, sourceJob, jobIndex);
                if (retorno.Response.IsSuccessStatusCode)
                {
                    navegacaoVaga = _mapper.Map<NavegacaoVagaResponse, NavegacaoVaga>(retorno.Body);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            return navegacaoVaga;
        }

        private async Task<PlanoPremium> Plano(decimal cpf)
        {
            PlanoPremium plano = null;

            try
            {
                var retorno = await _apiClient.Plano.GetPlanoWithHttpMessagesAsync(cpf);
                if (retorno.Response.IsSuccessStatusCode)
                    plano = _mapper.Map<PlanoResponse, PlanoPremium>(retorno.Body);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return plano;
        }

        /// <summary>
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        private async Task<int> CarregarNumeroCadidaturasDegustacao(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                var retorno = await _apiClient.Curriculo.GetCadidaturasDegustacaoWithHttpMessagesAsync(cpf, dataNascimento);

                if (retorno.Response.IsSuccessStatusCode)
                    return _mapper.Map<CandidaturaDegustacao, Models.CandidaturaDegustacao>(retorno.Body).QuantidadeCandidatura;
            }
            catch (Exception ex)
            {
                _logger.Error($"CPF {cpf} DN {dataNascimento}", ex);
            }
            return 0;
        }

        private async Task<ResultadoPesquisaSalarioBR> CarregarMediaSalarioBR(int idFuncao)
        {
            ResultadoPesquisaSalarioBR resultadoSlbr = null;

            var cts = new CancellationTokenSource();

            try
            {
                var urlSalarioBR = "http://salariobr.com/api/Funcoes/RetornarInformacoesFuncao?funcao=" + idFuncao + "&funcoes_sbr=false";
                var serializer = new JavaScriptSerializer();

                var client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true }) { Timeout = TimeSpan.FromSeconds(5) };

                cts.CancelAfter(TimeSpan.FromSeconds(5));
                var task = await client.GetAsync(urlSalarioBR, cts.Token);

                var retorno = task.Content.ReadAsStringAsync().Result;

                resultadoSlbr = serializer.Deserialize<ResultadoPesquisaSalarioBR>(retorno);
            }
            catch (Exception ex)
            {
                _logger.Error("PF WEB Carregar media salarial", ex);
            }

            return resultadoSlbr;
        }

        private async Task<IEnumerable<Pergunta>> CarregarPerguntas(int idVaga)
        {
            try
            {
                var retorno = await _apiClient.VagaPergunta.GetPerguntasWithHttpMessagesAsync(idVaga);
                if (retorno.Response.IsSuccessStatusCode)
                    return _mapper.Map<IEnumerable<VagaPerguntaResponse>, IEnumerable<Pergunta>>(retorno.Body);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Cadastro")]
        public async Task<PartialViewResult> Cadastro(PreCurriculo model)
        {
            try
            {
                var command = _mapper.Map<PreCurriculo, SalvarPreCurriculoCommand>(model);
                var retorno = await _apiClient.PreCurriculo.CadastrarWithHttpMessagesAsync(command);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    model.Id = retorno.Body.Id;
                    model.DDDCelular = retorno.Body.DDDCelular;
                    model.NumeroCelular = retorno.Body.Celular;
                    return PartialView("_FinalizarPreCurriculo", model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("PF WEB cadastro", ex);

                throw;
            }

            return null;
        }

        public ActionResult CompraCandidatura(Curriculo model)
        {
            try
            {
                var user = BNEAutenticacao.User();

                if (user == null)
                    BNEAutenticacao.LogarCandidato(model.Nome, Convert.ToDecimal(model.CPF), Convert.ToDateTime(model.DataNascimento));
                return Content(string.Format("<script>window.location = '{0}'</script>", model.UrlVoltarLogado));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                throw;
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FinalizarCadastro")]
        public async Task<PartialViewResult> FinalizarCadastro(Curriculo model)
        {
            try
            {
                if (model.DataNascimento > DateTime.Now.AddYears(-13))
                {
                    ViewBag.ErrorDataNascimento = "Data de nascimento inválida.";
                    return null;
                }

                #region Perguntas
                if (model.Vaga.Perguntas == null)
                {
                    model.Vaga.Perguntas = (List<Pergunta>)await CarregarPerguntas((int)model.IdVaga);
                    if (model.Vaga.Perguntas.Any())
                    {
                        model.Candidatar = false;

                        var command = _mapper.Map<Curriculo, SalvarCurriculoCommand>(model);
                        var retornoPerguntas = await _apiClient.Curriculo.CadastrarWithHttpMessagesAsync(command);
                        if (retornoPerguntas.Response.IsSuccessStatusCode)
                        {
                            var cpf = retornoPerguntas.Body.CPF;
                            var dataNascimento = retornoPerguntas.Body.DataNascimento;
                            BNEAutenticacao.LogarCandidato(retornoPerguntas.Body.Nome, Convert.ToDecimal(cpf), dataNascimento);
                        }
                        return PartialView("_Pergunta", model);
                    }
                }
                #endregion

                model.Candidatar = true; //sem pergunta candidata direto
                var payload = _mapper.Map<Curriculo, SalvarCurriculoCommand>(model);
                var retorno = await _apiClient.Curriculo.CadastrarWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    if (!Convert.ToBoolean(retorno.Body.Candidatou))
                        return PartialView("_ConfirmacaoPreCurriculo", await Confirmacao(model, model.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, string.Empty, retorno.Body.Candidatou));

                    return PartialView("_Confirmacao", await Confirmacao(model, model.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, string.Empty, retorno.Body.Candidatou));
                }
                ViewBag.Erros = "Não foi possível efetuar a sua candidatura!";
                return PartialView("_FinalizarPreCurriculo", _mapper.Map<Curriculo, PreCurriculo>(model));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "FazerLogin")]
        public async Task<PartialViewResult> FazerLogin(Curriculo model)
        {
            try
            {
                var cpfModel = Convert.ToDecimal(model.CPF.Replace(".", "").Replace("-", ""));

                var informacoesCurriculo = await CarregarInformacoesCurriculo(cpfModel, model.IdVaga.Value);

                if (informacoesCurriculo == null)
                    return PartialView("_CadastroPasso1", model);

                model.CurriculoVIP = informacoesCurriculo.CurriculoVIP;

                if (informacoesCurriculo.EstaNaRegiaoBH)
                {
                    ViewBag.Erros = false;
                    return PartialView("_PedirIndicacaoBH", model);
                }

                if (informacoesCurriculo.EmpresaBloqueada)
                {
                    BNEAutenticacao.LogarCandidato("", Convert.ToDecimal(cpfModel), model.DataNascimento.Value, true);
                    return PartialView("_VagaDeEmpresaBloqueadaPeloUsuario", model);
                }

                #region Perguntas
                if (model.Vaga.Perguntas == null)
                {
                    model.Vaga.Perguntas = (List<Pergunta>)await CarregarPerguntas(model.IdVaga.Value);
                    if (model.Vaga.Perguntas.Any())
                        return PartialView("_Pergunta", model);
                }
                #endregion

                var payload = _mapper.Map<Curriculo, SalvarCurriculoCommand>(model);
                var retorno = await _apiClient.Curriculo.CandidatarWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    if (retorno.Body.UsuarioInvalido)
                        return PartialView("_ConfirmacaoUsuarioInvalido", model);
                    return PartialView("_Confirmacao", await Confirmacao(model, retorno.Body.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, retorno.Body.Url, retorno.Body.Candidatou));
                }
                //TODO: CPF ou data de nascimento não confere?
                //model.NumeroCandidaturasGratis = "Erro";
                return PartialView("_Confirmacao", model);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(model), ex);
                //throw;
            }
            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "EnviarCurriculo")]
        public async Task<PartialViewResult> EnviarCurriculo(Curriculo model)
        {
            try
            {
                #region CandidatoDeBHNaoVip
                if (!model.CurriculoVIP && model.EstaEmBH)
                {
                    ViewBag.Erros = false;
                    return PartialView("_PedirIndicacaoBH", model);
                }
                #endregion

                #region Perguntas
                if (model.Vaga.Perguntas == null)
                {
                    model.Vaga.Perguntas = (List<Pergunta>)await CarregarPerguntas(model.IdVaga.Value);
                    if (model.Vaga.Perguntas.Any())
                        return PartialView("_Pergunta", model);
                }
                #endregion

                #region [Oportunidade]
                if (model.Vaga.FlgArquivada && !model.Candidatar)
                {
                    model.Candidatar = true;
                    return PartialView("_Oportunidade", model);
                }
                #endregion

                var payload = _mapper.Map<Curriculo, SalvarCurriculoCommand>(model);
                var retorno = await _apiClient.Curriculo.CandidatarWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                    return PartialView("_Confirmacao", await Confirmacao(model, retorno.Body.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, retorno.Body.Url, retorno.Body.Candidatou));

                ViewBag.ErrosCandidatura = true;
                return PartialView("_EnviarCV", model);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(model), ex);
                throw;
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "VoltarparaPesquisa")]
        public ActionResult VoltarParaPesquisa(Curriculo model)
        {
            return Content(string.Format("<script>window.location = '{0}'</script>", model.UrlVoltarLogado));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "AdicionarExperiencia")]
        public async Task<PartialViewResult> AdicionarExperiencia(Curriculo model)
        {
            try
            {
                #region Login e dados do candidato

                //var user = Auth.NET45.BNEAutenticacao.User();
                model.ExperienciaProfissional.IdVaga = model.IdVaga;
                model.ExperienciaProfissional.UrlPesquisa = model.UrlPesquisa;
                #endregion

                #region Perguntas
                if (model.Vaga.Perguntas == null)
                {
                    model.Vaga.Perguntas = (List<Pergunta>)await CarregarPerguntas(model.IdVaga.Value);
                    if (model.Vaga.Perguntas.Any())
                    {
                        model.ExperienciaProfissional.Candidatar = false;

                        var payloadPerguntas = _mapper.Map<ExperienciaProfissional, SalvarExperienciaProfissionalCommand>(model.ExperienciaProfissional);
                        var retornoPerguntas = await _apiClient.Curriculo.CadastrarExperienciaProfissionalWithHttpMessagesAsync(payloadPerguntas);

                        if (retornoPerguntas.Response.IsSuccessStatusCode)
                            return PartialView("_Pergunta", model);
                    }
                }
                #endregion

                model.ExperienciaProfissional.Candidatar = true; //sem pergunta candidata direto
                var payload = _mapper.Map<ExperienciaProfissional, SalvarExperienciaProfissionalCommand>(model.ExperienciaProfissional);
                var retorno = await _apiClient.Curriculo.CadastrarExperienciaProfissionalWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                    return PartialView("_Confirmacao", await Confirmacao(model, retorno.Body.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, retorno.Body.Url, true));

                //TODO: Charan => implementar view de erro.
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(model), ex);
                throw;
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Formacao")]
        public async Task<PartialViewResult> AdicionarFormacao(Curriculo model)
        {
            try
            {
                model.NumeroCandidaturasGratis = 0;
                model.Formacao.Idvaga = model.IdVaga;
                model.Formacao.UrlPesquisa = model.UrlPesquisa;

                #region Perguntas
                if (model.Vaga.Perguntas == null)
                {
                    model.Vaga.Perguntas = (List<Pergunta>)await CarregarPerguntas(model.IdVaga.Value);
                    if (model.Vaga.Perguntas.Any())
                    {
                        model.Formacao.Candidatar = false;
                        var payloadPerguntas = _mapper.Map<Formacao, SalvarFormacaoCommand>(model.Formacao);
                        var retornoPerguntas = await _apiClient.Curriculo.CadastrarFormacaoWithHttpMessagesAsync(payloadPerguntas);

                        if (retornoPerguntas.Response.IsSuccessStatusCode)
                            return PartialView("_Pergunta", model);
                    }
                }
                #endregion

                model.Formacao.Candidatar = true; //sem pergunta candidata direto

                var payload = _mapper.Map<Formacao, SalvarFormacaoCommand>(model.Formacao);
                var retorno = await _apiClient.Curriculo.CadastrarFormacaoWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                    return PartialView("_Confirmacao", await Confirmacao(model, retorno.Body.Nome, retorno.Body.CPF, retorno.Body.DataNascimento, retorno.Body.Url, true));

                //TODO: Charan => implementar view de erro.
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(model), ex);
                throw;
            }
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "IndicarAmigos")]
        public async Task<PartialViewResult> IndicarAmigos(Curriculo model)
        {
            try
            {
                var objIndicacao = new Indicacao();
                IList<AmigoIndicado> listaIndicados = new List<AmigoIndicado>();

                listaIndicados.Add(model.IndicadoUm);
                listaIndicados.Add(model.IndicadoDois);
                listaIndicados.Add(model.IndicadoTres);

                objIndicacao.IdVaga = model.IdVaga.Value;
                objIndicacao.CPF = model.CPF.Replace(".", "").Replace("-", "");
                objIndicacao.ListaAmigos = listaIndicados;

                var payload = _mapper.Map<Indicacao, IndicarAmigosCommand>(objIndicacao);
                var retorno = await _apiClient.PessoaFisica.IndicarAmigosWithHttpMessagesAsync(payload);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    BNEAutenticacao.LogarCandidato(retorno.Body.Nome, Convert.ToDecimal(retorno.Body.CPF), model.DataNascimento.Value, true);
                    model.NumeroCandidaturasGratis = await CarregarNumeroCadidaturasDegustacao(retorno.Body.CPF, model.DataNascimento.Value);
                    model.UrlVoltarLogado = retorno.Body.Url;
                    return PartialView("_ConfirmacaoIndicacao", model);
                }
                ViewBag.Erros = true;
                return PartialView("_PedirIndicacaoBH", model);
            }
            catch (Exception ex)
            {
                _logger.Error(JsonConvert.SerializeObject(model), ex);
                throw;
            }
        }

        private async Task<Curriculo> Confirmacao(Curriculo model, string nome, decimal cpf, DateTime dataNascimento, string url, bool? candidatou)
        {
            BNEAutenticacao.LogarCandidato(nome, cpf, dataNascimento, true);

            if (model.Vaga.FlgPremium && (model.Vaga.PlanoPremium == null))
                model.Vaga.PlanoPremium = await Plano(cpf);

            model.NumeroCandidaturasGratis = await CarregarNumeroCadidaturasDegustacao(cpf, dataNascimento.Date);
            model.UrlVoltarLogado = url;

            if (candidatou.HasValue)
                model.Candidatou = candidatou.Value;

            return model;
        }


        public async Task<PartialViewResult> DadosDaEmpresa2(int idVaga, int? idCurriculo)
        {
            var model = new Models.DadosEmpresa();
            var command = _mapper.Map<Models.DadosEmpresa, DadosEmpresaCommand>(model);
            var retorno = await _apiClient.DadosEmpresa.GetDadosWithHttpMessagesAsync(idVaga, idCurriculo);

            if (retorno.Response.IsSuccessStatusCode)
            {
                model.NumeroCNPJ = retorno.Body.NumeroCNPJ.Value;
                model.NomeEmpresa = retorno.Body.NomeEmpresa;
                model.NumeroTelefone = retorno.Body.NumeroTelefone;
                model.QuantidadeCurriculosVisualizados = retorno.Body.QuantidadeCurriculosVisualizados;
                model.QuantidadeFuncionarios = retorno.Body.QuantidadeFuncionarios;
                model.QuantidadeVagasDivulgadas = retorno.Body.QuantidadeVagasDivulgadas;
                model.VagaConfidencial = retorno.Body.VagaConfidencial.Value;
                model.VagaSine = retorno.Body.VagaSine.Value;
                model.ValorPlanoVIP = retorno.Body.ValorPlanoVIP.Value;
                model.MensagemEmpresaConfidencial = retorno.Body.MensagemEmpresaConfidencial;
                model.Cidade = retorno.Body.Cidade;
                model.Bairro = retorno.Body.Bairro;
                model.CurriculoVIP = retorno.Body.CurriculoVIP.Value;
                if(retorno.Body.DataCadastro.HasValue)
                model.DataCadastro =  retorno.Body.DataCadastro.Value;
                model.DesAreaBne = retorno.Body.DesAreaBne;

                return PartialView("../Modal/_ModalDadosEmpresa", model);
            }
            else
                return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "DadosDaEmpresa")]
        public async Task<PartialViewResult> DadosDaEmpresa(Curriculo modelcv)
        {
            try
            {
                
                var model = new Models.DadosEmpresa();
                var command = _mapper.Map<Models.DadosEmpresa, DadosEmpresaCommand>(model);
              
                var retorno = await _apiClient.DadosEmpresa.GetDadosWithHttpMessagesAsync(modelcv.IdVaga, modelcv.IdCurriculo);

                if (retorno.Response.IsSuccessStatusCode)
                {
                    model.NumeroCNPJ = retorno.Body.NumeroCNPJ.Value;
                    model.NomeEmpresa = retorno.Body.NomeEmpresa;
                    model.NumeroTelefone = retorno.Body.NumeroTelefone;
                    model.QuantidadeCurriculosVisualizados = retorno.Body.QuantidadeCurriculosVisualizados;
                    model.QuantidadeFuncionarios = retorno.Body.QuantidadeFuncionarios;
                    model.QuantidadeVagasDivulgadas = retorno.Body.QuantidadeVagasDivulgadas;
                    model.VagaConfidencial = retorno.Body.VagaConfidencial.Value;
                    model.VagaSine = retorno.Body.VagaSine.Value;
                    model.ValorPlanoVIP = retorno.Body.ValorPlanoVIP.Value;
                    model.MensagemEmpresaConfidencial = retorno.Body.MensagemEmpresaConfidencial;
                    model.Cidade = retorno.Body.Cidade;
                    model.Bairro = retorno.Body.Bairro;
                    model.CurriculoVIP = retorno.Body.CurriculoVIP.Value;
                    model.DataCadastro = retorno.Body.DataCadastro.Value;
                    model.DesAreaBne = retorno.Body.DesAreaBne;

                    return PartialView("_ModelDadosEmpresa", model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("recuperar dados da empresa", ex);

                throw;
            }

            return null;
        }

    }
}