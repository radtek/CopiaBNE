using Microsoft.IdentityModel.Claims;
using System;
namespace BNE.Auth
{
    public interface ILoginPadraoAspNet : IDisposable
    {
        LoginBehaviorBase Behavior { get; set; }
        void Logar(ClaimsIdentity identity, bool rememberMe);
        void Deslogar(bool abandonarSessao = true);
        bool OverrideAspNetDefaultCookieInLogOut { get; }
    }
}
