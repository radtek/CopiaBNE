using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanHouse.Entities.BNE;

namespace LanHouse.API.Controllers
{

    public class AutoCompleteController : LanHouse.API.Code.BaseController
    {
        #region ValidaCidade
        /// <summary>
        /// Validar nome da Cidade
        /// </summary>
        /// <param name="cidade"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ValidaCidade(string cidade)
        {
            object retorno = null;

            try
            {
                if (Business.Cidade.ValidarCidade(cidade))
                {
                    retorno = new { valid = true };
                }
                else
                {
                    retorno = new { valid = false };
                }

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Validar nome da Cidade");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region GetCidade
        /// <summary>
        /// Listar cidades
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IList GetCidade(string query, int limit)
        {
            try
            {
                var cidades = Business.Cidade.ListarSugestaodeCidade(query, limit);
                return cidades;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar cidades");
                return null;
            }
        }

        #endregion

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}