using BNE.BLL;
using BNE.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IMG = System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Bne.Web.Services.API.Business;
using System.Linq;
using Bne.Web.Services.API.DTO.CadastroCV;
using Swashbuckle.Swagger.Annotations;
using BNE.EL;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// API para consulta dos currículos presentes na base do BNE.
    /// </summary>
    public class CurriculoController : BNEApiController
    {
        #region RetornoAuditoriaEmpresa
        private HttpResponseMessage RetornoAuditoriaEmpresa(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo)
        {
            int QuantidadeCurriculosVIPVisualizadosPelaEmpresa = BNE.BLL.CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

            if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
            }
        }
        #endregion

        #region Metodos Da API

        #region ObterCV
        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do CPF e Data de Nascimento.
        /// </summary>
        /// <param name="cpf">Cpf do curriculo desejado</param>
        /// <param name="nascimento">Data de nascimento do currículo desejado</param>
        /// <returns>Objeto com dados completos do curriculo correspondente ao Cpf e Data de Nascimento informado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente ou Data de nascimento informada não confere com a presente no banco de dados.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculoCompleto))]
        public HttpResponseMessage ObterCVPorCpf([FromUri] decimal cpf, [FromUri] DateTime nascimento)
        {
            try
            {
                int idCurriculo;
                if (!BNE.BLL.Curriculo.CarregarIdPorCpf(cpf, out idCurriculo))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Currículos não encontrado");

                var cv = Curriculos.VerDadosCompleto(idCurriculo, null, null, true);
                if (cv.dtaNascimento != nascimento.ToString("yyyy-MM-dd"))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data de nascimento não confere");

                return Request.CreateResponse(HttpStatusCode.OK, cv);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do IdCurriculo.
        /// </summary>
        /// <param name="IdCurriculo">Id do currículo a ser retornado.</param>
        /// <param name="FlgDadosdeContato">Indica se deseja que os dados de contato devem ser retornados. Se true, a consulta será contabilizada como visualização do currículo e será descontada do plano do cliente.</param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculoCompleto))]
        public HttpResponseMessage ObterCV(int IdCurriculo, Boolean? FlgDadosdeContato)
        {
            try
            {
                var objUsuarioFilialPerfil = Login(Request);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        var flgDadosPessoais = FlgDadosdeContato ?? false;
                        var objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                            return errorRequestPost(HttpStatusCode.NotFound, "Currículo inexistente em nossa base de dados!");
                        else
                        {
                            if (flgDadosPessoais)
                            {
                                if (!objFilial.EmpresaEmAuditoria())
                                {
                                    var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                                    var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                                    if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                                                || autorizacaoPelaWebEstagios) // Se tem o parâmetro especifico da webestagios) 
                                    {
                                        if (objCurriculo.VIP())
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(objCurriculo.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objUsuarioFilialPerfil, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, false));
                        }
                    }
                    else
                        return errorRequestPost(HttpStatusCode.Forbidden, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Retorna o cadastro do curriculo selecionado pelo usuario a partir do IdCurriculo.
        /// </summary>
        /// <remarks>
        /// Esse endpoint deve ser utilizado para que a selecionadora tenha acesso às mesmas informações do cadastro do currículo do candidato.
        /// Para recuperar as informações do currículos para o CPF informado na apikey, utilize o endpoint CadastroCv com o method GET.
        /// </remarks>
        /// <param name="IdCurriculo">Id do currículo a ser retornado.</param>
        /// <param name="FlgDadosdeContato">Indica se deseja que os dados de contato devem ser retornados. Se true, a consulta será contabilizada como visualização do currículo e será descontada do plano do cliente.</param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(CadastroCurriculo))]
        public HttpResponseMessage ObterCadastroCV(int IdCurriculo, bool? FlgDadosdeContato)
        {
            try
            {
                var objUsuarioFilialPerfil = Login(Request);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        var flgDadosPessoais = FlgDadosdeContato ?? false;
                        var objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                            return errorRequestPost(HttpStatusCode.NotFound, "Currículo inexistente em nossa base de dados!");
                        else
                        {
                            if (flgDadosPessoais)
                            {
                                if (!objFilial.EmpresaEmAuditoria())
                                {
                                    var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                                    var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                                    if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                                                || autorizacaoPelaWebEstagios) // Se tem o parâmetro especifico da webestagios) 
                                    {
                                        if (objCurriculo.VIP())
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCadastro(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCadastro(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCadastro(objCurriculo.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objUsuarioFilialPerfil, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCadastro(IdCurriculo, objFilial, objUsuarioFilialPerfil, false));
                        }
                    }
                    else
                        return errorRequestPost(HttpStatusCode.Forbidden, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }


        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do IdCurriculo.
        /// </summary>
        /// <param name="objParamentros"></param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente</response>
        /// <response code="500">Internal Server Error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculoCompleto))]
        public HttpResponseMessage ObterCV([FromBody]DTO.PesquisaCurriculoCompleto objParametros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (Request.Headers.Contains("Num_CPF"))
                    objUsuarioFilialPerfil = Login(Request);
                else
                    objUsuarioFilialPerfil = Login(Convert.ToDecimal(objParametros.CPF), objParametros.DataNascimento);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        bool flgDadosPessoais = objParametros.FlgDadosdeContato.HasValue ? objParametros.FlgDadosdeContato.Value : false;
                        Filial objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(objParametros.IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                            return errorRequestPost(HttpStatusCode.NotFound, "Currículo inexistente em nossa base de dados!");
                        else
                        {
                            if (flgDadosPessoais)
                            {
                                if (!objFilial.EmpresaEmAuditoria())
                                {
                                    var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                                    var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                                    if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                                                || autorizacaoPelaWebEstagios) // Se tem o parâmetro especifico da webestagios) 
                                    {
                                        if (objCurriculo.VIP())
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(objCurriculo.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objUsuarioFilialPerfil, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, Curriculos.VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, false));
                        }
                    }
                    else
                        return errorRequestPost(HttpStatusCode.Forbidden, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #region DownloadArquivo
        [HttpGet]
        public HttpResponseMessage DownloadArquivo(decimal cpf)
        {
            try
            {
                string sNomeArquivo;
                var bytes = PessoaFisicaComplemento.CarregarAnexoPorCPFDoStorage(cpf, out sNomeArquivo);

                if (String.IsNullOrEmpty(sNomeArquivo) || bytes == null || bytes.Length <= 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Arquivo inexistente");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(bytes);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(BNE.BLL.Custom.Helper.GetMIMEType(sNomeArquivo));
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = sNomeArquivo
                };
                return result;

            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        #endregion DownloadArquivo

        #endregion

        #region PesquisaAvancada
        /// <summary>
        /// Retorna uma lista de currículos baseada nos parâmetros informados.
        /// </summary>
        /// <param name="filtros">Objeto com os filtros a serem aplicados na pesquisa de curriculos</param>
        /// <returns>Objeto contendo a lista de mini currículos filtrada, juntamente com os registros de totalização para paginação</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculo))]
        public HttpResponseMessage PesquisaAvancada([FromBody]DTO.PesquisaCurriculo filtros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
#if DEBUG
                objUsuarioFilialPerfil = Login(Request);
#else
                if (Request.Headers.Contains("Num_CPF"))
                    objUsuarioFilialPerfil = Login(Request);
                else
                    objUsuarioFilialPerfil = Login(Convert.ToDecimal(filtros.CPF), filtros.DataNascimento);
#endif

                if (objUsuarioFilialPerfil == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Usuário não encontrado");
                }

                BNE.BLL.PesquisaCurriculo objPesquisaCurriculo = new BNE.BLL.PesquisaCurriculo();
                objPesquisaCurriculo.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objPesquisaCurriculo.FlagPesquisaAvancada = true;
                objPesquisaCurriculo.DescricaoIP = HttpContext.Current.Request.UserHostAddress;

                #region criando objeto de consulta

                Cidade objCidade = BNE.BLL.Custom.Helper.DetectaCidade(filtros.Cidade);
                if (objCidade != null)
                {
                    objPesquisaCurriculo.Cidade = new Cidade(objCidade.IdCidade) { NomeCidade = objCidade.NomeCidade };
                }

                var listaFuncao = new List<PesquisaCurriculoFuncao>();
                if (!String.IsNullOrEmpty(filtros.Funcao))
                {
                    var funcao = Funcao.CarregarPorDescricao(filtros.Funcao);
                    if (funcao != null)
                        listaFuncao.Add(new PesquisaCurriculoFuncao { Funcao = funcao });
                }
                if (filtros.ListaFuncoes != null)
                {
                    foreach (var f in filtros.ListaFuncoes)
                    {
                        var funcao = Funcao.CarregarPorDescricao(f);
                        if (funcao != null)
                            listaFuncao.Add(new PesquisaCurriculoFuncao { Funcao = funcao });
                    }
                }

                objPesquisaCurriculo.DescricaoPalavraChave = filtros.PalavraChave;
                objPesquisaCurriculo.DescricaoFiltroExcludente = filtros.ExcluirPalavraChave;
                objPesquisaCurriculo.DescricaoExperiencia = filtros.PalavraChaveExperiencia;

                if (!String.IsNullOrEmpty(filtros.FuncaoExperiencia))
                {
                    var funcao = Funcao.CarregarPorDescricao(filtros.FuncaoExperiencia);
                    if (funcao != null)
                        objPesquisaCurriculo.FuncaoExercida = new Funcao(funcao.IdFuncao) { DescricaoFuncao = funcao.DescricaoFuncao };
                    else
                        objPesquisaCurriculo.DescricaoFuncaoExercida = filtros.FuncaoExperiencia;
                }

                if (!String.IsNullOrEmpty(filtros.Estado))
                    objPesquisaCurriculo.Estado = new Estado(filtros.Estado);

                if (!String.IsNullOrEmpty(filtros.Escolaridade))
                {
                    Escolaridade escolaridade;
                    if (Escolaridade.CarregarPorNome(filtros.Escolaridade, out escolaridade))
                        objPesquisaCurriculo.Escolaridade = escolaridade;
                }

                if (filtros.Sexo.HasValue)
                {
                    objPesquisaCurriculo.Sexo = new Sexo((int)filtros.Sexo);
                    objPesquisaCurriculo.Sexo.SiglaSexo = filtros.Sexo.Value.ToString()[0];
                }

                if (filtros.IdadeMinima.HasValue)
                    objPesquisaCurriculo.NumeroIdadeMin = (short)filtros.IdadeMinima.Value;

                if (filtros.IdadeMaxima.HasValue)
                    objPesquisaCurriculo.NumeroIdadeMax = (short)filtros.IdadeMaxima.Value;

                objPesquisaCurriculo.NumeroSalarioMin = filtros.SalarioMinimo;
                objPesquisaCurriculo.NumeroSalarioMax = filtros.SalarioMaximo;
                objPesquisaCurriculo.QuantidadeExperiencia = (short?)filtros.QuantidadeExperiencia;

                if (!string.IsNullOrEmpty(filtros.CodCPFNome))
                    objPesquisaCurriculo.DescricaoCodCPFNome = filtros.CodCPFNome.Replace(";", ",");
                else
                    objPesquisaCurriculo.DescricaoCodCPFNome = filtros.CodCPFNome;

                if (!string.IsNullOrEmpty(filtros.EstadoCivil))
                {
                    var ec = EstadoCivil.LoadObject(filtros.EstadoCivil);
                    if (ec != null)
                        objPesquisaCurriculo.EstadoCivil = ec;
                }

                objPesquisaCurriculo.DescricaoBairro = filtros.Bairro;
                objPesquisaCurriculo.DescricaoLogradouro = filtros.Logradouro;
                objPesquisaCurriculo.NumeroCEPMin = filtros.CEPMinimo;
                objPesquisaCurriculo.NumeroCEPMax = filtros.CEPMaximo;

                //Fonte e curso
                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(filtros.InstituicaoTecnicoGraduacao))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(filtros.InstituicaoTecnicoGraduacao, out objFonte))
                        objPesquisaCurriculo.FonteTecnicoGraduacao = objFonte;
                }

                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(filtros.CursoTecnicoGraduacao))
                {
                    BNE.BLL.Curso objCurso;
                    if (BNE.BLL.Curso.BuscaCurso(filtros.CursoTecnicoGraduacao, out objCurso))
                        objPesquisaCurriculo.CursoTecnicoGraduacao = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao = filtros.CursoTecnicoGraduacao;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.InstituicaoOutrosCursos))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(filtros.InstituicaoOutrosCursos, out objFonte))
                        objPesquisaCurriculo.FonteOutrosCursos = objFonte;
                }

                if (!String.IsNullOrEmpty(filtros.CursoOutrosCursos))
                {
                    BNE.BLL.Curso objCurso;
                    if (BNE.BLL.Curso.BuscaCurso(filtros.CursoOutrosCursos, out objCurso))
                        objPesquisaCurriculo.CursoOutrosCursos = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoOutrosCursos = filtros.CursoOutrosCursos;
                    }
                }

                objPesquisaCurriculo.RazaoSocial = filtros.EmpresaQueJaTrabalhou;

                //@todo table
                if (!String.IsNullOrEmpty(filtros.AreaEmpresaQueJaTrabalhou))
                {
                    AreaBNE objAreaBNE;
                    if (AreaBNE.CarregarPorDescricao(filtros.AreaEmpresaQueJaTrabalhou, out objAreaBNE))
                    {
                        objPesquisaCurriculo.AreaBNE = objAreaBNE;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.CategoriaHabilitacao))
                {
                    CategoriaHabilitacao objCategoriaHabilitacao;
                    if (CategoriaHabilitacao.CarregarPorDescricao(filtros.CategoriaHabilitacao, out objCategoriaHabilitacao))
                    {
                        objPesquisaCurriculo.CategoriaHabilitacao = objCategoriaHabilitacao;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.TipoVeiculo))
                {
                    TipoVeiculo objTipoVeiculo = TipoVeiculo.CarregarPorDescricao(filtros.TipoVeiculo);
                    if (objTipoVeiculo != null)
                    {
                        objPesquisaCurriculo.TipoVeiculo = objTipoVeiculo;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.Deficiencia))
                {
                    Deficiencia objDeficiencia = Deficiencia.CarregarPorDescricao(filtros.Deficiencia);
                    if (objDeficiencia != null)
                    {
                        objPesquisaCurriculo.Deficiencia = objDeficiencia;
                    }
                }

                objPesquisaCurriculo.NumeroDDDTelefone = filtros.DDDTelefone;
                objPesquisaCurriculo.NumeroTelefone = filtros.NumeroTelefone;
                objPesquisaCurriculo.EmailPessoa = filtros.Email;

                if (!String.IsNullOrEmpty(filtros.Raca))
                {
                    Raca objRaca = Raca.CarregarPorDescricao(filtros.Raca);
                    objPesquisaCurriculo.Raca = objRaca;
                }

                if (filtros.PossuiFilhos.HasValue)
                    objPesquisaCurriculo.FlagFilhos = filtros.PossuiFilhos.Value;

                var listPesquisaCurriculoIdioma = new List<PesquisaCurriculoIdioma>();
                if (filtros.Idioma != null)
                {
                    foreach (var idioma in filtros.Idioma)
                    {
                        Idioma objIdioma;
                        if (Idioma.CarregarPorNome(idioma, out objIdioma))
                            listPesquisaCurriculoIdioma.Add(new PesquisaCurriculoIdioma()
                            {
                                Idioma = objIdioma
                            });
                    }
                }

                var listPesquisaCurriculoDisponibilidade = new List<PesquisaCurriculoDisponibilidade>();
                if (filtros.Disponibilidades != null)
                {
                    foreach (var d in filtros.Disponibilidades)
                    {
                        listPesquisaCurriculoDisponibilidade.Add(new PesquisaCurriculoDisponibilidade()
                        {
                            Disponibilidade = new Disponibilidade((int)d)
                        });
                    }
                }

                #region Formacao
                if (filtros.Formacao != null)
                {
                    if (!string.IsNullOrEmpty(filtros.Formacao.Escolaridade))
                    {
                        Escolaridade escolaridade;
                        if (Escolaridade.CarregarPorNome(filtros.Formacao.Escolaridade, out escolaridade))
                            objPesquisaCurriculo.EscolaridadeFormacao = escolaridade;
                    }

                    if (!string.IsNullOrEmpty(filtros.Formacao.Curso))
                    {
                        BNE.BLL.Curso objCurso;
                        if (BNE.BLL.Curso.BuscaCurso(filtros.Formacao.Curso, out objCurso))
                            objPesquisaCurriculo.CursoFormacao = objCurso;
                        else
                        {
                            objPesquisaCurriculo.DescricaoCursoFormacao = filtros.Formacao.Curso;
                        }
                    }

                    if (!string.IsNullOrEmpty(filtros.Formacao.Instituicao))
                    {
                        Fonte objFonte;
                        if (Fonte.BuscarPorDescricao(filtros.Formacao.Instituicao, out objFonte))
                            objPesquisaCurriculo.FonteFormacao = objFonte;
                        else
                            objPesquisaCurriculo.DescricaoFonteFormacao = filtros.Formacao.Instituicao;
                    }

                    objPesquisaCurriculo.AnoConclusaoFormacao = filtros.Formacao.AnoConclusao.HasValue ?
                        (short?)filtros.Formacao.AnoConclusao.Value : null;

                    objPesquisaCurriculo.NumeroPeriodoFormacao = filtros.Formacao.Periodo.HasValue ?
                        (short?)filtros.Formacao.Periodo.Value : null;

                    if (filtros.Formacao.SituacaoCurso.HasValue)
                        objPesquisaCurriculo.SituacaoCursoFormacao = 
                            new SituacaoFormacao((short)filtros.Formacao.SituacaoCurso);

                    if (!string.IsNullOrWhiteSpace(filtros.Formacao.Cidade))
                    {
                        Cidade objCidadeCurso = BNE.BLL.Custom.Helper.DetectaCidade(filtros.Formacao.Cidade);
                        if (objCidadeCurso != null)
                        {
                            objPesquisaCurriculo.CidadeFormacao = new Cidade(objCidadeCurso.IdCidade) { NomeCidade = objCidadeCurso.NomeCidade };
                        }
                    }

                    
                }
                #endregion Formacao

                var listaCampoBuscaPalavraChave = new List<CampoPalavraChavePesquisaCurriculo>();
                #endregion criando objeto de consulta

                //não sei se vai ser implementado no mobile / api
                var listaDeficiencia = new List<PesquisaCurriculoDeficiencia>();

                objPesquisaCurriculo.Salvar(listaFuncao, listPesquisaCurriculoIdioma, listPesquisaCurriculoDisponibilidade, listaCampoBuscaPalavraChave, listaDeficiencia, filtros.QueroContratarEstagiarios);

                DTO.ResultadoPesquisaCurriculo resultadoPesquisa = new DTO.ResultadoPesquisaCurriculo();
                int numeroRegistros;
                decimal mediaSalarial;
                int PageSize = filtros.RegistrosPorPagina > 0 ? filtros.RegistrosPorPagina : Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));
                int? filial = objUsuarioFilialPerfil.Filial == null ? null : (int?)objUsuarioFilialPerfil.Filial.IdFilial;
                var listFiltros = new Dictionary<string, int>();
                DataTable resultado = BNE.BLL.PesquisaCurriculo.BuscaCurriculo(PageSize, filtros.Pagina - 1, 1, filial, null, objPesquisaCurriculo, out numeroRegistros, out mediaSalarial,out listFiltros ,false, false);

                resultadoPesquisa.TotalDeRegistros = numeroRegistros;
                resultadoPesquisa.RegistrosPorPagina = PageSize;
                List<DTO.MiniCurriculo> lstRetorno = new List<DTO.MiniCurriculo>();
                foreach (DataRow row in resultado.Rows)
                {
                    lstRetorno.Add(new DTO.MiniCurriculo(row));
                }
                resultadoPesquisa.Curriculos = lstRetorno;
                return Request.CreateResponse(HttpStatusCode.OK, resultadoPesquisa);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #endregion

        #region CadastroCurriculo
        /// <summary>
        /// Obtem os dados do cadastro do currículo.
        /// </summary>
        /// <remarks>Endpoint utilizado para a obtenção dos dados de cadastro do candidato.
        /// O currículo retornado será o pertencente ao CPF informado na apikey ou do CPF informado na query string.
        /// Esse endpoint deve ser utilizado para a exibição dos dados exclusivamente aos candidatos ou para sistemas internos do BNE.
        /// Para a exibição de currículos às empresas, o endpoint /v1.0/Curriculo/ObterCV deve ser utilizado.</remarks>
        /// <param name="cpf">CPF do currículo a ser carregado. Somente usuários administradores podem informar esse parâmetro.</param>
        /// <returns>Dados para o novo currículo</returns>
        [HttpGet]
        [ResponseType(typeof(CadastroCurriculo))]
        [Route("v1.0/Curriculo/CadastroCV")]
        public HttpResponseMessage GetCadastroCV(decimal? cpf = null)
        {
            PessoaFisica objPessoaFisica;
            var objUsuarioFilialPerfil = Login(Request, out objPessoaFisica);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            // Se CPF informado, somente usuarios administradores devem ser permitidos
            if (cpf != null && cpf.HasValue)
            {
                UsuarioFilialPerfil objUsuarioFilialPerfilAdm;
                if (!UsuarioFilialPerfil.ObterPorCpfPerfil(objPessoaFisica.CPF, (int)BNE.BLL.Enumeradores.Perfil.AdministradorSistema, out objUsuarioFilialPerfilAdm))
                    return errorRequestPost(HttpStatusCode.Forbidden, "Somente usuários administradores podem obter o currículo de outro CPF.");

                try
                {
                    objPessoaFisica = PessoaFisica.CarregarPorCPF(cpf.Value);
                }
                catch (RecordNotFoundException)
                {
                    return errorRequestPost(HttpStatusCode.NotFound, "Não existe currículo para o CPF informado.");
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, Business.Curriculos.CarregarCV(objPessoaFisica));
        }
        #endregion CadastroCurriculo

        #region CadastroCurriculo
        /// <summary>
        /// Cadastra um novo currículo
        /// </summary>
        /// <remarks>Endpoint utilizado somente para a inserção de novos currículos. 
        /// Se um currículo já existe for informado, a chamada será respondida com um erro.</remarks>
        /// <param name="curriculo"></param>
        /// <returns>Dados para o novo currículo</returns>
        [HttpPost]
        [ResponseType(typeof(ResultadoCadastroCVDTO))]
        [Route("v1.0/Curriculo/CadastroCV")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "CPF já cadastrado para um currículo")]
        public HttpResponseMessage PostCadastroCV([FromBody]DTO.CadastroCV.CadastroCurriculo curriculo)
        {
            int idOrigem;
            string message;
            if (!Request.Headers.Contains("OrigemBNE") ||
                !int.TryParse(Request.Headers.GetValues("OrigemBNE").First(), out idOrigem))
                idOrigem = 0;

            int? idCurriculo;
            if (Business.Curriculos.Salvar(null,
                curriculo,
                idOrigem <= 0 ? null : (int?)idOrigem,
                out idCurriculo,
                out message))
                return Request.CreateResponse(HttpStatusCode.OK, new ResultadoCadastroCVDTO() { CodigoCurriculo = idCurriculo });
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);

        }
        #endregion CadastroCurriculo

        #region CadastroCurriculo
        /// <summary>
        /// Atualiza um currículo
        /// </summary>
        /// <remarks>Endpoint utilizado somente para a atualiza de currículos. 
        /// O currículo alterado será o pertencente ao CPF informado na apikey.
        /// As propriedades são opcionais e não serão atualizadas caso não sejam informadas.
        /// Para apagar o valor da propriedade, indique um valor vazio na atualização.</remarks>
        /// <param name="curriculo">Dados do currículo para atualização</param>
        /// <returns>Dados para o novo currículo</returns>
        [HttpPut]
        [ResponseType(typeof(ResultadoCadastroCVDTO))]
        [Route("v1.0/Curriculo/CadastroCV")]
        public HttpResponseMessage PutCadastroCV([FromBody]DTO.CadastroCV.CadastroCurriculo curriculo)
        {
            PessoaFisica objPessoaFisica;
            var objUsuarioFilialPerfil = Login(Request, out objPessoaFisica);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            int idOrigem;
            string message;
            if (!Request.Headers.Contains("OrigemBNE") ||
                !int.TryParse(Request.Headers.GetValues("OrigemBNE").First(), out idOrigem))
                idOrigem = 0;

            int? idCurriculo;
            if (Business.Curriculos.Salvar(objPessoaFisica,
                curriculo,
                idOrigem <= 0 ? null : (int?)idOrigem,
                out idCurriculo,
                out message))
                return Request.CreateResponse(HttpStatusCode.OK, new ResultadoCadastroCVDTO() { CodigoCurriculo = idCurriculo });
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, message);

        }
        #endregion CadastroCurriculo

        #region PostFoto
        /// <summary>
        /// Atualiza a foto do currículo
        /// </summary>
        /// <remarks>A foto será definida para o pertencente ao CPF informado na apikey.
        /// O tamanho mínimo para uma imagem é de 100px X 100px.
        /// Para apagar a foto presente no currículo, efetue um POST sem nenhum dado.</remarks>
        /// <returns>Dados para o novo currículo</returns>
        [HttpPost]
        [ResponseType(typeof(ResultadoCadastroCVDTO))]
        [Route("v1.0/Curriculo/Foto")]
        public HttpResponseMessage PostFoto()
        {
            try
            {
                PessoaFisicaFoto objPessoaFisicaFoto;
                PessoaFisica objPessoaFisica;
                var objUsuarioFilialPerfil = Login(Request, out objPessoaFisica);

                if (objUsuarioFilialPerfil == null)
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

                if (!PessoaFisicaFoto.CarregarFoto(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaFoto))
                    objPessoaFisicaFoto = new PessoaFisicaFoto() { PessoaFisica = objPessoaFisica, FlagInativo = false };

                if (!Request.Content.IsMimeMultipartContent())
                    return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Tipo arquivo não suportado");

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var postedFile = httpRequest.Files[0];

                    byte[] ba = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        ba = binaryReader.ReadBytes(postedFile.ContentLength);
                    }

                    try
                    {
                        ValidateImage(ba);
                    }
                    catch (Exception ex)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                    }

                    var s = new System.Drawing.Size(178, 133);

                    System.Drawing.Image image = null;
                    using (var ms = new MemoryStream(ba))
                    {
                        image = System.Drawing.Image.FromStream(ms);
                    }

                    var bitmap = ResizeImage(image, s);

                    var converter = new System.Drawing.ImageConverter();
                    ba = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

                    objPessoaFisicaFoto.FlagInativo = false;
                    objPessoaFisicaFoto.ImagemPessoa = ba;
                }
                else
                {
                    objPessoaFisicaFoto.ImagemPessoa = null;
                    objPessoaFisicaFoto.FlagInativo = true;
                }

                objPessoaFisicaFoto.Save();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        #endregion PostFoto

        #region ValidateImage
        /// <summary>
        ///     Valida o MimeType de uma imagem, ou se consegue abrir o arquivo corretamente usando o objeto Image
        /// </summary>
        /// <param name="ba">A imagem</param>
        /// <returns>True se a imagem for válida, false do contrário</returns>
        public bool ValidateImage(byte[] ba)
        {
            if (ba == null)
                throw new ArgumentNullException("ba");

            System.Drawing.Image image = null;
            try
            {
                try
                {
                    using (var ms = new MemoryStream(ba))
                    {
                        image = System.Drawing.Image.FromStream(ms);
                    }
                }
                catch //Resultado esperando quando não está em um formato válido de imagem.
                {
                    throw new Exception("Formato de imagem inválido");
                }

                if (image != null)
                {
                    if (image.Height < 100)
                        throw new Exception("Altura menor que o tamanho esperado: 100px");

                    if (image.Width < 100)
                        throw new Exception("Comprimento menor que o tamanho esperado: 100px");

                    return true;
                }
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }

            return false;
        }
        #endregion

        #region ResizeImage
        /// <summary>
        ///     Resize image
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private System.Drawing.Bitmap ResizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            float nPercent;

            var nPercentW = (size.Width / (float)sourceWidth);
            var nPercentH = (size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new System.Drawing.Bitmap(destWidth, destHeight);
            var g = System.Drawing.Graphics.FromImage(b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }
        #endregion ResizeImage

        #endregion
    }
}
