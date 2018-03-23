using System;
using System.Data.Entity;

namespace BNE.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DbContext Get();
    }
}