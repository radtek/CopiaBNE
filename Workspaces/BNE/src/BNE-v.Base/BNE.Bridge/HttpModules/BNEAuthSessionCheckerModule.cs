using BNE.Auth;
using BNE.Auth.HttpModules;
using BNE.Common.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace BNE.Bridge.HttpModules
{
    public class BNEAuthSessionCheckerModule : BNE.Auth.HttpModules.BNEAuthModule
    {
        protected override void PostRequestWithAvailableSession(System.Web.HttpApplication app)
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

            var session = new SessionVariable<int>(BNE.Common.Enumeradores.Chave.Permanente.IdPessoaFisicaLogada.ToString());
            if (session.ValueOrDefault != pfId)
            {
                AuthEventAggregator.Instance.OnUserAuthenticatedWithDifferentSession(this, new BNEAuthEventArgs(context) { Identity = identity });
            }
        }

      
    }
}
