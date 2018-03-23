using LanHouse.Business;
using LanHouse.Business.EL;
using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class CandidaturaController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage GetQuantidadeCandidaturaDisponivelUsuario(int idCurriculo)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Business.Candidatura.QuantidadeCandidaturasDisponiveisUsuario(idCurriculo));
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

        [HttpPost]
        public HttpResponseMessage AtualizaNumeroCandidaturasUsuario(int idCurriculo)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Business.Candidatura.AtualizaNumeroCandidaturasUsuario(idCurriculo));
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

        [HttpPost]
        public bool VerificaCandidaturaVaga(int idCurriculo, int idVaga)
        {
            try
            {
                bool existeCandidatura = Business.Candidatura.VerificaCandidaturaVaga(idCurriculo, idVaga);
                return existeCandidatura;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public HttpResponseMessage EfetuarCandidatura(int idCurriculo, int idVaga)
        {
            try
            {
                /*
                 * PODE CANDIDATAR-SE CASO:
                 * 
                 *  1 - Candidato é vip ou possui candidaturas disponíveis
                 *  2 - Vaga marcada com "FlagBNERecomenda"
                 *  
                 */

                //Valida se o CV pode realmente fazer a candidatura
                if (!Curriculo.ValidaCandidatura(idCurriculo))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");

                bool retorno = Business.Candidatura.InserirCandidatura(idCurriculo, idVaga);

                //Envio de e-mail

                //Após candidatar, descontar o número de candidaturas disponíveis (verificar existencia de trigger fazendo este processo)
                Candidatura.AtualizaNumeroCandidaturasUsuario(idCurriculo);

                return Request.CreateResponse(HttpStatusCode.OK, retorno);

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
    }
}
