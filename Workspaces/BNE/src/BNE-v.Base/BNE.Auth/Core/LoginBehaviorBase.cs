using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Auth
{
    public abstract class LoginBehaviorBase
    {
        public struct LogOffInfo
        {
            public bool AbandonSession { get; set; }

            public IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> AuthCookies { get; set; }
        }

        internal Exception BeforeLogInfEx { get; private set; }
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
        public void OnBeforeLogoff(Func<HttpContextBase> contextAccessor, LoginBehaviorBase.LogOffInfo info)
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

        protected virtual void OnBeforeLogoffSafe(Func<HttpContextBase> context, LoginBehaviorBase.LogOffInfo info)
        {

        }


        public virtual void OnAfterLogoff(HttpContextBase context, LoginBehaviorBase.LogOffInfo info)
        {

        }

        internal void SetExceptionBeforeLogIn(Exception exp)
        {
            this.BeforeLogInfEx = exp;
        }

        internal void SetExceptionBeforeLogoff(Exception exp)
        {
            this.BeforeLogOffEx = exp;
        }

    }
}
