using System;
using System.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace Bne.Web.Services.API.App_Start
{
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        ICorsPolicyProvider _provider = new MyCorsPolicyProvider();

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            return _provider;
        }
    }

    public class MyCorsPolicyProvider : ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public MyCorsPolicyProvider()
        {
            // Create a CORS policy.
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            var crossOrigins = ConfigurationManager.AppSettings["CORS.AllowOrigins"];
            if (!string.IsNullOrEmpty(crossOrigins))
            {
                var origins = crossOrigins.Split(',');
                foreach (var origin in origins)
                {
                    _policy.Origins.Add(origin.Trim());
                }
            }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}