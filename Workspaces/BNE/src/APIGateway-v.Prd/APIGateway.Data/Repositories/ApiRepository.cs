using BNE.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Data.Repositories
{
    public class ApiRepository : RepositoryBase<Model.Api>, IApiRepository
    {
        public ApiRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IApiRepository : IRepository<Model.Api> { }
}
