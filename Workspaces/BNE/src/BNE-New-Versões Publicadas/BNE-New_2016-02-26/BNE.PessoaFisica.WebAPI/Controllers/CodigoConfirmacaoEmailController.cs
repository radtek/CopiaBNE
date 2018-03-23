using BNE.Data.Infrastructure;
using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class CodigoConfirmacaoEmailController : ApiController
    {
        private readonly Domain.CodigoConfirmacaoEmail _codigoConfirmacaoEmail;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CodigoConfirmacaoEmailController(Domain.CodigoConfirmacaoEmail codigoConfirmacaoEmail, IUnitOfWork unitOfWork, ILogger logger)
        {
            _codigoConfirmacaoEmail = codigoConfirmacaoEmail;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public ResponseMessageResult Get()
        {
            try
            {
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, "Não é permitido listar todos os objetos desta classe."));
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Pessoa Fisica API - Codigo Confirmação Email Get");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        public ResponseMessageResult Get(int id)
        {
            try
            {
                var objCodigo = _codigoConfirmacaoEmail.GetById(id);

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, objCodigo));
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Pessoa Fisica API - Codigo Confirmação Email Get by Id");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        #region GetUtilizarCodigo 
        public ResponseMessageResult GetUtilizarCodigo(string codigo)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(codigo);
                var txtDesCriptografa = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var indexCodigo = txtDesCriptografa.IndexOf("&codigo=");

                if(indexCodigo > 0)
                {
                    txtDesCriptografa = txtDesCriptografa.Substring(indexCodigo + 8);
                }

                var objCodigo = _codigoConfirmacaoEmail.GetByCodigo(txtDesCriptografa);

                //validar dados
                if(objCodigo == null)
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound, "Código inexistente!"));

                if(objCodigo.DataUtilizacao.HasValue)
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound, "Código já utilizado!"));

                objCodigo.DataUtilizacao = DateTime.Now;
                _codigoConfirmacaoEmail.Salvar(objCodigo);

                _unitOfWork.Commit();

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, "true"));
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Pessoa Fisica API - Codigo Confirmação Email Get by Codigo");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        public ResponseMessageResult Post(Model.CodigoConfirmacaoEmail codigoConfirmacaoEmail)
        {
            try
            {
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, 
                    _codigoConfirmacaoEmail.Salvar(codigoConfirmacaoEmail)));
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "Pessoa Fisica API - Codigo Confirmação Email - Gerar código");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

    }
}
