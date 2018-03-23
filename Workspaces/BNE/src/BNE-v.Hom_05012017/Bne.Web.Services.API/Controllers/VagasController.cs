using Bne.Web.Services.API.DTO;
using BNE.BLL;
using BNE.BLL.AsyncServices.Enumeradores;
using BNE.BLL.Custom;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using bs = Bne.Web.Services.API.Business;

namespace Bne.Web.Services.API.Controllers
{
    public class VagasController : ApiController
    {
        /// <summary>
        /// Cadastra uma nova vaga.
        /// </summary>
        /// <param name="vaga">
        ///     Vaga que será publicada na filial do usuário.
        /// </param>
        /// <see cref="Bne.Web.Services.API.DTO.Vaga"/>
        /// <returns>Retorna objeto ResultadoVagaDTO que contém o código da vaga.</returns>
        /// 
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [System.Web.Http.HttpPost]
        [ResponseType(typeof(DTO.ResultadoVagaDTO))]
        public HttpResponseMessage Salvar([FromBody] DTO.Vaga vaga)
        {
            try
            {
                #region Verifica o estado das informações enviadas
                if (vaga == null)
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Nenhum objeto VagaDTO foi informado! Verfique se o json enviado é válido." });

                string error_message = "";
                if (!ModelState.IsValid)
                {
                    foreach (var ms in ModelState.Values)
                    {
                        foreach (var error in ms.Errors)
                        {
                            if (error.Exception != null)
                                error_message += error.Exception.Message + " ";
                            else if (!string.IsNullOrEmpty(error.ErrorMessage))
                                error_message += error.ErrorMessage + " ";
                        }
                    }
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = error_message });

                }

                return Business.Vagas.Salvar(Request, vaga);

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
        /// <param name="IdVaga">Identificador da vaga a ser atualizada</param>
        /// <see cref="Bne.Web.Services.API.DTO.Vaga"/>
        /// <returns>Retorna objeto ResultadoVagaDTO que contém o código da vaga.</returns>
        /// 
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [System.Web.Http.HttpPut]
        [ResponseType(typeof(DTO.ResultadoVagaDTO))]
        public HttpResponseMessage Salvar(int IdVaga, [FromBody] DTO.Vaga vaga)
        {

            #region Verifica o estado das informações enviadas
            if (vaga == null)
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Nenhum objeto VagaDTO foi informado! Verfique se o json enviado é válido." });

            string error_message = "";
            if (!ModelState.IsValid)
            {
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if (error.Exception != null)
                            error_message += error.Exception.Message + " ";
                        else if (!string.IsNullOrEmpty(error.ErrorMessage))
                            error_message += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = error_message });
            }
            #endregion

            return Business.Vagas.Salvar(Request, vaga, IdVaga);

        }


        /// <summary>
        /// Lista os candidatos de uma determinada vaga.
        /// </summary>
        /// <param name="CodigoVaga"> Código da vaga.</param>
        /// <seealso cref="System.Int32"/>
        /// <param name="Pagina"> Número da página que deseja acessar. O valor padrão é 1</param>
        /// <seealso cref="System.Int32"/>
        /// <returns>Retorna um objeto com a lista de candidatos</returns>
        /// <seealso cref="Bne.Web.Services.API.DTO.ResultadoCandidatosDTO"/>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(DTO.ResultadoCandidatosDTO))]
        public HttpResponseMessage ObterCandidatos([FromUri] int CodigoVaga, int Pagina = 1)
        {
            decimal num_cpf = decimal.Parse(Request.Headers.GetValues("Num_CPF").First());
            decimal num_cnpj = decimal.Parse(Request.Headers.GetValues("Num_CNPJ").First());

            HttpResponseMessage response;

            int total_de_registros = 0;
            int total_de_paginas = 0;

            try
            {
                ResultadoCandidatosDTO result = new ResultadoCandidatosDTO();
                result.Curriculos = bs.Candidatos.ObterCandidatos(CodigoVaga, num_cnpj, Pagina, out total_de_registros, out total_de_paginas);
                result.TotalPaginas = total_de_paginas;
                result.TotalRegistros = total_de_registros;
                result.Pagina = Pagina;
                response = Request.CreateResponse<ResultadoCandidatosDTO>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [System.Web.Http.HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage XmlBNE_Sine()
        {
            byte[] xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNE_SINE.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        [System.Web.Http.HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage XmlBNE()
        {
            byte[] xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNE.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        [System.Web.Http.HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage XmlBNE_Trovit()
        {
            byte[] xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\TROVITVagas.xml");

            return new HttpResponseMessage()
            {
                RequestMessage = Request,
                Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
            };
        }

        [System.Web.Http.HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage XmlBNE_VagasSemSine()
        {
            try
            {
                byte[] xml = File.ReadAllBytes(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.UrlXmlVagasIntegracao) + "\\VagasBNESemSine.xml");

                return new HttpResponseMessage()
                {
                    RequestMessage = Request,
                    Content = new XmlContent(System.Text.Encoding.UTF8.GetString(xml))
                };

            }
            catch(Exception ex)
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
