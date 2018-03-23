using System.Web.Http;
using LanHouse.Business.EL;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System;

namespace LanHouse.API.Controllers
{
    public class FaseCadastroController : LanHouse.API.Code.BaseController
    {
        //GET api/controller
        public IEnumerable<Entities.BNE.LAN_Fase_Cadastro> Get()
        {
            try
            {
                return new Business.FaseCadastro().GetAll();
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar Fase de cadastro");
                return null;
            }
        }

        //GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Entities.BNE.LAN_Fase_Cadastro objFaseCadastro = new Business.FaseCadastro().GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, objFaseCadastro);
            }
            catch(RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Fase de cadastro");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Fase de cadastro");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //PPOST api/<controller>
        public HttpResponseMessage Post([FromBody]Entities.BNE.LAN_Fase_Cadastro faseCadastro)
        {
            if(!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Business.FaseCadastro().Create(faseCadastro));
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Fase de cadastro");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        //PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody]Entities.BNE.LAN_Fase_Cadastro faseCadastro)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                Entities.BNE.LAN_Fase_Cadastro objFaseCadastro = new Business.FaseCadastro().Update(faseCadastro);
                return Request.CreateResponse(HttpStatusCode.OK, objFaseCadastro);
            }
                catch(RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Fase de cadastro");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Fase de cadastro");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            try
            {
                var business = new Business.FaseCadastro();
                var objFaseCadastro = business.GetById(id);

                if (objFaseCadastro != null)
                    business.Delete(objFaseCadastro);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Deletar Fase de cadastro");
            }
        }
    }
}
