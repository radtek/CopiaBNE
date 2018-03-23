using System;
using System.Threading.Tasks;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using Microsoft.AspNet.SignalR;
using System.Linq;

namespace BNE.Chat.Core.EventModel
{
    public class ChatResultEventArgs : ChatEventArgs, IResultChatEventArgs
    {
        public ChatResultEventArgs(Hub hub)
            : base(hub)
        {

        }

        public ChatResultEventArgs(Hub hub, object callParams)
            : base(hub, callParams)
        {

        }


        public Task<ISignalRGenericResult> TaskValueResult { get; set; }

        Task IResultChatEventArgs.TaskResult
        {
            get
            {
                return TaskValueResult;
            }
            set
            {
                if (value == null)
                {
                    TaskValueResult = null;
                    return;
                }

                var args = value.GetType().GetGenericArguments();

                var gen = args.FirstOrDefault();
                if (gen == null)
                {
                    TaskValueResult = Task.Factory.StartNew<ISignalRGenericResult>(() => 
                        {
                            value.Wait();
                            return new SignalRGenericResult<object>();
                        });
                    return;
                }

                if (gen == typeof(Task<ISignalRGenericResult>))
                {
                    TaskValueResult = (Task<ISignalRGenericResult>)value;
                    return;
                }

                if (typeof(ISignalRGenericResult).IsAssignableFrom(gen))
                {
                    TaskValueResult = Task.Factory.StartNew<ISignalRGenericResult>(() =>
                    {
                        value.Wait();

                        dynamic dyn = value;
                        var realResult = dyn.Result;

                        var customType = typeof(ISignalRGenericResult<>).MakeGenericType(gen);

                        var instance = Activator.CreateInstance(customType, realResult);
                        return (ISignalRGenericResult)instance;
                    });
                  
                    return;
                }

                throw new InvalidCastException("TaskResult to TaskValueResult");
            }
        }
    }
}