﻿using BNE.Data.Infrastructure;

namespace BNE.Mensagem.Data.Repositories
{
    public class EmailRepository : RepositoryBase<Model.Email>, IEmailRepository
    {
        public EmailRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }

    public interface IEmailRepository : IRepository<Model.Email> { }
}