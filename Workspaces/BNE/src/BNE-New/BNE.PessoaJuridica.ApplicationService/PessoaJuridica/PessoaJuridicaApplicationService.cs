using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using AutoMapper;
using SharedKernel.Repositories.Contracts;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Command;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Interface;
using BNE.PessoaJuridica.ApplicationService.PessoaJuridica.View;
using BNE.PessoaJuridica.Domain.Command;
using BNE.PessoaJuridica.Domain.Model.Enumeradores;
using BNE.PessoaJuridica.Domain.Services;
using log4net;
using log4net.Core;
using MailSender;
using MailSender.Models;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaJuridica.ApplicationService.PessoaJuridica
{
    public class PessoaJuridicaApplicationService : SharedKernel.ApplicationService.ApplicationService, IPessoaJuridicaApplicationService
    {
        private readonly ILog _logger;
        private readonly IMailSenderAPI _mailSenderApi;
        private readonly IMapper _mapper;
        private readonly NCallService _ncall;

        private readonly PessoaJuridicaService _pessoaJuridica;
        private readonly UsuarioService _usuario;
        private readonly UsuarioAdicionalService _usuarioAdicionalService;

        private readonly string from = ConfigurationManager.AppSettings.Get("Mailsender-From");
        private readonly string processKey = ConfigurationManager.AppSettings.Get("Mailsender-ProcessKey");

        public PessoaJuridicaApplicationService(
            ILog logger,
            IMapper mapper,
            IMailSenderAPI mailSenderApi,
            PessoaJuridicaService pessoaJuridica,
            UsuarioService usuario,
            NCallService ncall,
            UsuarioAdicionalService usuarioAdicionalService,
            IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool,
            IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _logger = logger;
            _mapper = mapper;
            _pessoaJuridica = pessoaJuridica;
            _usuario = usuario;
            _ncall = ncall;
            _usuarioAdicionalService = usuarioAdicionalService;
            _mailSenderApi = mailSenderApi;
        }

        public CadastroEmpresaView CadastrarEmpresa(CadastroEmpresa command)
        {
            try
            {
                var domainCommand = _mapper.Map<CadastroEmpresa, CadastroPessoaJuridica>(command);

                var empresaJaCadastrada = _pessoaJuridica.ExisteCNPJ(domainCommand.NumeroCNPJ);

                var retorno = _pessoaJuridica.SalvarPessoaJuridica(domainCommand);

                if (Commit())
                {
                    #region Email - UsuariosAdicionais

                    if (command.UsuariosAdicionais != null && command.UsuariosAdicionais.Any())
                    {
                        try
                        {
                            var toList = new List<String>();
                            var parametros = new InclusaoUsuarioAdicionalEmpresaParameters
                            {
                                Section = new InclusaoUsuarioAdicionalEmpresaParameters.SectionParameters
                                {
                                    nomeempresa = command.RazaoSocial,
                                    nomeresponsavel = command.Usuario.Nome,
                                    primeironomeresponsavel = Core.Common.Utils.RetornarPrimeiroNome(command.Usuario.Nome)
                                },
                                Substitution = new InclusaoUsuarioAdicionalEmpresaParameters.SubstitutionParameters()
                            };

                            foreach (var objUsuarioAdicional in command.UsuariosAdicionais)
                            {
                                toList.Add(objUsuarioAdicional.Email);
                                parametros.Substitution.nome.Add(Core.Common.Utils.RetornarPrimeiroNome(objUsuarioAdicional.Nome));
                                parametros.Substitution.linkconvite.Add(_usuarioAdicionalService.RecuperarLink(retorno.Item1, objUsuarioAdicional.Email, objUsuarioAdicional.Nome));
                            }

                            var response = _mailSenderApi.Mail.PostWithHttpMessagesAsync(new SendCommand(processKey, from, toList, null, null, null, null, null, Guid.Parse(Email.InclusaoUsuarioAdicionalEmpresa.GetDescription()), parametros.Substitution, parametros.Section)).Result;
                            if (!response.Response.IsSuccessStatusCode)
                            {
                                throw new Exception("Falha ao enviar email");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }
                    }

                    #endregion

                    if (retorno != null && retorno.Item1 != null)
                    {
                        return new CadastroEmpresaView
                        {
                            LinkAcesso = _usuario.GerarHashAcessoLoginAutomatico(retorno.Item2, "/sala-selecionador"),
                            LinkPlanoFree = _usuario.GerarHashAcessoLoginAutomatico(retorno.Item2, "/sala-selecionador"),
                            LinkPlanoPago = _pessoaJuridica.GerarHashCompraDePlano(retorno.Item2),
                            Click2Call = _ncall.FilaBoasVindasDisponivel() || _ncall.FilaCIADisponivel(),
                            NumeroTelefone = command.Usuario.NumeroDDDComercial + command.Usuario.NumeroComercial,
                            Nome = command.Usuario.Nome,
                            EmpresaJaCadastrada = empresaJaCadastrada,
                            ValorPlanoPagoDe = _pessoaJuridica.ValorPlanoDe(),
                            ValorPlanoPagoPara = _pessoaJuridica.ValorPlanoPara()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                var customError = string.Empty;

                var exception = ex as DbEntityValidationException;
                if (exception != null)
                    customError = string.Join("; ", exception.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));

                _logger.Error(customError, ex);

                return new CadastroEmpresaView
                {
                    Error = ex
                };
            }
            return null;
        }
    }

    internal class InclusaoUsuarioAdicionalEmpresaParameters
    {
        public SubstitutionParameters Substitution { get; set; }
        public SectionParameters Section { get; set; }
        internal class SubstitutionParameters
        {
            public SubstitutionParameters()
            {
                nome = new List<string>();
                linkconvite = new List<string>();
            }
            public IList<string> nome { get; set; }
            public IList<string> linkconvite { get; set; }
        }

        public class SectionParameters
        {
            public string nomeempresa { get; set; }
            public string nomeresponsavel { get; set; }
            public string primeironomeresponsavel { get; set; }
        }
    }
}