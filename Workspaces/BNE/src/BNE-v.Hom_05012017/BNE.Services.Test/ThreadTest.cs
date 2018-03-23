using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using AllInTriggers.Helper;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BNE.Services.Test
{
    [TestClass]
    public class ThreadTest
    {
     
        SyncContextEventLoopScheduler scheduler = new SyncContextEventLoopScheduler();

        [TestMethod]
        public void TestAsyncAwait()
        {
            Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);

            scheduler.Schedule(async () =>
                {
                    Trace.WriteLine("1");
                    Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
                    await TaskEx.Delay(1000);

                    Trace.WriteLine("2");
                    Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
                });

            scheduler.Schedule(async () =>
            {
                Trace.WriteLine("3");
                Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
                await TaskEx.Delay(1000);

                Trace.WriteLine("4");
                Trace.WriteLine("T="+ Thread.CurrentThread.ManagedThreadId);
            });


            scheduler.Schedule(async () =>
            {
                Trace.WriteLine("5");
                Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
                await TaskEx.Run(() =>
                    {
                        Trace.WriteLine("async");
                        Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
                        Thread.Sleep(1000);
                    });

                Trace.WriteLine("6");
                Trace.WriteLine("T=" + Thread.CurrentThread.ManagedThreadId);
            });

            new ManualResetEventSlim(false).Wait();
        }
    }
}
