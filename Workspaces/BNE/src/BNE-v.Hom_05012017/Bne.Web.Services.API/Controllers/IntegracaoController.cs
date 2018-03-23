using Bne.Web.Services.API.Business.Integracao;
using Bne.Web.Services.API.DTO.Integracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// Integração de currículos BNE
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IntegracaoController : ApiController
    {

        /// <summary>
        /// Exporta um novo candidato para a base do BNE
        /// </summary>
        /// <param name="param">
        ///     Dados do candidato que será exportado para o BNE.
        /// </param>
        /// <see cref="Bne.Web.Services.API.DTO.Integracao.ExportaCandidatoParam"/>
        /// <returns>Retorna True se o candidato foi exportado corretamento.</returns>
        /// 
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [ResponseType(typeof(ExportaCandidatoResult))]
        public HttpResponseMessage ExportaCandidato([FromBody] ExportaCandidatoParam param)
        {
            ExportaCandidatoResult result = new ExportaCandidatoResult();

            if (ModelState.IsValid)
            {
                Integrador integrador = new Integrador();
                try
                {
                    integrador.ExportarCandidato(param);
                    result.Message = "Currículo exportado com sucesso!";
                    return Request.CreateResponse<ExportaCandidatoResult>(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    return Request.CreateResponse<ExportaCandidatoResult>(HttpStatusCode.InternalServerError, result);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ActionContext.ModelState);
            }
        }



        /// <summary>
        /// Retorna a url para vaga com o usuário já logado.
        /// </summary>
        /// <param name="param">
        ///    Código da vaga e CPF do candidato.
        /// </param>
        /// <returns>Retorna a URL para a candidatura.</returns>
        /// 
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(CandidaturaURLResult))]
        public HttpResponseMessage CandidaturaURL([FromUri] int IdVaga, [FromUri] decimal NumCPF)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var UrlAmbiente = BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.URLAmbiente);

#if DEBUG
                    UrlAmbiente = "localhost:2000";
#endif

                    int idPessoaFisica;
                    if (BNE.BLL.PessoaFisica.ExistePessoaFisica(NumCPF, out idPessoaFisica))
                    {
                        var pf = new BNE.BLL.PessoaFisica(idPessoaFisica);
                        string vaga_url = String.Format("http://{0}/vaga-de-emprego-na-area-a-em-a-aa/a/{1}", UrlAmbiente, IdVaga);

                        string credenciais = String.Format("\"IdPessoFisica\":null,\"Url\":\"{0}\",\"NumeroCPF\":{1},\"DataNascimento\":\"{2}\"",
                                                             vaga_url, NumCPF, pf.RecuperarDataNascimento().ToString("yyyy-MM-dd"));

                        credenciais = "{" + credenciais + "}";

                        var CredenciaisBytes = System.Text.Encoding.UTF8.GetBytes(credenciais);
                        var CredenciaisBase64 = System.Convert.ToBase64String(CredenciaisBytes);

                        var LogarURL = String.Format("http://{0}/logar/{1}?utm_Source=www.sine.com.br", UrlAmbiente, CredenciaisBase64);

                        return Request.CreateResponse<CandidaturaURLResult>(HttpStatusCode.OK, new CandidaturaURLResult() { ResultURL = LogarURL });
                    }
                    else
                    {
                        return Request.CreateResponse<CandidaturaURLResult>(HttpStatusCode.NotFound, new CandidaturaURLResult() { Message = "Pessoa Física não encontrada." });
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse<CandidaturaURLResult>(HttpStatusCode.InternalServerError, new CandidaturaURLResult() { Message = ex.Message });
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ActionContext.ModelState);
            }
        }










    }
}
