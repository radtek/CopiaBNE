using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class CNAEDomainService
    {

        private readonly ICNAERepository _cnaeRepository;

        public CNAEDomainService(ICNAERepository cnaeRepository)
        {
            _cnaeRepository = cnaeRepository;
        }

        public Model.CNAE GetByCodigo(string codigo)
        {
            return _cnaeRepository.Get(n => n.Codigo == codigo);
        }

    }
}
