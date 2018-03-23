using BNE.Web.Services.Solr.Database.Infrastructure;
using BNE.Web.Services.Solr.Models;

namespace BNE.Web.Services.Solr.Database.Repositories
{
    public class FuncaoNaoEncontradaRepository : RepositoryBase<FuncaoNaoEncontrada>, IFuncaoNaoEncontradaRepository
    {
        public FuncaoNaoEncontradaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IFuncaoNaoEncontradaRepository : IRepository<FuncaoNaoEncontrada> { }

}