using System.Linq;
using System.Threading.Tasks;
using BNE.ExceptionLog.LogServer.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.ExceptionLog.LogServer
{
    [HubName("logserver")]
    public class LogServerHub : Hub
    {

        public Task<LogInfo[]> GetAllErrors()
        {
            var errors = LogServerManager.Instance.GetAllErrors();

            return Task.Factory.StartNew(()
                =>
                {
                    return errors.OrderByDescending(a => a.LastIncident).ThenByDescending(a => a.TotalIncidents).ToArray();
                });
        }

        public static void PushLog(LogInfo logInfo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LogServerHub>();

            switch (logInfo.Level)
            {
                case LogLevel.Information:
                    goto default;
                case LogLevel.Warning:
                    goto default;
                case LogLevel.Error:
                    goto default;
                case LogLevel.Flow:
                    goto default;
                default:
                    context.Clients.All.pushLog(logInfo);
                    break;
            }
        }

    }
}
