using System;
using System.Web;
using System.Web.SessionState;

namespace BNE.Common.Session
{

    /// <summary>
    /// Wrapper class for <see cref='HttpContext.Session'/>.
    /// Author: Lars Corneliussen
    /// Source: http://startbigthinksmall.wordpress.com/2008/05/14/how-to-wrap-the-aspnet-session-state/
    /// </summary>
    /// <typeparam name="T">
    /// The type of the value to be stored.
    /// </typeparam>
    public sealed class WrapperSessionVariable<T>
    {
        private readonly string _key;
        private readonly Func<T> _initializer;
        private readonly HttpContextBase _contextWrapped;
        private readonly HttpSessionStateBase _sessionWrapped;

        #region SessionVariable
        public WrapperSessionVariable(HttpSessionStateBase wrapSession, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (wrapSession == null)
                throw new NullReferenceException("wrapSession");

            _key = typeof(SessionVariable<T>) + key;
            _sessionWrapped = wrapSession;
        }

        public WrapperSessionVariable(HttpContextBase wrapContext, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (wrapContext == null)
                throw new NullReferenceException("wrapContext");

            _key = typeof(SessionVariable<T>) + key;
            _contextWrapped = wrapContext;
        }

        public WrapperSessionVariable(HttpSessionStateBase wrapSession, string key, Func<T> initializer)
            : this(wrapSession, key)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (wrapSession == null)
                throw new NullReferenceException("wrapContext");

            _initializer = initializer;
            _key = typeof(SessionVariable<T>) + key;
            _sessionWrapped = wrapSession;
        }

        public WrapperSessionVariable(HttpContextBase wrapContext, string key, Func<T> initializer)
            : this(wrapContext, key)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (wrapContext == null)
                throw new NullReferenceException("wrapContext");

            _initializer = initializer;
            _key = typeof(SessionVariable<T>) + key;
            _contextWrapped = wrapContext;
        }
        #endregion

        #region GetInternalValue
        private object GetInternalValue(bool initializeIfNessesary)
        {
            var session = CurrentSession;

            var value = session[_key];

            if (value == null && initializeIfNessesary
              && _initializer != null)
                session[_key] = value = _initializer();

            return value;
        }
        #endregion

        #region CurrentSession
        private HttpSessionStateBase CurrentSession
        {
            get
            {
                if (_sessionWrapped != null)
                    return _sessionWrapped;

                HttpSessionStateBase session;
                if (_contextWrapped == null)
                {
                    var current = HttpContext.Current;

                    if (current == null)
                        throw new InvalidOperationException("No HttpContext is not available.");
                    else
                        session = new HttpSessionStateWrapper(current.Session);
                }
                else
                {
                    session = _contextWrapped.Session;
                }

                if (session == null)
                    throw new InvalidOperationException("No Session available on current HttpContext.");

                return session;
            }
        }
        #endregion

        #region HasValue
        /// <summary>
        /// Indicates wether there is a value present or not.
        /// </summary>
        public bool HasValue
        {
            get { return GetInternalValue(true) != null; }
        }
        #endregion

        #region Value
        /// <summary>
        /// Sets or gets the value in the current session.
        /// </summary>
        /// <exception cref='InvalidOperationException'>
        /// If you try to get a value while none is set.
        /// Use <see cref='ValueOrDefault'/> for safe access.
        /// </exception>
        public T Value
        {
            get
            {
                object v = GetInternalValue(true);

                if (v == null)
                    throw new InvalidOperationException("The session does not contain any value for ‘" + _key + "‘.");

                return (T)v;
            }
            set { CurrentSession[_key] = value; }
        }
        #endregion

        #region ValueOrDefault
        /// <summary>
        /// Gets the value in the current session or if
        /// none is available <c>default(T)</c>.
        /// </summary>
        public T ValueOrDefault
        {
            get
            {
                object v = GetInternalValue(true);

                if (v == null)
                    return default(T);

                return (T)v;
            }
        }
        #endregion

        #region Clear
        /// <summary>
        /// Clears the value in the current session.
        /// </summary>
        public void Clear()
        {
            CurrentSession.Remove(_key);
        }
        #endregion

    }

}