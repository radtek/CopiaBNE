using BNE.Auth.Core;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using BNE.Auth.Core.ClaimTypes;

namespace BNE.Auth.NET45
{
    public class LoginPadraoComIdentity : LoginPadraoAspNet
    {

        private Func<HttpContextBase> _contextAccessor;
        private bool _isDisposed;

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

        protected override void LoginInternal(HttpContextBase context, ClaimsIdentity identity, bool rememberMe, out HttpCookie cookie)
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

        private void SetOwinAuthModel(IAuthenticationManager manager, ClaimsIdentity identity, bool rememberMe)
        {
            var toSerialize = identity.Claims
                                    .Where(a => a.ClaimType != ClaimsIdentity.DefaultNameClaimType)
                                    .Select(c => new System.Security.Claims.Claim(c.ClaimType, c.Value))
                                    .Concat(new[] { new System.Security.Claims.Claim(ClaimTypes.IsPersistent, rememberMe.ToString()) });

            var other = new System.Security.Claims.ClaimsIdentity(new GenericIdentity(identity.Name, DefaultAuthenticationTypes.ApplicationCookie), toSerialize);
            manager.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, other);
        }

        protected override void LogoffInternal(HttpContextBase context, bool limparSessao, bool clearSession = false)
        {
            try
            {
                base.LogoffInternal(context, limparSessao, clearSession);
            }
            finally
            {
                if (context != null)
                {
                    var owin = context.GetOwinContext();
                    owin.Authentication.SignOut();
                }
            }
        }

        public void Logar(ClaimsIdentity identity, bool rememberMe)
        {
            if (identity == null)
                throw new NullReferenceException("identity");

            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().Name);

            HttpContextBase context = null;
            if (Behavior != null)
            {
                Behavior.OnBeforeLogin(() => context = _contextAccessor(), identity);
                if (context == null)
                    context = _contextAccessor();
            }
            else
            {
                context = _contextAccessor();
            }

            HttpCookie cookie;
            LoginInternal(context, identity, rememberMe, out cookie);

            LoginStaticItems(identity, context);

            if (Behavior != null)
                if (Behavior.BeforeLogInfEx != null)
                    throw Behavior.BeforeLogInfEx;
                else
                    Behavior.OnAfterLogin(context, identity, cookie);
        }

        public User AuthenticatedUser()
        {
            HttpContextBase context = null;
            if (Behavior != null)
            {
                if (context == null)
                    context = _contextAccessor();
            }
            else
            {
                context = _contextAccessor();
            }

            return User.GetUser(context);
        }

    }
}
