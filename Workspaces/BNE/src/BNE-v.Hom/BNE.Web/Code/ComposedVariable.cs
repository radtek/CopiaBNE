using System.Web;
using BNE.Common.Session;

namespace BNE.Web.Code
{
    public class ComposedVariable<T> where T : class, new()
    {
        #region [ Const ]
        public enum PriorityType
        {
            Request,
            Session
        }
        #endregion

        #region [ Fields / Attributes ]
        private readonly string _sessionKey;
        private readonly PriorityType _priorityType;

        private QueryVariable<T> _queryVariable;
        private SessionVariable<T> _sessionVariable;
        #endregion

        #region [ Constructor ]
        public ComposedVariable(string sessionKey)
        {
            this._sessionKey = sessionKey;
        }

        public ComposedVariable(PriorityType priorityType, string sessionKey)
        {
            this._priorityType = priorityType;
            this._sessionKey = sessionKey;
        }
        #endregion

        #region [ Properties ]

        public T ValueOrDefault
        {
            get { return GetValueOrDefault(); }
        }

        public T Value
        {
            get { return GetValue(); }
        }

        public bool HasValue
        {
            get { return GetHasValue(); }
        }

        public QueryVariable<T> QueryVariable
        {
            get
            {
                return _queryVariable ??
                       (_queryVariable =
                           new QueryVariable<T>());
            }
        }

        public SessionVariable<T> SessionVariable
        {
            get { return _sessionVariable ?? (_sessionVariable = new SessionVariable<T>(_sessionKey)); }
        }
        #endregion

        private T GetValue()
        {
            if (_priorityType == PriorityType.Request)
            {
                if (QueryVariable.HasValue)
                {
                    return QueryVariable.Value;
                }

                return SessionVariable.Value;
            }
            if (SessionVariable.HasValue)
            {
                return SessionVariable.Value;
            }

            return QueryVariable.Value;
        }

        private bool GetHasValue()
        {
            return QueryVariable.HasValue || SessionVariable.HasValue;
        }

        private T GetValueOrDefault()
        {
            if (_priorityType == PriorityType.Request)
            {
                if (QueryVariable.HasValue)
                    return QueryVariable.Value;

                if (SessionVariable.HasValue)
                    return SessionVariable.Value;

                return QueryVariable.ValueOrDefault;
            }

            if (SessionVariable.HasValue)
                return SessionVariable.Value;

            if (QueryVariable.HasValue)
                return QueryVariable.Value;

            return SessionVariable.ValueOrDefault;
        }

        public void Clear()
        {
            if (SessionVariable.HasValue)
            {
                SessionVariable.Clear();
            }
        }
    }
}