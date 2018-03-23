using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public abstract class DefFilterInit<TArg, TRes> : IDefFilter
    {
        public abstract bool SafeConversion { get; }

        private Func<TArg, TRes> _func;
        protected Func<TArg, TRes> Func
        {
            get { return _func; }
        }

        private Func<string> _targetPropertyAccessor;

        public string TargetProperty
        {
            get { return _targetPropertyAccessor(); }
            set { _targetPropertyAccessor = () => value ?? string.Empty; }
        }


        protected DefFilterInit()
        {

        }

        public DefFilterInit(Func<TArg, TRes> func)
        {
            this._func = func;
            this._targetPropertyAccessor = () => string.Empty;
        }
        public DefFilterInit(string prop)
        {
            this._targetPropertyAccessor = () => string.Empty;
        }
        public DefFilterInit(string prop, Func<TArg, TRes> func)
        {
            this._targetPropertyAccessor = () => prop;
            this._func = func;
        }

        public virtual TRes DoFilter(TArg value)
        {
            return Func(value);
        }

        protected abstract TArg ConvertArgBeforeFilter(object value);

        public virtual object DoFilter(object value)
        {
            return DoFilter(ConvertArgBeforeFilter(value));
        }

        protected DefFilterInit<TArg, TRes> Override(Func<TArg, TRes> handler)
        {
            _func = handler;
            return this;
        }

        public DefFilterInit<TArg, TRes> For(string propName)
        {
            TargetProperty = propName;
            return this;
        }

        public DefFilterInit<TArg, TRes> For(Func<string> accessorPropName)
        {
            _targetPropertyAccessor = accessorPropName;
            return this;
        }

        internal virtual DefFilterInit<TArg, TRes> CopyWith(Func<TArg, TRes> otherEvaluator)
        {
            var copy = (DefFilterInit<TArg, TRes>)Activator.CreateInstance(this.GetType(), otherEvaluator ?? _func);
            copy._targetPropertyAccessor = _targetPropertyAccessor;
            return copy;
        }
    }

}
