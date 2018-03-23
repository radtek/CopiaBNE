using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.CampanhaRecrutamento
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.ScheduleQuartzJobAsService(
                    q => q.WithJob(() => JobBuilder.Create<CampanhaRecrutamentoJob>().Build())
                        .AddTrigger(() => TriggerBuilder.Create().WithCronSchedule("0 0 8-20/4 * * ?").Build()));
            });
        }
    }
}
