using BNE.Auth;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace BNE.Auth
{
    public static class BNEAutenticacao
    {
        static BNEAutenticacao()
        {
            _authenticationManager = BuiltDefaultAuthManager;
        }

        private static Func<Func<HttpContextBase>, LoginBehaviorBase, ILoginPadraoAspNet> _authenticationManager;
        public static Func<Func<HttpContextBase>, LoginBehaviorBase, ILoginPadraoAspNet> DefaultAuthManagerFactory
        {
            get
            {
                return _authenticationManager;
            }
            set
            {
                _authenticationManager = value;
            }
        }

        private static ILoginPadraoAspNet BuiltDefaultAuthManager(Func<HttpContextBase> f, LoginBehaviorBase b)
        {
            return f == null ? new LoginPadraoAspNet(b) : new LoginPadraoAspNet(f, b);
        }


#if DEBUG
        public const string DomainToUse = null;
#else
        public const string DomainToUse = ".bne.com.br";
#endif

        public static ClaimsIdentity LogarCPF(string name, int pfId,
                                        decimal cpf,
                                        bool lembrarUsuario = false)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, name));
            claims.Add(new Claim(BNEClaimTypes.CPF, cpf.ToString()));
            claims.Add(new Claim(BNEClaimTypes.PessoaFisicaId, pfId.ToString()));
            claims.Add(new Claim(BNEClaimTypes.PerfilUsuario, AuthPerfilType.NenhumOuDesconhecido.ToString()));
            var identity = new ClaimsIdentity(claims);

            using (var l = DefaultAuthManagerFactory(null, new EventBehavior(LogoffType.NONE)))
            {
                l.Logar(identity, lembrarUsuario);
            }

            return identity;
        }

        public static ClaimsIdentity LogarCandidato(string name,
                                        int pfId,
                                        decimal cpf,
                                        int curriculoId,
                                        bool lembrarUsuario = false)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, name));
            claims.Add(new Claim(BNEClaimTypes.CPF, cpf.ToString()));
            claims.Add(new Claim(BNEClaimTypes.CurriculoId, curriculoId.ToString()));
            claims.Add(new Claim(BNEClaimTypes.PessoaFisicaId, pfId.ToString()));
            claims.Add(new Claim(BNEClaimTypes.PerfilUsuario, AuthPerfilType.Candidato.ToString()));
            var identity = new ClaimsIdentity(claims);

            using (var l = DefaultAuthManagerFactory(null, new EventBehavior(LogoffType.NONE)))
            {
                l.Logar(identity, lembrarUsuario);
            }

            return identity;
        }

        #region [ IoC HttpContext ]

        public static void DeslogarPadrao(HttpContext context)
        {
            var behavior = new MultipleAuthBehavior(new BNECleanDefaultSession(), new CleanDefaultHttpAppCacheBehavior(), new CleanCookieBehavior(CookiesPadraoParaLimpar()));

            using (var l = DefaultAuthManagerFactory(() => new HttpContextWrapper(context), behavior))
            {
                l.Deslogar(true);
            }
        }
        public static void DeslogarPadrao(HttpContextBase context)
        {
            var behavior = new MultipleAuthBehavior(new BNECleanDefaultSession(), new CleanDefaultHttpAppCacheBehavior(), new CleanCookieBehavior(CookiesPadraoParaLimpar()));

            using (var l = DefaultAuthManagerFactory(() => context, behavior))
            {
                l.Deslogar(true);
            }
        }
        #endregion

        #region [ Deslogar Padrao ]
        public static void DeslogarPadrao(LogoffType logoffType = LogoffType.BY_USER, params string[] outrosCookiesParaLimpar)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(
                                    (outrosCookiesParaLimpar ?? new string[0])
                                        .Select(a => new LoginPadraoAspNet.CookieSimpleInfo { CookieName = a, CookieDomain = DomainToUse }));

            LogoutDefault(logoffType, otherCookies);
        }

        public static void DeslogarPadrao(IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> outrosCookiesParaLimpar, LogoffType logoffType = LogoffType.BY_USER)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(outrosCookiesParaLimpar ?? new LoginPadraoAspNet.CookieSimpleInfo[0]);

            LogoutDefault(logoffType, otherCookies);
        }

        #endregion

        #region [ Deslogar Pagamento ]
        public static void DeslogarPagamento(LogoffType logoffType = LogoffType.BY_USER, params string[] outrosCookiesParaLimpar)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(
                                    (outrosCookiesParaLimpar ?? new string[0])
                                        .Select(a => new LoginPadraoAspNet.CookieSimpleInfo { CookieName = a, CookieDomain = DomainToUse }));

            LogoutPayment(logoffType, otherCookies);
        }

        public static void DeslogarPagamento(IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> outrosCookiesParaLimpar, LogoffType logoffType = LogoffType.BY_USER)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(outrosCookiesParaLimpar ?? new LoginPadraoAspNet.CookieSimpleInfo[0]);

            LogoutPayment(logoffType, otherCookies);
        }
        #endregion

        private static void LogoutDefault(LogoffType logoffType, IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> otherCookies)
        {
            var behavior = new MultipleAuthBehavior(new BNECleanDefaultSession(),
                                        new EventBehavior(logoffType),
                                        new CleanDefaultHttpAppCacheBehavior(),
                                     new CleanCookieBehavior(otherCookies));

            using (var l = DefaultAuthManagerFactory(null, behavior))
            {
                l.Deslogar(true);
            }
        }

        private static void LogoutPayment(LogoffType logoffType, IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> otherCookies)
        {
            var behavior = new MultipleAuthBehavior(new BNECleanDefaultSession(),
                                       new BNECleanPaymentSession(),
                                        new EventBehavior(logoffType),
                                       new CleanDefaultHttpAppCacheBehavior(),
                                       new CleanCookieBehavior(otherCookies));

            using (var l = DefaultAuthManagerFactory(null, behavior))
            {
                l.Deslogar(true);
            }
        }

        private static IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> CookiesPadraoParaLimpar()
        {
            yield return new LoginPadraoAspNet.CookieSimpleInfo { CookieName = "BNE", CookieDomain = DomainToUse };    // cookie vagas
            yield return new LoginPadraoAspNet.CookieSimpleInfo { CookieName = "BNE_Acesso", CookieDomain = DomainToUse };    // cookie acesso
            yield return new LoginPadraoAspNet.CookieSimpleInfo { CookieName = "TRACKER_CONTROLLER_SESSION_ID", CookieDomain = DomainToUse };    // cookie track
        }
    }
}
