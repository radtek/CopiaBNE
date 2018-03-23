using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using BNE.ExceptionLog.LogServer.Model;

namespace BNE.ExceptionLog.LogServer
{
    public sealed class LogServerManager
    {

        private readonly ConcurrentDictionary<string, LogInfo> _errorLog = new ConcurrentDictionary<string, LogInfo>();
        private EventHandler<LogEventArgs> _newLog;

        OrderedTaskScheduler _taskScheduler;
        bool _schedulerInicialized;

        TaskFactory _taskFactory;
        bool _taskFactInicialized;

        object _instanceSync = new object();

        #region [ Singleton ]
        private static bool _inicialized;
        private static LogServerManager _instance;
        private static object _syncronization = new object();

        public static LogServerManager Instance
        {
            get
            {
                return LazyInitializer.EnsureInitialized(ref _instance, ref _inicialized, ref _syncronization, () => new LogServerManager());
            }
        }
        #endregion

        #region Event
        public event EventHandler<LogEventArgs> NewLog
        {
            add
            {
                var current = _newLog;
                if (current == null)
                {
                    Interlocked.Exchange(ref _newLog, _newLog += value);
                    return;
                }

                if (current.GetInvocationList().All(obj => ((EventHandler<LogEventArgs>)obj) != value))
                {
                    Interlocked.Exchange(ref _newLog, _newLog += value);
                }
            }
            remove
            {
                Interlocked.Exchange(ref _newLog, _newLog -= value);
            }
        }
        #endregion

        #region Metodos

        #region OnNewLog
        private void OnNewLog(LogInfo info)
        {
            LazyInitializer.EnsureInitialized(ref _taskScheduler, ref _schedulerInicialized, ref _instanceSync, () => new OrderedTaskScheduler());
            LazyInitializer.EnsureInitialized(ref _taskFactory, ref _taskFactInicialized, ref _instanceSync, () => new TaskFactory(_taskScheduler));

            var handler = _newLog;
            if (handler == null)
                return;

            _taskFactory.StartNew(() =>
            {
                var err = new List<Exception>();

                foreach (var item in handler.GetInvocationList())
                {
                    try
                    {
                        ((EventHandler<LogEventArgs>)item)(this, new LogEventArgs { Value = info });
                    }
                    catch (Exception ex)
                    {
                        err.Add(ex);
                    }
                }

                if (err.Count > 0)
                {
                    if (err.Count == 1)
                    {
                        throw err[0];
                    }
                    throw new AggregateException(err);
                }
            });
        }
        #endregion

        #region Publicos

        #region Log
        public void Log(LogInfo info)
        {
            var x  = _errorLog.AddOrUpdate(info.Guid.ToString(), info, (up, current) => current);
            OnNewLog(info);
        }
        #endregion

        #region GetAllErrors
        public IEnumerable<LogInfo> GetAllErrors()
        {
            return _errorLog.Select(logNode => logNode.Value);
        }
        #endregion

        #endregion

        #endregion

    }

    public class LogEventArgs : EventArgs
    {
        public LogInfo Value { get; set; }
    }

}