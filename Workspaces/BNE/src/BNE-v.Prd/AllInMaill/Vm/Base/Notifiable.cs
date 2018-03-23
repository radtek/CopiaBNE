using AllInMail.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Reactive;
using System.Reactive.Linq;

namespace AllInMail.Base.Vm
{
    [Serializable]
    public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs, INotifyValue
    {
        private readonly object oldValue;
        private readonly object newValue;

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        object INotifyValue.OldValue
        {
            get { return oldValue; }
        }

        object INotifyValue.NewValue
        {
            get { return newValue; }
        }

        bool INotifyValue.WasDefiniedValues
        {
            get { return true; }
        }

        public T OldValue
        {
            get
            {
                return (T)oldValue;
            }
        }

        public T NewValue
        {
            get
            {
                return (T)newValue;
            }
        }

    }

    [Serializable]
    public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs, INotifyValue
    {
        public object OldValue { get; protected set; }
        public object NewValue { get; protected set; }
        public bool WasDefiniedValues { get; protected set; }

        public PropertyChangedExtendedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
            WasDefiniedValues = true;
        }

        public PropertyChangedExtendedEventArgs(string propertyName)
            : base(propertyName)
        {
            WasDefiniedValues = false;
        }

    }

    [Serializable]
    public class TArgs<T> : BasicTArgs, ITArgs<T>
    {
        public TArgs(T args)
            : base(args)
        {

        }

        public T Value
        {
            get
            {
                return (T)this.RealValue;
            }
        }
    }

    [Serializable]
    public class BasicTArgs : EventArgs, ITArgs
    {
        protected readonly object RealValue;
        public BasicTArgs(object value)
        {
            this.RealValue = value;
        }

        object ITArgs.Value
        {
            get { return RealValue; }
        }

    }

    public interface INotifyValue
    {
        object OldValue { get; }
        object NewValue { get; }
        string PropertyName { get; }
        bool WasDefiniedValues { get; }
    }

    public interface INotifyPropertyChangedExtended : INotifyPropertyChanged
    {
        event EventHandler<TArgs<INotifyValue>> PropertyChangedExtended;
    }

    public interface ITArgs
    {
        object Value { get; }
    }

    public interface ITArgs<out T>
    {
        T Value { get; }
    }

    [Serializable]
    public class ThottleNotifiable : Notifiable
    {
        private static readonly EventLoopScheduler _scheduler = new EventLoopScheduler();
        private TimeSpan _sampleTime = TimeSpan.FromMilliseconds(1000);

        [Browsable(false)]
        public TimeSpan TimeUpdateView
        {
            get { return _sampleTime; }
            set { _sampleTime = value; }
        }

        private Subject<string> _regulatePropertyChanged = new Subject<string>();

        public ThottleNotifiable()
        {
            _regulatePropertyChanged.GroupBy(a => a).SelectMany(a => a.Sample(_sampleTime, _scheduler)).Do(RaisePropertyChanged).Subscribe();
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            _regulatePropertyChanged.OnNext(propertyName);
        }

        private void RaisePropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
        protected override void OnPropertyChangedExt<T>(string propertyName, T oldValue, T newValue)
        {
            throw new NotImplementedException("OnPropertyChangedExt");
        }

    }

    [Serializable]
    public abstract class Notifiable : INotifyPropertyChangedExtended
    {
        private PropertyChangedEventHandler notificationEvents;
        private EventHandler<TArgs<INotifyValue>> extendedEvents;
        private SynchronizationContext _context;

        public void SetNotificationContext(SynchronizationContext context)
        {
            if (context == null)
                throw new NullReferenceException("context");

            this._context = context;
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, T oldValue, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return;

            var propertyName = GetPropertyName(propertyExpression);

            OnPropertyChangedExt<T>(propertyName, oldValue, newValue);
        }

        protected virtual void OnPropertyChangedExt<T>(string propertyName, T oldValue, T newValue)
        {
            SendOrPostCallback cb = (a) =>
            {

                var extNotifications = GetExtendedPropertyChangedHandlers();
                if (extNotifications != null)
                {
                    extNotifications(this, new TArgs<INotifyValue>(new PropertyChangedExtendedEventArgs<T>(propertyName, oldValue, newValue)));
                }

                var notifications = GetPropertyChangedHandlers();
                if (notifications != null)
                {
                    notifications(this, new PropertyChangedEventArgs(propertyName));
                }
            };

            if (_context != null && SynchronizationContext.Current != _context)
            {
                _context.Post(cb, null);
            }
            else
            {
                cb(null);
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);

            OnPropertyChanged(propertyName);

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            SendOrPostCallback cb = (a) =>
                {
                    var extNotifications = GetExtendedPropertyChangedHandlers();
                    if (extNotifications != null)
                    {
                        extNotifications(this, new TArgs<INotifyValue>(new PropertyChangedExtendedEventArgs(propertyName)));
                    }

                    var notifications = GetPropertyChangedHandlers();
                    if (notifications != null)
                    {
                        notifications(this, new PropertyChangedEventArgs(propertyName));
                    }
                };

            if (_context != null && SynchronizationContext.Current != _context)
            {
                _context.Post(cb, null);
            }
            else
            {
                cb(null);
            }

        }

        protected string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            return ExtGen.GetLastNodeName(expression);
        }

        protected virtual void RemoveHandlers()
        {
            extendedEvents = null;
            notificationEvents = null;
        }

        protected virtual EventHandler<TArgs<INotifyValue>> GetExtendedPropertyChangedHandlers()
        {
            var handlers = extendedEvents;
            return handlers;
        }

        protected virtual PropertyChangedEventHandler GetPropertyChangedHandlers()
        {
            var handlers = notificationEvents;
            return handlers;
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                if (notificationEvents == null ||
                    notificationEvents.GetInvocationList()
                                        .All(obj => (PropertyChangedEventHandler)obj != value))
                    notificationEvents += value;
            }
            remove
            {
                if (notificationEvents != null)
                    notificationEvents -= value;
            }
        }

        public event EventHandler<TArgs<INotifyValue>> PropertyChangedExtended
        {
            add
            {
                if (extendedEvents == null ||
                    extendedEvents.GetInvocationList()
                                        .All(obj => (EventHandler<TArgs<INotifyValue>>)obj != value))
                    extendedEvents += value;
            }
            remove
            {
                if (extendedEvents != null)
                    extendedEvents -= value;
            }
        }
    }


}
