using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class TelefoneComercialRepository : RepositoryBase<Model.TelefoneComercial>, ITelefoneComercialRepository
    {
        public TelefoneComercialRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITelefoneComercialRepository : IRepository<Model.TelefoneComercial> { }
}
