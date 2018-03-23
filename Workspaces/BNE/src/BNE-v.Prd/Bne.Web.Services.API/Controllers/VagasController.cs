using Bne.Web.Services.API.DTO;
using BNE.BLL.Custom;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BNE.BLL.Enumeradores;
using bs = Bne.Web.Services.API.Business;
using VagaEmpresa = Bne.Web.Services.API.DTO.VagaEmpresa;
using Bne.Web.Services.API.DTO.Enum;
using System.Web.Http.ModelBinding;
using Bne.Web.Services.API.DTO.Query;
using Bne.Web.Services.API.DTO.Util;
using Swashbuckle.Swagger.Annotations;
using Bne.Web.Services.API.DTO.Empresas;
using BNE.BLL;
using SharedKernel.API.ModelBinders;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// Endpoints para manutenção de vagas
    /// </summary>
    public class VagasController : BNEApiController
    {
        /// <summary>
        /// Recupera a lista de vagas cadastradas para a api key informada
        /// </summary>
        /// <param name="pagina">Página que deve ser retornada. A primeira página é "1".</param>
        /// <param name="registrosPorPagina">Número de registros a ser retornado por página. Default: 10</param>
        /// <param name="somenteMinhasVagas">Se true, retorna somente as vagas que foram anunciadas pelo CPF presente na ApiKey. Se false, retorna as vagas cadastradas para a empresa informada no APIKey.</param>
        /// <param name="statusVaga">Status da vaga.</param>
        /// <param name="tipoVinculo">Tipo de vínculo utilizado para filtrar as vagas.</param>
        /// <remarks>Endpoint utilizado para acesso às vagas da empresa. As vagas retornadas serão filtradas baseadas nas informações presentes na APIKey passada.</remarks>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResultadoPaginado<VagaEmpresa>))]
        [Route("v1.0/Vagas/Empresa")]
        public HttpResponseMessage Get(int pagina = 1,
            int registrosPorPagina = 10,
            bool somenteMinhasVagas = true,
            DTO.Enum.TipoVinculo? tipoVinculo = null,
            StatusVaga? statusVaga = null)
        {
            var objUsuarioFilialPerfil = Login(Request);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            int? idfUsuarioFilialPerfil = somenteMinhasVagas ? (int?)objUsuarioFilialPerfil.IdUsuarioFilialPerfil : null;
            bool? flgInativo = null; bool? flgArquivada = null; bool? flgAuditada = null;
            switch (statusVaga)
            {
                case StatusVaga.EmPublicacao:
                    flgInativo = flgArquivada = flgAuditada = false;
                    break;
                case StatusVaga.Ativa:
                    flgAuditada = true;
                    flgInativo = flgArquivada = false;
                    break;
                case StatusVaga.Arquivada:
                    flgArquivada = true;
                    break;
                case StatusVaga.Inativa:
                    flgInativo = true;
                    break;
                default:
                    break;
            }

            List<VagaEmpresa> retorno = new List<DTO.VagaEmpresa>();

            int totalRegistros = 0;
            foreach (var objVaga in BNE.BLL.Vaga.ObterVagasFilial(pagina, registrosPorPagina, objUsuarioFilialPerfil.Filial.IdFilial, idfUsuarioFilialPerfil, (int?)tipoVinculo, flgInativo, flgArquivada, flgAuditada, out totalRegistros))
            {
                retorno.Add(new VagaEmpresa(objVaga));
            }

            return Request.CreateResponse(HttpStatusCode.OK, new ResultadoPaginado<VagaEmpresa>(totalRegistros, pagina, registrosPorPagina, retorno));
        }

        /// <summary>
        /// Recupera uma vaga baseada em seu ID
        /// </summary>
        /// <param name="id">Id da Vaga a ser recuperada</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(VagaEmpresa))]
        [Route("v1.0/Vagas/Empresa/{id}")]
        public HttpResponseMessage GetEmpresa(int id)
        {
            var objUsuarioFilialPerfil = Login(Request);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            var objVaga = BNE.BLL.Vaga.LoadObject(id);

            if (objVaga.Filial.IdFilial != objUsuarioFilialPerfil.Filial.IdFilial)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "O usuário não tem acesso a essa vaga");

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new VagaEmpresa(objVaga));
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao recuperar a vaga");
        }

        /// <summary>
        /// Pesquisa de vagas
        /// </summary>
        /// <remarks>Endpoint para pesquisa de vagas.</remarks>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResultadoPesquisaVaga))]
        [Route("v1.0/Vagas")]
        [SwaggerResponse(HttpStatusCode.Forbidden, "ApiKey não informada ou falha no login para os dados informados")]
        public HttpResponseMessage Get([ModelBinder(typeof(DataMemberFromUri))] QueryVagas p)
        {
            try
            {
                //var objUsuarioFilialPerfil = Login(Request);

                //if (objUsuarioFilialPerfil == null)
                //    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

                var resultado = BNE.BLL.Vaga.BuscarVagasSolr(p.Pagina - 1,
                    p.RegistrosPorPagina,
                    p.Query,
                    p.SalarioMinimo,
                    p.SalarioMaximo,
                    p.TipoVinculo?.Cast<int>().ToArray(),
                    p.Funcao,
                    p.Area,
                    p.Escolaridade,
                    p.Deficiencia?.Cast<int>().ToArray(),
                    p.SiglaEstado,
                    p.NomeCidade,
                    p.Empresa,
                    p.DataInicio,
                    p.DataFim,
                    p.Oportunidade,
                    p.IdOrigem,
                    p.Ordenacao,
                    p.Curso,
                    p.IdCurso,
                    p.Curriculo,
                    p.Disponibilidade?.Cast<int>().ToArray(),
                    p.OfereceCurso,
                    p.FuncaoAgrupadora,
                    p.CidadeRegiao,
                    p.IdfFilial,
                    p.Confidencial,
                    p.Campanha,
                    p.UsuarioFilial?.Cast<int>().ToArray(),
                    Business.Vagas.MapFacetFields(p.FacetField));

                var registros = new List<DTO.Vaga>();
                foreach (var item in resultado.response.docs)
                {
                    registros.Add(new DTO.Vaga(item));
                }

                var retorno = new ResultadoPesquisaVaga(resultado.response.numFound,
                            p.Pagina,
                            p.RegistrosPorPagina,
                            registros);

                if (resultado.facet_counts != null)
                    foreach (var facetField in resultado.facet_counts.facet_fields)
                        retorno.Facets.Add(new FacetField(Business.Vagas.GetFacetField(facetField.Key), facetField.Value));

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Recupera uma vaga baseada em seu ID
        /// </summary>
        /// <param name="id">Id da Vaga a ser recuperada</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(VagaEmpresa))]
        [Route("v1.0/Vagas/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var objVaga = BNE.BLL.Vaga.LoadObject(id);

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new DTO.Vaga(objVaga));
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao recuperar a vaga");
        }

        /// <summary>
        /// Recupera os dados da empresa que anunciou a vaga
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(DadosEmpresa))]
        [Route("v1.0/Vagas/DadosDaEmpresa")]
        [SwaggerResponse(HttpStatusCode.Forbidden, "ApiKey não informada ou falha no login para os dados informados")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "Empresa optou por deixar a vaga confidencial.")]
        public HttpResponseMessage DadosDaEmpresa(int idVaga)
        {
            try
            {
                var objVaga = BNE.BLL.Vaga.LoadObject(idVaga);

                if (objVaga.FlagConfidencial)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "A empresa optou por deixar seus dados confidenciais");

                objVaga.Filial.CompleteObject();
                objVaga.Filial.Endereco.CompleteObject();
                objVaga.Filial.Endereco.Cidade.CompleteObject();

                var objDadosEmpresa = new DadosEmpresa();

                objDadosEmpresa.NumeroCNPJ = objVaga.Filial.NumeroCNPJ.Value;
                objDadosEmpresa.DesAreaBne = Filial.AreaBne(objVaga.Filial.IdFilial);
                objDadosEmpresa.NomeEmpresa = objVaga.Filial.RazaoSocial;
                objDadosEmpresa.DataCadastro = objVaga.Filial.DataCadastro;
                objDadosEmpresa.QuantidadeFuncionarios = objVaga.Filial.QuantidadeFuncionarios;
                objDadosEmpresa.Cidade = Helper.FormatarCidade(objVaga.Filial.Endereco.Cidade.NomeCidade, objVaga.Filial.Endereco.Cidade.Estado.SiglaEstado);
                objDadosEmpresa.Bairro = objVaga.Filial.Endereco.DescricaoBairro;
                objDadosEmpresa.QuantidadeVagasDivulgadas = objVaga.Filial.RecuperarQuantidadeVagasDivuldadas();
                objDadosEmpresa.NumeroTelefone = Helper.FormatarTelefone(objVaga.Filial.NumeroDDDComercial, objVaga.Filial.NumeroComercial);

                return Request.CreateResponse(HttpStatusCode.OK, objDadosEmpresa);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Recupera uma vaga baseada em seu ID
        /// </summary>
        /// <param name="id">Id da Vaga a ser recuperada</param>
        /// <param name="novoStatusVaga">Status a ser assumido pela vaga. 
        /// O status "Inativa", irá exibir a vaga como oportunidade para o candidato, informando que a vaga está fechada, mas a empresa aceita receber currículos com o perfil da vaga.
        /// Para o status "Arquivada", A vaga deixará de ser exibida para os candidatos.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("v1.0/Vagas/AtualizarStatus/{id}")]
        [ResponseType(typeof(VagaEmpresa))]
        public HttpResponseMessage AtualizarStatus(int id, StatusVaga novoStatusVaga)
        {
            var objUsuarioFilialPerfil = Login(Request);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            var objVaga = BNE.BLL.Vaga.LoadObject(id);

            if (objVaga.Filial.IdFilial != objUsuarioFilialPerfil.Filial.IdFilial)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "O usuário não tem acesso a essa vaga");

            try
            {
                switch (novoStatusVaga)
                {
                    case StatusVaga.Ativa:
                        objVaga.AtivarVaga(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, VagaLog.API);
                        break;
                    case StatusVaga.Arquivada:
                        objVaga.ArquivarVaga(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, VagaLog.API);
                        break;
                    case StatusVaga.Inativa:
                        BNE.BLL.Vaga.InativarVaga(objVaga.IdVaga, objUsuarioFilialPerfil.IdUsuarioFilialPerfil, VagaLog.API);
                        break;
                    case StatusVaga.EmPublicacao:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não é possível atribuir o status EmPublicação a uma vaga. Esse status é controlado pelos processos internos do BNE.");
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Status sem tratamento previsto.");
                }
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao recuperar a vaga");
        }

        /// <summary>
        /// Realiza a candidatura a uma vaga.
        /// </summary>
        /// <param name="idVaga">Identificador da vaga</param>
        /// <param name="candidatura">Informações adicionais para a candidatura</param>
        /// <remarks>A candidatura será efetuada para o usuário informado na ApiKey</remarks>
        /// <response code="204" >OK - No Content</response>
        /// <response code="400" >Não foi possível localizar o currículo para o CPF informado no cabeçalho</response>
        [HttpPost]
        [ResponseType(null)]
        [Route("v1.0/Vagas/Candidatar/{idVaga}")]
        public HttpResponseMessage Candidatar(int idVaga, [FromBody] Candidatura candidatura)
        {
            try
            {
                IEnumerable<string> cpfHeader;
                if (!Request.Headers.TryGetValues("Num_CPF", out cpfHeader) || cpfHeader.Count() <= 0)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não foi possível localizar o CPF no cabeçalho");

                decimal numCpf;
                if (!decimal.TryParse(cpfHeader.First(), out numCpf))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Erro ao recuperar CPF");

                var objVaga = BNE.BLL.Vaga.LoadObject(idVaga);
                BNE.BLL.Curriculo objCurriculo;
                if (!BNE.BLL.Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não foi possível localizar o currículo para o CPF informado");

                var listPerguntas = new List<VagaCandidatoPergunta>();
                foreach (var objResposta in candidatura.Respostas)
                {
                    var objVagaCandidatoPergunta = new VagaCandidatoPergunta()
                    {
                        VagaPergunta = new VagaPergunta(objResposta.IdPergunta),
                        FlagResposta = objResposta.Resposta,
                    };

                    listPerguntas.Add(objVagaCandidatoPergunta);
                }

                int? quantidadeCandidaturas;
                if (!VagaCandidato.Candidatar(objCurriculo,
                    objVaga,
                    null,
                    listPerguntas,
                    GetClientIp(),
                    false,
                    false,
                    false,
                    BNE.BLL.Enumeradores.OrigemCandidatura.WebAPI,
                    out quantidadeCandidaturas))
                {
                    string msg = "Não foi possível realizar a candidatura.";
                    if (quantidadeCandidaturas <= 0)
                    {
                        msg += " O candidato não possui candidaturas.";
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, msg);
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Consulta a vagas candidatadas.
        /// </summary>
        /// <param name="pagina">Pagina a ser retornada</param>
        /// <param name="registrosPorPagina">Informações adicionais para a candidatura</param>
        /// <param name="tipoVinculo">Tipo do vínculo das vagas candidatadas</param>
        /// <remarks>As vagas serão retornadas para o usuário informado na ApiKey</remarks>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Não foi possível localizar o currículo para o CPF informado no cabeçalho</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(ResultadoPaginado<DTO.Vaga>))]
        [Route("v1.0/Vagas/Candidatadas")]
        public HttpResponseMessage Candidatadas(DTO.Enum.TipoVinculo? tipoVinculo = null,
            int pagina = 1,
            int registrosPorPagina = 10)
        {
            try
            {
                IEnumerable<string> cpfHeader;
                if (!Request.Headers.TryGetValues("Num_CPF", out cpfHeader) || cpfHeader.Count() <= 0)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não foi possível localizar o CPF no cabeçalho");

                decimal numCpf;
                if (!decimal.TryParse(cpfHeader.First(), out numCpf))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Erro ao recuperar CPF");

                BNE.BLL.Curriculo objCurriculo;
                if (!BNE.BLL.Curriculo.CarregarPorCpf(numCpf, out objCurriculo))
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Não foi possível localizar o currículo para o CPF informado");

                var registros = new List<DTO.Vaga>();
                int totalRegistros;
                foreach (var objVaga in VagaCandidato.ListarVagaCandidatadaPorCurriculo(
                    objCurriculo.IdCurriculo,
                    (int?)tipoVinculo,
                    pagina,
                    registrosPorPagina,
                    out totalRegistros))
                {
                    registros.Add(new DTO.Vaga(objVaga));
                }

                return Request.CreateResponse(HttpStatusCode.OK, new ResultadoPaginado<DTO.Vaga>(totalRegistros, pagina, registrosPorPagina, registros));
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        /// <summary>
        /// Cadastra uma nova vaga.
        /// </summary>
        /// <param name="vaga">
        ///     Vaga que será publicada na filial do usuário.
        /// </param>
        /// <see cref="VagaEmpresa"/>
        /// <returns>Retorna objeto ResultadoVagaDTO que contém o código da vaga.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ResponseType(typeof(ResultadoVagaDTO))]
        [Route("v1.0/Vagas/Salvar")]
        public HttpResponseMessage Salvar([FromBody] VagaEmpresa vaga)
        {
            try
            {
                #region Verifica o estado das informações enviadas

                if (vaga == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Nenhum objeto VagaDTO foi informado! Verfique se o json enviado é válido." });

                var errorMessage = "";
                if (ModelState.IsValid) return Business.Vagas.Salvar(Request, vaga);

                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if (error.Exception != null)
                            errorMessage += error.Exception.Message + " ";
                        else if (!string.IsNullOrEmpty(error.ErrorMessage))
                            errorMessage += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = errorMessage });

                #endregion
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Atualiza uma vaga já cadastrada.
        /// </summary>
        /// <param name="vaga">
        ///     Vaga que será publicada na filial do usuário.
        /// </param>
        /// <param name="idVaga">Identificador da vaga a ser atualizada</param>
        /// <see cref="Bne.Web.Services.API.DTO.VagaEmpresa"/>
        /// <returns>Retorna objeto ResultadoVagaDTO que contém o código da vaga.</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut]
        [ResponseType(typeof(ResultadoVagaDTO))]
        [Route("v1.0/Vagas/Salvar/{idVaga}")]
        public HttpResponseMessage Salvar(int idVaga, [FromBody] VagaEmpresa vaga)
        {
            #region Verifica o estado das informações enviadas

            if (vaga == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Nenhum objeto VagaDTO foi informado! Verfique se o json enviado é válido." });

            var errorMessage = "";
            if (!ModelState.IsValid)
            {
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if (error.Exception != null)
                            errorMessage += error.Exception.Message + " ";
                        else if (!string.IsNullOrEmpty(error.ErrorMessage))
                            errorMessage += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = errorMessage });
            }

            #endregion

            return Business.Vagas.Salvar(Request, vaga, idVaga);
        }

        /// <summary>
        /// Tipo retornado na obtenção dos candidatos
        /// </summary>
        public enum ReturnType
        {
            /// <summary>
            /// Mini curriculo será retornado
            /// </summary>
            MiniCurriculo,

            /// <summary>
            /// Currículo completo será retornado
            /// </summary>
            CurriculoCompleto
        }

        /// <summary>
        /// Lista os minicurrículos dos candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga"> Código da vaga.</param>
        /// <param name="pagina"> Número da página que deseja acessar. O valor padrão é 1</param>
        /// <returns>Retorna um objeto com a lista de minicurrículos dos candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(ResultadoCandidatosDTO<DTO.MiniCurriculo>))]
        [Route("v1.0/Vagas/ObterMiniCurriculosCandidatos")]
        public HttpResponseMessage ObterMiniCurriculosCandidatos([FromUri] int codigoVaga, int pagina = 1)
        {
            return ObterCandidatos(codigoVaga, pagina, ReturnType.MiniCurriculo);
        }

        /// <summary>
        /// Lista os currículos completos dos candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga"> Código da vaga.</param>
        /// <param name="pagina"> Número da página que deseja acessar. O valor padrão é 1</param>
        /// <returns>Retorna um objeto com a lista de minicurrículos dos candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(ResultadoCandidatosDTO<DTO.ResultadoPesquisaCurriculoCompleto>))]
        [Route("v1.0/Vagas/ObterCurriculosCandidatos")]
        public HttpResponseMessage ObterCurriculosCandidatos([FromUri] int codigoVaga, int pagina = 1)
        {
            return ObterCandidatos(codigoVaga, pagina, ReturnType.CurriculoCompleto);
        }

        /// <summary>
        /// Lista os candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga"> Código da vaga.</param>
        /// <seealso cref="System.Int32"/>
        /// <param name="pagina"> Número da página que deseja acessar. O valor padrão é 1</param>
        /// <param name="returnType">Formato desejado para os currículos candidatos. O valor padrão é 'Minicurriculo'</param>
        /// <seealso cref="System.Int32"/>
        /// <returns>Retorna um objeto com a lista de candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(ResultadoCandidatosDTO<DTO.Curriculo>))]
        [Route("v1.0/Vagas/ObterCandidatos")]
        public HttpResponseMessage ObterCandidatos([FromUri] int codigoVaga, int pagina = 1, ReturnType returnType = ReturnType.MiniCurriculo)
        {
            var objUsuarioFilialPerfil = Login(Request);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            if (string.IsNullOrEmpty(objUsuarioFilialPerfil.Filial.CNPJ)) objUsuarioFilialPerfil.Filial.CompleteObject();

            HttpResponseMessage response;

            try
            {
                var result = new ResultadoCandidatosDTO<DTO.Curriculo>();
                int totalDeRegistros;
                int totalDePaginas;
                result.Curriculos = bs.Candidatos.ObterCandidatos(codigoVaga, objUsuarioFilialPerfil, pagina, returnType == ReturnType.CurriculoCompleto, out totalDeRegistros, out totalDePaginas);
                result.TotalPaginas = totalDePaginas;
                result.TotalRegistros = totalDeRegistros;
                result.Pagina = pagina;
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Lista os minicurriculos candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga">Código da vaga.</param>
        /// <param name="data">Data e hora a partir de qual as candidaturas devem ser recuperadas.</param>
        /// <returns>Retorna um objeto com a lista de candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(List<DTO.MiniCurriculo>))]
        [Route("v1.0/Vagas/ObterUltimosMiniCurriculosCandidatos")]
        public HttpResponseMessage ObterUltimosMiniCurriculosCandidatos([FromUri] int codigoVaga, DateTime data)
        {
            return ObterUltimosCandidatos(codigoVaga, data, ReturnType.MiniCurriculo);
        }

        /// <summary>
        /// Lista os currículos candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga">Código da vaga.</param>
        /// <param name="data">Data e hora a partir de qual as candidaturas devem ser recuperadas.</param>
        /// <returns>Retorna um objeto com a lista de candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(List<DTO.ResultadoPesquisaCurriculoCompleto>))]
        [Route("v1.0/Vagas/ObterUltimosCurriculosCandidatos")]
        public HttpResponseMessage ObterUltimosCurriculosCandidatos([FromUri] int codigoVaga, DateTime data)
        {
            return ObterUltimosCandidatos(codigoVaga, data, ReturnType.CurriculoCompleto);
        }

        /// <summary>
        /// Lista os candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="codigoVaga">Código da vaga.</param>
        /// <param name="data">Data e hora a partir de qual as candidaturas devem ser recuperadas.</param>
        /// <param name="returnType">Formato desejado para os currículos candidatos. O valor padrão é 'Minicurriculo'</param>
        /// <returns>Retorna um objeto com a lista de candidatos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(List<DTO.Curriculo>))]
        [Route("v1.0/Vagas/ObterUltimosCandidatos")]
        public HttpResponseMessage ObterUltimosCandidatos([FromUri] int codigoVaga, DateTime data, ReturnType returnType = ReturnType.MiniCurriculo)
        {
            var objUsuarioFilialPerfil = Login(Request);

            if (objUsuarioFilialPerfil == null)
                return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");

            if (string.IsNullOrEmpty(objUsuarioFilialPerfil.Filial.CNPJ)) objUsuarioFilialPerfil.Filial.CompleteObject();

            HttpResponseMessage response;

            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, bs.Candidatos.ObterCandidatos(codigoVaga, objUsuarioFilialPerfil, data, returnType == ReturnType.CurriculoCompleto));
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Retorna o XML para indexação do SINE
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("v1.0/Vagas/XmlBNE_Sine")]
        public HttpResponseMessage XmlBNE_Sine()
        {
            var xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNE_SINE.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        /// <summary>
        /// Retorna o XML de vagas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("v1.0/Vagas/XmlBne")]
        public HttpResponseMessage XmlBne()
        {
            var xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNE.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        /// <summary>
        /// Retorna o XML para indexação do Trovit
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("v1.0/Vagas/XmlBNE_Trovit")]
        public HttpResponseMessage XmlBNE_Trovit()
        {
            var xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\TROVITVagas.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        /// <summary>
        /// Retorna o XML de vagas sem as vagas do SINE
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("v1.0/Vagas/XmlBNE_VagasSemSine")]
        public HttpResponseMessage XmlBNE_VagasSemSine()
        {
            try
            {
                var xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNESemSine.xml");

                return new HttpResponseMessage()
                {
                    RequestMessage = Request,
                    Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
                };
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retorna o relatórios de vagas para o dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("v1.0/Vagas/ReportVagas")]
        public HttpResponseMessage ReportVagas()
        {
            try
            {
                var retorno = new
                {
                    totalProjetosParados = BNE.BLL.VagasSINE.ListarTotalProjetosSemRodar(),
                    statusProjetos = BNE.BLL.VagasSINE.ListarStatusProjetos(),
                    projetosParados = BNE.BLL.VagasSINE.ListarProjetosSemRodar(),
                    projetosQtdVagas = BNE.BLL.VagasSINE.ListarProjetosQtdVagas(),
                    totalvagasimportadas = BNE.BLL.VagasSINE.ListarTotalVagasImportadas()
                };

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
    }
}
