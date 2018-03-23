using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Auth.Core;
using BNE.Auth.Core.ClaimTypes;
using BNE.Auth.Core.Enumeradores;
using BNE.Auth.Core.Interface;
using Microsoft.IdentityModel.Claims;

namespace BNE.Auth
{
    public static class BNEAutenticacao
    {
        static BNEAutenticacao()
        {
            DefaultAuthManagerFactory = BuiltDefaultAuthManager;
        }

        public static Func<Func<HttpContextBase>, LoginBehaviorBase, ILoginPadraoAspNet> DefaultAuthManagerFactory { get; set; }

        private static ILoginPadraoAspNet BuiltDefaultAuthManager(Func<HttpContextBase> f, LoginBehaviorBase b)
        {
            return f == null ? new LoginPadraoAspNet(b) : new LoginPadraoAspNet(f, b);
        }


#if DEBUG
        public const string DomainToUse = null;
#else
        public const string DomainToUse = ".bne.com.br";
#endif

        public static ClaimsIdentity LogarCPF(string name, int pfId, decimal cpf, DateTime dataNascimento, bool lembrarUsuario = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim(BNEClaimTypes.CPF, cpf.ToString()), 
                new Claim(BNEClaimTypes.PessoaFisicaId, pfId.ToString()), 
                new Claim(BNEClaimTypes.DataNascimento, dataNascimento.Date.ToShortDateString()), 
                new Claim(BNEClaimTypes.PerfilUsuario, AuthPerfilType.NenhumOuDesconhecido.ToString())
            };
            var identity = new ClaimsIdentity(claims);

            using (var l = DefaultAuthManagerFactory(null, new EventBehavior(LogoffType.NONE)))
            {
                l.Logar(identity, lembrarUsuario);
            }

            return identity;
        }

        public static ClaimsIdentity LogarCandidato(string name, int pfId, decimal cpf, DateTime dataNascimento, int curriculoId, bool lembrarUsuario = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name), 
                new Claim(BNEClaimTypes.CPF, cpf.ToString()), 
                new Claim(BNEClaimTypes.CurriculoId, curriculoId.ToString()), 
                new Claim(BNEClaimTypes.PessoaFisicaId, pfId.ToString()), 
                new Claim(BNEClaimTypes.DataNascimento, dataNascimento.Date.ToShortDateString()), 
                new Claim(BNEClaimTypes.PerfilUsuario, AuthPerfilType.Candidato.ToString())
            };
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
        public static void DeslogarWithoutAuth(HttpContext context)
        {
            var behavior = new MultipleAuthBehavior();

            using (var l = DefaultAuthManagerFactory(() => new HttpContextWrapper(context), behavior))
            {
                l.Deslogar(false);
            }
        }
        public static void DeslogarWithoutAuthClearSession(HttpContext context)
        {
            var behavior = new MultipleAuthBehavior();

            using (var l = DefaultAuthManagerFactory(() => new HttpContextWrapper(context), behavior))
            {
                l.Deslogar(true, true);
            }
        }
        #endregion

        #region [ Deslogar Padrao ]
        public static void DeslogarPadrao(LogoffType logoffType = LogoffType.BY_USER, params string[] outrosCookiesParaLimpar)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(
                                    (outrosCookiesParaLimpar ?? new string[0])
                                        .Select(a => new CookieSimpleInfo { CookieName = a, CookieDomain = DomainToUse }));

            LogoutDefault(logoffType, otherCookies);
        }

        public static void DeslogarPadrao(IEnumerable<CookieSimpleInfo> outrosCookiesParaLimpar, LogoffType logoffType = LogoffType.BY_USER)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(outrosCookiesParaLimpar ?? new CookieSimpleInfo[0]);

            LogoutDefault(logoffType, otherCookies);
        }

        #endregion

        #region [ Deslogar Pagamento ]
        public static void DeslogarPagamento(LogoffType logoffType = LogoffType.BY_USER, params string[] outrosCookiesParaLimpar)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(
                                    (outrosCookiesParaLimpar ?? new string[0])
                                        .Select(a => new CookieSimpleInfo { CookieName = a, CookieDomain = DomainToUse }));

            LogoutPayment(logoffType, otherCookies);
        }

        public static void DeslogarPagamento(IEnumerable<CookieSimpleInfo> outrosCookiesParaLimpar, LogoffType logoffType = LogoffType.BY_USER)
        {
            var otherCookies = CookiesPadraoParaLimpar().Concat(outrosCookiesParaLimpar ?? new CookieSimpleInfo[0]);

            LogoutPayment(logoffType, otherCookies);
        }
        #endregion

        private static void LogoutDefault(LogoffType logoffType, IEnumerable<CookieSimpleInfo> otherCookies)
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

        private static void LogoutPayment(LogoffType logoffType, IEnumerable<CookieSimpleInfo> otherCookies)
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

        #region User
        public static User User()
        {
            var behavior = new MultipleAuthBehavior();
            using (var l = DefaultAuthManagerFactory(null, behavior))
            {
                return (User)l.AuthenticatedUser();
            }
        }
        #endregion

        private static IEnumerable<CookieSimpleInfo> CookiesPadraoParaLimpar()
        {
            yield return new CookieSimpleInfo { CookieName = "BNE", CookieDomain = DomainToUse };    // cookie vagas
            yield return new CookieSimpleInfo { CookieName = "BNE_Acesso", CookieDomain = DomainToUse };    // cookie acesso
            yield return new CookieSimpleInfo { CookieName = "TRACKER_CONTROLLER_SESSION_ID", CookieDomain = DomainToUse };    // cookie track
        }
    }
}
