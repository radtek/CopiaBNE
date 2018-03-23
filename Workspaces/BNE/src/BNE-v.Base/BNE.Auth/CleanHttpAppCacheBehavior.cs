using BNE.Auth.HttpModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Auth
{
    public class CleanDefaultHttpAppCacheBehavior : BNE.Auth.LoginBehaviorBase
    {
        private IEnumerable<string> _cacheKeyToRemove;

        public CleanDefaultHttpAppCacheBehavior()
        {
            this._cacheKeyToRemove = Enumerable.Empty<string>();
        }
      
        public CleanDefaultHttpAppCacheBehavior(params string[] cacheKeyToRemove)
        {
            this._cacheKeyToRemove = cacheKeyToRemove ?? Enumerable.Empty<string>();
        }

        public CleanDefaultHttpAppCacheBehavior(IEnumerable<string> cacheKeyToRemove)
        {
            this._cacheKeyToRemove = cacheKeyToRemove ?? Enumerable.Empty<string>();
        }

        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> context, LoginBehaviorBase.LogOffInfo info)
        {
            var current = context();

            if (current.Cache == null)
                return;

            foreach (var item in _cacheKeyToRemove)
            {
                current.Cache.Remove(item);
            }

            if (current.Session == null)
                return;

            current.Cache.Remove(typeof(SessionTimeoutModule).Name + "|" + current.Session.SessionID);
        }
    }
}
