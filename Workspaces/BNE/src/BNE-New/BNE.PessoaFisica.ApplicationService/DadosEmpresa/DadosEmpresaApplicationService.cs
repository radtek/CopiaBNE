using AutoMapper;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Command;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Interface;
using BNE.PessoaFisica.ApplicationService.DadosEmpresa.Model;
using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.ApplicationService.DadosEmpresa
{
    public class DadosEmpresaApplicationService : IDadosEmpresaApplicationService
    {
        private readonly IMapper _mapper;
        private readonly IDadosEmpresaRepository _DadosEmpresaRepository;

        public DadosEmpresaApplicationService(IDadosEmpresaRepository dadosEmpresaRepository, IMapper mapper)
        {
            _DadosEmpresaRepository = dadosEmpresaRepository;
            _mapper = mapper;
        }

        public DadosEmpresaResponse getDados(DadosEmpresaCommand command)
        {
            return _mapper.Map<Domain.Model.DadosEmpresa, DadosEmpresaResponse>(_DadosEmpresaRepository.RecuperarDados(command.idCurriculo, command.idVaga));
        }
    }
}