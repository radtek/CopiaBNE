using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BNE.Auth.EventArgs;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth.HttpModules
{
    public class BNEAuthModule : FormsToClaimsAuthModule
    {
        protected override void OnStart(HttpApplication context)
        {
            base.OnStart(context);
            context.PreRequestHandlerExecute += context_PostAcquireRequestState;
        }

        void context_PostAcquireRequestState(object sender, System.EventArgs e)
        {
            var app = (HttpApplication)sender;
            if (app.Context.Handler is IReadOnlySessionState || app.Context.Handler is IRequiresSessionState)
            {
                if (!PreConditions(app.Context))
                    return;

                PostRequestWithAvailableSession(app);
            }
        }

        protected virtual void PostRequestWithAvailableSession(HttpApplication app)
        {
            CheckIfSessionIsStarting(app.Context);
        }

        private bool PreConditions(HttpContext context)
        {
            if (context.Session == null)
                return false;

            /*
            var user = context.User;
            if (user.Identity == null)
                return false;
            */

            return true;
        }

        protected virtual void CheckIfSessionIsStarting(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                BNEAutenticacao.DeslogarWithoutAuth(context);
                return;
            }

            var sessionVar = new SessionVariable<SessionStartEventControlType>(Chave.Permanente.SesssaoIniciadaControleEventos.ToString());
            if (sessionVar.ValueOrDefault == SessionStartEventControlType.None)
            {
                var res = IsRemeberMeLogin(identity);
                var authArgs = new BNEAuthStartSessionControlArgs(context)
                {
                    Identity = identity,
                    EventType = res ? SessionStartEventControlType.NewSessionAuthenticatedUsingRememberMeLogin : SessionStartEventControlType.NewSessionAuthenticated
                };

                try
                {
                    AuthEventAggregator.Instance.OnNewSessionWithAutheticatedUser(null, authArgs);
                }
                finally
                {
                    sessionVar.Value = authArgs.EventType;
                }
            }
        }

        public enum SessionStartEventControlType
        {
            None = 0,
            NewSessionAuthenticated = 1,
            NewSessionAuthenticatedUsingRememberMeLogin = 2
        }

        public static bool IsRemeberMeLogin(ClaimsIdentity identity)
        {
            var persistent = identity.Claims.FirstOrDefault(a => a.ClaimType == ClaimTypes.IsPersistent);
            if (persistent == null || (persistent.Value != "1" && !StringComparer.OrdinalIgnoreCase.Equals(persistent.Value, "true")))
                return false;

            return true;
        }

    }
}
