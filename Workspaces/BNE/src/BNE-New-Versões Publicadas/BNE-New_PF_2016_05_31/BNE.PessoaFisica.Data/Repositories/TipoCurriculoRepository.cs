using BNE.Data.Infrastructure;
using BNE.PessoaFisica.Model;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class TipoCurriculoRepository : RepositoryBase<TipoCurriculo>, ITipoCurriculoRepository
    {
        public TipoCurriculoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITipoCurriculoRepository : IRepository<TipoCurriculo> { }
}		