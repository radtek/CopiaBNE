using System;

namespace BNE.Dashboard.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DashboardEntities Get();
    }
}
