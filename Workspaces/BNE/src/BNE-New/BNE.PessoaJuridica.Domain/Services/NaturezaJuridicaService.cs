using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class NaturezaJuridicaService
    {

        private readonly INaturezaJuridicaRepository _naturezaJuridicaRepository;

        public NaturezaJuridicaService(INaturezaJuridicaRepository naturezaJuridicaRepository)
        {
            _naturezaJuridicaRepository = naturezaJuridicaRepository;
        }

        public Model.NaturezaJuridica GetByCodigo(string codigo)
        {
            return _naturezaJuridicaRepository.Get(n => n.Codigo == codigo);
        }

    }
}
