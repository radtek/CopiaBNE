using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class ParametroRepository : RepositoryBase<Model.Parametro>, IParametroRepository
    {
        public ParametroRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IParametroRepository : IRepository<Model.Parametro> { }
}