using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class NaturezaJuridica
    {

        private readonly INaturezaJuridicaRepository _naturezaJuridicaRepository;

        public NaturezaJuridica(INaturezaJuridicaRepository naturezaJuridicaRepository)
        {
            _naturezaJuridicaRepository = naturezaJuridicaRepository;
        }

        public Model.NaturezaJuridica GetByCodigo(string codigo)
        {
            return _naturezaJuridicaRepository.Get(n => n.Codigo == codigo);
        }

    }
}
