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
    [Authorize]
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
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Carregar a QTD de candidaturas disponíveis para o usuário");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch(Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a QTD de candidaturas disponíveis para o usuário");
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
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Atualizar número de  candidaturas do usuário");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Atualizar número de  candidaturas do usuário");
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
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Verificar candidatura Vaga");
                return false;
            }
        }

        [HttpPost]
        public HttpResponseMessage EfetuarCandidatura(int idCurriculo, int idVaga, bool isEstagiario = false)
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

                // Valida se o CV pode realmente fazer a candidatura.
                // Caso a flag estagiário seja true o candidato tem livre acesso
                // as candidaturas. Essa flag é utilizada pelo webestágio.
                if (!Curriculo.ValidaCandidatura(idCurriculo) && !isEstagiario)
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");

                bool retorno = Business.Candidatura.InserirCandidatura(idCurriculo, idVaga);

                //Envio de e-mail

                //Após candidatar, descontar o número de candidaturas disponíveis (verificar existencia de trigger fazendo este processo)
                Candidatura.AtualizaNumeroCandidaturasUsuario(idCurriculo);

                return Request.CreateResponse(HttpStatusCode.OK, retorno);

            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Efetuar candidatura");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch(Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Efetuar candidatura");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage EfetuarCandidaturaAzulzinho(int idCurriculo, int idVaga)
        {
            try
            {
                bool retorno = Business.Candidatura.InserirCandidatura(idCurriculo, idVaga);
                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Efetuar candidatura Azulzinho");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Efetuar candidatura Azulzinho");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
