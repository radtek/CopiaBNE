using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using BNE.Chat.DTO.Log;

namespace BNE.Chat.Core.Log
{
    public class LogController
    {
        #region [ Singleton ]
        private static LogController _instance;
        private static bool _inicialized;
        private static object _syncronization;

        public static LogController Instance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _instance, ref _inicialized, ref _syncronization,
                                                         () => new LogController());
            }
        }
        #endregion

        private ReplaySubject<LogEntry> _loggerCache;
        private readonly LogConfig _config;

        private LogController()
        {
            _config = GetConfig();
            _loggerCache = LoadSubscription();

            _config.PropertyChanged += config_PropertyChanged;
        }

        private ReplaySubject<LogEntry> LoadSubscription()
        {
            return new ReplaySubject<LogEntry>();
        }

        private LogConfig GetConfig()
        {
            return new LogConfig();
        }

        void config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            

        }

        public LogConfig Config
        {
            get { return _config; }
        }

        public IObservable<LogEntry> LoggerCache
        {
            get { return _loggerCache; }
        }

        public IObservable<LogEntry> SubsNewLog()
        {
            return _loggerCache;
        }

        public void Append(LogEntry entry)
        {

        }
    }

    public class LogConfig : INotifyPropertyChanged
    {
        private bool _enabled;
        private int _limitItem;
        private TimeSpan _maxTime;
        private bool _beginChange;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (_beginChange)
                return;

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public IDisposable AccumulateChanges()
        {
            _beginChange = true;

            return Disposable.Create(CommitChanges);
        }

        private void CommitChanges()
        {
            _beginChange = false;
            OnPropertyChanged("Commit");
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        public int LimitItem
        {
            get { return _limitItem; }
            set
            {
                if (value == _limitItem)
                    return;

                _limitItem = value;
                OnPropertyChanged("LimitItem");
            }
        }

        public TimeSpan MaxTime
        {
            get { return _maxTime; }
            set
            {
                if (_maxTime == value)
                    return;

                _maxTime = value;
                OnPropertyChanged("MaxTime");
            }
        }
    }
}
