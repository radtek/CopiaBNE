using System.Collections.Generic;
using AutoMapper;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Command;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Interface;
using BNE.PessoaFisica.ApplicationService.InstituicaoEnsino.Model;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.InstituicaoEnsino
{
    public class InstituicaoEnsinoApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IInstituicaoEnsinoApplicationService
    {
        private readonly IInstituicaoEnsinoRepository _instituicaoEnsinoRepository;
        private readonly IMapper _mapper;

        public InstituicaoEnsinoApplicationService(IInstituicaoEnsinoRepository instituicaoEnsinoRepository,
            IMapper mapper, IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(
            unitOfWork, assertEventPool, bus)
        {
            _instituicaoEnsinoRepository = instituicaoEnsinoRepository;
            _mapper = mapper;
        }

        public IEnumerable<InstituicaoEnsinoResponse> GetList(GetInstituicaoEnsinoCommand command)
        {
            var lista = _instituicaoEnsinoRepository.GetList(
                _mapper.Map<Domain.Command.GetInstituicaoEnsinoCommand>(command));

            return _mapper
                .Map<IEnumerable<Domain.Model.InstituicaoEnsino>, IEnumerable<InstituicaoEnsinoResponse>>(lista);
        }
    }
}