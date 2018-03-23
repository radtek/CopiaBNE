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
    public class SessionVariable<T>
    {
        private readonly string _key;
        private readonly Func<T> _initializer;

        #region SessionVariable
        /// <summary>
        /// Initializes a new session variable.
        /// </summary>
        /// <param name='key'>
        /// The key to use for storing the value in the session.
        /// </param>
        public SessionVariable(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            _key = GetType() + key;
        }
        /// <summary>
        /// Initializes a new session variable with a initializer.
        /// </summary>
        /// <param name='key'>
        /// The key to use for storing the value in the session.
        /// </param>
        /// <param name='initializer'>
        /// A function that is called in order to create a
        /// default value per session.
        /// </param>
        public SessionVariable(string key, Func<T> initializer) : this(key)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            _initializer = initializer;
        }
        #endregion

        #region GetInternalValue
        private object GetInternalValue(bool initializeIfNessesary)
        {
            HttpSessionState session = CurrentSession;

            var value = session[_key];

            if (value == null && initializeIfNessesary
              && _initializer != null)
                session[_key] = value = _initializer();

            return value;
        }
        #endregion

        #region CurrentSession
        private HttpSessionState CurrentSession
        {
            get
            {
                var current = HttpContext.Current;

                if (current == null)
                    throw new InvalidOperationException("No HttpContext is not available.");

                var session = current.Session;
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
                    throw new InvalidOperationException("The session does not contain any value for ‘"+ _key + "‘.");

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