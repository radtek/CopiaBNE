using System;
using BNE.Auth.Core;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth.NET45.Interface
{
    public interface ILoginPadraoComIdentity : IDisposable
    {
        LoginBehaviorBase Behavior { get; set; }
        void Logar(ClaimsIdentity identity, bool rememberMe);
        void Deslogar(bool abandonarSessao = true);
        bool OverrideAspNetDefaultCookieInLogOut { get; }
        User AuthenticatedUser();
    }
}
