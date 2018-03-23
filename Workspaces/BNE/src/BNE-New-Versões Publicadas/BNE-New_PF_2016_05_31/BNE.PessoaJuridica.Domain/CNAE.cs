using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class CNAE
    {

        private readonly ICNAERepository _cnaeRepository;

        public CNAE(ICNAERepository cnaeRepository)
        {
            _cnaeRepository = cnaeRepository;
        }

        public Model.CNAE GetByCodigo(string codigo)
        {
            return _cnaeRepository.Get(n => n.Codigo == codigo);
        }

    }
}
