using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Vagas.Code.Helpers.Paginacao;
using BNE.Web.Vagas.Models;
using Vaga = BNE.Web.Vagas.Models.Vaga;
using BNE.Web.Vagas.Code.Helpers.SEO;
using System.Web;
using System.Web.UI;
using BNE.BLL.Common;

namespace BNE.Web.Vagas.Controllers
{
    public class ResultadoPesquisaVagaController : BaseController
    {

        #region PesquisarVagasFuncaoCidade
        [HttpGet]
        public ActionResult PesquisarVagasFuncaoCidade(string funcao, string cidade, string siglaEstado, int? pagina)
        {
            Response.Cache.SetOmitVaryStar(true);

            try
            {
                if (!string.IsNullOrWhiteSpace(funcao))
                    funcao = funcao.DesnormalizarURL();
                if (!string.IsNullOrWhiteSpace(cidade))
                    cidade = Helper.FormatarCidade(cidade.DesnormalizarURL(), siglaEstado);

                BLL.PesquisaVaga objPesquisaVaga;
                RecuperarDadosPesquisaVaga(funcao, cidade, out objPesquisaVaga);

                TipoGatilho.DispararGatilhoPesquisaCandidato(System.Web.HttpContext.Current != null ? new System.Web.HttpContextWrapper(System.Web.HttpContext.Current) : HttpContext, objPesquisaVaga);

                //EL.GerenciadorException.GravarExcecao(new Exception(), string.Format("Pesquisou PesquisarVagasFuncaoCidade vagas =>{0},{1},{2},{3} ", funcao, cidade, siglaEstado, pagina));
                //EL.GerenciadorException.GravarInformacao(string.Format("Pesquisou vagas =>{0},{1},{2},{3} ", funcao, cidade, siglaEstado, pagina));

                return ResultadoPesquisaVagas(objPesquisaVaga, pagina);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region PesquisarVagasPalavraChave
        [HttpGet]
        public ActionResult PesquisarVagasPalavraChave(string palavraChave, int? pagina)
        {
            palavraChave = palavraChave.DesnormalizarURL();

            BLL.PesquisaVaga objPesquisaVaga;
            RecuperarDadosPesquisaVaga(palavraChave, string.Empty, out objPesquisaVaga);

            return ResultadoPesquisaVagas(objPesquisaVaga, pagina);
        }
        #endregion

        #region PesquisarVagasAreaBNE
        [HttpGet]
        public ActionResult PesquisarVagasAreaBNE(string areaBNE, int? pagina)
        {
            areaBNE = areaBNE.DesnormalizarURL();

            BLL.PesquisaVaga objPesquisaVaga;
            RecuperarDadosPesquisaVaga(areaBNE, out objPesquisaVaga);

            return ResultadoPesquisaVagas(objPesquisaVaga, pagina);
        }
        #endregion

        #region PesquisarVagas
        [HttpGet]
        public ActionResult PesquisarVagas(string txtFuncao, string txtCidade, string txtPalavraChave, string siglaEstado, int? pagina)
        {
            try
            {
                BLL.PesquisaVaga objPesquisaVaga = null;
                //int dummy;
                //if (TempData["Identificador"] != null && Int32.TryParse(TempData["Identificador"].ToString(), out dummy))
                //    objPesquisaVaga = BLL.PesquisaVaga.LoadObject(dummy);
                //else
                RecuperarDadosPesquisaVaga(txtFuncao, string.Format("{0}/{1}", txtCidade, siglaEstado), out objPesquisaVaga);

                TipoGatilho.DispararGatilhoPesquisaCandidato(System.Web.HttpContext.Current != null ? new System.Web.HttpContextWrapper(System.Web.HttpContext.Current) : HttpContext, objPesquisaVaga);

                //EL.GerenciadorException.GravarExcecao(new Exception(), string.Format("Pesquisou PesquisarVagas vagas =>{0},{1},{2},{3} ", txtFuncao, txtCidade, siglaEstado, pagina));

                //EL.GerenciadorException.GravarInformacao(string.Format("Pesquisou vagas =>{0},{1},{2},{3} ", txtFuncao, txtCidade, siglaEstado, pagina));

                return ResultadoPesquisaVagas(objPesquisaVaga, pagina);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult PesquisarVagas(string txtFuncao, string txtCidade, string txtPalavraChave, string siglaEstado)
        {
            try
            {

                if (!string.IsNullOrWhiteSpace(txtFuncao))
                {
                    if (!BLL.Funcao.ValidarFuncaoPorOrigem(null, txtFuncao))
                    {
                        txtFuncao = string.Empty;
                    }
                }

                BLL.PesquisaVaga objPesquisaVaga;
                RecuperarDadosPesquisaVaga(txtFuncao, txtCidade, out objPesquisaVaga);

                #region Recuperando estado, se tiver
                if (!String.IsNullOrEmpty(txtCidade))
                {
                    const string pattern = @"([\w\s]+)[/](\w+)"; //Ex. Curitiba/Paraná
                    var regex = new Regex(pattern);
                    Match match = regex.Match(txtCidade);
                    if (match.Success)
                    {
                        txtCidade = match.Groups[1].Value;
                        siglaEstado = match.Groups[2].Value;
                    }
                }
                #endregion

                //TempData["Identificador"] = objPesquisaVaga.IdPesquisaVaga;

                #region Redirecionando para url's amigaveis
                //Nenhum parametro foi informado
                if (String.IsNullOrEmpty(txtFuncao) && String.IsNullOrEmpty(txtCidade) && String.IsNullOrEmpty(siglaEstado))
                    return RedirectToAction("PesquisarVagas", "ResultadoPesquisaVaga");

                //Todos os parametros foram informados
                if (!String.IsNullOrEmpty(txtFuncao) && !String.IsNullOrEmpty(txtCidade) && !String.IsNullOrEmpty(siglaEstado))
                    return RedirectToAction("PesquisarVagasFuncaoCidade", "ResultadoPesquisaVaga", new { Funcao = txtFuncao, Cidade = txtCidade, SiglaEstado = siglaEstado });

                //Somente função foi informada
                if (!String.IsNullOrEmpty(txtFuncao))
                    return RedirectToAction("PesquisarVagasFuncaoCidade", "ResultadoPesquisaVaga", new { Funcao = txtFuncao });

                //Somente cidade foi informada
                if (!String.IsNullOrEmpty(txtCidade) && !String.IsNullOrEmpty(siglaEstado))
                    return RedirectToAction("PesquisarVagasFuncaoCidade", "ResultadoPesquisaVaga", new { Cidade = txtCidade, SiglaEstado = siglaEstado });

                #endregion Redirecionando para url's amigaveis

                //EL.GerenciadorException.GravarExcecao(new Exception(), string.Format("Pesquisou vagas =>{0},{1},{2},{3} ", txtFuncao, txtCidade, siglaEstado));

                //EL.GerenciadorException.GravarInformacao(string.Format("Pesquisou vagas =>{0},{1},{2}", txtFuncao, txtCidade, siglaEstado));

                return RedirectToAction("PesquisarVagas", "ResultadoPesquisaVaga", new { Funcao = txtFuncao, Cidade = txtCidade, PalavraChave = txtPalavraChave, SiglaEstado = siglaEstado });
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region PesquisarVagas
        [HttpGet]
        public ActionResult PesquisaVagaAvancada(int identificador, int? pagina)
        {
            BLL.PesquisaVaga objPesquisaVaga = BLL.PesquisaVaga.LoadObject(identificador);

            return ResultadoPesquisaVagas(objPesquisaVaga, pagina);
        }

        private bool FuncaoUtilizadaComoContratoDeTrabalho(string txtFuncao)
        {
            if (string.IsNullOrWhiteSpace(txtFuncao))
                return false;

            var funcaoTratada = Helper.RemoverAcentos(txtFuncao.Trim());

            if (funcaoTratada.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                || funcaoTratada.Equals("Estagiario", StringComparison.OrdinalIgnoreCase)
                || funcaoTratada.Equals("Estagiaria", StringComparison.OrdinalIgnoreCase)
                //|| funcaoTratada.Equals("Aprendiz")
                )
            {
                return true;
            }

            return false;
        }

        private ActionResult RedirecionarParaPesquisaAvancada(string txtFuncao, string txtCidade, string txtPalavraChave, string siglaEstado)
        {
            var url = Helper.RecuperarURLAmbiente() ?? string.Empty;
            var toRedirect = Rota.RecuperarURLRota(BLL.Enumeradores.RouteCollection.PesquisaVagaAvancada);

            if (url.LastOrDefault() != '/')
                url += '/';
            if (!string.Empty.StartsWith("http"))
                url = "http://" + url;

            var actions = new RouteValueDictionary();
            actions.Add("funcao", Helper.RemoverAcentos(txtFuncao));
            if (!string.IsNullOrWhiteSpace(txtCidade))
            {
                actions.Add("cidade", Helper.RemoverAcentos(txtCidade));
            }

            if (!string.IsNullOrWhiteSpace(txtPalavraChave))
            {
                actions.Add("palavra-chave", Helper.RemoverAcentos(txtPalavraChave));
            }

            if (!string.IsNullOrWhiteSpace(siglaEstado))
            {
                actions.Add("estado", Helper.RemoverAcentos(siglaEstado));
            }

            var linkTotal =
                this.Redirect(url + toRedirect + "?" +
                              actions.Select(a => a.Key + "=" + a.Value).Aggregate((a, b) => a + "&" + b));

            return linkTotal;
        }
        #endregion

        #region PesquisarVagasEmpresa
        [HttpGet]
        public ActionResult PesquisarVagasEmpresa(string empresa, int codigoEmpresa, int? pagina)
        {
            //Task 41309 - nas vagas das empresas não mostrar as confidenciais
            return ResultadoPesquisaVagas(null, pagina, codigoEmpresa, false);
        }
        #endregion

        #region ResultadoPesquisaVagas
        public ActionResult ResultadoPesquisaVagas(BLL.PesquisaVaga objPesquisaVaga, int? pagina, int? codigoEmpresa = null, bool mostrarConfidencial = true)
        {
            #region Parametros Paginacao
            int currentPageIndex = 1;

            if (pagina.HasValue)
                currentPageIndex = pagina.Value;

            int totalRegistros;
            int totalRegistrosBuscaOriginal;
            int tamanhoPagina = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaVaga));
            #endregion

            #region Parametros
            //Seta Origem na busca de vagas somente quando estiver no RHOffice
            int? idOrigemPesquisaVaga = null;
            if (base.STC.ValueOrDefault)
                idOrigemPesquisaVaga = base.IdOrigem.Value;

            int? idCurriculo = null;
            if (base.IdCurriculo.HasValue)
                idCurriculo = base.IdCurriculo.Value;

            OrdenacaoBuscaVaga ordenacao = OrdenacaoBuscaVaga.Padrao;
            if (Request.QueryString["ordenacaoVagas"] != null)
            {
                ordenacao = (OrdenacaoBuscaVaga)Convert.ToInt32(Request.QueryString["ordenacaoVagas"]);
            }
            #endregion

            #region Pesquisa
            //realizar a busca com os termos preenchidos pelo usuário
            var retorno = BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVaga, tamanhoPagina, currentPageIndex, idCurriculo, null, null, String.Empty, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, null, codigoEmpresa, null, ordenacao, out totalRegistros, mostrarConfidencial);

            //buscando os totais do solr
            //if (objPesquisaVaga != null)
            //{
            //    int SolrCount = BLL.PesquisaVaga.RecuperarCountSolr(objPesquisaVaga);
            //    if (SolrCount > 0)
            //        totalRegistros = SolrCount;
            //}

            if ((objPesquisaVaga == null || !objPesquisaVaga.FlagPesquisaAvancada) && totalRegistros.Equals(0) && !codigoEmpresa.HasValue)
            {
                totalRegistrosBuscaOriginal = 0;

                Cidade objCidade = null;
                if (objPesquisaVaga != null && objPesquisaVaga.Cidade != null)
                    objCidade = objPesquisaVaga.Cidade;

                int? idFuncao = null;
                int? idFuncaoArea = null;

                if (objPesquisaVaga != null && objPesquisaVaga.Funcao != null)
                    idFuncaoArea = idFuncao = objPesquisaVaga.Funcao.IdFuncao;

                if (objCidade != null)
                {
                    string desPalavraChave = objPesquisaVaga.DescricaoPalavraChave;

                    //realizar a busca pelo estado da cidade
                    objCidade.CompleteObject();
                    retorno = BLL.PesquisaVaga.BuscaVagaFullText(null, tamanhoPagina, currentPageIndex, idCurriculo, idFuncao, null, desPalavraChave, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, objCidade.Estado.SiglaEstado, null, null, ordenacao, out totalRegistros);

                    if (totalRegistros.Equals(0))
                        retorno = BLL.PesquisaVaga.BuscaVagaFullText(null, tamanhoPagina, currentPageIndex, idCurriculo, idFuncao, null, desPalavraChave, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, null, null, null, ordenacao, out totalRegistros);

                    if (totalRegistros.Equals(0))
                        retorno = BLL.PesquisaVaga.BuscaVagaFullText(null, tamanhoPagina, currentPageIndex, idCurriculo, null, null, desPalavraChave, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, null, null, idFuncaoArea, ordenacao, out totalRegistros);
                }
                else
                    retorno = BLL.PesquisaVaga.BuscaVagaFullText(null, tamanhoPagina, currentPageIndex, idCurriculo, null, null, String.Empty, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, null, null, idFuncaoArea, ordenacao, out totalRegistros);
            }
            else
                totalRegistrosBuscaOriginal = totalRegistros;


            if (objPesquisaVaga == null && codigoEmpresa.HasValue)
                objPesquisaVaga = new BLL.PesquisaVaga { RazaoSocial = RouteData.Values["Empresa"].ToString().DesnormalizarURL() };

            objPesquisaVaga.TotalRegistros = totalRegistros;

            #endregion

            #region Facebook
            string url = "/Images/logo_bne_facebook.png";

            if (base.STC.HasValue && base.STC.ValueOrDefault)
            {
                url = "/Images/logo_vaga_facebook.png";
            }

            url = String.Concat("http://", Helper.RecuperarURLVagas(), url);
            #endregion

            #region Modelo
            var viewModel = new ResultadoPesquisaVaga();

            var vagas = new List<Vaga>();
            foreach (DataRow vaga in retorno.Rows)
            {
                var vagaModel = Mapper.Map<DataRow, Vaga>(vaga);
                
                #region MediaSalarial
                if (vagaModel.SalarioInicial == null && vagaModel.SalarioFinal == null)
                {
                    Funcao oFuncao = Funcao.CarregarPorDescricao(vagaModel.Funcao);
                    try
                    {
                        if (oFuncao != null)
                        {
                            BLL.DTO.MediaSalarial objMedia = BLL.Vaga.MediaSalarialPorFuncao(oFuncao.IdFuncao);
                            if (objMedia != null && objMedia.DetalhesFuncao.SalarioPequena != null) //Validar se a função tem media salarial
                            {
                                vagaModel.MediaSalarial = string.Format("{0:C} a {1:C}", Math.Round(objMedia.DetalhesFuncao.SalarioPequena.Trainee), Math.Round(objMedia.DetalhesFuncao.SalarioPequena.Master));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string saida = string.Empty;
                        EL.GerenciadorException.GravarExcecao(ex, out saida, String.Format("Função: {0} id:{1} dando error no WebService da media salarial do salario br", vagaModel.Funcao, oFuncao.IdFuncao.ToString()));
                    }

                }
                #endregion

                vagaModel.URL = SitemapHelper.MontarUrlVaga(vagaModel.Funcao, vagaModel.AreaBNE, vagaModel.NomeCidade, vagaModel.SiglaEstado, vagaModel.IdentificadorVaga);
                vagaModel.URLIconeFacebook = url;

                //Tratamento para destacar o texto quando a busca é por palavra-chave
                if (objPesquisaVaga != null && !string.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                {
                    vagaModel.Requisito = vagaModel.Requisito.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Atribuicao = vagaModel.Atribuicao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Funcao = vagaModel.Funcao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Beneficio = vagaModel.Beneficio.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                }

                //Cookie para guardar o id da pesquisa para o novo BNE
                HttpCookie cookie = new HttpCookie("CodigoPesquisa");
                cookie.Value = objPesquisaVaga.IdPesquisaVaga.ToString();
                Response.Cookies.Add(cookie);

                //linha para identificar os acesso no HotJar
                vagaModel.URL = vagaModel.URL + "?utm_term=CadastroRapido";

                //RETIRADA VAGA PREMIUM TASK: 41857
                vagaModel.Premium = false;//ParametroVaga.Premium((int)BNE.BLL.Enumeradores.Parametro.VagaPremium, vagaModel.IdentificadorVaga);
                vagaModel.VagaLivre = new Filial(vagaModel.IdFilial).PossuiPlanoAtivo() || vagaModel.FlagVagaArquivada || vagaModel.FlagInativo;

                vagas.Add(vagaModel);
            }

            viewModel.Vagas = vagas;
            #endregion

            #region ViewBag
            var primeiraVaga = vagas.FirstOrDefault();
            if (primeiraVaga != null && ordenacao == OrdenacaoBuscaVaga.Padrao && (!pagina.HasValue || (pagina.HasValue && pagina == 1)))
                Session["DataCadastroPrimeiraVaga" + objPesquisaVaga.IdPesquisaVaga] = primeiraVaga.DataAnuncio;

            ViewBag.Identificador = objPesquisaVaga.IdPesquisaVaga;

            PageAttributes pageAttributes = PageAttributes.Get(RouteData, totalRegistrosBuscaOriginal, objPesquisaVaga);
            ViewBag.H1 = pageAttributes.H1;
            ViewBag.TotalDeRegistros = pageAttributes.TotalDeRegistros;
            ViewBag.Description = pageAttributes.Description;
            ViewBag.Keywords = string.Join(",", pageAttributes.Keywords);
            ViewBag.Title = pageAttributes.Title;
            ViewBag.STC = base.STC.ValueOrDefault;
            ViewBag.STCUniversitario = base.STCUniversitario.ValueOrDefault;
            ViewBag.Funcao = objPesquisaVaga.DescricaoFuncao;
            ViewBag.IdCurriculo = base.IdCurriculo.ValueOrDefault;
            if (!String.IsNullOrEmpty(objPesquisaVaga.NomeCidade))
            {
                ViewBag.Cidade = objPesquisaVaga.NomeCidade + "/" + objPesquisaVaga.SiglaEstado;
                ViewBag.IdCidade = objPesquisaVaga.Cidade.IdCidade;
            }
            ViewBag.Vip = false;
            ViewBag.Logado = true;
            //Funcao no banner, se a busca não tiver funcao pegar a do curriculo
            if(!base.IdCurriculo.HasValue)
                ViewBag.Logado = false;
            else if (!String.IsNullOrEmpty(objPesquisaVaga.DescricaoFuncao))
                 ViewBag.FuncaoPesquisada = objPesquisaVaga.DescricaoFuncao;
            else 
            {
                FuncaoPretendida objFuncaoPretendida = new FuncaoPretendida();
                FuncaoPretendida.CarregarPorCurriculo(base.IdCurriculo.Value, out objFuncaoPretendida);

                if (objFuncaoPretendida.Funcao != null)
                {
                    objFuncaoPretendida.Funcao.CompleteObject();
                    ViewBag.FuncaoPesquisada = objFuncaoPretendida.Funcao.DescricaoFuncao;
                }
                else
                {
                    ViewBag.FuncaoPesquisada = objFuncaoPretendida.DescricaoFuncaoPretendida;
                }
            }
            if (objPesquisaVaga.Curriculo != null)
            {
                objPesquisaVaga.Curriculo.CompleteObject();
                ViewBag.Vip = objPesquisaVaga.Curriculo.FlagVIP;
            }
                
         
           
           

            //ViewBag.IdFuncao = objPesquisaVaga.Funcao != null ? objPesquisaVaga.Funcao.IdFuncao : -1;
            if (objPesquisaVaga.Funcao != null)
                ViewBag.IdFuncao = objPesquisaVaga.Funcao.IdFuncao;
            
            #endregion
            
            #region Breadcrumb
            //Guardar valores em tempData para requisição e montagem do breadcrumb no BreadcrumbController
            TempData["temp_DesBreadcrumb"] = pageAttributes.DesBreadcrumb;
            TempData["temp_DesBreadcrumbURL"] = pageAttributes.DesBreadcrumbURL;
            #endregion
            
            int duplicateCount;
            viewModel = TratamentoParaWebEstagios(objPesquisaVaga, viewModel, out duplicateCount);

            if (duplicateCount <= 0)
                return View("Index", viewModel.Vagas.ToPagedList(currentPageIndex - 1, 10, totalRegistros));

            return View("Index",
                viewModel.Vagas.ToLimitLessPagedCollection(currentPageIndex - 1, 10, totalRegistros));
        }

        private ResultadoPesquisaVaga TratamentoParaWebEstagios(BLL.PesquisaVaga pesquisaVaga, ResultadoPesquisaVaga viewModel, out int duplicateCount)
        {
            if (pesquisaVaga != null)
            {
                var tipoVinculo = PesquisaVagaTipoVinculo.ListarPorPesquisaList(pesquisaVaga.IdPesquisaVaga);

                if (tipoVinculo.Count == 1
                    &&
                    tipoVinculo.Any(obj => obj.TipoVinculo.IdTipoVinculo == (int)BNE.BLL.Enumeradores.TipoVinculo.Estágio))
                {
                    foreach (var result in viewModel.Vagas)
                    {
                        AlterarUrlNaVagaDeEstagio(result);
                    }
                    ViewData["FlagPesquisaExclusivaDeEstagio"] = true;
                    duplicateCount = 0;
                    return viewModel;
                }
            }
            ViewData["FlagPesquisaExclusivaDeEstagio"] = false;

            var originalCopy = viewModel.Vagas.ToArray();

            var collectionResult = viewModel.Vagas as List<Vaga>;
            if (collectionResult == null)
            {
                collectionResult = new List<Vaga>(viewModel.Vagas);
                viewModel.Vagas = collectionResult;
            }

            int moveCount = 0;
            for (int i = 0; i < originalCopy.Length; i++)
            {
                var original = originalCopy[i];

                if (original.TipoVinculo.Trim() == "Estágio")
                {
                    AlterarUrlNaVagaDeEstagio(original);

                }
                else if (original.TipoVinculo != null
                   && original.TipoVinculo.IndexOf("Estágio", StringComparison.OrdinalIgnoreCase) > -1
                )
                {
                    if(!original.Funcao.Equals("Estagiário")) //Caso a função seja diferente de estagiário, clona a vaga
                    {
                        var vagaWebEstagios = original.ShallowCopy();
                        vagaWebEstagios.TipoVinculo = "Estágio";

                        AlterarUrlNaVagaDeEstagio(vagaWebEstagios);

                        original.TipoVinculo = original.TipoVinculo.Replace("Estágio", "");
                        collectionResult.Insert(i + moveCount, vagaWebEstagios);
                        moveCount = moveCount + 1;
                    }
                    else  //Caso a função seja estagiário, apenas troca a url e o tipo vinculo
                    {
                        AlterarUrlNaVagaDeEstagio(original);
                        original.TipoVinculo = "Estágio";
                    }
                }
            }

            duplicateCount = collectionResult.Count - originalCopy.Length;
            return viewModel;
        }

        private static void AlterarUrlNaVagaDeEstagio(Vaga result)
        {
            var totalUrl = result.URL.Split('/');

            var penultimoArgumento = totalUrl.Select((a, b) => new { a, b }).Reverse().Skip(2).FirstOrDefault();
            if (penultimoArgumento == null)
                return;

            if ("estagiario".Equals(penultimoArgumento.a))
                return;

            totalUrl[penultimoArgumento.b] = "estagio-para-" + penultimoArgumento.a;

            result.URL = totalUrl.Aggregate((a, b) => a + "/" + b);
        }

        #endregion

        #region RecuperarDadosPesquisaVaga
        private void RecuperarDadosPesquisaVaga(string descricaoFuncao, string descricaoCidade, out BLL.PesquisaVaga objPesquisaVaga)
        {
            int idPesquisa;
            if (Request.QueryString["idPesquisa"] != null && Int32.TryParse(Request.QueryString["idPesquisa"], out idPesquisa))
            {
                objPesquisaVaga = BLL.PesquisaVaga.LoadObject(idPesquisa);

                if (objPesquisaVaga.Funcao != null)
                    objPesquisaVaga.Funcao = BLL.Funcao.LoadObject(objPesquisaVaga.Funcao.IdFuncao);

                if (objPesquisaVaga.Cidade != null)
                    objPesquisaVaga.Cidade = BLL.Cidade.LoadObject(objPesquisaVaga.Cidade.IdCidade);

                if (objPesquisaVaga.Estado != null && objPesquisaVaga.Estado.IdEstado > 0)
                    objPesquisaVaga.Estado = BLL.Estado.LoadObject(objPesquisaVaga.Estado.IdEstado);

                return;
            }

            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);

            Curriculo objCurriculo = null;
            if (IdCurriculo.HasValue)
                objCurriculo = new Curriculo(IdCurriculo.Value);

            objPesquisaVaga = BLL.PesquisaVaga.RecuperarDadosPesquisaVaga(objUsuarioFilialPerfil, objCurriculo, Common.Helper.RecuperarIP(), descricaoFuncao, descricaoCidade);
        }

        private void RecuperarDadosPesquisaVaga(string descricaoAreaBNE, out BLL.PesquisaVaga objPesquisaVaga)
        {
            int idPesquisa;
            if (Request.QueryString["idPesquisa"] != null && Int32.TryParse(Request.QueryString["idPesquisa"], out idPesquisa))
            {
                objPesquisaVaga = BLL.PesquisaVaga.LoadObject(idPesquisa);
                objPesquisaVaga.Cidade.CompleteObject();
                objPesquisaVaga.Funcao.CompleteObject();
                objPesquisaVaga.Estado.CompleteObject();
                return;
            }

            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);

            Curriculo objCurriculo = null;
            if (IdCurriculo.HasValue)
                objCurriculo = new Curriculo(IdCurriculo.Value);

            objPesquisaVaga = BLL.PesquisaVaga.RecuperarDadosPesquisaVaga(objUsuarioFilialPerfil, objCurriculo, Common.Helper.RecuperarIP(), descricaoAreaBNE);
        }
        #endregion

    }
}
