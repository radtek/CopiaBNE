using AllInMail;
using AllInMail.Core;
using AllInMail.Core.Model;
using AllInMail.Template;
using BNE.Services.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace BNE.Services.Code
{
    public class AllinCurriculoDefaultExporter : ExporterCurriculoBasic
    {
        public AllinCurriculoDefaultExporter(MainExporterSettings vm)
            : base(vm)
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
                yield return BNE.BLL.Curriculo.BuscarCurriculosIdModificacaoExportacao(vm.StartSettings.FromDate.GetValueOrDefault(),
                                                                            vm.StartSettings.StartAboveTargetId,
                                                                            vm.StartSettings.BatchSize, skip, null)
                                                                            .AsQueryable();

                skip = vm.StartSettings.BatchSize + skip;

            } while (true);
        }
    }
}
