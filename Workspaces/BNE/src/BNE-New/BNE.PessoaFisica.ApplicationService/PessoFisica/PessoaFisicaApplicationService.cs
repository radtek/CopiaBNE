using AutoMapper;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Command;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Interface;
using BNE.PessoaFisica.ApplicationService.PessoFisica.Model;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.PessoFisica
{
    public class PessoaFisicaApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IPessoaFisicaApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;

        public PessoaFisicaApplicationService(IMapper mapper, IPessoaFisicaRepository pessoaFisicaRepository,
            IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork,
            assertEventPool, bus)
        {
            _mapper = mapper;
            _pessoaFisicaRepository = pessoaFisicaRepository;
        }

        public IndicarAmigosResponse IndicarAmigos(IndicarAmigosCommand command)
        {
            var domainCommand = _mapper.Map<IndicarAmigosCommand, SalvarIndicarAmigoCommand>(command);
            if (_pessoaFisicaRepository.IndicarAmigos(domainCommand))
            {
                var nome = _pessoaFisicaRepository.GetNomeJaExiste(domainCommand.CPF, domainCommand.DataNascimento);
                return new IndicarAmigosResponse {CPF = domainCommand.CPF, Nome = nome};
            }

            return null;
        }
    }
}