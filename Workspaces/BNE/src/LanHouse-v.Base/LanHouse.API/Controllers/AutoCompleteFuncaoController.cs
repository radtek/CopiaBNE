using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class AutoCompleteFuncaoController : LanHouse.API.Code.BaseController
    {
        #region ValidaFuncao
        /// <summary>
        /// Validar função
        /// </summary>
        /// <param name="funcao"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ValidaFuncao(string funcao)
        {
            int idFuncao = 0;

            try
            {
                idFuncao = Business.Funcao.RecuperarIDporNome(funcao);
                return Request.CreateResponse(HttpStatusCode.OK, idFuncao);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Validar função");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region GetFuncao
        /// <summary>
        /// Listar funções
        /// </summary>
        /// <param name="query"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IList GetFuncao(string query, int limit)
        {
            try
            {
                var funcoes = Business.Funcao.ListarSugestaodeFuncao(query, limit);
                return funcoes;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar funções");
                return null;
            }
        }

        #endregion

        #region GetSugestaoAtividades
        /// <summary>
        /// Carregar sugestão de atividades da função
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetSugestaoAtividades(string query)
        {
            try
            {
                var descricao = Business.Funcao.ListarSugestaoAtividades(query);
                return descricao;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar sugestão de atividades da função");
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