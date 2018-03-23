using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Chat.Helper
{
    public class SetValueOrDefaultFact<TActor, TValue> : SetValueOrDefault<TValue>
    {
        private readonly TActor _actor;
        private readonly Lazy<TValue> _defaultValue;

        public SetValueOrDefaultFact(TActor actor, Func<TActor, TValue> defaultValueFactory)
        {
            if (defaultValueFactory == null)
                throw new NullReferenceException("defaultValueFactory");

            if ((object)actor == null)
                throw new NullReferenceException("actor");

            this._actor = actor;
            this._defaultValue = new Lazy<TValue>(() => defaultValueFactory(Actor));
        }

        public TActor Actor
        {
            get { return _actor; }
        }

        public override TValue Value
        {
            get
            {
                if (!IsSet)
                    return this._defaultValue.Value;

                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }

    public class SetValueOrDefaultLazy<T> : SetValueOrDefault<T>
    {
        private readonly Lazy<T> _defaultValue;
        public SetValueOrDefaultLazy(Func<T> defaultValueAccessor)
        {
            if (defaultValueAccessor == null)
                throw new NullReferenceException("defaultValueAccessor");

            this._defaultValue = new Lazy<T>(defaultValueAccessor);
        }

        public override T Value
        {
            get
            {
                if (!IsSet)
                    return this._defaultValue.Value;

                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }

    public class SetValueOrDefault<T>
    {
        public SetValueOrDefault()
        {

        }

        public SetValueOrDefault(T defaultValue)
        {
            _value = defaultValue;
        }

        private T _value;

        public virtual T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                IsSet = true;
            }
        }

        public bool IsSet { get; protected set; }

    }

    public static class SetValueOrDefaultExtension
    {
        public static SetValueOrDefaultFact<TActor, TValue> Wrap<TActor, TValue>(this TActor actor,
                                                                                   Func<TActor, TValue> accessor)
        {
            return new SetValueOrDefaultFact<TActor, TValue>(actor, accessor);
        }

    }
}
