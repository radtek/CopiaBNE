using AllInMail.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AllInMail.Core
{
    public abstract class ExporterCurriculoBasic : ExporterDataBasic<AllInCurriculoTemplateModel>
    {
        public ExporterCurriculoBasic(IMainExporter exporter)
            : base(exporter, new AllInCurriculoDefConverter())
        {

        }

        protected override Task MapData(IAllInDefConverter<AllInCurriculoTemplateModel> main, TaskFactory writerProducerWrapper, Lazy<System.IO.StreamWriter> streamWriter, Template.IProgressIntegration progress, int nextTarget, ExporterDataBasic<AllInCurriculoTemplateModel>.BatchExceptionControl exControl, System.Data.SqlClient.SqlTransaction trans)
        {
            BNE.BLL.DTO.AllInCurriculo load = BNE.BLL.Curriculo.CarregarCurriculoExportacaoAllIn(nextTarget, trans);
            progress.QuantityQuery = progress.QuantityQuery + 1;
            if (load == null)
                return null;

            progress.QuantityLoaded = progress.QuantityLoaded + 1;

            return writerProducerWrapper.StartNew(() =>
            {
                if (exControl.MainException != null)
                    return;

                var convertedModel = Helper.ModelTranslator.TranslateToAllInCurriculoModel(load);
                var res = main.Parse(convertedModel);
                streamWriter.Value.WriteLine(res);
                progress.QuantityProcessed = progress.QuantityProcessed + 1;
                progress.LastTargetId = (int)nextTarget;

            }).ContinueWith((t) =>
            {
                if (t.Exception == null)
                    return;
                exControl.MainException = t.Exception;
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, writerProducerWrapper.Scheduler);
        }
    }
}
