using System;
using AutoMapper;
using BNE.Global.Data.Repositories;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Command;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Interface;
using BNE.PessoaFisica.ApplicationService.PreCurriculo.Model;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.PreCurriculo
{
    public class PreCurriculoApplicationService : SharedKernel.ApplicationService.ApplicationService, IPreCurriculoApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPreCurriculoRepository _preCurriculoRepository;
        private readonly ISexoRepository _sexoRepository;

        public PreCurriculoApplicationService(IMapper mapper, IPreCurriculoRepository preCurriculoRepository, ISexoRepository sexoRepository, IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _mapper = mapper;
            _preCurriculoRepository = preCurriculoRepository;
            _sexoRepository = sexoRepository;
        }

        public PreCurriculoResponse Cadastrar(SalvarPreCurriculoCommand command)
        {
            var domainCommand = _mapper.Map<SalvarPreCurriculoCommand, Domain.Command.SalvarPreCurriculoCommand>(command);

            short tempoExperiencia = 0;

            if (domainCommand.TempoExperienciaAnos != null && domainCommand.TempoExperienciaMeses != null)
                tempoExperiencia = short.Parse(((domainCommand.TempoExperienciaAnos*12) + domainCommand.TempoExperienciaMeses).ToString());

            var objSexo = _sexoRepository.GetByChar(domainCommand.Sexo);
            var objPreCurriculo = new Domain.Model.PreCurriculo
            {
                Nome = domainCommand.Nome,
                Email = domainCommand.Email,
                DDDCelular = domainCommand.DDDCelular,
                Celular = domainCommand.NumeroCelular,
                TempoExperiencia = tempoExperiencia,
                PretensaoSalarial = domainCommand.PretensaoSalarial,
                IdCidade = domainCommand.IdCidade,
                IdFuncao = domainCommand.IdFuncao,
                IdVaga = domainCommand.IdVaga,
                DataCadastro = DateTime.Now,
                DescricaoFuncao = domainCommand.DescricaoFuncao,
                Sexo = objSexo
            };

            _preCurriculoRepository.Add(objPreCurriculo);

            if (Commit())
            {
                return _mapper.Map<Domain.Model.PreCurriculo, PreCurriculoResponse>(objPreCurriculo);
            }

            return null;
        }
    }
}