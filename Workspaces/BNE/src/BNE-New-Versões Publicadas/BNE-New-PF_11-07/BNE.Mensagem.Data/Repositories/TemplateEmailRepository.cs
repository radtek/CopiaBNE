using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Repositories
{
    public class TemplateEmailRepository : RepositoryBase<Model.TemplateEmail>, ITemplateEmailRepository
    {
        public TemplateEmailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface ITemplateEmailRepository : IRepository<Model.TemplateEmail> { }
}
