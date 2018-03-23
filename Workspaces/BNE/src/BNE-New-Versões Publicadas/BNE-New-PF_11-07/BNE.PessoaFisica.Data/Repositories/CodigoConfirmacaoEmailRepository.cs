using BNE.Data.Infrastructure;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class CodigoConfirmacaoEmailRepository : RepositoryBase<Model.CodigoConfirmacaoEmail>, ICodigoConfirmacaoEmailRepository
    {
        public CodigoConfirmacaoEmailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ICodigoConfirmacaoEmailRepository : IRepository<Model.CodigoConfirmacaoEmail> { }
}