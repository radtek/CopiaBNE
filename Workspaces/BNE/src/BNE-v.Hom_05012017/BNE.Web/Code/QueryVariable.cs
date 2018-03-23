using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using Telerik.Web.UI.Editor;

namespace BNE.Web.Code
{
    public class QueryVariable<T> where T : class, new()
    {
        #region [ Const ]
        private const string TheRequestIsNoLongerAvailable = "'request' (the request from HttpContext.Current is no longer available).";
        #endregion

        #region [ Fields ]
        private bool _hasValue;
        private bool _isEvaluated;
        #endregion

        #region [ Private Properties ]
        private TaskCompletionSource<T> _result;

        private HttpRequest Request
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null)
                    return null;

                return context.Request;
            }
        }
        #endregion

        #region [ Public Properties ]
        public bool HasValue
        {
            get
            {
                if (_isEvaluated)
                    return _hasValue;

                Do();

                return _hasValue;
            }
        }

        public T Value
        {
            get
            {
                if (_isEvaluated)
                    return _result.Task.Result;

                Do();

                return _result.Task.Result;
            }
        }

        public T ValueOrDefault
        {
            get
            {
                if (_isEvaluated)
                {
                    if (_result.Task.IsFaulted
                        || _result.Task.IsCanceled)
                    {
                        return default(T);
                    }
                    return _result.Task.Result;
                }

                Do();

                if (_result.Task.IsFaulted
                    || _result.Task.IsCanceled)
                {
                    return default(T);
                }
                return _result.Task.Result;
            }
        }
        #endregion

        #region [ Constructor ]
        public QueryVariable()
        {
        }
        #endregion

        #region [ Public ]
        public string GetRequestQueryString()
        {
            var request = GetCurrentRequest();

            return request.Url.Query;
        }

        public NameValueCollection GetRequestCollection()
        {
            var request = GetCurrentRequest();

            return request.QueryString;
        }
        #endregion

        #region [ Private / Protected ]
        protected void Do()
        {
            Evaluate();
            _isEvaluated = true;
        }

        private HttpRequest GetCurrentRequest(bool throwException = true)
        {
            var request = Request;
            if (request == null)
            {
                if (throwException)
                    throw new NullReferenceException(TheRequestIsNoLongerAvailable);
            }
            return request;
        }

        protected void Evaluate()
        {
            var request = GetCurrentRequest(false);

            if (request == null)
            {
                _hasValue = false;
                var ts = new TaskCompletionSource<T>();
                ts.TrySetException(new NullReferenceException(TheRequestIsNoLongerAvailable));
                this._result = ts;
                return;
            }

            try
            {
                T result;
                if (GetFromQueryString(request.QueryString, out result))
                {
                    var ts = new TaskCompletionSource<T>();
                    ts.TrySetResult(result);
                    _hasValue = true;
                    this._result = ts;
                }
                else
                {
                    var ts = new TaskCompletionSource<T>();
                    ts.TrySetResult(null);
                    _hasValue = false;
                    this._result = ts;
                }
            }
            catch (Exception exception)
            {
                _hasValue = false;
                var ts = new TaskCompletionSource<T>();
                ts.TrySetException(exception);
                this._result = ts;
            }
        }
        #endregion

        #region [ Private Static ]
        private static bool GetFromQueryString(NameValueCollection queryString, out T obj)
        {
            obj = new T();
            bool hasValues = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var valueAsString = queryString[property.Name] ??
                                    queryString.AllKeys.FirstOrDefault(
                                        a => a.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                var value = Parse(valueAsString, property.PropertyType);

                if (value == null || !property.CanWrite)
                    continue;

                property.SetValue(obj, value, null);
                hasValues = true;
            }
            return hasValues;
        }

        private static object Parse(string valueAsString, Type propertyType)
        {
            return Convert.ChangeType(valueAsString, propertyType);
        }
        #endregion
    }
}