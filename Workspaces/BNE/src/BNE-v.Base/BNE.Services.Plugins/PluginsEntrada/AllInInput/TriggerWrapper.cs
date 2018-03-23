using AllInTriggers;
using BNE.Services.Plugins.PluginResult;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Reactive.Disposables;
using System.Collections.Immutable;
using AllInTriggers.Helper;
using AllInTriggers.Model;

namespace BNE.Services.Plugins.PluginsEntrada.AllInInput
{
    public sealed class TriggerWrapper : IDisposable
    {
        private bool _isDisposed;
        private ImmutableList<IDisposable> _disposables;
        private ReactiveFlowInvoker _workFlowControl;
        private const int TimeoutProcessTriggerAndPrepareOutput = 120000;

        public TriggerWrapper()
        {
            this._workFlowControl = new ReactiveFlowInvoker(new TriggerProcessor());
            this._disposables = ImmutableList.Create<IDisposable>();
        }

        public System.Threading.Tasks.Task<TriggerPluginResult> Shoot(GatilhosEmail input, BLL.Enumeradores.TipoGatilho gatilho, DynObj data, CancellationToken token)
        {
            TaskCompletionSource<TriggerPluginResult> tcs;
            if (_isDisposed)
            {
                tcs = new TaskCompletionSource<TriggerPluginResult>();
                tcs.SetException(new ObjectDisposedException("Before Start (" + this.GetType().Name + ")"));
                return tcs.Task;
            }

            if (token.IsCancellationRequested)
            {
                tcs = new TaskCompletionSource<TriggerPluginResult>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            tcs = Do(input, (int)gatilho, data, token);
            return tcs.Task;
        }

        private TaskCompletionSource<TriggerPluginResult> Do(GatilhosEmail input, int gatilho, DynObj data, CancellationToken token)
        {
            var tcs = new TaskCompletionSource<TriggerPluginResult>();
            IDisposable disp = null;
            disp = _workFlowControl.Context.Schedule(async () =>
                {
                    try
                    {
                        var processValue = await Run(gatilho, data, token);

                        var valueResult = ExtractResult(input, processValue);

                        tcs.TrySetResult(valueResult);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                    finally
                    {
                        try
                        {
                            if (disp != null)
                                disp.Dispose();

                            Interlocked.Exchange(ref _disposables, _disposables.Remove(disp));
                        }
                        catch
                        {

                        }
                    }
                });

            if (_isDisposed)
            {
                disp.Dispose();
                tcs.TrySetException(new ObjectDisposedException("After Scheduling (" + this.GetType().Name + ")"));
            }
            else
                Interlocked.Exchange(ref _disposables, _disposables.Add(disp));

            return tcs;
        }

        private async Task<DynObj> Run(int gatilho, DynObj data, CancellationToken token)
        {
            data["Key"] = gatilho;

            var compositionResult = _workFlowControl.PostObservable(data)
                                    .FirstAsync()
                                    .PublishLast();

            if (_isDisposed)
                throw new ObjectDisposedException("In Execution of the Scheduling (" + this.GetType().Name + ")");

            token.ThrowIfCancellationRequested();

            data["CancellationToken"] = token;

            CancellationTokenRegistration cancelationDisp = default(CancellationTokenRegistration);
            IDisposable callDisp = null;
            var dispRes = compositionResult.Connect();
            try
            {
                callDisp = _workFlowControl.PostRaise(data);
                var sxTimeout = compositionResult.Timeout(TimeSpan.FromMilliseconds(TimeoutProcessTriggerAndPrepareOutput), _workFlowControl.Context);

                cancelationDisp = token.Register(() => callDisp.Dispose());

                var taskProcess = await sxTimeout.ToTask();
                return taskProcess;
            }
            finally
            {
                try
                {
                    cancelationDisp.Dispose();
                    dispRes.Dispose();
                    if (callDisp != null)
                    {
                        callDisp.Dispose();
                    }
                }
                catch
                {

                }
            }
        }

        private TriggerPluginResult ExtractResult(GatilhosEmail input, DynObj result)
        {
            if (result.Maybe<bool>("ToIgnore").ValueOrDefault)
                return new TriggerPluginResult(input, false);

            if (result.Maybe<bool>("Error").ValueOrDefault)
            {
                var ex = result.Maybe<Exception>("Exception");
                if (ex.Valid)
                    throw ex.ValueOrDefault;
                else
                    throw new InvalidOperationException("Unknown Error");
            }

            if (result.Maybe<NotificaCicloDeVidaAllIn>("Result").Valid)
            {
                return (TriggerPluginResult)new TriggerPluginResultEx<NotificaCicloDeVidaAllIn>(input, false, AllInResultType.LifeCycle, result.Maybe<NotificaCicloDeVidaAllIn>("Result").ValueOrDefault);
            }

            if (result.Maybe<EnviaEmailTransacaoAllIn>("Result").Valid)
            {
                return (TriggerPluginResult)new TriggerPluginResultEx<EnviaEmailTransacaoAllIn>(input, false, AllInResultType.Transaction, result.Maybe<EnviaEmailTransacaoAllIn>("Result").ValueOrDefault);
            }

            throw new InvalidOperationException("Result from 'ExtractResult' method is invalid");
        }

        public void Dispose()
        {
            _isDisposed = true;

            do
            {
                foreach (var item in _disposables.ToArray())
                {
                    _disposables = Interlocked.Exchange(ref _disposables, _disposables.Remove(item));
                    try
                    {
                        item.Dispose();
                    }
                    catch
                    {

                    }
                }
            } while (!_disposables.IsEmpty);

            this._workFlowControl.Dispose();
        }
    }

}
