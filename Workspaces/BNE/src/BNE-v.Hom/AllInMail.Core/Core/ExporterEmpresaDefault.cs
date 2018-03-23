using System.Collections.Specialized;
using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AllInMail.Core
{
    public class ExporterEmpresaDefault : ExporterDataBasic<AllInEmpresaTemplateModel>
    {
        public ExporterEmpresaDefault(IMainExporter info)
            : base(info, new AllInEmpresaDefConverter())
        {
        }

        protected override Tuple<IEnumerable<IQueryable<int>>, SqlTransaction, IDisposable> PrepareDataEnvironment(IMainExporter vm, CancellationToken token)
        {
            SqlConnection conn = null;
            var disp = new System.Reactive.Disposables.CompositeDisposable();
            try
            {
                conn = new SqlConnection(BNE.BLL.DataAccessLayer.CONN_STRING);
                conn.Open();

                SqlTransaction trans = null;
                try
                {
                    trans = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                    disp.Add(trans);
                    disp.Add(conn);

                    IEnumerable<IQueryable<int>> ret = MapCollectionData(vm);
                    return Tuple.Create(ret, trans, (IDisposable)disp);
                }
                catch
                {
                    if (trans != null)
                        trans.Dispose();

                    throw;
                }
            }
            catch
            {
                if (conn != null)
                    conn.Dispose();

                throw;
            }
        }

        private IEnumerable<IQueryable<int>> MapCollectionData(IMainExporter vm)
        {
            int skip = 0;
            do
            {
                yield return BNE.BLL.Filial.BuscarFiliaisIdModificacaoExportacao(vm.StartSettings.FromDate,
                                                                            vm.StartSettings.StartAboveTargetId,
                                                                            vm.StartSettings.BatchSize, skip, null)
                                                                            .AsQueryable();

                skip = vm.StartSettings.BatchSize + skip;

            } while (true);
        }

        protected override System.Threading.Tasks.Task MapData(IAllInDefConverter<AllInEmpresaTemplateModel> mainConv,
                                                            System.Threading.Tasks.TaskFactory writerProducerWrapper,
                                                            Lazy<System.IO.StreamWriter> ouputStreamWriter,
                                                            Template.IProgressIntegration progressInfo,
                                                            int nextTarget,
                                                            ExporterDataBasic<AllInEmpresaTemplateModel>.BatchExceptionControl errorControl,
                                                            System.Data.SqlClient.SqlTransaction trans)
        {
            var load = BNE.BLL.Filial.CarregaFilialExportacaoAllIn(nextTarget, trans);

            progressInfo.QuantityQuery = progressInfo.QuantityQuery + 1;
            if (load == null)
                return null;

            progressInfo.QuantityLoaded = progressInfo.QuantityLoaded + 1;

            return writerProducerWrapper.StartNew(() =>
            {
                if (errorControl.MainException != null)
                    return;

                int count = 0;
                foreach (var item in Helper.ModelTranslator.TranslateToAllInEmpresaModel(load))
                {
                    if (count > 0)
                        ouputStreamWriter.Value.Flush();

                    var res = mainConv.Parse(item);
                    ouputStreamWriter.Value.WriteLine(res);

                    count++;
                }

                progressInfo.QuantityProcessed = progressInfo.QuantityProcessed + 1;
                progressInfo.LastTargetId = (int)nextTarget;

            }).ContinueWith((t) =>
            {
                if (t.Exception == null)
                    return;
                errorControl.MainException = t.Exception;

            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}
