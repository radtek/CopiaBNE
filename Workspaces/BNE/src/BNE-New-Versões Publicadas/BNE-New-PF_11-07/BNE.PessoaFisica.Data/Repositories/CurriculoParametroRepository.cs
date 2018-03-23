using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CurriculoParametroRepository : RepositoryBase<Model.CurriculoParametro>, ICurriculoParametroRepository
    {
        public CurriculoParametroRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICurriculoParametroRepository : IRepository<Model.CurriculoParametro> { }
}