using System;
using System.Reflection;
using log4net;
using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.NotificarClienteVagaComPoucasCandidaturas
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static void Main()
        {
            Logger.Info("Starting service...");

            try
            {
                HostFactory.Run(c =>
                {
                    c.ScheduleQuartzJobAsService(q =>
                    {
                        Logger.Info("Configurando job...");

                        q.WithJob(() => JobBuilder.Create<Job>().WithIdentity("NotificarClienteVagaComPoucasCandidaturas", null).Build());
                        //q.AddTrigger(() => TriggerBuilder.Create().StartNow().Build());
                        q.AddTrigger(() => TriggerBuilder.Create().WithCronSchedule("0 0 20 1/1 * ? *").Build());

                        Logger.Info("Configuração finalizada da job...");
                    });
                });
            }
            catch (Exception ex)
            {
                Logger.Fatal("Ferrou!", ex);
                throw;
            }
            Logger.Info("Stopping service...");
        }
    }
}
