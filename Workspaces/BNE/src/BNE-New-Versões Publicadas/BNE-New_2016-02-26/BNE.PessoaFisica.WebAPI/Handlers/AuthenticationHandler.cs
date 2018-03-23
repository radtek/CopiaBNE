using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BNE.PessoaFisica.WebAPI.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            IEnumerable<string> authData;
            if(!request.Headers.TryGetValues("authenticarion_data", out authData))
                return base.SendAsync(request, cancellationToken);
            
            string[] rolesArray = { "managers", "executives" };

            JObject j = JObject.Parse(authData.FirstOrDefault());

            ClaimsIdentity identity = new ClaimsIdentity("APIGateway");

            foreach (var item in j.Properties())
                identity.AddClaim(new Claim(item.Name, item.Value.ToString()));


            Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
            HttpContext.Current.User = Thread.CurrentPrincipal;

            return base.SendAsync(request, cancellationToken);
        }
    }
}