﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BNE.ExceptionLog.Interface;
using AutoMapper;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class PessoaFisicaController : ApiController
    {
        private readonly Domain.PessoaFisica _pessoaFisica;
        private readonly ILogger _logger;

        #region PessoaFisicaController
        public PessoaFisicaController(Domain.PessoaFisica pessoaFisica, ILogger logger)
        {
            _pessoaFisica = pessoaFisica;
            _logger = logger;
        }
        #endregion

        #region GetCadidaturasDegustacao
        public ResponseMessageResult GetCadidaturasDegustacao(string cpf, string dataNascimento)
        {
            try
            {
                var result = _pessoaFisica.GetCadidaturasDegustacao(Convert.ToDecimal(cpf), Convert.ToDateTime(dataNascimento));

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, result.Replace("\"","")));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Checar se candidato tem Experiência profissional e formação CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region GetCandidatoTemExperienciaeFormacao
        public ResponseMessageResult GetCandidatoTemExperienciaeFormacao(string cpf, DateTime dataNascimento)
        {
            int retorno = 0;

            try
            {
                var result = _pessoaFisica.TemExperiencia(Convert.ToDecimal(cpf), dataNascimento);

                if (!result)
                {
                    retorno = 1;
                }
                else
                {
                    retorno = _pessoaFisica.TemFormacao(Convert.ToDecimal(cpf), dataNascimento) ? 3:2;
                }

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Checar se candidato tem Experiência profissional e formação CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region CadastrarExperienciaProfissional
        [HttpPost]
        public ResponseMessageResult CadastrarExperienciaProfissional(Models.ExperienciaProfissional objExperiencia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.ExperienciaProfissional, Domain.Command.ExperienciaProfissional>(objExperiencia);
                    bool resultado = _pessoaFisica.PostExperienciaProfissional(command);

                    if (resultado)
                    {
                        var retorno = new
                        {
                            cpf = command.Cpf,
                            datanascimento = command.DataNascimento.Date,
                            url = command.UrlPesquisa
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
                }

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Cadastrar Experiência profissional CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region CadastrarFormação
        [HttpPost]
        public ResponseMessageResult CadastrarFormacao(Models.Formacao objFormacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.Formacao, Domain.Command.Formacao>(objFormacao);

                    bool resultado = _pessoaFisica.PostFormacao(command);

                    if (resultado)
                    {
                        var retorno = new
                        {
                            cpf = command.Cpf,
                            datanascimento = command.DataNascimento.Date,
                            url = command.UrlPesquisa
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
                }

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Cadastrar Formação CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region CadastrarMini
        [HttpPost]
        public ResponseMessageResult CadastrarMini(Models.PessoaFisica objPessoaFisica)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Models.PessoaFisica, Domain.Command.PessoaFisica>(objPessoaFisica);

                    bool resultado = _pessoaFisica.CadastrarMiniCV(command);

                    if (resultado)
                    {
                        var retorno = new
                        {

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
                _logger.Error(ex, "Pessoa Fisica API - Cadastro Mini CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
                throw;
            }
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public ResponseMessageResult GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            try
            {
                string[] urls = _pessoaFisica.GetLinksPaginasSemelhantes(funcao, cidade, siglaEstado, areaBNE);

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, urls));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao montar os Links de páginas semelhantes");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
                throw;
            }
        }
        #endregion
    }
}