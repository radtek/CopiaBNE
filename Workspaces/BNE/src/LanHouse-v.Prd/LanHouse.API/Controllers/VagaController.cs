using LanHouse.Business.EL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class VagaController : LanHouse.API.Code.BaseController
    {
        #region BuscarVagasLanHouse
        /// <summary>
        /// Busca as vagas para a LanHouse
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="filtro"></param>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="geoLocalizacao"></param>
        /// <returns></returns>
        /// GET: api/Vaga
        [HttpGet]
        public HttpResponseMessage Get(string idFuncao, string filtro, int start, int rows, string geoLocalizacao)
        {
            try
            {
                if (filtro == null)
                    filtro = "";

                var vagas = Business.Custom.SOLR.PesquisaVagasSolr.ObterRegistros(idFuncao, filtro, start, rows, geoLocalizacao);
                return Request.CreateResponse(HttpStatusCode.OK, vagas);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Listar vagas Azulzinho");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar vagas");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region BuscarVagasLanHousePlaca
        /// <summary>
        /// Busca as vagas para a LanHouse
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="filtro"></param>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="geoLocalizacao"></param>
        /// <returns></returns>
        /// GET: api/Vaga
        [HttpGet]
        public HttpResponseMessage GetVagasPlaca(string idCidade, string filtro, int start, int rows, string idExcluidos, Boolean? aceitaAConbinar, string vlrInicial, string vlrFinal)
        {
            try
            {
                if (filtro == null)
                    filtro = "";

                var vagas = Business.Custom.SOLR.PesquisaVagasSolr.ObterVagasPlaca(filtro, start, rows, idCidade, idExcluidos, aceitaAConbinar, vlrInicial, vlrFinal);
                return Request.CreateResponse(HttpStatusCode.OK, vagas);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Listar vagas placa LanHouse");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar vagas placa LanHouse");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region BuscarVagasAzulzinho
        /// <summary>
        /// Busca as vagas para o Jornal Azulzinho
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        /// GET: api/Vaga
        [HttpGet]
        public HttpResponseMessage GetVagasAzulzinho(string filtro, int start, int rows, string idFuncao)
        {
            try
            {
                if (filtro == null)
                    filtro = "";

                var vagas = Business.Custom.SOLR.PesquisaVagasSolr.ObterVagasJornalAzulzinho(filtro, start, rows, idFuncao);
                return Request.CreateResponse(HttpStatusCode.OK, vagas);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Listar vagas Azulzinho");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar vagas Azulzinho");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        // GET: api/Vaga/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var vaga = Business.Custom.SOLR.PesquisaVagasSolr.CarregarVaga(id);
                return Request.CreateResponse(HttpStatusCode.OK, vaga);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, string.Format("Lan House API - Carregar vaga => {1}", id));
                throw;
            }
        }

        // POST: api/Vaga
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Vaga/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Vaga/5
        public void Delete(int id)
        {
        }


        [HttpGet]
        public HttpResponseMessage PesquisarVagasDeEstagio(string filtro, int start, int rows, bool IsEstagio)
        {
            try
            {
                filtro =  (filtro != null) ? filtro :  "";
                var vagas = Business.Custom.SOLR.PesquisaVagasSolr.CarregarVagasDeEstagio(filtro, start, rows);
                return Request.CreateResponse(HttpStatusCode.OK, vagas);
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
