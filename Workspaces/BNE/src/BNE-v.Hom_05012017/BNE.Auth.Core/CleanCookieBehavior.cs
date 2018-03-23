using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Auth.Core
{
    public class CleanCookieBehavior : LoginBehaviorBase
    {
        private readonly IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> _cookiesToClean;

        public CleanCookieBehavior(IEnumerable<LoginPadraoAspNet.CookieSimpleInfo> cookiesToClean)
        {
            this._cookiesToClean = cookiesToClean;
        }

        public CleanCookieBehavior(params LoginPadraoAspNet.CookieSimpleInfo[] cookiesToClean)
        {
            this._cookiesToClean = cookiesToClean;
        }

        protected override void OnBeforeLogoffSafe(Func<HttpContextBase> contextAccessor, LogOffInfo info)
        {
            var context = contextAccessor();

            if (context == null)
                return;

            foreach (var item in _cookiesToClean ?? Enumerable.Empty<LoginPadraoAspNet.CookieSimpleInfo>())
            {
                HttpCookie cookie;
                if (item.CookieDomain == null)
                    cookie = new HttpCookie(item.CookieName, string.Empty) { Expires = DateTime.Now.AddDays(-1) };
                else
                    cookie = new HttpCookie(item.CookieName, string.Empty) { Expires = DateTime.Now.AddDays(-1), Domain = item.CookieDomain };

                context.Response.Cookies.Add(cookie);
            }

        }
    }
}
