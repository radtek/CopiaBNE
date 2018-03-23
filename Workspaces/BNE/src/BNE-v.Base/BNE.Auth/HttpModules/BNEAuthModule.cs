using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
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

        void context_PostAcquireRequestState(object sender, EventArgs e)
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

            var user = context.User;
            if (user.Identity == null)
                return false;

            if (!user.Identity.IsAuthenticated)
                return false;

            return true;
        }

    
        private static bool CheckIfSessionIsStarting(HttpContext context)
        {
            if (!context.Session.IsNewSession)
                return false;

            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
                return true;

            var sessionVar = new SessionVariable<SessionStartEventControlType>(Chave.Permanente.SesssaoIniciadaControleEventos.ToString());
            if (sessionVar.ValueOrDefault == SessionStartEventControlType.None)
            {
                var res = IsRemeberMeLogin(identity);
                BNEAuthStartSessionControlArgs authArgs;
                if (res)
                {
                    authArgs = new BNEAuthStartSessionControlArgs(context) { Identity = identity, EventType = SessionStartEventControlType.NewSessionAuthenticatedUsingRememberMeLogin };
                }
                else
                {
                    authArgs = new BNEAuthStartSessionControlArgs(context) { Identity = identity, EventType = SessionStartEventControlType.NewSessionAuthenticated };
                }

                try
                {
                    AuthEventAggregator.Instance.OnNewSessionWithAutheticatedUser(null, authArgs);
                }
                finally
                {
                    sessionVar.Value = authArgs.EventType;
                }
            }

            return true;
        }

        public enum SessionStartEventControlType
        {
            None = 0,
            NewSessionAuthenticated = 1,
            NewSessionAuthenticatedUsingRememberMeLogin = 2
        }

        private static bool IsRemeberMeLogin(ClaimsIdentity identity)
        {
            var persistent = identity.Claims.FirstOrDefault(a => a.ClaimType == ClaimTypes.IsPersistent);
            if (persistent == null || (persistent.Value != "1" && !StringComparer.OrdinalIgnoreCase.Equals(persistent.Value, "true")))
                return false;

            return true;
        }

        //protected override void OnConvertedIdentity(System.Web.HttpApplication app, Microsoft.IdentityModel.Claims.ClaimsPrincipal identity)
        //{
            //var sessionModule = app.Modules["Session"] as SessionStateModule;
            //if (sessionModule == null)
            //    return;

            //var result =
            //    LazyInitializer.EnsureInitialized(ref _disp, ref _interceptInicialized, ref _synchronizationRoot, () => CreateSessionInterceptor(sessionModule));

            //if (result != _disp)
            //    _disp = result;
        //}

        //private static IDisposable _disp;
        //private static bool _interceptInicialized;
        //private static object _synchronizationRoot = new object();

        //private static IDisposable CreateSessionInterceptor(SessionStateModule sessionModule)
        //{
        //    EventHandler handler = null;
        //    handler = (s, e) => CheckIfSessionIsStarting();
        //    sessionModule.Start += handler;

        //    return System.Reactive.Disposables.Disposable.Create(() => sessionModule.Start -= handler);
        //}

    }
}
