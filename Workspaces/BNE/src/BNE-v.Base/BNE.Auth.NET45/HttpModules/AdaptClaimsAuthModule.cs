using BNE.Auth.Helper;
using Microsoft.IdentityModel.Claims;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

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

            IPrincipal claimsPrincipal;
            if (user.Identity is FormsIdentity)
            {
                var formsIdentity = (FormsIdentity)user.Identity;

                claimsPrincipal = new ClaimsPrincipal(user);
                var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

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
            else if (user.Identity is System.Security.Claims.ClaimsIdentity)
            {
                var securityIdentity = (System.Security.Claims.ClaimsIdentity)user.Identity;

                // user is authenticated - we will transform his identity
                claimsPrincipal = new ClaimsPrincipal(user);
                var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;

                foreach (var sc in securityIdentity.Claims)
                {
                    if (sc.Type == ClaimsIdentity.DefaultNameClaimType)
                        continue;

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

            HttpContext.Current.User = claimsPrincipal;
            Thread.CurrentPrincipal = claimsPrincipal;
        }

        public void Dispose()
        {
        }
    }
}
