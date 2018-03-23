using AutoMapper;
using BNE.PessoaFisica.ApplicationService.Vaga.Command;
using BNE.PessoaFisica.ApplicationService.Vaga.Interface;
using BNE.PessoaFisica.ApplicationService.Vaga.Model;
using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.ApplicationService.Vaga
{
    public class VagaApplicationService : IVagaApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IVagaRepository _repository;

        public VagaApplicationService(IVagaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public VagaResponse CarregarVaga(GetByIdCommand command)
        {
            return _mapper.Map<Domain.Model.Vaga, VagaResponse>(_repository.RecuperarVaga(command.IdVaga));
        }
    }
}