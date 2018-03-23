using System;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Web.Helpers
{
    public static class TaskHelper
    {
        public static Task TimeoutAfter(this Task task, int millisecondsTimeout)
        {
            // tcs.Task will be returned as a proxy to the caller
            var tcs =
                new TaskCompletionSource<VoidTypeStruct>();

            // Set up a timer to complete after the specified timeout period
            var timer = new Timer(state =>
            {
                // Recover our state data
                var myTcs = (TaskCompletionSource<VoidTypeStruct>) state;

                // Fault our proxy Task with a TimeoutException
                myTcs.TrySetException(new TimeoutException());
            }, tcs, millisecondsTimeout, Timeout.Infinite);

            // Wire up the logic for what happens when source task completes
            task.ContinueWith((antecedent, state) =>
            {
                // Recover our state data
                var tuple =
                    (Tuple<Timer, TaskCompletionSource<VoidTypeStruct>>) state;

                // Cancel the timer
                tuple.Item1.Dispose();
                // Marshal results to proxy
                MarshalTaskResults(antecedent, tuple.Item2);
            },
                Tuple.Create(timer, tcs), // See Footnote #2
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            return tcs.Task;
        }

        internal static void MarshalTaskResults<TResult>(Task source, TaskCompletionSource<TResult> proxy)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    proxy.TrySetException(source.Exception);
                    break;
                case TaskStatus.Canceled:
                    proxy.TrySetCanceled();
                    break;
                case TaskStatus.RanToCompletion:
                    var castedSource = source as Task<TResult>;
                    proxy.TrySetResult(
                        castedSource == null ? default(TResult) : // source is a Task
                            castedSource.Result); // source is a Task<TResult>
                    break;
            }
        }

        internal struct VoidTypeStruct
        {
        }
    }
}