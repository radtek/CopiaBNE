using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class FuncaoPretendidaRepository : RepositoryBase<Model.FuncaoPretendida>, IFuncaoPretendidaRepository
    {
        public FuncaoPretendidaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IFuncaoPretendidaRepository : IRepository<Model.FuncaoPretendida> { }
}