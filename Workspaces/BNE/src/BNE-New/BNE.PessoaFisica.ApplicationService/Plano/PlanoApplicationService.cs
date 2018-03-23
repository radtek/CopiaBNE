using AutoMapper;
using BNE.PessoaFisica.ApplicationService.Plano.Command;
using BNE.PessoaFisica.ApplicationService.Plano.Interface;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.Plano
{
    public class PlanoApplicationService : SharedKernel.ApplicationService.ApplicationService, IPlanoApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPlanoRepository _planoRepository;

        public PlanoApplicationService(IPlanoRepository planoRepository, IMapper mapper, IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _planoRepository = planoRepository;
            _mapper = mapper;
        }

        public Model.PlanoResponse GetPlano(GetPlanoCommand command)
        {
            var plano = _planoRepository.GetPlano(_mapper.Map<Domain.Command.GetPlanoCommand>(command));
            return _mapper.Map<Domain.Model.Plano, Model.PlanoResponse>(plano);
        }
    }
}