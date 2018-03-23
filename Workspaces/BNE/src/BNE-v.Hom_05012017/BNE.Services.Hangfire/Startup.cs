using BNE.Services.Hangfire;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace BNE.Services.Hangfire
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new DashboardOptions
            {
                AppPath = "http://dashboard.bne.com.br",
                AuthorizationFilters = new[] { new AuthorizationFilter() }
            };
            app.UseHangfireDashboard("/dash", options);
            
            Jobs.ConfigureAll();
        }
    }
}
