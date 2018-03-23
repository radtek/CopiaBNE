using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Web;

namespace BNE.Auth.Core
{
    public abstract class LoginBehaviorBase
    {
        public struct LogOffInfo
        {
            public bool AbandonSession { get; set; }

            public IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> AuthCookies { get; set; }
        }

        public Exception BeforeLogInfEx { get; private set; }

        public void OnBeforeLogin(Func<HttpContextBase> contextAccessor, ClaimsIdentity claims)
        {
            try
            {
                OnBeforeLogInSafe(contextAccessor, claims);
            }
            catch (Exception ex)
            {
                BeforeLogInfEx = ex;
            }
        }

        protected virtual void OnBeforeLogInSafe(Func<HttpContextBase> contextAccessor, ClaimsIdentity info)
        {

        }

        public virtual void OnAfterLogin(HttpContextBase context, ClaimsIdentity claims, HttpCookie formsAuthCookie)
        {

        }

        internal Exception BeforeLogOffEx { get; private set; }
        public void OnBeforeLogoff(Func<HttpContextBase> contextAccessor, LogOffInfo info)
        {
            try
            {
                OnBeforeLogoffSafe(contextAccessor, info);
            }
            catch (Exception ex)
            {
                BeforeLogOffEx = ex;
            }
        }

        protected virtual void OnBeforeLogoffSafe(Func<HttpContextBase> context, LogOffInfo info)
        {

        }


        public virtual void OnAfterLogoff(HttpContextBase context, LogOffInfo info)
        {

        }

        protected internal void SetExceptionBeforeLogIn(Exception exp)
        {
            BeforeLogInfEx = exp;
        }

        protected internal void SetExceptionBeforeLogoff(Exception exp)
        {
            BeforeLogOffEx = exp;
        }

    }
}
