using System;
using System.Reflection;
using log4net;
using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.JornalVagas
{
    internal static class Program
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
                        Logger.Info("Configurando job para o jornal");

                        //q.WithJob(() => JobBuilder.Create<JornalVagasJob>().WithIdentity("JornalVagas", null).Build());
                        q.AddTrigger(() => TriggerBuilder.Create().WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(10)).Build());

                        Logger.Info("Configuração finalizada da job para o jornal");
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