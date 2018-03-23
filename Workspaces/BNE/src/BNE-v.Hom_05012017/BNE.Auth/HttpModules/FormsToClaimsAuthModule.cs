using Microsoft.IdentityModel.Claims;
using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using BNE.Auth.Core.Helper;

namespace BNE.Auth.HttpModules
{
    public class FormsToClaimsAuthModule : IHttpModule
    {
        private object _inicialization;
        private object _syncRoot = new object();
        private bool _inicitialized;

        public void Init(HttpApplication context)
        {
            LazyInitializer.EnsureInitialized(ref _inicialization, ref _inicitialized, ref _syncRoot, () =>
            {
                OnStart(context);
                return new object();
            });
        }

        protected virtual void OnStart(HttpApplication context)
        {
            context.PostAuthenticateRequest += context_PostAuthenticateRequest;
        }

        void context_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            var user = HttpContext.Current.User;
            if (user == null
                || !user.Identity.IsAuthenticated
                || !(user.Identity is FormsIdentity))
                return;

            var identity = ConvertIdentity(user);
            OnConvertedIdentity((HttpApplication)sender, identity);
        }

        private ClaimsPrincipal ConvertIdentity(IPrincipal user)
        {
            // to do change to lazy mode

            var formsIdentity = (FormsIdentity)user.Identity;
            // user is authenticated - we will transform his identity
            var claimsPrincipal = new ClaimsPrincipal(user);
            var claimsIdentity = (ClaimsIdentity)claimsPrincipal.Identity;
            claimsIdentity.Claims.Add(new Claim(ClaimTypes.IsPersistent, formsIdentity.Ticket.IsPersistent ? "1" : "0"));

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

            HttpContext.Current.User = claimsPrincipal;
            Thread.CurrentPrincipal = claimsPrincipal;

            return claimsPrincipal;
        }

        protected virtual void OnConvertedIdentity(HttpApplication app, ClaimsPrincipal identity)
        {

        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool p)
        {
        }
    }
}