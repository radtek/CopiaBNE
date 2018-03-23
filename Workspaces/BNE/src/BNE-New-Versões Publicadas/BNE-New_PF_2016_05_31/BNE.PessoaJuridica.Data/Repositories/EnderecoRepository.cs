using BNE.Data.Infrastructure;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class EnderecoRepository : RepositoryBase<Model.Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IEnderecoRepository : IRepository<Model.Endereco> { }
}
