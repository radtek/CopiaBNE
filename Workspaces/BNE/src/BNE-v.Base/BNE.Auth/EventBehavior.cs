using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BNE.Auth
{
    public class EventBehavior : LoginBehaviorBase
    {
        private LogoffType _logoffType;
        public EventBehavior(LogoffType logoffType)
        {
            this._logoffType = logoffType;
        }

        public override void OnAfterLogin(HttpContextBase context, Microsoft.IdentityModel.Claims.ClaimsIdentity claims, HttpCookie formsAuthCookie)
        {
            AuthEventAggregator.Instance.OnUserEnterSuccessfully(this, new HttpModules.BNEAuthLoginControlEventArgs(claims, context));
        }

        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> context, LoginBehaviorBase.LogOffInfo info)
        {
            if (_logoffType == LogoffType.NONE)
                throw new InvalidOperationException("_logOffType is invalid");

            AuthEventAggregator.Instance.OnClosingManuallySessionWithUserAuth(this, new HttpModules.BNELogoffAuthControlEventArgs(context()) { LogoffType = _logoffType });
        }
    }
}
