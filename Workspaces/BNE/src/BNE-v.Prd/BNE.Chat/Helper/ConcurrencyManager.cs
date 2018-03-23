using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace BNE.Chat.Helper
{
    public sealed class ConcurrencyManager<T>
    {
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> CacheInstanceConcurrentTokenAccess =
       new SetValueOrDefaultFact<HardConfig<int>, int>(new HardConfig<int>("concurrency_manager_controle_de_instancia_em_minutos", 10),
                                                       a => a.Value);

        public event EventHandler<TimeoutEventArgs<T>> Timeout;

        public int MsTimeout
        {
            get
            {
                if (_useTimeoutAccessor)
                {
                    return Math.Max(-1, _timeoutAccessorInMs());
                }
                return _msTimeout;
            }
        }

        private void OnTimeout(T key)
        {
            var handler = Timeout;

            if (handler != null)
            {
                handler(this, new TimeoutEventArgs<T>(key));
            }
        }

        private readonly bool _useTimeoutAccessor;
        private readonly int _msTimeout;
        private readonly RenewableTimeCache<T, Tuple<ThreadLocal<bool>, SemaphoreSlim>> _dictTokenAccess;
        private readonly RenewableTimeCache<int, Action> _reentranceChecker;
        private readonly Func<int> _timeoutAccessorInMs;

        public ConcurrencyManager(Func<int> timeoutAccessorInMs, IEqualityComparer<T> comparer = null)
        {
            if (timeoutAccessorInMs == null)
                throw new NullReferenceException("timeoutAccessorInMs");

            _useTimeoutAccessor = true;
            _timeoutAccessorInMs = timeoutAccessorInMs;

            this._reentranceChecker =
            new RenewableTimeCache<int, Action>(
                () => TimeSpan.FromMinutes(CacheInstanceConcurrentTokenAccess.Value), true);
            this._dictTokenAccess =
              new RenewableTimeCache<T, Tuple<ThreadLocal<bool>, SemaphoreSlim>>(
                  () => TimeSpan.FromMinutes(CacheInstanceConcurrentTokenAccess.Value), true,
                  (a, b) =>
                  {
                      b.Item1.Dispose();
                      b.Item2.Dispose();
                  }, comparer);
        }

        public ConcurrencyManager(int msTimeout, IEqualityComparer<T> comparer = null)
        {
            if (msTimeout < -1)
                throw new ArgumentOutOfRangeException("msTimeout");

            this._msTimeout = msTimeout;
            this._reentranceChecker =
                new RenewableTimeCache<int, Action>(
                    () => TimeSpan.FromMinutes(CacheInstanceConcurrentTokenAccess.Value), true);
            this._dictTokenAccess =
              new RenewableTimeCache<T, Tuple<ThreadLocal<bool>, SemaphoreSlim>>(
                  () => TimeSpan.FromMinutes(CacheInstanceConcurrentTokenAccess.Value), true,
                  (a, b) =>
                  {
                      b.Item1.Dispose();
                      b.Item2.Dispose();
                  }, comparer);
        }

        public IDisposable GetToken(T key)
        {
            object obj = key;
            if (obj == null)
                throw new NullReferenceException("key");

            Tuple<ThreadLocal<bool>, SemaphoreSlim> syncronization;
            IDisposable empty;
            if (IsReentry(key, out syncronization, out empty)) 
                return empty;

            var enterSyncronization = syncronization.Item2.Wait(MsTimeout);
            if (!enterSyncronization)
            {
                OnTimeout(key);
                return FakeToken.Empty;
            }

            var oldThreadId = Thread.CurrentThread.ManagedThreadId;
            syncronization.Item1.Value = true;
            return new InlineToken(key, sender =>
            {
                try
                {
                    if (Thread.CurrentThread.ManagedThreadId == oldThreadId)
                    {
                        syncronization.Item1.Value = false;
                    }
                    else
                    {
                        _reentranceChecker.AddOrUpdate(oldThreadId, keyF => () => syncronization.Item1.Value = false,
                            (keyU, old) => () =>
                            {
                                syncronization.Item1.Value = false;
                                old();
                            });
                    }
                }
                finally
                {
                    syncronization.Item2.Release();
                }
            });
        }

        private bool IsReentry(T key, out Tuple<ThreadLocal<bool>, SemaphoreSlim> syncronization, out IDisposable ret)
        {
            syncronization = _dictTokenAccess.GetOrAdd(key,
                str => Tuple.Create(new ThreadLocal<bool>(), new SemaphoreSlim(1, 1)));

            if (syncronization.Item1.Value
                && syncronization.Item2.CurrentCount == 0)
            {
                Action pendingAction;
                if (_reentranceChecker.TryGetValue(Thread.CurrentThread.ManagedThreadId, out pendingAction))
                {
                    pendingAction();
                }
                else
                {
                    // todo (it is possible improvement with counter)
                    {
                        ret = FakeToken.Empty;
                        return true;
                    }
                }
            }
            ret = null;
            return false;
        }

        internal sealed class FakeToken : IDisposable
        {
            public void Dispose()
            {

            }

            public bool CanReleaseFlow
            {
                get { return false; }
            }

            private static readonly IDisposable EmptyToken = new FakeToken();

            public static IDisposable Empty
            {
                get { return EmptyToken; }
            }
        }

        internal sealed class InlineToken : IDisposable
        {
            private readonly T _key;
            private Action<InlineToken> _onDispose;

            public InlineToken(T key, Action<InlineToken> onDispose)
            {
                this._key = key;
                this._onDispose = onDispose;
            }

            public bool CanReleaseFlow
            {
                get { return true; }
            }

            public T Key
            {
                get { return _key; }
            }

            public void Dispose()
            {
                try
                {
                    _onDispose(this);
                }
                finally
                {
                    _onDispose = null;
                }
            }
        }
    }

    public class TimeoutEventArgs<T> : EventArgs
    {
        public TimeoutEventArgs(T key)
        {
            this.Key = key;
        }

        public T Key { get; protected set; }
    }
}
