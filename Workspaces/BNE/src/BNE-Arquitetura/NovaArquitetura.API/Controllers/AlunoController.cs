using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NovaArquitetura.API.DTO;
using NovaArquitetura.Business.EL;

namespace NovaArquitetura.API.Controllers
{
    public class AlunoController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Entities.Aluno> Get()
        {
            return new Business.Aluno().GetAll();
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                Entities.Aluno objAluno = new Business.Aluno().GetById(id);
                return Request.CreateResponse(HttpStatusCode.OK, objAluno);
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

        // GET api/<controller>/<nome>
        public HttpResponseMessage Get(string partialName)
        {
            try
            {
                var alunos = new Business.Aluno().GetByPartialName(partialName);

                var retorno = new List<AlunoDisciplina>();

                retorno.AddRange(alunos.Select(aluno => new AlunoDisciplina
                {
                    Aluno = aluno,
                    Disciplinas = aluno.Disciplinas.Select(m => m.Id).ToList()
                }));

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody]Entities.Aluno aluno)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, new Business.Aluno().Create(aluno));
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        // PUT api/<controller>/5
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Entities.Aluno aluno)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                Entities.Aluno objAlunoUpdate = new Business.Aluno().Update(aluno);
                return Request.CreateResponse(HttpStatusCode.OK, objAlunoUpdate);
            }
            catch (RecordNotFoundException ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var business = new Business.Aluno();
            var objAluno = business.GetById(id);

            if (objAluno != null )
                business.Delete(objAluno);
        }

    }
}