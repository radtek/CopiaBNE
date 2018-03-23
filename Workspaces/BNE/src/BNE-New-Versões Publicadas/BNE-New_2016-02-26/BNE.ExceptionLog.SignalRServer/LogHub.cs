using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BNE.ExceptionLog.SignalRServer
{
    [HubName("logserver")]
    public class LogHub : Hub
    {

        //public static void PushLog(LogInfo logInfo)
        public static void PushLog(string logInfo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<LogHub>();

            /*switch (logInfo.Level)
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
                    context.Clients.Group("all").pushLog(logInfo);
                    break;
            }*/
            context.Clients.Group("all").pushLog(logInfo);
        }
    }
}
