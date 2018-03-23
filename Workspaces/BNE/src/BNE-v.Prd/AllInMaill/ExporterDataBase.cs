using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using AllInMail.Base.Vm;
using AllInMail.Vm;
using AllInMail.Core;
using System.Data.SqlClient;
using BNE.BLL;
using System.Data.Common;
using System.Reactive.Concurrency;
using AllInMail.Template;

namespace AllInMail
{

    public class ExporterDataBase : ExporterCurriculoBasic
    {
        public ExporterDataBase(IMainExporter mainVm)
            : base(mainVm)
        {
        }

        protected override Tuple<IEnumerable<IQueryable<int>>, SqlTransaction, IDisposable> PrepareDataEnvironment(IMainExporter vm, CancellationToken token)
        {
            var watch = Stopwatch.StartNew();
            Entities entities = null;
            var disp = new System.Reactive.Disposables.CompositeDisposable();
            try
            {
                entities = new Entities();
                entities.Configuration.LazyLoadingEnabled = true;
                entities.Configuration.ProxyCreationEnabled = true;
                entities.Database.Connection.Open();

                DbConnection con = null;
                try
                {
                    con = new SqlConnection(DataAccessLayer.CONN_STRING);
                    con.Open();

                    SqlTransaction trans = null;
                    try
                    {
                        trans = (SqlTransaction)con.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                        watch.Stop();
                        MainVm.Progress.TimeDbFirstLoad = new TimeSpan(watch.ElapsedTicks);

                        disp.Add(trans);
                        disp.Add(con);
                        disp.Add(entities);

                        return Tuple.Create(MapContinousData(vm, entities), trans, (IDisposable)disp);
                    }
                    catch
                    {
                        if (trans != null)
                        {
                            trans.Dispose();
                        }
                        throw;
                    }

                }
                catch
                {
                    if (con != null)
                    {
                        con.Dispose();
                    }
                    throw;
                }
            }
            catch
            {
                if (entities != null)
                {
                    entities.Dispose();
                }
                throw;
            }
        }

        private IEnumerable<IQueryable<int>> MapContinousData(IMainExporter vm, Entities entities)
        {
            bool execute = true;
            int skip = 0;
            do
            {
                if (vm.StartSettings.FromDate.HasValue)
                {
                    var next = vm.StartSettings.FromDate.Value;

                    yield return entities.BNE_Curriculo
                                 .Where(a => !a.Flg_Inativo && (a.Dta_Cadastro > next || a.Dta_Modificacao_CV > next))
                                 .Select(a => a.Idf_Curriculo)
                                 .Where(a => a > vm.StartSettings.StartAboveTargetId)
                                 .OrderBy(a => a)
                                 .Skip(skip)
                                 .Take(vm.StartSettings.BatchSize);
                }
                else
                {
                    yield return entities.BNE_Curriculo
                                 .Where(a => !a.Flg_Inativo)
                                 .Select(a => a.Idf_Curriculo)
                                 .Where(a => a > vm.StartSettings.StartAboveTargetId)
                                 .OrderBy(a => a)
                                 .Skip(skip)
                                 .Take(vm.StartSettings.BatchSize);
                }

                skip = vm.StartSettings.BatchSize + skip;

            } while (execute);
        }
    }
}
