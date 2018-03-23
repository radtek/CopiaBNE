using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using AllInMail.Core;
using AllInMail.Core.Model;
using AllInMail.Template;
using BNE.BLL;

namespace BNE.Services.Code
{
    public class AllinCurriculoDefaultExporter : ExporterCurriculoBasic
    {
        public AllinCurriculoDefaultExporter(MainExporterSettings vm)
            : base(vm)
        {
        }

        /// <summary>
        ///     Teste charan
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override Tuple<IEnumerable<IQueryable<int>>, SqlTransaction, IDisposable> PrepareDataEnvironment(
            IMainExporter vm, CancellationToken token)
        {
            SqlConnection conn = null;
            var disp = new CompositeDisposable();
            try
            {
                conn = new SqlConnection(DataAccessLayer.CONN_STRING);
                conn.Open();

                SqlTransaction trans = null;
                try
                {
                    trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted);

                    disp.Add(trans);
                    disp.Add(conn);

                    var ret = MapCollectionData(vm);
                    return Tuple.Create(ret, trans, (IDisposable) disp);
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
            var skip = 0;
            do
            {
                yield return
                    Curriculo.BuscarCurriculosIdModificacaoExportacao(vm.StartSettings.FromDate.GetValueOrDefault(),
                        vm.StartSettings.StartAboveTargetId,
                        vm.StartSettings.BatchSize, skip, null)
                        .AsQueryable();

                skip = vm.StartSettings.BatchSize + skip;
            } while (true);
        }
    }
}