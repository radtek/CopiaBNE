using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace BNE.Auth
{

    public sealed class SessionAbandonRestoreMediator : IDisposable
    {
        private SessionAbandonRestoreMediator()
        {

        }

        private static SessionAbandonRestoreMediator _instance;
        private static object _synchronization = new object();
        private static bool _initializeSingleton;
        public static SessionAbandonRestoreMediator Instance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _instance, ref _initializeSingleton, ref _synchronization, () => new SessionAbandonRestoreMediator());
            }
        }

        private EventHandler<SimpleSessionEventArgs> _sessionStarted;
        private EventHandler<SimpleSessionEventArgs> _sessionEnd;

        private IDisposable _disposed;
        private bool _iniciliazedSubscription;

        public void Start()
        {
            LazyInitializer.EnsureInitialized(ref _disposed, ref _iniciliazedSubscription, ref _synchronization, () =>
                Task.Factory.StartNew(() => // just to move to another synchronization context
                    {
                        var inSx = Observable.FromEventPattern<SimpleSessionEventArgs>(add => _sessionStarted += add, rem => _sessionStarted += rem).Publish().RefCount();
                        var outSx = Observable.FromEventPattern<SimpleSessionEventArgs>(add => _sessionEnd += add, rem => _sessionEnd += rem);

                        var disp = outSx.Select(a => Observable.Amb(GetTimeoutSessionAbandon().Select(b => (HttpSessionStateBase)null),
                                                                    inSx.FirstOrDefaultAsync(b => b.EventArgs.Session.SessionID == a.EventArgs.Session.SessionID).Select(b => b.EventArgs.Session)).Take(1)
                                                                .Where(b => b != null)
                                                                .Select(b => Tuple.Create(a.EventArgs.Session, b)))
                              .Merge()
                              .Do(a => RepopulateSession(a.Item1, a.Item2))
                              .Subscribe();

                        return disp;
                    }).Result);
        }

        private void RepopulateSession(HttpSessionStateBase oldSession, HttpSessionStateBase newSession)
        {
            for (int i = 0; i < oldSession.Count; i++)
            {
                newSession.Add(oldSession.Keys[i], oldSession[i]);
            }
            newSession.Remove("SessionEndManually");
        }

        private static IObservable<long> GetTimeoutSessionAbandon()
        {
            // just to move to another synchronization context
            return Task.Factory.StartNew(() => (IObservable<long>)Observable.Timer(TimeSpan.FromSeconds(30), Scheduler.Default)).Result;
        }

        public void RaiseSessionStarted(HttpSessionStateBase session)
        {
            OnSessionStart(new SimpleSessionEventArgs { Session = session });
        }

        public void RaiseSessionCloseManually(HttpSessionStateBase session)
        {
            session["SessionEndManually"] = true;
            OnSessionEnd(new SimpleSessionEventArgs { Session = session });
        }

        private void OnSessionStart(SimpleSessionEventArgs args)
        {
            var handlers = _sessionStarted;
            if (handlers != null)
            {
                handlers(this, args);
            }
        }

        private void OnSessionEnd(SimpleSessionEventArgs args)
        {
            var handlers = _sessionEnd;
            if (handlers != null)
            {
                handlers(this, args);
            }
        }

        public void Dispose()
        {
            if (_disposed != null)
            {
                try
                {
                    _disposed.Dispose();
                }
                catch
                {

                }
            }
        }
    }
}
