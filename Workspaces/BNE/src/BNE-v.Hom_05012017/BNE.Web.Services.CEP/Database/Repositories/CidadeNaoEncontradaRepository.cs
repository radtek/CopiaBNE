using BNE.Web.Services.Solr.Database.Infrastructure;
using BNE.Web.Services.Solr.Models;

namespace BNE.Web.Services.Solr.Database.Repositories
{
    public class CidadeNaoEncontradaRepository : RepositoryBase<CidadeNaoEncontrada>, ICidadeNaoEncontradaRepository
    {
        public CidadeNaoEncontradaRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICidadeNaoEncontradaRepository : IRepository<CidadeNaoEncontrada> { }

}