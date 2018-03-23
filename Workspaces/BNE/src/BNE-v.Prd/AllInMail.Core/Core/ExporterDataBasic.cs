using AllInMail.Template;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace AllInMail.Core
{
    public abstract class ExporterDataBasic<T> : AllInMail.Core.IExporterDataBasic<T>
    {
        public class BatchExceptionControl
        {
            private readonly object _syncRoot = new object();

            private Exception _firstError;
            public Exception MainException
            {
                get
                {
                    return _firstError;
                }
                set
                {
                    lock (_syncRoot)
                    {
                        if (_firstError != null)
                        {
                            SubsequentErrors.Add(value);
                            return;
                        }
                        _firstError = value;
                    }
                }
            }

            private readonly List<Exception> _subsequentErrors = new List<Exception>();

            public List<Exception> SubsequentErrors
            {
                get { return _subsequentErrors; }
            }
        }

        private readonly IMainExporter _mainVm;
        private IAllInDefConverter<T> _converter;

        public IAllInDefConverter Converter
        {
            get { return _converter; }
        }

        public IMainExporter MainVm
        {
            get { return _mainVm; }
        }

        public ExporterDataBasic(IMainExporter mainVm, IAllInDefConverter<T> converter)
        {
            if (converter == null)
                throw new NullReferenceException("converter");

            if (mainVm == null)
                throw new NullReferenceException("mainVm");

            this._mainVm = mainVm;
            this._converter = converter;
        }

        public System.Threading.Tasks.Task<string> Process(Lazy<Stream> lazyOutputStream, CancellationToken token)
        {
            return Task.Factory.StartNew(new Func<object, Task<string>>((arg) =>
            {
                MainVm.Progress.StartedTime = DateTime.Now;
                var vm = MainVm;

                Lazy<StreamWriter> textWriter = new Lazy<StreamWriter>(() =>
                    {
                        StreamWriter writer = null;

                        try
                        {
                            writer = new StreamWriter(lazyOutputStream.Value, Encoding.Default);
                        }
                        catch
                        {
                            if (writer != null)
                                writer.Dispose();

                            throw;
                        }

                        return writer;
                    });


                Tuple<IEnumerable<IQueryable<int>>, SqlTransaction, IDisposable> queryItems = null;
                try
                {
                    queryItems = PrepareDataEnvironment(vm, token);
                    return DoProcess(vm, textWriter, queryItems.Item1, queryItems.Item2, token);
                }
                finally
                {
                    if (queryItems != null)
                        queryItems.Item3.Dispose();

                    if (textWriter.IsValueCreated)
                    {
                        textWriter.Value.Close();
                        textWriter.Value.Dispose();
                    }
                    MainVm.Progress.FinishTime = DateTime.Now;
                }
                // }), null, token, TaskCreationOptions.LongRunning, TaskScheduler.Default).Unwrap(); // code for run in x86
            }), token).Unwrap(); // code for run in x64
        }
        protected abstract Tuple<IEnumerable<IQueryable<int>>, SqlTransaction, IDisposable> PrepareDataEnvironment(IMainExporter vm, CancellationToken token);

        protected virtual Task<string> DoProcess(IMainExporter vm, Lazy<StreamWriter> textWriter, IEnumerable<IQueryable<int>> querySelector, SqlTransaction trans, CancellationToken token)
        {
            var worker = new OrderedTaskScheduler();
            var fact = new TaskFactory(worker);
            Task completed = null;
            try
            {
                Lazy<StreamWriter> newStream;
                if (vm.StartSettings.WriterHeader)
                {
                    newStream = new Lazy<StreamWriter>(() =>
                    {
                        var writer = textWriter.Value;
                        writer.WriteLine(_converter.GetDeclaration());
                        return writer;
                    });
                }
                else
                {
                    newStream = textWriter;
                }

                var watch = Stopwatch.StartNew();
                bool firstBatch = true;
                foreach (var batch in querySelector)
                {
                    token.ThrowIfCancellationRequested();

                    if (firstBatch)
                    {
                        watch.Stop();
                        MainVm.Progress.TimeDbStartFirstBatch = new TimeSpan(watch.ElapsedTicks);
                        firstBatch = false;
                    }

                    var read = ScheduleBatches(vm, newStream, trans, _converter, batch, fact);

                    if (read.Item2 != null)
                        completed = read.Item2;

                    if (!read.Item1)
                        break;

                    textWriter.Value.Flush();

                    if (vm.StartSettings.MaxQuantity != 0 && vm.Progress.TimeBufferLoadCompleted.TotalCount * vm.StartSettings.BufferSize >= vm.StartSettings.MaxQuantity)
                        break;
                }

                if (completed == null)
                {
                    var tcs = new TaskCompletionSource<string>();
                    tcs.SetResult("OK");
                    return tcs.Task;
                }
                return completed.ContinueWith(t => "OK");
            }
            catch
            {
                throw;
            }
            finally
            {
                var disp = ((object)worker) as IDisposable;

                if (disp != null)
                    disp.Dispose();
            }
        }

        private Tuple<bool, Task> ScheduleBatches(IMainExporter vm, Lazy<StreamWriter> stream, SqlTransaction trans, IAllInDefConverter<T> main, IQueryable<int> batch, TaskFactory writerProducerTask)
        {
            var firstRow = true;

            var progress = MainVm.Progress;
            bool atLeastOne = false;

            var exControl = new BatchExceptionControl();
            Task lastWriter = null;

            var batchTime = Stopwatch.StartNew();
            foreach (var item in batch.Buffer(vm.StartSettings.BufferSize))
            {
                if (exControl.MainException != null)
                    throw exControl.MainException;

                atLeastOne = true;
                if (firstRow)
                {
                    progress.TimeDbBatchQuery.Increment(new TimeSpan(batchTime.ElapsedTicks));
                    firstRow = false;
                }

                var watch = Stopwatch.StartNew();
                try
                {
                    foreach (var next in item)
                    {
                        lastWriter = MapData(main, writerProducerTask, stream, progress, next, exControl, trans);
                    }
                }
                finally
                {
                    watch.Stop();
                    progress.TimeBufferLoadCompleted.Increment(new TimeSpan(watch.ElapsedTicks));
                }

                if (vm.StartSettings.MaxQuantity != 0 && progress.TimeBufferLoadCompleted.TotalCount * vm.StartSettings.BufferSize >= vm.StartSettings.MaxQuantity)
                    break;
            }
            batchTime.Stop();

            if (atLeastOne)
            {
                vm.Progress.TimeBatchLoadCompleted.Increment(new TimeSpan(batchTime.ElapsedTicks));
            }

            return Tuple.Create(atLeastOne, lastWriter);
        }

        protected abstract Task MapData(IAllInDefConverter<T> mainConv, TaskFactory writerProducerWrapper, Lazy<StreamWriter> outputStreamWriter, IProgressIntegration progressInfo, int nextTarget, BatchExceptionControl errorControl, SqlTransaction trans);


        public IAllInDefConverter<T> ConverterTyped
        {
            get { return _converter; }
        }
    }
}
