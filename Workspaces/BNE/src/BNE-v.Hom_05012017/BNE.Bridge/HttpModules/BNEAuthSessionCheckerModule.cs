using BNE.Auth;
using BNE.Auth.Core.ClaimTypes;
using BNE.Auth.EventArgs;
using BNE.Auth.HttpModules;
using BNE.Common.Session;
using System;
using System.Linq;
using System.Web;
using BNE.Common.Enumeradores;
using Microsoft.IdentityModel.Claims;

namespace BNE.Bridge.HttpModules
{
    public class BNEAuthSessionCheckerModule : BNEAuthModule
    {
        protected override void PostRequestWithAvailableSession(HttpApplication app)
        {
            base.PostRequestWithAvailableSession(app);

            CheckIfIsDiferentUserAuthenticated(app.Context);
        }

        private void CheckIfIsDiferentUserAuthenticated(HttpContext context)
        {
            if (context.Session.IsNewSession)
                return;

            var identity = context.User.Identity as Microsoft.IdentityModel.Claims.ClaimsIdentity;
            if (identity == null)
                return;

            var claim = identity.Claims.FirstOrDefault(a => a.ClaimType == BNEClaimTypes.PessoaFisicaId);
            if (claim == null)
                return;

            int pfId;
            if (!Int32.TryParse(claim.Value, out pfId) || pfId <= 0)
                return;

            var session = new SessionVariable<int>(Common.Enumeradores.Chave.Permanente.IdPessoaFisicaLogada.ToString());
            if (session.ValueOrDefault != pfId)
            {
                AuthEventAggregator.Instance.OnUserAuthenticatedWithDifferentSession(this, new BNEAuthEventArgs(context) { Identity = identity });
            }
        }

        protected override void CheckIfSessionIsStarting(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                BNEAutenticacao.DeslogarWithoutAuthClearSession(context);
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


    }
}
