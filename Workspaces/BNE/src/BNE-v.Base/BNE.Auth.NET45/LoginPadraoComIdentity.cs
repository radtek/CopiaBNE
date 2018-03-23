using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace BNE.Auth.NET45
{
    public class LoginPadraoComIdentity : LoginPadraoAspNet
    {
        public LoginPadraoComIdentity(Func<object, string> serializerAccessor, Func<HttpContextBase> contextAccessor,
        LoginBehaviorBase behavior, IEnumerable<CookieSimpleInfo> authCookies)
            : base(serializerAccessor, contextAccessor, behavior, authCookies) { }

        public LoginPadraoComIdentity(Func<HttpContextBase> contextAccessor, LoginBehaviorBase behavior,
         IEnumerable<CookieSimpleInfo> customAuthCookies)
            : base(contextAccessor, behavior, customAuthCookies) { }

        public LoginPadraoComIdentity(Func<HttpContextBase> contextAccessor)
            : base(contextAccessor) { }

        public LoginPadraoComIdentity(Func<HttpContextBase> contextAccessor, LoginBehaviorBase behavior)
            : base(contextAccessor, behavior) { }

        public LoginPadraoComIdentity() : base() { }

        public LoginPadraoComIdentity(LoginBehaviorBase behavior)
            : base(behavior) { }

        protected override void LoginInternal(HttpContextBase context, Microsoft.IdentityModel.Claims.ClaimsIdentity identity, bool rememberMe, out HttpCookie cookie)
        {
            IOwinContext owin;
            if (context == null)
            {
                owin = null;
            }
            else
            {
                owin = context.GetOwinContext();
                owin.Authentication.SignOut();
            }

            base.LoginInternal(context, identity, rememberMe, out cookie);

            if (owin != null)
                SetOwinAuthModel(owin.Authentication, identity, rememberMe);
        }

        private void SetOwinAuthModel(Microsoft.Owin.Security.IAuthenticationManager manager, ClaimsIdentity identity, bool rememberMe)
        {
            var toSerialize = identity.Claims
                                    .Where(a => a.ClaimType != ClaimsIdentity.DefaultNameClaimType)
                                    .Select(c => new System.Security.Claims.Claim(c.ClaimType, c.Value))
                                    .Concat(new[] { new System.Security.Claims.Claim(ClaimTypes.IsPersistent, rememberMe.ToString()) });

            var other = new System.Security.Claims.ClaimsIdentity(new GenericIdentity(identity.Name, DefaultAuthenticationTypes.ApplicationCookie), toSerialize);
            manager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, other);
        }

        protected override void LogoffInternal(HttpContextBase context, bool limparSessao)
        {
            try
            {
                base.LogoffInternal(context, limparSessao);
            }
            finally
            {
                IOwinContext owin;
                if (context == null)
                {
                    owin = null;
                }
                else
                {
                    owin = context.GetOwinContext();
                    owin.Authentication.SignOut();
                }
            }
        }
    }
}
