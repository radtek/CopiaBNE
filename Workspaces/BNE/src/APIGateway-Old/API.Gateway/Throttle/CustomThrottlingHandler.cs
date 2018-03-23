using API.Gateway.Auth;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using WebApiThrottle;

namespace API.Gateway.Throttle
{
    public class CustomThrottlingHandler : ThrottlingHandler
    {

        protected override RequestIdentity SetIndentity(HttpRequestMessage request)
        {
            return new RequestIdentity()
            {
                ClientKey = GetAuthId(request),
                ClientIp = base.GetClientIp(request).ToString(),
                Endpoint = request.RequestUri.AbsolutePath.ToLowerInvariant()
            };
        }

        private string GetAuthId(HttpRequestMessage request)
        {
            var claim_keys = CustomClaim.Build((ClaimsIdentity)request.GetOwinContext().Authentication.User.Identity);
            if (!string.IsNullOrEmpty(request.RequestUri.AbsolutePath) && claim_keys.Count > 0) 
            {
                return string.Format("{0}{1}", claim_keys["Idf_Perfil_Usuario"].ToString(), request.RequestUri.AbsolutePath);
            }
            return "";
        }

    }
}