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
using BNE.BLL.Common;
using Notificacao = BNE.BLL.Notificacao;

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
                bool VagaNoPerfil = false;
                if (base.IdCurriculo.HasValue)
                {
                    int idPerfil;
                    if (Request.QueryString["perfil"] != null && Int32.TryParse(Request.QueryString["perfil"], out idPerfil) && idPerfil.Equals(base.IdCurriculo.Value))
                        VagaNoPerfil = true;
                }
                BLL.PesquisaVaga objPesquisaVaga = null;
                RecuperarDadosPesquisaVaga(txtFuncao, string.Format("{0}/{1}", txtCidade, siglaEstado), out objPesquisaVaga);
                return ResultadoPesquisaVagas(objPesquisaVaga, pagina, null, true, VagaNoPerfil);

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
        public ActionResult ResultadoPesquisaVagas(BLL.PesquisaVaga objPesquisaVaga, int? pagina, int? codigoEmpresa = null, bool mostrarConfidencial = true, bool VagaNoPerfil = false)
        {
            //Exibir banner posso ajudar
            ViewBag.ExibirPossoAjudar = false;
            if (base.IdCurriculo.HasValue && base.IdCurriculo.Value > 0)
                ViewBag.ExibirPossoAjudar = new Curriculo(IdCurriculo.Value).VIP();
            //--------------
            //A consulta na api por codigo da vaga é feita no banco - sendo assim ja consulto o banco pela aplicação.
            if (VagaNoPerfil || (objPesquisaVaga != null && !string.IsNullOrEmpty(objPesquisaVaga.DescricaoCodVaga)))//Implementar pesquisa na API
                return BuscaVagaSQL(objPesquisaVaga, pagina, codigoEmpresa, mostrarConfidencial, VagaNoPerfil);

            try
            {
                return BuscaVagaAPI(objPesquisaVaga, pagina, codigoEmpresa, mostrarConfidencial, VagaNoPerfil);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro na consulta da Api de vagas");
                return BuscaVagaSQL(objPesquisaVaga, pagina, codigoEmpresa, mostrarConfidencial, VagaNoPerfil);
            }

        }

        private ResultadoPesquisaVaga TratamentoParaWebEstagios(BLL.PesquisaVaga pesquisaVaga, ResultadoPesquisaVaga viewModel, out int duplicateCount)
        {
            if (pesquisaVaga != null)
            {
                var tipoVinculo = PesquisaVagaTipoVinculo.ListarIdentificadores(pesquisaVaga);

                if (tipoVinculo.Count == 1 && tipoVinculo.Contains((int)BLL.Enumeradores.TipoVinculo.Estágio))
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
                    if (!original.Funcao.Equals("Estagiário")) //Caso a função seja diferente de estagiário, clona a vaga
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

        #region MyRegion
        /// <summary>
        /// Em construção
        /// </summary>
        /// <param name="pagina"></param>
        /// <returns></returns>
        private ActionResult PesquisaVagaPerfil(int? pagina)
        {
            if (!pagina.HasValue)
                pagina = 1;
            var viewModel = new ResultadoPesquisaVaga();

            return View("Index",
               viewModel.Vagas.ToLimitLessPagedCollection(pagina.Value - 1, 10, 222));

        }
        #endregion

        #region [SalvarAlerta]
        public ActionResult SalvarAlerta(int idCidade, int idFuncao, string cidade, string uf, string descfuncao)
        {
            if (base.IdCurriculo.HasValue && idFuncao > 0 && idCidade > 0)
            {
                try
                {
                    if (!Notificacao.AlertaCidades.ExisteAlerta(base.IdCurriculo.Value, idCidade))
                    {
                        Notificacao.AlertaCidades objAlertaCidade = new Notificacao.AlertaCidades();
                        objAlertaCidade.IdCidade = idCidade;
                        objAlertaCidade.AlertaCurriculos = new Notificacao.AlertaCurriculos(base.IdCurriculo.Value);
                        objAlertaCidade.FlagInativo = false;
                        objAlertaCidade.NomeCidade = cidade;
                        objAlertaCidade.SiglaEstado = uf;
                        objAlertaCidade.Save();
                    }
                    if (!Notificacao.AlertaFuncoes.ExisteAlerta(base.IdCurriculo.Value, idFuncao))
                    {
                        Notificacao.AlertaFuncoes objAlertaFuncoes = new BLL.Notificacao.AlertaFuncoes();
                        objAlertaFuncoes.AlertaCurriculos = new BLL.Notificacao.AlertaCurriculos(base.IdCurriculo.Value);
                        objAlertaFuncoes.IdFuncao = idFuncao;
                        objAlertaFuncoes.FlagInativo = false;
                        objAlertaFuncoes.DescricaoFuncao = descfuncao;
                        objAlertaFuncoes.Save();
                    }
                }
                catch (Exception ex)
                {
                    //EL.GerenciadorException.GravarExcecao(ex, $"Erro ao salvar alerta de vagas pela modal, ao pesquisar uma função e cidade que não tenha no alerta - cv: {base.IdCurriculo.Value} Funcao: {idFuncao} Cidade: {idCidade}" );
                    EL.GerenciadorException.GravarExcecao(ex, string.Format("Erro ao salvar alerta de vagas pela modal, ao pesquisar uma função e cidade que não tenha no alerta - cv: {0} Funcao: {1} Cidade: {2}", base.IdCurriculo.Value, idFuncao, idCidade));
                }

            }
            return Json("Concluido", JsonRequestBehavior.AllowGet);
        }
        #endregion

        private ActionResult BuscaVagaSQL(BLL.PesquisaVaga objPesquisaVaga, int? pagina, int? codigoEmpresa = null, bool mostrarConfidencial = true, bool VagaNoPerfil = false)
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
            DataTable retorno;
            totalRegistrosBuscaOriginal = 0;

            if (VagaNoPerfil)
                retorno = BLL.PesquisaVaga.BuscaVagaNoPerfil(tamanhoPagina, currentPageIndex, IdCurriculo.Value, out totalRegistros);
            else
            {
                //realizar a busca com os termos preenchidos pelo usuário
                retorno = BLL.PesquisaVaga.BuscaVagaFullText(objPesquisaVaga, tamanhoPagina, currentPageIndex, idCurriculo, null, null, String.Empty, idOrigemPesquisaVaga, base.STCUniversitario.ValueOrDefault, null, codigoEmpresa, null, ordenacao, out totalRegistros, mostrarConfidencial);

                //buscando os totais do solr
                //if (objPesquisaVaga != null)
                //{
                //    int SolrCount = BLL.PesquisaVaga.RecuperarCountSolr(objPesquisaVaga);
                //    if (SolrCount > 0)
                //        totalRegistros = SolrCount;
                //}

                if ((objPesquisaVaga == null || !objPesquisaVaga.FlagPesquisaAvancada) && totalRegistros.Equals(0) && !codigoEmpresa.HasValue)
                {


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
            }
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
            //Task 46068
            //if(objPesquisaVaga.Funcao !=null && objPesquisaVaga.Cidade != null)//pergunta se quer salvar o alerta
            //{
            //    Curriculo.BuscarCurriculosIdModificacaoExportacao
            //}
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

                vagaModel.SalarioFinal = vagaModel.SalarioFinal.HasValue ? vagaModel.SalarioFinal.Value : 0;
                vagaModel.SalarioInicial = vagaModel.SalarioInicial.HasValue ? vagaModel.SalarioInicial : 0;

                if (objPesquisaVaga != null)
                {
                    vagaModel.URL = SitemapHelper.MontarUrlVagaComPesquisa(vagaModel.Funcao, vagaModel.AreaBNE, vagaModel.NomeCidade, vagaModel.SiglaEstado, vagaModel.IdentificadorVaga, objPesquisaVaga.IdPesquisaVaga);
                }
                else
                {
                    vagaModel.URL = SitemapHelper.MontarUrlVaga(vagaModel.Funcao, vagaModel.AreaBNE, vagaModel.NomeCidade, vagaModel.SiglaEstado, vagaModel.IdentificadorVaga);
                }
                vagaModel.URLIconeFacebook = url;

                //Tratamento para destacar o texto quando a busca é por palavra-chave
                if (objPesquisaVaga != null && !string.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                {
                    vagaModel.Requisito = vagaModel.Requisito.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Atribuicao = vagaModel.Atribuicao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Funcao = vagaModel.Funcao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Beneficio = vagaModel.Beneficio.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                }

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
            if (!base.IdCurriculo.HasValue)
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


            //Task 46068
            ViewBag.PerguntarAlerta = false; ViewBag.Cidade = ViewBag.Funcao = ViewBag.Uf = string.Empty;
            ViewBag.IdCidade = ViewBag.IdFuncao = 0;
            if (base.IdCurriculo.HasValue && objPesquisaVaga.Funcao != null && objPesquisaVaga.Cidade != null)
            {//Primeiro verifca se a pessoa possui registo no alerta de curriculos, para poder salvar o alerta que não tenha.
                if (Notificacao.AlertaCurriculos.ExisteAlerta(base.IdCurriculo.Value) && (!Notificacao.AlertaCidades.ExisteAlerta(base.IdCurriculo.Value, objPesquisaVaga.Cidade.IdCidade)
                    || !Notificacao.AlertaFuncoes.ExisteAlerta(base.IdCurriculo.Value, objPesquisaVaga.Funcao.IdFuncao)))
                {
                    objPesquisaVaga.Cidade.CompleteObject(); //Me prevalecendo por causa do cache
                    ViewBag.PerguntarAlerta = true;
                    ViewBag.CidadePesquisada = objPesquisaVaga.NomeCidade;
                    ViewBag.IdCidade = objPesquisaVaga.Cidade.IdCidade;
                    ViewBag.IdFuncao = objPesquisaVaga.Funcao.IdFuncao;
                    ViewBag.FuncaoPesquisada = objPesquisaVaga.Funcao.DescricaoFuncao;
                    ViewBag.Uf = objPesquisaVaga.Cidade.Estado.SiglaEstado;
                }
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

        #region [BuscaVagaAPI]
        private ActionResult BuscaVagaAPI(BLL.PesquisaVaga objPesquisaVaga, int? pagina, int? codigoEmpresa = null, bool mostrarConfidencial = true, bool VagaNoPerfil = false)
        {
            #region Parametros Paginacao
            int currentPageIndex = 1;

            if (pagina.HasValue)
                currentPageIndex = pagina.Value;

            int totalRegistros;
            int tamanhoPagina = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaVaga));
            #endregion

            if (objPesquisaVaga == null && codigoEmpresa.HasValue)
                objPesquisaVaga = new BLL.PesquisaVaga { RazaoSocial = RouteData.Values["Empresa"].ToString().DesnormalizarURL() };

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

            #region Tratamento STC
            var mostrarVagasBNEnoSTC = false;
            OrigemFilial objOrigemFilial = null;
            if (base.STC.HasValue && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
                mostrarVagasBNEnoSTC = objOrigemFilial.Filial.PossuiSTCUniversitario() || objOrigemFilial.Filial.PossuiSTCLanhouse();
            #endregion Tratamento STC

            #endregion

            var retorno = BLL.PesquisaVaga.BuscaVagaAPI(objPesquisaVaga, tamanhoPagina, currentPageIndex, codigoEmpresa, idOrigemPesquisaVaga, mostrarVagasBNEnoSTC, ordenacao);

            #region Pesquisa

            #region [Quando pesquisa retorna nenhum resultado]
            //*não pode ter sido pesquisa pelas vagas da filial
            //*não poder ter sido pesquisa avançada.
            //*************************************************
            //1-Pesquisa no estado, da cidade pesquisada
            //2-Na Area da Função pesquisada.
            if ((objPesquisaVaga == null || !objPesquisaVaga.FlagPesquisaAvancada) && retorno.TotalRegistros.Equals(0) && !codigoEmpresa.HasValue)
            {

                Cidade objCidade = null;
                if (objPesquisaVaga != null && objPesquisaVaga.Cidade != null)
                {
                    objCidade = objPesquisaVaga.Cidade;
                    objCidade.CompleteObject();
                }

                int? idFuncao = null;
                int? idFuncaoArea = null;
                if (objPesquisaVaga != null && objPesquisaVaga.Funcao != null)
                {
                    objPesquisaVaga.Funcao.CompleteObject();
                    idFuncaoArea = idFuncao = objPesquisaVaga.Funcao.AreaBNE.IdAreaBNE;
                }

                //***Verificar função agrupadora.
                if (objCidade != null)
                {
                    //realizar a busca da função no estado da cidade.
                    if (objPesquisaVaga.Funcao != null)
                        retorno = BLL.PesquisaVaga.BuscaVagaAPI(new BLL.PesquisaVaga() { Estado = objCidade.Estado, DescricaoPalavraChave = objPesquisaVaga != null ? objPesquisaVaga.DescricaoPalavraChave : string.Empty, Funcao = objPesquisaVaga.Funcao, Curriculo = objPesquisaVaga != null ? objPesquisaVaga.Curriculo : null },
                                                                tamanhoPagina, currentPageIndex, codigoEmpresa, idOrigemPesquisaVaga, false, ordenacao);
                    //Pesquisa na cidade por função na Area.
                    if (retorno.TotalRegistros.Equals(0) && idFuncao.HasValue)
                        retorno = BLL.PesquisaVaga.BuscaVagaAPI(new BLL.PesquisaVaga() { Cidade = objCidade, AreaBNE = new AreaBNE(idFuncaoArea.Value), DescricaoPalavraChave = objPesquisaVaga != null ? objPesquisaVaga.DescricaoPalavraChave : string.Empty, Curriculo = objPesquisaVaga != null ? objPesquisaVaga.Curriculo : null },
                                                                tamanhoPagina, currentPageIndex, codigoEmpresa, idOrigemPesquisaVaga, false, ordenacao);
                    //Pesquisa função area no estado.
                    if (retorno.TotalRegistros.Equals(0) && idFuncaoArea.HasValue)
                        retorno = BLL.PesquisaVaga.BuscaVagaAPI(new BLL.PesquisaVaga() { Estado = objCidade.Estado, AreaBNE = new AreaBNE(idFuncaoArea.Value), DescricaoPalavraChave = objPesquisaVaga != null ? objPesquisaVaga.DescricaoPalavraChave : string.Empty, Curriculo = objPesquisaVaga != null ? objPesquisaVaga.Curriculo : null },
                                                                tamanhoPagina, currentPageIndex, codigoEmpresa, idOrigemPesquisaVaga, false, ordenacao);
                }
                else
                    retorno = BLL.PesquisaVaga.BuscaVagaAPI(new BLL.PesquisaVaga() { AreaBNE = new AreaBNE(idFuncaoArea.Value), DescricaoPalavraChave = objPesquisaVaga != null ? objPesquisaVaga.DescricaoPalavraChave : string.Empty, Curriculo = objPesquisaVaga != null ? objPesquisaVaga.Curriculo : null },
                                                           tamanhoPagina, currentPageIndex, codigoEmpresa, idOrigemPesquisaVaga, false, ordenacao);

            }
            #endregion

            #endregion


            totalRegistros = retorno.TotalRegistros;
            //BreadCrumb 0 vagas no google
            objPesquisaVaga.TotalRegistros = totalRegistros;

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

            for (var index = 0; index < retorno.Registros.Length; index++)
            {
                var vaga = retorno.Registros[index];
                var vagaModel = Mapper.Map<BLL.DTO.Registro, Vaga>(vaga);

                #region MediaSalarial

                if (vagaModel.SalarioInicial.HasValue && vagaModel.SalarioInicial.Value <= 0 && vagaModel.SalarioFinal.HasValue && vagaModel.SalarioFinal.Value <= 0)
                {
                    Funcao oFuncao = Funcao.CarregarPorDescricao(vaga.Funcao);
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

                vagaModel.URLIconeFacebook = url;

                //Tratamento para destacar o texto quando a busca é por palavra-chave
                if (objPesquisaVaga != null && !string.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
                {
                    vagaModel.Requisito = vagaModel.Requisito.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Atribuicao = vagaModel.Atribuicao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Funcao = vagaModel.Funcao.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                    vagaModel.Beneficio = vagaModel.Beneficio.HighlightText(objPesquisaVaga.DescricaoPalavraChave.Replace(' ', ','), "highlight");
                }

                if (objPesquisaVaga != null)
                {
                    //Adicionando pesquisa para recuperar na tela de visualizacao de vaga e possibilitar a navegação entre as vagas.
                    vagaModel.URL += $"/{objPesquisaVaga.IdPesquisaVaga}?jobindex={index + 1 + (currentPageIndex - 1) * tamanhoPagina}";
                }
                vagaModel.URL = vagaModel.URL.Replace("#", "(csharp)");

                if (base.IdCurriculo.HasValue)
                {
                    vagaModel.Candidatou = VagaCandidato.CurriculoJaCandidatouVaga(new BLL.Curriculo(base.IdCurriculo.Value), new BLL.Vaga(vagaModel.IdentificadorVaga));

                    if (vagaModel.Candidatou)
                    {
                        vagaModel.IndicadoPeloBNE = VagaCandidato.CurriculoFoiCandidatadoAutomaticamente(new BLL.Curriculo(base.IdCurriculo.Value), new BLL.Vaga(vagaModel.IdentificadorVaga));
                    }
                }

                vagaModel.VagaLivre = vagaModel.FlagVagaArquivada ? true : vagaModel.Plano;
                vagaModel.FlagVagaArquivada = vagaModel.FlagVagaArquivada;
                vagas.Add(vagaModel);
            }

            viewModel.Vagas = vagas;
            #endregion

            #region ViewBag
            var primeiraVaga = vagas.FirstOrDefault();
            if (primeiraVaga != null && ordenacao == OrdenacaoBuscaVaga.Padrao && (!pagina.HasValue || (pagina.HasValue && pagina == 1)))
                Session["DataCadastroPrimeiraVaga" + objPesquisaVaga.IdPesquisaVaga] = primeiraVaga.DataAnuncio;

            ViewBag.Identificador = objPesquisaVaga.IdPesquisaVaga;

            PageAttributes pageAttributes = PageAttributes.Get(RouteData, 5, objPesquisaVaga);
            ViewBag.H1 = pageAttributes.H1;
            ViewBag.TotalDeRegistros = pageAttributes.TotalDeRegistros;
            ViewBag.Description = pageAttributes.Description;
            ViewBag.Keywords = string.Join(",", pageAttributes.Keywords);
            ViewBag.Title = pageAttributes.Title;
            ViewBag.STC = base.STC.ValueOrDefault;
            ViewBag.STCUniversitario = base.STCUniversitario.ValueOrDefault;
            ViewBag.Funcao = objPesquisaVaga.DescricaoFuncao;
            ViewBag.IdCurriculo = base.IdCurriculo.ValueOrDefault;
            ViewBag.EmpresaLogada = base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue;

           
            if (!String.IsNullOrEmpty(objPesquisaVaga.NomeCidade))
            {
                ViewBag.Cidade = objPesquisaVaga.NomeCidade + "/" + objPesquisaVaga.SiglaEstado;
                ViewBag.IdCidade = objPesquisaVaga.Cidade.IdCidade;
            }
            ViewBag.Vip = false;
            ViewBag.Logado = true;
            //Funcao no banner, se a busca não tiver funcao pegar a do curriculo
            if (!base.IdCurriculo.HasValue)
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


            //Task 46068
            ViewBag.PerguntarAlerta = false; ViewBag.Cidade = ViewBag.Funcao = ViewBag.Uf = string.Empty;
            ViewBag.IdCidade = ViewBag.IdFuncao = 0;
            if (base.IdCurriculo.HasValue && objPesquisaVaga.Funcao != null && objPesquisaVaga.Cidade != null)
            {//Primeiro verifca se a pessoa possui registo no alerta de curriculos, para poder salvar o alerta que não tenha.
                if (Notificacao.AlertaCurriculos.ExisteAlerta(base.IdCurriculo.Value) && (!Notificacao.AlertaCidades.ExisteAlerta(base.IdCurriculo.Value, objPesquisaVaga.Cidade.IdCidade)
                    || !Notificacao.AlertaFuncoes.ExisteAlerta(base.IdCurriculo.Value, objPesquisaVaga.Funcao.IdFuncao)))
                {
                    objPesquisaVaga.Cidade.CompleteObject(); //Me prevalecendo por causa do cache
                    ViewBag.PerguntarAlerta = true;
                    ViewBag.CidadePesquisada = objPesquisaVaga.NomeCidade;
                    ViewBag.IdCidade = objPesquisaVaga.Cidade.IdCidade;
                    ViewBag.IdFuncao = objPesquisaVaga.Funcao.IdFuncao;
                    ViewBag.FuncaoPesquisada = objPesquisaVaga.Funcao.DescricaoFuncao;
                    ViewBag.Uf = objPesquisaVaga.Cidade.Estado.SiglaEstado;
                }
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

            #region [Total de vagas mostrado conforme parametros]

            var LimiteResultadoPesquisa = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.LimiteResultadoPesquisa));
            var LimiteResultadoPesquisaSemValoresInformados = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.LimiteResultadoPesquisaSemValoresInformados));

            if (objPesquisaVaga.Funcao != null || objPesquisaVaga.Cidade != null || !String.IsNullOrEmpty(objPesquisaVaga.DescricaoPalavraChave))
            {
                if (totalRegistros > LimiteResultadoPesquisa)
                    totalRegistros = LimiteResultadoPesquisa;
            }
            else if (totalRegistros > LimiteResultadoPesquisaSemValoresInformados)
                totalRegistros = LimiteResultadoPesquisaSemValoresInformados;

            #endregion

            int duplicateCount;
            viewModel = TratamentoParaWebEstagios(objPesquisaVaga, viewModel, out duplicateCount);

            if (duplicateCount <= 0)
                return View("Index", viewModel.Vagas.ToPagedList(currentPageIndex - 1, 10, totalRegistros));

            return View("Index",
                viewModel.Vagas.ToLimitLessPagedCollection(currentPageIndex - 1, 10, totalRegistros));
        }
        #endregion

    }
}
