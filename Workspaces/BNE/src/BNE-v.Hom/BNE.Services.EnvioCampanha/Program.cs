using System;
using System.Reflection;
using log4net;
using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.EnvioCampanha
{
    internal class Program
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
                        Logger.Info("Configurando job para o envio de email para campanha");

                        q.WithJob(() => JobBuilder.Create<EmailJob>().WithIdentity("EmailJob", null).Build());
                        q.AddTrigger(() => TriggerBuilder.Create().StartNow().WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForever(1)).Build());

                        Logger.Info("Configuração finalizada da job para o envio de email para campanha");
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