using BNE.Auth.HttpModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BNE.Auth
{
    public class AuthEventAggregator
    {
        private static AuthEventAggregator _instance;
        private static object _sync;
        private static bool _initialized;

        public static AuthEventAggregator Instance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _instance, ref _initialized, ref _sync, () => new AuthEventAggregator());
            }
        }

        private AuthEventAggregator()
        {

        }

        private EventHandler<BNEAuthStartSessionControlArgs> _newSessionWithAuthenticatedLogin;
        public event EventHandler<BNEAuthStartSessionControlArgs> NewSessionWithAutheticatedLogin
        {
            add
            {
                lock (_sync)
                {
                    var cur = _newSessionWithAuthenticatedLogin;
                    if (cur == null)
                    {
                        _newSessionWithAuthenticatedLogin += value;
                        return;
                    }

                    if (cur.GetInvocationList().Any(a => (EventHandler<BNEAuthStartSessionControlArgs>)a == value))
                        return;

                    _newSessionWithAuthenticatedLogin += value;
                }

            }
            remove
            {
                lock (_sync)
                {
                    _newSessionWithAuthenticatedLogin -= value;
                }
            }

        }


        internal void OnNewSessionWithAutheticatedUser(object sender, BNEAuthStartSessionControlArgs args)
        {
            var handler = _newSessionWithAuthenticatedLogin;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        private EventHandler<BNEAuthEventArgs> _userAuthenticatedWithDifferentSession;
        public event EventHandler<BNEAuthEventArgs> UserAuthenticatedWithDifferentSession
        {
            add
            {
                lock (_sync)
                {
                    var cur = _userAuthenticatedWithDifferentSession;
                    if (cur == null)
                    {
                        _userAuthenticatedWithDifferentSession += value;
                        return;
                    }

                    if (cur.GetInvocationList().Any(a => (EventHandler<BNEAuthEventArgs>)a == value))
                        return;

                    _userAuthenticatedWithDifferentSession += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _userAuthenticatedWithDifferentSession -= value;
                }
            }
        }

        internal void OnUserAuthenticatedWithDifferentSession(object sender, BNEAuthEventArgs args)
        {
            var handler = _userAuthenticatedWithDifferentSession;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        private EventHandler<BNEAuthEventArgs> _sessionEndByTimeoutWithUserAuth;
        public event EventHandler<BNEAuthEventArgs> SessionEndByTimeoutWithUserAuth
        {
            add
            {
                lock (_sync)
                {
                    var cur = _sessionEndByTimeoutWithUserAuth;
                    if (cur == null)
                    {
                        _sessionEndByTimeoutWithUserAuth += value;
                        return;
                    }

                    if (cur.GetInvocationList().Any(a => (EventHandler<BNEAuthEventArgs>)a == value))
                        return;

                    _sessionEndByTimeoutWithUserAuth += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _sessionEndByTimeoutWithUserAuth -= value;
                }
            }
        }


        internal void OnSessionEndByTimeoutWithUserAuth(object sender, BNEAuthEventArgs args)
        {
            var handler = _sessionEndByTimeoutWithUserAuth;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        private EventHandler<BNELogoffAuthControlEventArgs> _closingManuallySessionWithUserAuth;
        public event EventHandler<BNELogoffAuthControlEventArgs> ClosingManuallySessionWithUserAuth
        {
            add
            {
                lock (_sync)
                {
                    var cur = _closingManuallySessionWithUserAuth;
                    if (cur == null)
                    {
                        _closingManuallySessionWithUserAuth += value;
                        return;
                    }

                    if (cur.GetInvocationList().Any(a => (EventHandler<BNELogoffAuthControlEventArgs>)a == value))
                        return;

                    _closingManuallySessionWithUserAuth += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _closingManuallySessionWithUserAuth -= value;
                }
            }
        }


        public void OnClosingManuallySessionWithUserAuth(object sender, BNELogoffAuthControlEventArgs args)
        {
            var handler = _closingManuallySessionWithUserAuth;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        private EventHandler<BNEAuthLoginControlEventArgs> _userEnterAuthSuccessfully;
        public event EventHandler<BNEAuthLoginControlEventArgs> UserEnterAuthSuccessfully
        {
            add
            {
                lock (_sync)
                {
                    var cur = _userEnterAuthSuccessfully;
                    if (cur == null)
                    {
                        _userEnterAuthSuccessfully += value;
                        return;
                    }

                    if (cur.GetInvocationList().Any(a => (EventHandler<BNEAuthLoginControlEventArgs>)a == value))
                        return;

                    _userEnterAuthSuccessfully += value;
                }
            }
            remove
            {
                lock (_sync)
                {
                    _userEnterAuthSuccessfully -= value;
                }
            }
        }


        public void OnUserEnterSuccessfully(object sender, BNEAuthLoginControlEventArgs args)
        {
            var handler = _userEnterAuthSuccessfully;
            if (handler != null)
            {
                handler(sender, args);
            }
        }


    }

}
