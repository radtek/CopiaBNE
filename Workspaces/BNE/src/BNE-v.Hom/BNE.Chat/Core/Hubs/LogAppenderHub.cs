using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using BNE.Chat.Core.Log;
using BNE.Chat.DTO.Log;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.Chat.Core.Hubs
{
    [HubName("logAppender")]
    public class LogAppenderHub : Hub
    {
        public static readonly ConcurrentDictionary<string, IDisposable> Subscriptions = new ConcurrentDictionary<string, IDisposable>();

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var t1 = base.OnConnected();
            if (t1 == null)
            {
                return new Task(() => StartListen(connectionId));
            }
            t1.ContinueWith(a => StartListen(connectionId));
            return t1;
        }

        private void StartListen(string connectionId)
        {

        }

        public override Task OnDisconnected()
        {
            var connectionId = Context.ConnectionId;

            IDisposable disp;
            if (Subscriptions.TryRemove(connectionId, out disp))
            {
                disp.Dispose();
            }

            return base.OnDisconnected();
        }

        public bool GetStatus()
        {
            return LogController.Instance.Config.Enabled;
        }

        public Task SetStatus(bool status)
        {
            return Task.Factory.StartNew(() =>
                {
                    LogController.Instance.Config.Enabled = status;
                });
        }

        public void ConfigureMostRecent(int? countItems, int? limitMinutes)
        {
            var count = countItems ?? 0;
            var limit = limitMinutes ?? 0;

            if (count <= 0 && limit <= 0)
                return;

            Task.Factory.StartNew(() =>
                {
                    using (LogController.Instance.Config.AccumulateChanges())
                    {
                        if (count != 0)
                        {
                            LogController.Instance.Config.LimitItem = count;
                        }

                        if (limit != 0)
                        {
                            LogController.Instance.Config.MaxTime = TimeSpan.FromMinutes(limit);
                        }
                    }
                });
        }
    }
}
