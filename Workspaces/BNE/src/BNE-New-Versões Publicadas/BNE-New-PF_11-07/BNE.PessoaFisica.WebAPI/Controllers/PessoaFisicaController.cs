﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using AutoMapper;
using BNE.Logger.Interface;
using BNE.PessoaFisica.WebAPI.Models;

namespace BNE.PessoaFisica.WebAPI.Controllers
{
    public class PessoaFisicaController : ApiController
    {
        private readonly ILogger _logger;
        private readonly Domain.PessoaFisica _pessoaFisica;

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

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, result.Replace("\"", "")));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Checar se candidato tem Experiência profissional e formação CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region GetPlano
        public ResponseMessageResult GetPlano(string cpf)
        {
            try
            {
                var result = _pessoaFisica.GetPlano(Convert.ToDecimal(cpf));
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, result));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Pegar o plano para vaga premium");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region IndicarAmigos
        [HttpPost]
        public ResponseMessageResult IndicarAmigos(Indicacao objIndicacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Indicacao, Domain.Command.Indicacao>(objIndicacao);
                    var resultado = _pessoaFisica.IndicarAmigos(command);

                    if (resultado)
                    {
                        var retorno = new
                        {
                            cpf = objIndicacao.CPF,
                            datanascimento = "",
                            url = ""
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }
                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.PreconditionFailed, "Falha"));
                }

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Indicação de Amigos");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region CadastrarExperienciaProfissional
        [HttpPost]
        public ResponseMessageResult CadastrarExperienciaProfissional(ExperienciaProfissional objExperiencia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<ExperienciaProfissional, Domain.Command.ExperienciaProfissional>(objExperiencia);
                    var resultado = _pessoaFisica.PostExperienciaProfissional(command);

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
                _logger.Error(ex, "Pessoa Fisica API - Cadastrar Experiência profissional CV", new JavaScriptSerializer().Serialize(objExperiencia));
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region CadastrarFormação
        [HttpPost]
        public ResponseMessageResult CadastrarFormacao(Formacao objFormacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = Mapper.Map<Formacao, Domain.Command.Formacao>(objFormacao);

                    var resultado = _pessoaFisica.PostFormacao(command);

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
                    bool cand;
                    var resultado = _pessoaFisica.CadastrarMiniCV(command, out cand);

                    if (resultado)
                    {
                        var retorno = new
                        {
                            candidatura = cand
                        };
                        return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, retorno));
                    }

                    return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest));
                }
                var errors = new Dictionary<string, IEnumerable<string>>();
                foreach (var keyValue in ModelState)
                {
                    errors[keyValue.Key] = keyValue.Value.Errors.Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage);
                }

                _logger.Error(new Exception(errors.ToString()), "PF modelo inválido");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Pessoa Fisica API - Cadastro Mini CV");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public ResponseMessageResult GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            try
            {
                var urls = _pessoaFisica.GetLinksPaginasSemelhantes(funcao, cidade, siglaEstado, areaBNE);

                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.Created, urls));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao montar os Links de páginas semelhantes");
                return new ResponseMessageResult(Request.CreateResponse(HttpStatusCode.InternalServerError, ex));
            }
        }
        #endregion
    }
}