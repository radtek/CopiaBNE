using System;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.SessionState;
using BNE.Auth.EventArgs;

namespace BNE.Auth.HttpModules
{
    public class SessionTimeoutModule : IHttpModule
    {
        private object _inicialization;
        private object _syncRoot = new object();
        private bool _inicitialized;
        private readonly Lazy<TimeSpan> _getTimeoutSession = new Lazy<TimeSpan>(GetTimeoutInAppConfig);

        public void Init(HttpApplication context)
        {
            LazyInitializer.EnsureInitialized(ref _inicialization, ref _inicitialized, ref _syncRoot, () =>
          {
              OnStart(context);
              return new object();
          });

        }

        public void Dispose()
        {

        }

        private void OnStart(HttpApplication context)
        {
            context.PreRequestHandlerExecute += context_PreRequestHandlerExecute;
        }

        private void context_PreRequestHandlerExecute(object sender, System.EventArgs e)
        {
            var app = (HttpApplication)sender;
            if (app.Context.Handler is IReadOnlySessionState || app.Context.Handler is IRequiresSessionState)
            {
                if (!PreConditions(app.Context))
                    return;

                PostRequestWithAvailableSession(app);
            }
        }

        private void PostRequestWithAvailableSession(HttpApplication app)
        {
            InsertSessionItemToCache(app.Session);
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

        private void InsertSessionItemToCache(HttpSessionState session)
        {
            HttpContext.Current.Cache.Insert(typeof(SessionTimeoutModule).Name + "|" + session.SessionID,
                                                    session,
                                                    null,
                                                    DateTime.MaxValue,
                                                    GetSessionTimeout(),
                                                    CacheItemPriority.NotRemovable,
                                                    OnCacheItemRemoved);
        }

        private static TimeSpan GetTimeoutInAppConfig()
        {
            try
            {
                var sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                return sessionSection.Timeout;
            }
            catch
            {
                return FormsAuthentication.Timeout;
            }

        }

        private TimeSpan GetSessionTimeout()
        {
            return _getTimeoutSession.Value;
        }

        private void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            if (reason != CacheItemRemovedReason.Expired)
                return;

            AuthEventAggregator.Instance.OnSessionEndByTimeoutWithUserAuth(this, new BNEAuthEventArgs((HttpSessionState)value));
        }

    }
}
