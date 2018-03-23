using System;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth.Core.Interface
{
    public interface ILoginPadraoAspNet : IDisposable
    {
        LoginBehaviorBase Behavior { get; set; }
        void Logar(ClaimsIdentity identity, bool rememberMe);
        void Deslogar(bool abandonarSessao = true, bool clearSession = false);
        bool OverrideAspNetDefaultCookieInLogOut { get; }
        User AuthenticatedUser();
    }
}
