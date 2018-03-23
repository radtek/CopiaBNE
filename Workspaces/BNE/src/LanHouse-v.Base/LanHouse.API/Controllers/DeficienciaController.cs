using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LanHouse.API.Controllers
{
   [Authorize]
    public class DeficienciaController : LanHouse.API.Code.BaseController
    {
       // GET api/<controller>
       /// <summary>
       /// Listar Deficiências
       /// </summary>
       /// <returns></returns>
       public IList Get()
       {
           try
           {
               return Business.Deficiencia.ListarDeficiencias();
           }
           catch (Exception ex)
           {
               string msgErro;
               Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar Deficiências");
               return null;
           }
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