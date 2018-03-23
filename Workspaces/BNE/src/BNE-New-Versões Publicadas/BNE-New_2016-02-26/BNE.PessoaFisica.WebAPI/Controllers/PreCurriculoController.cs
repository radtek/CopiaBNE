using AutoMapper;
using BNE.ExceptionLog.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class PreCurriculoController : ApiController
    {
        private readonly Domain.PreCurriculo _preCurriculo;
        private readonly ILogger _logger;

        public PreCurriculoController(Domain.PreCurriculo preCurriculo,ILogger logger)
        {
            _preCurriculo = preCurriculo;
            _logger = logger;
        }


        [HttpPost]
        public ResponseMessageResult CandidatoExiste(string cpf, string dataNascimento)
        {
            try
            {
                
                if (ModelState.IsValid)
                {

                    Model.PessoaFisica resultado = _preCurriculo.CandidatoExiste(Convert.ToDecimal(cpf),Convert.ToDateTime(dataNascimento));

                    if (resultado != null)
                    {
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.OK, resultado));
                    }

                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }

                    _logger.Error(new Exception(errors.ToString()), "PF Candidatar - modelo inválido");
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Candidatar CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [HttpPost]
        public ResponseMessageResult Candidatar(Models.PreCurriculo objPreCurriculo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.PreCurriculo, Domain.Command.PreCurriculo>(objPreCurriculo);

                    string resultado = _preCurriculo.CandidatarCurriculo(command);

                    if (resultado != "" && resultado != "Inativa")
                    {
                        //_logger.Error(new Exception(), "Pessoa Fisica API - Cadastro Mini CV");
                        var retorno = new
                        {
                            cpf = command.CPF,
                            datanascimento = command.DataNascimento.Value.Date,
                            url = command.UrlPesquisa,
                            nome = resultado
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
                    else if (resultado == "Inativa")
                    {
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.PreconditionFailed, resultado));
                    }

                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }

                    _logger.Error(new Exception(errors.ToString()), "PF Candidatar - modelo inválido");
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Candidatar CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [HttpPost]
        public ResponseMessageResult CadastroMini(Models.PreCurriculo objPreCurriculo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.PreCurriculo, Domain.Command.PreCurriculo>(objPreCurriculo);

                    bool resultado = _preCurriculo.GerarMiniCurriculo(command);

                    if (resultado)
                    {
                        var retorno = new
                        {
                            cpf = command.CPF,
                            datanascimento = command.DataNascimento.Value.Date,
                            url = command.UrlPesquisa,
                            nome = command.Nome
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }

                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }

                    _logger.Error(new Exception(errors.ToString()), "PF modelo inválido");
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Cadastro Mini CV com pré-currículo");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }

        [HttpPost]
        public HttpResponseMessage Cadastro(Models.PreCurriculo objPreCurriculo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.PreCurriculo, Domain.Command.PreCurriculo>(objPreCurriculo);

                    Model.PreCurriculo resultado = _preCurriculo.Post(command);

                    if (resultado != null)
                        return Request.CreateResponse(HttpStatusCode.Created, resultado);

                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    foreach (var keyValue in ModelState)
                    {
                        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}