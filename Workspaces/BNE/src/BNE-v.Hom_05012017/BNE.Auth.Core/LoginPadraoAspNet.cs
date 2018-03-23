using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using BNE.Auth.Core.Helper;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth.Core
{
    public class LoginPadraoAspNet : ILoginPadraoAspNet
    {
        public class SimpleClaim
        {
            public string ClaimType { get; set; }
            public string Value { get; set; }
        }

        public class CookieSimpleInfo
        {
            public string CookieDomain { get; set; }
            public string CookieName { get; set; }
        }

        private Func<IEnumerable<SimpleClaim>, string> _serializerAccessor;
        private Func<HttpContextBase> _contextAccessor;
        private bool _isDisposed;
        private IEnumerable<CookieSimpleInfo> _authenticationCookies;

        public LoginPadraoAspNet(Func<IEnumerable<SimpleClaim>, string> serializerAccessor, Func<HttpContextBase> contextAccessor, LoginBehaviorBase behavior,
            IEnumerable<CookieSimpleInfo> authCookies)
        {
            if (serializerAccessor == null)
                throw new NullReferenceException("serializer");
            if (contextAccessor == null)
                throw new NullReferenceException("context");

            this.Behavior = behavior;
            this._serializerAccessor = serializerAccessor;
            this._contextAccessor = contextAccessor;
            this._authenticationCookies = authCookies ?? GetAspNetAuthDefaultCookie();
        }

        public LoginPadraoAspNet(LoginBehaviorBase behavior)
            : this(DefaultSerializerFactory, DefaultContextFactory, behavior, null) { }

        public LoginPadraoAspNet(Func<HttpContextBase> context)
            : this(DefaultSerializerFactory, context, null, null) { }

        public LoginPadraoAspNet(Func<HttpContextBase> context, LoginBehaviorBase behavior)
            : this(DefaultSerializerFactory, context, behavior, null) { }

        public LoginPadraoAspNet(Func<HttpContextBase> context, LoginBehaviorBase behavior, IEnumerable<CookieSimpleInfo> customAuthCookies)
            : this(DefaultSerializerFactory, DefaultContextFactory, behavior, customAuthCookies) { }

        public LoginPadraoAspNet()
            : this(DefaultSerializerFactory, DefaultContextFactory, null, null) { }

        private static HttpContextBase DefaultContextFactory()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }

        private static string DefaultSerializerFactory(IEnumerable<SimpleClaim> arg)
        {
            return ClaimsDefaultSerializerHelper.SerializeClaims(arg);
        }

        public virtual bool OverrideAspNetDefaultCookieInLogOut
        {
            get
            {
                return true;
            }
        }

        public LoginBehaviorBase Behavior { get; set; }

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

        protected virtual void LoginStaticItems(ClaimsIdentity identity, HttpContextBase context)
        {
            var principal = new ClaimsPrincipal(new[] { identity });
            context.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        protected virtual void LoginInternal(HttpContextBase context, ClaimsIdentity identity, bool rememberMe, out HttpCookie cookie)
        {
            FormsAuthentication.SignOut();

            cookie = CreateFormsAuthCookie(identity, rememberMe);

            if (context == null)
                throw new NullReferenceException("context");

            context.Response.SetCookie(cookie);
        }

        public void Deslogar(bool abandonarSessao = true)
        {
            LoginBehaviorBase.LogOffInfo logoffInfo = default(LoginBehaviorBase.LogOffInfo);
            HttpContextBase context = null;
            if (Behavior != null)
            {
                logoffInfo = new LoginBehaviorBase.LogOffInfo
                {
                    AbandonSession = abandonarSessao,
                    AuthCookies = _authenticationCookies,
                };
                Behavior.OnBeforeLogoff(() => context = _contextAccessor(), logoffInfo);

                if (context == null)
                    context = _contextAccessor();
            }
            else
            {
                context = _contextAccessor();
            }

            LogoffInternal(context, abandonarSessao);

            if (Behavior != null)
            {
                if (Behavior.BeforeLogOffEx != null)
                    throw Behavior.BeforeLogOffEx;

                if (default(LoginBehaviorBase.LogOffInfo).Equals(logoffInfo))
                    logoffInfo = new LoginBehaviorBase.LogOffInfo { AbandonSession = abandonarSessao, AuthCookies = _authenticationCookies };

                Behavior.OnAfterLogoff(context, logoffInfo);
            }
        }

        protected virtual void LogoffInternal(HttpContextBase context, bool abandonSession)
        {
            try
            {
                FormsAuthenticationLogoff();

                if (!abandonSession)
                    return;

                if (_isDisposed)
                    return;

                if (context == null)
                    throw new NullReferenceException("context");

                LimparSessao(context.Session);
            }
            finally
            {
                LimparCookies(context, _authenticationCookies);
            }
        }

        protected virtual void LimparCookies(HttpContextBase context, IEnumerable<CookieSimpleInfo> authCookies)
        {
            foreach (var item in authCookies)
            {
                HttpCookie cookie;
                if (item.CookieDomain == null)
                    cookie = new HttpCookie(item.CookieName, string.Empty) { Expires = DateTime.Now.AddDays(-1) };
                else
                    cookie = new HttpCookie(item.CookieName, string.Empty) { Expires = DateTime.Now.AddDays(-1), Domain = item.CookieDomain };

                context.Response.Cookies.Add(cookie);
            }
        }

        protected virtual void FormsAuthenticationLogoff()
        {
            FormsAuthentication.SignOut();
        }

        protected virtual void LimparSessao(HttpSessionStateBase session)
        {
            if (session != null)
                session.Abandon();
        }

        /// <summary>
        /// http://blogs.msdn.com/b/webdev/archive/2013/07/03/understanding-owin-forms-authentication-in-mvc-5.aspx
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<CookieSimpleInfo> GetAspNetAuthDefaultCookie()
        {
            yield return new CookieSimpleInfo
            {
                CookieName = FormsAuthentication.FormsCookieName,
#if !DEBUG
                CookieDomain = FormsAuthentication.CookieDomain
#endif
            };
            yield return new CookieSimpleInfo
            {
                CookieName = ".AspNet.Application",
#if !DEBUG
                CookieDomain = FormsAuthentication.CookieDomain
#endif
            };

            yield return new CookieSimpleInfo
            {
                CookieName = ".AspNet.External",
#if !DEBUG
                CookieDomain = FormsAuthentication.CookieDomain
#endif
            };

            yield return new CookieSimpleInfo
            {
                CookieName = ".AspNet.ApplicationCookie",
#if !DEBUG
                CookieDomain = FormsAuthentication.CookieDomain
#endif
            };
            yield return new CookieSimpleInfo
            {
                CookieName = ".AspNet.ExternalCookie",
#if !DEBUG
                CookieDomain = FormsAuthentication.CookieDomain
#endif
            };
        }

        protected virtual HttpCookie CreateFormsAuthCookie(ClaimsIdentity identity, bool rememberMe)
        {
            /*
            var toSerialize = identity.Claims.Where(a => a.ClaimType != ClaimsIdentity.DefaultNameClaimType)
                                            .Select(c => new SimpleClaim { ClaimType = c.ClaimType, Value = c.Value })
                                            .Concat(new[] { new SimpleClaim { 
                                                                            ClaimType = "built", 
                                                                               Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm")}});
             * 
            */
            var toSerialize = identity.Claims.Select(c => new SimpleClaim { ClaimType = c.ClaimType, Value = c.Value }).Concat(new[] { new SimpleClaim { ClaimType = "built", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm") } });

            // we need to serialize claims to string
            var userData = _serializerAccessor(toSerialize);

            // then create an auth ticket
            var cookie = FormsAuthentication.GetAuthCookie(identity.Name, rememberMe);

#if DEBUG
            cookie.Domain = null;
#endif

            var authTicket = FormsAuthentication.Decrypt(cookie.Value);
            authTicket = new FormsAuthenticationTicket(authTicket.Version, authTicket.Name,
                                                       DateTime.Now, DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
                                                       authTicket.IsPersistent,
                                                       userData,
                                                       authTicket.CookiePath);
            cookie.Value = FormsAuthentication.Encrypt(authTicket);

            return cookie;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _isDisposed = true;

            _contextAccessor = null;
            _serializerAccessor = null;
            _authenticationCookies = Enumerable.Empty<CookieSimpleInfo>();
        }
    }
}
