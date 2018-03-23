using System;
using System.Web;
using BNE.Auth.Core.Enumeradores;
using BNE.Auth.EventArgs;

namespace BNE.Auth.Core
{
    public class EventBehavior : LoginBehaviorBase
    {
        private readonly LogoffType _logoffType;
        public EventBehavior(LogoffType logoffType)
        {
            this._logoffType = logoffType;
        }

        public override void OnAfterLogin(HttpContextBase context, Microsoft.IdentityModel.Claims.ClaimsIdentity claims, HttpCookie formsAuthCookie)
        {
            AuthEventAggregator.Instance.OnUserEnterSuccessfully(this, new BNEAuthLoginControlEventArgs(claims, context));
        }

        protected override void OnBeforeLogoffSafe(Func<HttpContextBase> context, LogOffInfo info)
        {
            if (_logoffType == LogoffType.NONE)
                throw new InvalidOperationException("_logOffType is invalid");

            AuthEventAggregator.Instance.OnClosingManuallySessionWithUserAuth(this, new BNELogoffAuthControlEventArgs(context()) { LogoffType = _logoffType });
        }
    }
}
