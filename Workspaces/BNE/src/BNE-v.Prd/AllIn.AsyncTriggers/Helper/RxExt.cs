using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AllInTriggers.Helper
{
    public static class RxExt
    {
        public static IObservable<T> WhereFallBack<T>(this IObservable<T> source, Func<T, bool> predicate, Action<T> fallBackPred)
        {
            var subs = source.Publish().RefCount();

            var ysx = subs.Where(predicate);

            var nsx = subs.Where(a => !predicate(a))
                          .Do(fallBackPred)
                          .IgnoreElements();

            return ysx.Merge(nsx);
        }

        //public static IObservable<T> WhereFallBack<T>(this IObservable<T> source, Func<T, bool> predicate, Action<T> fallBackPred, Action<T, Exception> fallBackEx)
        //{
        //    return new AnonymousObservable<T>(obs =>
        //    {
        //        return source.Subscribe(x =>
        //            {
        //                try
        //                {
        //                    var res = predicate(x);

        //                    if (res)
        //                        obs.OnNext(x);
        //                    else
        //                        fallBackPred(x);
        //                }
        //                catch (Exception ex)
        //                {
        //                    fallBackEx(x, ex);
        //                    obs.OnError(ex);
        //                }
        //            }, obs.OnError, obs.OnCompleted);
        //    });
        //}

        public static IObservable<T> DoAsyncWithFallBack<T>(this IObservable<T> source, Func<T, Task> accessor, Action<T, Exception> localizedFallback)
        {
            return DoWithFallBack(source, accessor, localizedFallback);
        }

        public static IObservable<T> DoWithFallBack<T>(this IObservable<T> source, Action<T> accessor, Action<T, Exception> localizedFallback)
        {
            return new AnonymousObservable<T>(obs =>
            {
                return source.Subscribe(x =>
                {
                    try
                    {
                        accessor(x);
                        obs.OnNext(x);
                    }
                    catch (Exception ex)
                    {
                        localizedFallback(x, ex);
                        obs.OnError(ex);
                    }
                }, obs.OnError, obs.OnCompleted);
            });
        }

        public static IObservable<T> DoWithFallBack<T>(this IObservable<T> source, Func<T, Task> accessor, Action<T, Exception> localizedFallback)
        {
            return new AnonymousObservable<T>(obs =>
            {
                return source.Subscribe(async x =>
                {
                    try
                    {
                        await accessor(x);
                        obs.OnNext(x);
                    }
                    catch (Exception ex)
                    {
                        localizedFallback(x, ex);
                        obs.OnError(ex);
                    }
                }, obs.OnError, obs.OnCompleted);
            });
        }
        //public static IObservable<TSelector> SelectWithFallBack<T, TSelector>(this IObservable<T> source, Func<T, TSelector> selector, Action<T, Exception> localizedFallBack)
        //{
        //    return new AnonymousObservable<TSelector>(obs =>
        //    {
        //        return source.Subscribe(x =>
        //        {
        //            try
        //            {
        //                var res = selector(x);
        //                obs.OnNext(res);
        //            }
        //            catch (Exception ex)
        //            {
        //                localizedFallBack(x, ex);
        //                obs.OnError(ex);
        //            }
        //        }, obs.OnError, obs.OnCompleted);
        //    });
        //}
    }
}
