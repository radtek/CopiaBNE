using NovaArquitetura.Business.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NovaArquitetura.API.Controllers
{
    public class DisciplinaController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Entities.Disciplina> Get()
        {
            return new Business.Disciplina().GetAll();
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Entities.Disciplina objDisciplina = new Business.Disciplina().GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, objDisciplina);
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // GET api/<controller>/GetByAluno/id
        [HttpGet]
        public HttpResponseMessage GetByAluno(int id)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Business.Disciplina().GetByAluno(id));
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Entities.Disciplina Disciplina)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Business.Disciplina().Create(Disciplina));
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/<controller>/AddAluno/5/10
        [HttpPost]
        public HttpResponseMessage AddAluno(int id, int idAluno)
        {
            try
            {
                new Business.Disciplina().AddAluno(id, idAluno);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/<controller>/RemoveAluno/5/10
        [HttpPost]
        public HttpResponseMessage RemoveAluno(int id, int idAluno)
        {
            try
            {
                new Business.Disciplina().RemoveAluno(id, idAluno);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Entities.Disciplina Disciplina)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Business.Disciplina().Update(Disciplina));
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var business = new Business.Disciplina();
            var objDisciplina = business.GetById(id);

            if (objDisciplina != null)
                business.Delete(objDisciplina);
        }
    }
}