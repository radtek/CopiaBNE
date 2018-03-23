using System.Threading.Tasks;
using BNE.Chat.Core.Interface;

namespace BNE.Chat.Helper
{
    internal static class TaskAsyncHelper
    {
        public static readonly Task<ISignalRGenericResult> EmptyGenericResult = (Task<ISignalRGenericResult>)MakeTask<ISignalRGenericResult>(null);
        public static readonly Task EmptyTask = MakeTask<object>(null);

        private static Task MakeTask<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}