using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Vagas.Code.ActionFilter;
using BNE.Web.Vagas.Models;
using Enumeradores = BNE.BLL.Enumeradores;
using Vaga = BNE.Web.Vagas.Models.Vaga;
using ExperienciaProfissional = BNE.Web.Vagas.Models.ExperienciaProfissional;
using System.Data;
using System.Web.UI;
using BNE.Web.Vagas.Code.Helpers.SEO;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using BNE.BLL.Common;

namespace BNE.Web.Vagas.Controllers
{
    public class VagaController : BaseController
    {

        #region VisualizarVaga
        [OutputCache(Duration = 86400, VaryByParam = "", Location = OutputCacheLocation.Client)]
        public ActionResult VisualizarVaga(int identificador)
        {

            #region Vaga
            var objVaga = BLL.Vaga.LoadObject(identificador);
            objVaga.Funcao.CompleteObject();
            objVaga.Funcao.AreaBNE.CompleteObject();
            objVaga.Cidade.CompleteObject();
            if (objVaga.Escolaridade != null)
                objVaga.Escolaridade.CompleteObject();
            if (objVaga.Deficiencia != null)
                objVaga.Deficiencia.CompleteObject();



            #endregion

            var model = new VisualizarVaga();

            model.Vaga = Mapper.Map<BLL.Vaga, Vaga>(objVaga);


            #region MediaSalarial
            if (objVaga.ValorSalarioDe == null && objVaga.ValorSalarioPara == null)
                model.Vaga.MediaSalarial = BLL.Vaga.MediaSalarialComRegra(objVaga.Funcao.IdFuncao);
            #endregion

            #region Disponibilidade
            var vagaDisponibilidade = VagaDisponibilidade.ListarDisponibilidadesPorVaga(objVaga);
            if (vagaDisponibilidade.Count > 0)
            {
                foreach (var objVagadisponibilidade in vagaDisponibilidade)
                {
                    if (string.IsNullOrEmpty(objVagadisponibilidade.Disponibilidade.DescricaoDisponibilidade))
                        objVagadisponibilidade.Disponibilidade.CompleteObject();
                    model.Vaga.Disponibilidade += string.Format("{0} ", objVagadisponibilidade.Disponibilidade.DescricaoDisponibilidade);
                }
            }
            #endregion

            #region Tipo de Vinculo
            var vagaTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
            if (vagaTipoVinculo.Count > 0)
            {
                foreach (var objVagaTipoVinculo in vagaTipoVinculo)
                {
                    model.Vaga.TipoVinculo += string.Format("{0} ", objVagaTipoVinculo.TipoVinculo.DescricaoTipoVinculo);
                }
            }
            #endregion

            //#region Deficiencia
            //if (VagaDeficiencia.ExisteDeficiencia(model.Vaga.IdentificadorVaga))
            //{
            //    List<DeficienciaModel> dm = new List<DeficienciaModel>();
            //    var ListDef = VagaDeficiencia.ListaDeficienciaDetalhe(model.Vaga.IdentificadorVaga);
            //    foreach (DataRow dr in ListDef.Rows)
            //    {
            //        var detalhe = Mapper.Map<DataRow, DeficienciaModel>(dr);
            //        dm.Add(detalhe);
            //    }
            //    model.Vaga.Deficiencias = dm;
            //}

            //#endregion

            model.Vaga.Candidatou = false;

            #region Verificação da candidatura e visualizacao
            if (base.IdCurriculo.HasValue)
            {
                var objCurriculo = new Curriculo(base.IdCurriculo.Value);
                if (VagaCandidato.CurriculoJaCandidatouVaga(objCurriculo, objVaga))
                    model.Vaga.Candidatou = true;
                VagaVisualizada.SalvarVisualizacaoVaga(objVaga, objCurriculo);
            }
            else
                VagaVisualizada.SalvarVisualizacaoVaga(objVaga);
            #endregion

            #region Definição de links para páginas de pesquisa semelhantes
            model.Vaga.URL = SitemapHelper.MontarUrlVaga(model.Vaga.Funcao, model.Vaga.AreaBNE, model.Vaga.NomeCidade, model.Vaga.SiglaEstado, model.Vaga.IdentificadorVaga);
            model.LinkVagasFuncao = LinkHelper.ObterLinkVagasFuncao(model.Vaga.Funcao);
            model.LinkVagasCidade = LinkHelper.ObterLinkVagasCidade(model.Vaga.NomeCidade, model.Vaga.SiglaEstado);
            model.LinkVagasFuncaoCidade = LinkHelper.ObterLinkVagasFuncaoCidade(model.Vaga.Funcao, model.Vaga.NomeCidade, model.Vaga.SiglaEstado);
            model.LinkVagasArea = LinkHelper.ObterLinkVagasArea(model.Vaga.AreaBNE);
            #endregion Definição de links para páginas de pesquisa semelhantes

            #region Vagas Similares
            model.VagasSimilares = new List<VagaSimilar>();
            var listaVagasSimilares = BNE.BLL.Vaga.ListarVagasSemelhantes(objVaga);
            foreach (var objVagaSemelhante in listaVagasSimilares)
            {
                model.VagasSimilares.Add(new VagaSimilar { Id = objVagaSemelhante.Id, Funcao = objVagaSemelhante.Funcao, Cidade = Helper.FormatarCidade(objVagaSemelhante.NomeCidade, objVagaSemelhante.SiglaEstado), URL = SitemapHelper.MontarUrlVaga(objVagaSemelhante.Funcao, objVagaSemelhante.AreaBNE, objVagaSemelhante.NomeCidade, objVagaSemelhante.SiglaEstado, objVagaSemelhante.Id), CodVaga = objVagaSemelhante.CodVaga });
            }
            #endregion

            #region Viewbag
            ViewBag.Title = SitemapHelper.MontarTituloVaga(model.Vaga.Funcao, model.Vaga.AreaBNE, (short)model.Vaga.QuantidadeVaga, model.Vaga.NomeCidade, model.Vaga.NomeEstado, model.Vaga.SiglaEstado, model.Vaga.IdentificadorVaga);
            ViewBag.Description = objVaga.RetornarDescription();
            ViewBag.Keywords = objVaga.RetornarKeywords();
            ViewBag.STC = base.STC.ValueOrDefault;
            ViewBag.STCUniversitario = base.STCUniversitario.ValueOrDefault;
            #endregion

            #region Breadcrumb
            PageAttributes pageAttributes = PageAttributes.Get(RouteData, null, model.Vaga);
            TempData["temp_DesBreadcrumb"] = pageAttributes.DesBreadcrumb; //Guardar para requisição no BreadcrumbController
            TempData["temp_DesBreadcrumbURL"] = pageAttributes.DesBreadcrumbURL;
            #endregion

            return View(model);
        }
        #endregion

        #region VisualizarVagaEmpresa
        [OutputCache(Duration = 86400, VaryByParam = "", Location = OutputCacheLocation.Client)]
        public ActionResult VisualizarVagaEmpresa(int identificador)
        {

            #region Vaga
            var objVaga = BLL.Vaga.LoadObject(identificador);
            objVaga.Funcao.CompleteObject();
            objVaga.Funcao.AreaBNE.CompleteObject();
            objVaga.Cidade.CompleteObject();
            if (objVaga.Escolaridade != null)
                objVaga.Escolaridade.CompleteObject();
            if (objVaga.Deficiencia != null)
                objVaga.Deficiencia.CompleteObject();
            #endregion

            var model = new VisualizarVaga();

            model.Vaga = Mapper.Map<BLL.Vaga, Vaga>(objVaga);


            #region MediaSalarial
            if (objVaga.ValorSalarioDe == null && objVaga.ValorSalarioPara == null)
                model.Vaga.MediaSalarial = BLL.Vaga.MediaSalarialComRegra(objVaga.Funcao.IdFuncao);
            #endregion

            #region Disponibilidade
            var vagaDisponibilidade = VagaDisponibilidade.ListarDisponibilidadesPorVaga(objVaga);
            if (vagaDisponibilidade.Count > 0)
            {
                foreach (var objVagadisponibilidade in vagaDisponibilidade)
                {
                    if (string.IsNullOrEmpty(objVagadisponibilidade.Disponibilidade.DescricaoDisponibilidade))
                        objVagadisponibilidade.Disponibilidade.CompleteObject();
                    model.Vaga.Disponibilidade += string.Format("{0} ", objVagadisponibilidade.Disponibilidade.DescricaoDisponibilidade);
                }
            }
            #endregion

            #region Tipo de Vinculo
            var vagaTipoVinculo = VagaTipoVinculo.ListarTipoVinculoPorVaga(objVaga.IdVaga);
            if (vagaTipoVinculo.Count > 0)
            {
                foreach (var objVagaTipoVinculo in vagaTipoVinculo)
                {
                    model.Vaga.TipoVinculo += string.Format("{0} ", objVagaTipoVinculo.TipoVinculo.DescricaoTipoVinculo);
                }
            }
            #endregion

            model.Vaga.Candidatou = false;
            model.Vaga.URL = SitemapHelper.MontarUrlVaga(model.Vaga.Funcao, model.Vaga.AreaBNE, model.Vaga.NomeCidade, model.Vaga.SiglaEstado, model.Vaga.IdentificadorVaga);

            #region Viewbag
            ViewBag.Title = SitemapHelper.MontarTituloVaga(model.Vaga.Funcao, model.Vaga.AreaBNE, (short)model.Vaga.QuantidadeVaga, model.Vaga.NomeCidade, model.Vaga.NomeEstado, model.Vaga.SiglaEstado, model.Vaga.IdentificadorVaga);
            ViewBag.Description = objVaga.RetornarDescription();
            ViewBag.Keywords = objVaga.RetornarKeywords();
            #endregion

            #region Breadcrumb
            PageAttributes pageAttributes = PageAttributes.Get(RouteData, null, model.Vaga);
            TempData["temp_DesBreadcrumb"] = pageAttributes.DesBreadcrumb; //Guardar para requisição no BreadcrumbController
            TempData["temp_DesBreadcrumbURL"] = pageAttributes.DesBreadcrumbURL;
            #endregion

            return View(model);
        }
        #endregion

        #region PerguntarSobreCelular

        [AllowCrossSiteJsonActionFilter]
        public PerguntaCelular PerguntarSobreCelular(VagaCandidatura vagaModel)
        {
            var model = new PerguntaCelular();

            BLL.PessoaFisica objPessoaFisica = BNE.BLL.PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
            
            model = Mapper.Map<BLL.PessoaFisica, PerguntaCelular>(objPessoaFisica);

            //gravar historico de exibição da pergunta
            model.IdPerguntaHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Celular);

            return new PerguntaCelular
            {
                Identificador = model.Identificador,
                NumeroDDDCelular = model.NumeroDDDCelular,
                NumeroCelular = model.NumeroCelular,
                IdentificadorVaga = vagaModel.IdentificadorVaga,
                IdPerguntaHistorico = model.IdPerguntaHistorico,
                NumeroDDDCelularAntigo = model.NumeroDDDCelularAntigo,
                NumeroCelularAntigo = model.NumeroCelularAntigo,
                FlagCelularConfirmado = model.FlagCelularConfirmado
            };
        }

        #endregion

        #region PerguntarSobreEmail

        [AllowCrossSiteJsonActionFilter]
        public PerguntaEmail PerguntarSobreEmail(VagaCandidatura vagaModel)
        {
            var model = new PerguntaEmail();

            BLL.PessoaFisica objPessoaFisica = BNE.BLL.PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);

            model = Mapper.Map<BLL.PessoaFisica, PerguntaEmail>(objPessoaFisica);

            //gravar historico de exibição da pergunta
            model.IdPerguntaHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(base.IdPessoaFisicaLogada.Value, BLL.Enumeradores.PerguntaSalaVip.Email);

            //return model;
            //return PartialView("_ModalPerguntaEmail", new PerguntaEmail { Identificador = model.Identificador, Email = model.Email, IdentificadorVaga = vagaModel.IdentificadorVaga, IdPerguntaHistorico = model.IdPerguntaHistorico, EmailAntigo =model.EmailAntigo });
            return new PerguntaEmail { Identificador = model.Identificador, Email = model.Email, IdentificadorVaga = vagaModel.IdentificadorVaga, IdPerguntaHistorico = model.IdPerguntaHistorico, EmailAntigo = model.EmailAntigo };
        }

        #endregion

        #region PerguntarSobreExperienciaProfissional

        [AllowCrossSiteJsonActionFilter]
        public ExperienciaProfissional PerguntarSobreExperienciaProfissional(VagaCandidatura vagaModel)
        {
            var model = new ExperienciaProfissional();

            BLL.ExperienciaProfissional objUltimaExperiencia = BNE.BLL.ExperienciaProfissional.CarregarUltimaExperienciaProfissional(base.IdPessoaFisicaLogada.Value);

            model = Mapper.Map<BLL.ExperienciaProfissional, ExperienciaProfissional>(objUltimaExperiencia);

            //gravar historico de exibição da pergunta
            model.IdPerguntaHistoricoExp = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(base.IdPessoaFisicaLogada.Value, BLL.Enumeradores.PerguntaSalaVip.Experiencia);

            return new ExperienciaProfissional { Identificador = model.Identificador, RazSocial = model.RazSocial, DesAtividade = model.DesAtividade, IdentificadorVaga = vagaModel.IdentificadorVaga, IdPerguntaHistoricoExp = model.IdPerguntaHistoricoExp };
        }

        #endregion

        #region Candidatar
        [AllowCrossSiteJsonActionFilter]
        public ActionResult Candidatar(VagaCandidatura model, string button, ExperienciaProfissional modelExp, PerguntaEmail modelEmail, PerguntaCelular modelCelular)
        {
            try
            {
                ViewBag.Error = string.Empty;
                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                {
                    DataTable dtaInformacoesPessoais = BLL.Curriculo.CarregarInformacoesCurriculo(model.IdentificadorVaga, base.IdPessoaFisicaLogada.Value);
                    InformacoesCurriculo informacoesCurriculo = new InformacoesCurriculo();

                    foreach (DataRow row in dtaInformacoesPessoais.Rows)
                    {
                        informacoesCurriculo.idCurriculo = (row[0].ToString() != "" ? Convert.ToInt32(row[0]) : 0);
                        informacoesCurriculo.EhVip = (row[1].ToString() != "" ? Convert.ToBoolean(row[1]) : false);
                        informacoesCurriculo.JaEnvioCvParaVaga = row[2].ToStringNullSafe() != null;
                        informacoesCurriculo.EmpresaBloqueada = row[3].ToStringNullSafe() != "";
                        informacoesCurriculo.EstaNaRegiaoBH = row[4].ToStringNullSafe() != "";
                        informacoesCurriculo.TemExperienciaProfissional = row[5].ToStringNullSafe() != "";
                        informacoesCurriculo.TemFormacao = row[6].ToStringNullSafe() != "";
                        informacoesCurriculo.DisseQueNaoTemExperiencia = row[8].ToStringNullSafe() != "";
                    }

                    //se não tem experiência
                    if (!informacoesCurriculo.DisseQueNaoTemExperiencia && !informacoesCurriculo.TemExperienciaProfissional)
                    {
                        return Content(string.Format("<script>window.location = '{0}'</script>", model.URL));
                    }

                    if (!informacoesCurriculo.TemFormacao)
                    {
                        return Content(string.Format("<script>window.location = '{0}'</script>", model.URL));
                    }

                    if (button != null)
                    {
                        //salvar dados da experiencia profissional
                        if (button.Equals("AtualizarExperiencia"))
                        {
                            SalvarExperienciaProfissional(modelExp, true);
                        }
                        if (button.Equals("ExperienciaCorreta"))
                        {
                            SalvarExperienciaProfissional(modelExp, false);
                        }
                        else if (button.Equals("AtualizarEmail"))
                        {
                            SalvarEmail(modelEmail, true);
                        }
                        else if (button.Equals("EmailCorreto"))
                        {
                            SalvarEmail(modelEmail, false);
                        }
                        else if (button.Equals("AtualizarCelular"))
                        {
                            SalvarCelular(modelCelular, true);
                        }
                        else if (button.Equals("CelularCorreto"))
                        {
                            SalvarCelular(modelCelular, false);
                        }
                        else if (button.Equals("CandidatarOportunidade"))
                            model.FlgCandidataOportunidade = true;
                    }

                    var objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                    var objCurriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                    var objVaga = new BLL.Vaga(model.IdentificadorVaga);
                    string textoCandidatura = string.Empty;

                    #region Exibir pergunta aleatória

                    if (button == null)
                    {
                        model.TotalCandRealizadas = VagaCandidato.QuantidadeVagaCandidato(base.IdCurriculo.Value);
                        int actionName = Convert.ToInt32(PerguntaSalaVipHistorico.CarregarPerguntaSalaVipExibirAoCandidato(base.IdPessoaFisicaLogada.Value));

                        if (model.TotalCandRealizadas > 0 && model.TotalCandRealizadas % 3 == 0)
                        {
                            int totalDias = (DateTime.Now - objPessoaFisica.DataAlteracao).Days;
                            DateTime dtaUltimaAlteracao = DateTime.MinValue;

                            actionName++;

                            switch (actionName)
                            {
                                case 1:
                                    if (totalDias > 30)
                                    {
                                        return PartialView("_ModalPerguntaCelular", PerguntarSobreCelular(model));
                                    }
                                    break;
                                case 2:
                                    if (totalDias > 30)
                                    {
                                        return PartialView("_ModalPerguntaEmail", PerguntarSobreEmail(model));
                                    }
                                    break;
                                case 3:
                                    if (totalDias > 60)
                                    {
                                        return PartialView("_ModalPerguntaExperienciaProfissional", PerguntarSobreExperienciaProfissional(model));
                                    }
                                    break;
                            }
                        }
                    }


                    #endregion

                    #region Curriculo já candidatou
                    if (VagaCandidato.CurriculoJaCandidatouVaga(objCurriculo, objVaga))
                    {
                        string nomeCandidato = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(objCurriculo)).PrimeiroNome;
                        string protocolo = BLL.Vaga.RetornarProtocolo(objCurriculo.IdCurriculo, objVaga.RecuperarCodigo());
                        return PartialView("_VagaCandidatura", new VagaCandidatura { Candidatou = true, IdentificadorVaga = model.IdentificadorVaga, Sucesso = new VagaCandidatura.SucessoCandidatura { Protocolo = protocolo, NomeCandidato = nomeCandidato }, Premium = model.Premium, IndicadoPeloBNE = VagaCandidato.CurriculoFoiCandidatadoAutomaticamente(objCurriculo, objVaga) });
                    }
                    #endregion

                    #region Perguntas
                    var existePerguntas = objVaga.ExistePerguntas();
                    if (existePerguntas && model.Perguntas == null)
                    {
                        var listaVagaPergunta = VagaPergunta.RecuperarListaPerguntas(objVaga.IdVaga, null);
                        var perguntasCandidatura = listaVagaPergunta.Select(objVagaPergunta => new PerguntasCandidatura.Pergunta { Descricao = objVagaPergunta.DescricaoVagaPergunta, Id = objVagaPergunta.IdVagaPergunta, TipoResposta = objVagaPergunta.TipoResposta.IdTipoResposta }).ToList();
                        return PartialView("_VagaCandidatura", new VagaCandidatura { IdentificadorVaga = model.IdentificadorVaga, Perguntas = new PerguntasCandidatura { Perguntas = perguntasCandidatura }, FlgVagaArquivada = model.FlgVagaArquivada, FlgInativa = model.FlgInativa });
                    }
                    #endregion

                    #region Respostas
                    var respostasCandidato = new List<VagaCandidatoPergunta>();

                    if (existePerguntas)
                    {
                        if (model.Perguntas.Perguntas.Count(p => p.Resposta == null && p.TipoResposta == (int)Enumeradores.TipoResposta.RespostaObjetiva) > 0
                            ||
                            model.Perguntas.Perguntas.Count(p => p.DescricaoResposta == null && p.TipoResposta == (int)Enumeradores.TipoResposta.RespostaDescritiva) > 0)
                        {
                            //TODO: Mensagem de erro
                            return PartialView("_VagaCandidatura", model);
                        }
                        respostasCandidato.AddRange(model.Perguntas.Perguntas.Select(pergunta => new VagaCandidatoPergunta { VagaPergunta = new VagaPergunta(pergunta.Id), FlagResposta = pergunta.Resposta, DescricaoResposta = pergunta.DescricaoResposta }));
                    }
                    #endregion

                    #region Oportunidade
                    if (model.FlgVagaArquivada && !model.FlgCandidataOportunidade)
                    {
                        model.FlgCandidataOportunidade = true;
                        return PartialView("_VagaCandidatura", model);
                    }
                    else
                        model.FlgCandidataOportunidade = false;

                    #endregion

                    #region [IndicarAmigos]
                    if (model.IndicouTresAmigos)
                    {
                        string error;
                        model.IndicouTresAmigos = IndicarEmailPessoas(model.PessoasIndicadas, out error);
                        ViewBag.Error = error;
                        if (!string.IsNullOrEmpty(error))
                            return PartialView("_ModalIndiqueTresAmigos", new VagaCandidatura { Candidatou = true, IdentificadorVaga = model.IdentificadorVaga, Degustacao = new VagaCandidatura.DegustacaoCandidatura { DescricaoCandidaturaRestante = string.Empty, QuantidadeCandidaturaRestante = 0 } });
                    }
                    #endregion

                    #region Candidatura
                    Origem objOrigem = null;
                    base.STCUniversitario.Value = false;
                    if (base.STCUniversitario.Value)
                        objOrigem = new Origem(base.IdOrigem.Value);

                    int? quantidadeCandidaturas;
                    var candidatura = VagaCandidato.Candidatar(objCurriculo, objVaga, objOrigem, respostasCandidato, Common.Helper.RecuperarIP(), base.STC.ValueOrDefault, base.STCUniversitario.Value, false, Enumeradores.OrigemCandidatura.Site, out quantidadeCandidaturas);
                    #endregion

                    #region Sucesso candidatura
                    if (quantidadeCandidaturas == null)
                    {
                        string nomeCandidato = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(objCurriculo)).PrimeiroNome;
                        string protocolo = BLL.Vaga.RetornarProtocolo(objCurriculo.IdCurriculo, objVaga.RecuperarCodigo());
                        model.IndicouTresAmigos = false;
                        return PartialView("_VagaCandidatura", new VagaCandidatura { Candidatou = true, IdentificadorVaga = model.IdentificadorVaga, Sucesso = new VagaCandidatura.SucessoCandidatura { Protocolo = protocolo, NomeCandidato = nomeCandidato }, Premium = model.Premium });

                    }
                    #endregion

                    #region Modal degustação
                    if (quantidadeCandidaturas >= 0)
                    {
                        int quantidadeCandidaturasRestantes = (int)quantidadeCandidaturas - 1;
                        var candidatou = quantidadeCandidaturasRestantes >= 0;


                        if (quantidadeCandidaturasRestantes.Equals(-1))
                        {
                            //-------------------------------------------------------------
                            /*
                                Nesse ponto o candidato não possuí mais candidaturas grátis
                                Entretanto se o candidato estiver dentro da região da campanha 
                                Indique Três amigos ele recebe mais Uma nova candidatura.
                            */
                            if (!informacoesCurriculo.EhVip && informacoesCurriculo.EstaNaRegiaoBH)
                            {
                                return PartialView("_ModalIndiqueTresAmigos", new VagaCandidatura { Candidatou = candidatou, IdentificadorVaga = model.IdentificadorVaga, Degustacao = new VagaCandidatura.DegustacaoCandidatura { DescricaoCandidaturaRestante = textoCandidatura, QuantidadeCandidaturaRestante = quantidadeCandidaturasRestantes } });
                            }
                            else
                                textoCandidatura = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoAcabou);
                            //-------------------------------------------------------------
                        }

                        if (quantidadeCandidaturasRestantes.Equals(0))
                        {
                            textoCandidatura = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoAcabou);
                        }

                        if (quantidadeCandidaturasRestantes.Equals(1))
                            textoCandidatura = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoUma);

                        if (quantidadeCandidaturasRestantes >= 2)
                        {
                            string texto = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoDuasOuMais);
                            var numeroPorExtenso = new NumeroPorExtenso(Convert.ToDecimal(quantidadeCandidaturasRestantes), false);
                            var parametros = new
                            {
                                quantidadeCandidaturas = numeroPorExtenso.ToString()
                            };

                            textoCandidatura = parametros.ToString(texto);
                        }



                        return PartialView("_VagaCandidatura", new VagaCandidatura { Candidatou = candidatou, IdentificadorVaga = model.IdentificadorVaga, Degustacao = new VagaCandidatura.DegustacaoCandidatura { DescricaoCandidaturaRestante = textoCandidatura, QuantidadeCandidaturaRestante = quantidadeCandidaturasRestantes }, FlgVagaArquivada = model.FlgVagaArquivada, FlgInativa = model.FlgInativa });
                    }
                    #endregion
                }

                //return Content(string.Format("<script>window.location = '{0}'</script>", model.URL));
                return RedirectToAction("Entrar", "Base");
            }
            catch (Exception ex)
            {
                string result;
                EL.GerenciadorException.GravarExcecao(ex, out result, string.Format("dados vaga => {0} - {1}", model.IdentificadorVaga.ToString(), base.IdPessoaFisicaLogada.Value.ToString()));
                throw;
            }
        }
        #endregion

        #region QueroSerVip
        [OutputCache(CacheProfile = "CacheUmDia")]
        public ActionResult QueroSerVip()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Escolha-de-Plano");
            return Redirect(url);
        }
        #endregion

        #region ComprarCandidatura
        [OutputCache(CacheProfile = "CacheUmDia")]
        public ActionResult ComprarCandidatura(int idVaga)
        {
            //CarregaPlano
            BLL.Plano oPlano =
                Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCandidaturaPremium)));
            //Carrega usuario
            UsuarioFilialPerfil oUsuarioFilialPerfil =
                UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

            var oPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(oUsuarioFilialPerfil, oPlano, null);
            //Vincula a vaga ao planoAdquirido
            PlanoAdquiridoDetalhes.CriarPlanoAdquiridoDetalheVagaPremium(oPlanoAdquirido, idVaga);

            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), String.Format("/Pagamento/{0}", oPlanoAdquirido.IdPlanoAdquirido));
            return Redirect(url);
        }
        #endregion

        #region DadosEmpresa
        //[OutputCache(Duration = 86400, VaryByParam = "", Location = OutputCacheLocation.Client)]
        public PartialViewResult DadosEmpresa(int vaga)
        {
            var objDadosEmpresa = new DadosEmpresa();

            var objVaga = BLL.Vaga.LoadObject(vaga);

            if (objVaga.FlagConfidencial)
            {
                objDadosEmpresa.VagaConfidencial = true;
                objDadosEmpresa.MensagemEmpresaConfidencial = "Esta empresa optou por fazer um processo de recrutamento sigiloso.";
            }
            else
            {
                if (base.IdCurriculo.HasValue)
                {
                    var objCurriculo = new Curriculo(base.IdCurriculo.Value);
                    var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(objCurriculo);
                    objDadosEmpresa.ValorPlanoVIP = new Plano(Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria)).RecuperarValor();
                    objDadosEmpresa.CurriculoVIP = objCurriculo.VIP();
                }
                else
                {
                    objDadosEmpresa.CurriculoVIP = false;
                }

                objVaga.Filial.CompleteObject();
                objVaga.Filial.Endereco.CompleteObject();
                objVaga.Filial.Endereco.Cidade.CompleteObject();
                objDadosEmpresa.NumeroCNPJ = objVaga.Filial.NumeroCNPJ.Value;

                //Verificando se a visualização é para uma vaga específica
                //utilizado para somente exibir a empresa e telefone da vaga se o tipo da origem for parceiro (não há filial cadastrada)
                Origem origem = objVaga.Origem;
                if (origem.TipoOrigem == null)
                {
                    origem.CompleteObject();
                }

                if (origem != null && origem.TipoOrigem.IdTipoOrigem == (int)Enumeradores.TipoOrigem.Parceiro)
                {
                    //TODO: Charan => Criar um parametro para a Origem Sine
                    objDadosEmpresa.VagaSine = objVaga.Filial.IdFilial == 158198;

                    objDadosEmpresa.NomeEmpresa = objVaga.NomeEmpresa;
                    if (String.IsNullOrEmpty(objVaga.NumeroDDD) || String.IsNullOrEmpty(objVaga.NumeroTelefone))
                        objDadosEmpresa.NumeroTelefone = "Não Informado";
                    else if (objVaga.FlagVagaArquivada)
                        objDadosEmpresa.NumeroTelefone = null;
                    else
                        objDadosEmpresa.NumeroTelefone = Helper.FormatarTelefone(objVaga.NumeroDDD, objVaga.NumeroTelefone);

                    //Buscar na API do SINE os dados, dataCadastro empresa, QTD vagas
                    try
                    {
                        BLL.VagaIntegracao objVagaIntegracao;
                        BLL.VagaIntegracao.RecuperarIntegradorPorVaga(objVaga.IdVaga, out objVagaIntegracao);

#if DEBUG
                        string urlApiSine = String.Format("http://localhost:61291/v1.0/");
#else
                            string urlApiSine = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SineApi);
#endif

                        var service = new HttpClient();

                        System.Net.Http.HttpResponseMessage response = service.GetAsync(urlApiSine + "User/GetEstatisticasUsuario?idVaga=" + objVagaIntegracao.CodigoVagaIntegrador).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var retorno = response.Content.ReadAsStringAsync().Result;
                            dynamic result = JsonConvert.DeserializeObject(retorno);

                            objDadosEmpresa.DataCadastro = result.DataCadastro;
                            objDadosEmpresa.QuantidadeVagasDivulgadas = result.TotalVagasAnunciadas;
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao consultar dados da empresa no Sine");
                    }
                }
                else
                {
                    if (objVaga.FlagVagaArquivada)
                        objDadosEmpresa.DesAreaBne = Filial.AreaBne(objVaga.Filial.IdFilial);

                    objDadosEmpresa.NomeEmpresa = objVaga.Filial.RazaoSocial;
                    objDadosEmpresa.DataCadastro = objVaga.Filial.DataCadastro;
                    objDadosEmpresa.QuantidadeFuncionarios = objVaga.Filial.QuantidadeFuncionarios;
                    objDadosEmpresa.Cidade = Helper.FormatarCidade(objVaga.Filial.Endereco.Cidade.NomeCidade, objVaga.Filial.Endereco.Cidade.Estado.SiglaEstado);
                    objDadosEmpresa.Bairro = objVaga.Filial.Endereco.DescricaoBairro;
                    objDadosEmpresa.QuantidadeCurriculosVisualizados = objVaga.Filial.RecuperarQuantidadeCurriculosVisualizados();
                    objDadosEmpresa.QuantidadeVagasDivulgadas = objVaga.Filial.RecuperarQuantidadeVagasDivuldadas();
                    objDadosEmpresa.NumeroTelefone = objVaga.FlagVagaArquivada ? null : Helper.FormatarTelefone(objVaga.Filial.NumeroDDDComercial, objVaga.Filial.NumeroComercial);
                }
            }

            return PartialView("_ModalDadosEmpresa", objDadosEmpresa);
        }
        #endregion

        #region ConfirmacaoCompatilhamentoEmail
        [OutputCache(CacheProfile = "CacheUmDia")]
        public PartialViewResult ConfirmacaoCompatilhamentoEmail()
        {
            return PartialView("_ModalSucessoVagaCompartilhamento", new { Mensagem = "E-mail enviado com sucesso!" }.ToExpando());
        }
        #endregion

        #region ConfirmacaoCompatilhamentoFacebook
        [OutputCache(CacheProfile = "CacheUmDia")]
        public PartialViewResult ConfirmacaoCompatilhamentoFacebook()
        {
            return PartialView("_ModalSucessoVagaCompartilhamento", new { Mensagem = "Vaga Compartilhada com sucesso!" }.ToExpando());
        }
        #endregion

        #region AbrirCompartilhamentoEmail
        public PartialViewResult AbrirCompartilhamentoEmail(int identificador, string url)
        {
            return PartialView("_ModalCompartilhamentoEmail", new CompartilhamentoEmail { IdentificadorVaga = identificador, URL = url, PodeInserirMaisEmails = true });
        }
        #endregion

        #region EnviarCompartilhamentoEmail
        [HttpPost]
        public PartialViewResult EnviarCompartilhamentoEmail(string button, CompartilhamentoEmail compartilhamentoEmail)
        {
            if (button.Equals("adicionar"))
            {
                if (ModelState.IsValid)
                {
                    if (!compartilhamentoEmail.ListaEmailDestinatario.Contains(compartilhamentoEmail.EmailDestinatario))
                        compartilhamentoEmail.ListaEmailDestinatario.Add(compartilhamentoEmail.EmailDestinatario);

                    compartilhamentoEmail.EmailDestinatario = string.Empty;

                    compartilhamentoEmail.PodeInserirMaisEmails = !UltrapassouQuantidadeMaximaEmails(compartilhamentoEmail);
                }
            }
            else if (button.Equals("enviar"))
            {
                int qtdeEmails = 0;

                var objPessoaFisica = BLL.PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                var objVaga = new BLL.Vaga(compartilhamentoEmail.IdentificadorVaga);

                if (!string.IsNullOrEmpty(compartilhamentoEmail.EmailDestinatario))
                {
                    var email = compartilhamentoEmail.ListaEmailDestinatario.Find(x => x == compartilhamentoEmail.EmailDestinatario);
                    if (email == null)
                        compartilhamentoEmail.ListaEmailDestinatario.Add(compartilhamentoEmail.EmailDestinatario);
                }
                foreach (var destinatario in compartilhamentoEmail.ListaEmailDestinatario)
                {
                    MensagemCS.EnviarCompartilhamentoVaga(objPessoaFisica, objVaga, compartilhamentoEmail.URL, destinatario);
                    qtdeEmails++;
                }

                return ConfirmacaoCompatilhamentoEmail();
            }
            return PartialView("_ModalCompartilhamentoEmail", compartilhamentoEmail);
        }
        #endregion

        #region DeletarCompartilhamentoEmail
        [HttpPost]
        public PartialViewResult DeletarCompartilhamentoEmail(string item, CompartilhamentoEmail compartilhamentoEmail)
        {
            compartilhamentoEmail.ListaEmailDestinatario.Remove(item);

            compartilhamentoEmail.PodeInserirMaisEmails = !UltrapassouQuantidadeMaximaEmails(compartilhamentoEmail);

            return PartialView("_ModalCompartilhamentoEmail", compartilhamentoEmail);
        }
        #endregion

        #region UltrapassouQuantidadeMaximaEmails
        private bool UltrapassouQuantidadeMaximaEmails(CompartilhamentoEmail compartilhamentoEmail)
        {
            const int quantidadeMaximaEmails = 5;
            return compartilhamentoEmail.ListaEmailDestinatario.Count >= quantidadeMaximaEmails;
        }
        #endregion

        #region ContabilizaVisualizacao
        [ActionName("VV")]
        public ActionResult ContabilizaVisualizacao(int i)
        {
            try
            {
                var objVaga = new BLL.Vaga(i);
                if (base.IdCurriculo.HasValue)
                {
                    var objCurriculo = new Curriculo(base.IdCurriculo.Value);
                    VagaVisualizada.SalvarVisualizacaoVaga(objVaga, objCurriculo);
                }
                else
                    VagaVisualizada.SalvarVisualizacaoVaga(objVaga);

                return Json(true);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return Json(false);
            }
        }
        #endregion

        #region SalvarCelular

        public void SalvarCelular(PerguntaCelular modelCelular, bool celularAlterado)
        {

            var objPessoa = BLL.PessoaFisica.LoadObject(modelCelular.Identificador);

            if (celularAlterado)
                PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelCelular.IdPerguntaHistorico, modelCelular.NumeroDDDCelularAntigo + "" + modelCelular.NumeroCelularAntigo, !celularAlterado);
            else
                PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelCelular.IdPerguntaHistorico, "", !celularAlterado);

            if (celularAlterado && modelCelular.NumeroCelular != modelCelular.NumeroCelularAntigo)
            {
                objPessoa.NumeroDDDCelular = modelCelular.NumeroDDDCelular;
                objPessoa.NumeroCelular = modelCelular.NumeroCelular;
                objPessoa.FlagCelularConfirmado = false;

                /*
                PessoaFisica.ValidacaoCelularEnviarCodigo(modelCelular.NumeroDDDCelular, modelCelular.NumeroCelular);
                */
            }

            objPessoa.SalvarPessoaFisica(objPessoa);
        }

        #endregion

        #region IndicarEmailPessoas
        private bool IndicarEmailPessoas(List<PessoaIndicada> indicados, out string error)
        {
            try
            {
                error = string.Empty;
                var curriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                var objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                Indicacao ico = new Indicacao(curriculo, objPessoaFisica);

                List<string> numeros = new List<string>();
                foreach (PessoaIndicada pi in indicados)
                {
                    if (pi.Email == objPessoaFisica.EmailPessoa)
                    {
                        error = "Você indicou o próprio e-mail.";
                        return false;
                    }
                    else
                        ico.AdicionarIndicado(pi.Nome, pi.CelularDDD, pi.CelularNumero, pi.Email);
                }


                if (ico.Indicar())
                {
                    return true;
                }
                else
                {
                    error = "Opss, Algo deu errado! Tente novamente mais tarde.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro método IndicarEmailPessoas");
                error = "Opss, Algo deu errado! Tente novamente mais tarde.";
                return false;
            }
        }
        #endregion

        #region IndicarCelularPessoas

        public ActionResult IndicarCelularPessoas(List<PessoaIndicada> indicados)
        {
            try
            {
                var curriculo = Curriculo.LoadObject(base.IdCurriculo.Value);
                var objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                Indicacao ico = new Indicacao(curriculo, objPessoaFisica);

                List<string> numeros = new List<string>();
                foreach (PessoaIndicada pi in indicados)
                {
                    if (pi.CelularDDD.Trim() == objPessoaFisica.NumeroDDDCelular.Trim() && pi.CelularNumero.Trim() == objPessoaFisica.NumeroCelular.Trim())
                    {
                        return Json(new { Status = false, Message = "Você indicou o próprio número de celular." });
                    }
                    else
                    {
                        numeros.Add("'" + pi.CelularDDD + pi.CelularNumero + "'");
                        ico.AdicionarIndicado(pi.Nome, pi.CelularDDD, pi.CelularNumero, pi.Email);
                    }
                }

                List<string> jaIndicados = ico.FiltrarSomenteIndicados(numeros);
                if (jaIndicados.Count > 0)
                {
                    string msg = "Você já indicou o(s) seguinte(s) numero(s): {0}";
                    return Json(new { Status = false, Message = string.Format(msg, string.Join(", ", jaIndicados.ToArray())) });
                }

                if (ico.Indicar())
                    return Json(new { Status = true, Message = "Parabéns! Sua indicação foi realizada com sucesso.<br/>Você ganhou três candidaturas gratuitas." });
                else
                    return Json(new { Status = false, Message = "Opss, Algo deu errado! Tente novamente mais tarde." });

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return Json(new { Status = false, Message = "Opss, Algo deu errado! Tente novamente mais tarde." });
            }
        }
        #endregion

        #region SalvarEmail

        public void SalvarEmail(PerguntaEmail modelEmail, bool emailAlterado)
        {

            var objPessoa = BLL.PessoaFisica.LoadObject(modelEmail.Identificador);

            if (emailAlterado)
            {
                if (modelEmail.Email.Length > 0 && modelEmail.Email.Contains('@'))
                {
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelEmail.IdPerguntaHistorico, modelEmail.EmailAntigo, !emailAlterado);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelEmail.IdPerguntaHistorico, "", !emailAlterado);
            }

            if (emailAlterado)
            {
                objPessoa.EmailPessoa = modelEmail.Email;
                objPessoa.EmailSituacaoConfirmacao = EmailSituacao.LoadObject(Convert.ToInt32(Enumeradores.EmailSituacao.NaoConfirmado));
                objPessoa.EmailSituacaoValidacao = EmailSituacao.LoadObject(Convert.ToInt32(Enumeradores.EmailSituacao.NaoValidado));
                objPessoa.FlagEmailConfirmado = false;
                EnviarCartaValidacaoEmail(objPessoa);
            }

            objPessoa.SalvarPessoaFisica(objPessoa);
        }

        #endregion

        #region SalvarExperienciaProfissional

        public void SalvarExperienciaProfissional(ExperienciaProfissional modelExp, bool experienciaAlterada)
        {
            if (modelExp.RazSocial != null)
            {
                var exp = BLL.ExperienciaProfissional.LoadObject(modelExp.Identificador);

                if (experienciaAlterada)
                {
                    string strAlteracao = string.Format("RazaoSocial:{0},DescricaoAtividades:{1}", modelExp.RazSocial, modelExp.DesAtividade);
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelExp.IdPerguntaHistoricoExp, strAlteracao, !experienciaAlterada);
                    exp.RazaoSocial = modelExp.RazSocial;
                    exp.DescricaoAtividade = modelExp.DesAtividade;
                }
                else
                {
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(modelExp.IdPerguntaHistoricoExp, "", !experienciaAlterada);
                }

                exp.Save();
            }
        }

        #endregion

        #region Enviar Carta de validação de E-mail

        private void EnviarCartaValidacaoEmail(PessoaFisica objPessoaFisica)
        {
            BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
            UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil);
            BLL.Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

            #region Carta de Validação de E-mail

            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente));

            var carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.ValidacaoEmail);

            string assuntoValidacaoEmail = carta.Assunto;
            string templateValidacaoEmail = carta.Conteudo;

            var parametrosValidacaoEmail = new
            {
                Link = string.Format("{0}/{1}?codigo={2}", urlSite, Rota.RecuperarURLRota(Enumeradores.RouteCollection.ConfirmacaoEmail), objPessoaFisica.ValidacaoEmailGerarCodigo(null)),
                NomeCandidato = objPessoaFisica.PrimeiroNome,
                cpf = objPessoaFisica.CPF.ToString(),
                dataNascimento = objPessoaFisica.DataNascimento.ToString("dd/MM/yyyy")
            };
            string mensagemValidacaoEmail = parametrosValidacaoEmail.ToString(templateValidacaoEmail);

            MensagemCS.SalvarEmail(objCurriculo, null, objUsuarioFilialPerfil, null, assuntoValidacaoEmail, mensagemValidacaoEmail, Enumeradores.CartaEmail.ValidacaoEmail,
                emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);

            #endregion
        }

        #endregion

    }
}
