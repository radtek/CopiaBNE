using System;
using System.Configuration;
using BNE.Cache;

namespace BNE.BLL
{
    public abstract class Entity
    {
        protected static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        protected static readonly ICachingService Cache = CachingServiceProvider.Instance;
    }
}