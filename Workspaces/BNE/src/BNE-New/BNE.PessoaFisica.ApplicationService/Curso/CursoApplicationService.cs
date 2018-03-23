using System.Collections.Generic;
using AutoMapper;
using BNE.PessoaFisica.ApplicationService.Curso.Command;
using BNE.PessoaFisica.ApplicationService.Curso.Interface;
using BNE.PessoaFisica.ApplicationService.Curso.Model;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.Curso
{
    public class CursoApplicationService : SharedKernel.ApplicationService.ApplicationService, ICursoApplicationService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMapper _mapper;

        public CursoApplicationService(ICursoRepository cursoRepository, IMapper mapper, IUnitOfWork unitOfWork, EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _cursoRepository = cursoRepository;
            _mapper = mapper;
        }

        public IEnumerable<CursoResponse> GetList(GetCursoCommand command)
        {
            var lista = _cursoRepository.GetList(_mapper.Map<Domain.Command.GetCursoCommand>(command));

            return _mapper.Map<IEnumerable<Domain.Model.Curso>, IEnumerable<CursoResponse>>(lista);
        }
    }
}