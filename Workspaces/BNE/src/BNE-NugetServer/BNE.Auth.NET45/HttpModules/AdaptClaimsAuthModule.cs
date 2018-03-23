using Microsoft.IdentityModel.Claims;
using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using BNE.Auth.Core.Helper;

namespace BNE.Auth.NET45.HttpModules
{
    public class AdjustClaimsAuthModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += context_PostAuthenticateRequest;
        }

        void context_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var user = HttpContext.Current.User;
            if (user == null || !user.Identity.IsAuthenticated)
                return;

            IPrincipal currentPrincipal;
            var formsIdentity = user.Identity as FormsIdentity;
            if (formsIdentity != null)
            {
                currentPrincipal = new ClaimsPrincipal(user);
                var claimsIdentity = (ClaimsIdentity)currentPrincipal.Identity;

                if (!String.IsNullOrEmpty(formsIdentity.Ticket.UserData))
                {
                    foreach (var sc in ClaimsDefaultSerializerHelper.DeserializeClaims(formsIdentity.Ticket.UserData))
                    {
                        var c = new Claim(sc.ClaimType, sc.Value);
                        if (!claimsIdentity.Claims.Contains(c))
                        {
                            claimsIdentity.Claims.Add(c);
                        }
                    }
                }
            }
            else
            {
                var securityIdentity = user.Identity as System.Security.Claims.ClaimsIdentity;
                if (securityIdentity != null)
                {
                    // user is authenticated - we will transform his identity
                    currentPrincipal = new ClaimsPrincipal(user);
                    var claimsIdentity = (ClaimsIdentity)currentPrincipal.Identity;

                    foreach (var sc in securityIdentity.Claims)
                    {
                        var c = new Claim(sc.Type, sc.Value);
                        if (!claimsIdentity.Claims.Contains(c))
                        {
                            claimsIdentity.Claims.Add(c);
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            HttpContext.Current.User = currentPrincipal;
            Thread.CurrentPrincipal = currentPrincipal;
        }

        public void Dispose()
        {
        }
    }
}
