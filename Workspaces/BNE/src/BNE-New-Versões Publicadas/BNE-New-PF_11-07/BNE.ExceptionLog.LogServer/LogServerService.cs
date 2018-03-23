using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BNE.ExceptionLog.LogServer
{
    public static class LogServerService
    {
        static readonly Lazy<Task> TaskInicializer = new Lazy<Task>(InicializeLogServer);
        public static IDisposable Subs { get; private set; }

        private static Task InicializeLogServer()
        {
            return Task.Factory.StartNew(() =>
            {
                ConfigureSignalR();
                StartSubscription();
            });
        }

        private static void ConfigureSignalR()
        {
            GlobalHost.DependencyResolver.Register(typeof(LogServerHub), () => new LogServerHub());
        }

        private static void StartSubscription()
        {
            var rx = Observable.FromEventPattern<LogEventArgs>(add => LogServerManager.Instance.NewLog += add,
                                                               rem => LogServerManager.Instance.NewLog -= rem);

            Subs = rx.Subscribe(PushLog);
        }

        private static void PushLog(System.Reactive.EventPattern<LogEventArgs> obj)
        {
            LogServerHub.PushLog(obj.EventArgs.Value);
        }


        public static Task Configure()
        {
            return TaskInicializer.Value;
        }
    }
}
