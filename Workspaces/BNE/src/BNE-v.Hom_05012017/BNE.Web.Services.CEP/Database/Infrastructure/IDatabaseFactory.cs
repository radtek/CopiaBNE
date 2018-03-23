using System;
using System.Data.Entity;

namespace BNE.Web.Services.Solr.Database.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}