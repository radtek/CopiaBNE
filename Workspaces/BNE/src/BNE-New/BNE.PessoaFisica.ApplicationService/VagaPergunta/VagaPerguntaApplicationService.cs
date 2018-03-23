using System.Collections.Generic;
using AutoMapper;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Command;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Interface;
using BNE.PessoaFisica.ApplicationService.VagaPergunta.Model;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.VagaPergunta
{
    public class VagaPerguntaApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IVagaPerguntaApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IVagaPerguntaRepository _vagaPerguntaRepository;

        public VagaPerguntaApplicationService(IVagaPerguntaRepository vagaPerguntaRepository, IMapper mapper,
            IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork,
            assertEventPool, bus)
        {
            _vagaPerguntaRepository = vagaPerguntaRepository;
            _mapper = mapper;
        }

        public IEnumerable<VagaPerguntaResponse> CarregarPergunta(GetByIdVagaCommand command)
        {
            var lista = _vagaPerguntaRepository.CarregarPergunta(command.IdVaga);
            return _mapper.Map<IEnumerable<Domain.Model.VagaPergunta>, IEnumerable<VagaPerguntaResponse>>(lista);
        }
    }
}