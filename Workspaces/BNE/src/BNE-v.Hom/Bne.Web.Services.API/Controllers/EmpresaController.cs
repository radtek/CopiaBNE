using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    public class EmpresaController : ApiController
    {
        #region ObterSelecionadora
        /// <summary>
        /// Retorna uma selecionadora a partir do CPF e Data de Nascimento.
        /// </summary>
        /// <param name="cpf">Cpf da selecionadora desejado</param>
        /// <param name="nascimento">Data de nascimento do currículo desejado</param>
        /// <returns>Objeto com dados da selecionadora correspondente ao Cpf e Data de Nascimento informado</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente ou Data de nascimento informada não confere com a presente no banco de dados.</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(DTO.Empresas.Selecionadora))]
        public HttpResponseMessage ObterSelecionadora([FromUri] decimal cpf, [FromUri] DateTime nascimento)
        {
            try
            {
                var selecionadora = Business.Empresa.ObterSelecionadora(cpf);
                if (selecionadora == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Selecionadora não encontrada");

                if (selecionadora.Nascimento != nascimento.ToString("yyyy-MM-dd"))
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data de nascimento não confere");


                return Request.CreateResponse(HttpStatusCode.OK, selecionadora);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }
        #endregion ObterSelecionadora
    }
}
