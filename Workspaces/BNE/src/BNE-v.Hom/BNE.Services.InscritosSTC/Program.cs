using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.InscritosSTC
{
    internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            HostFactory.Run(c =>
            {
                c.ScheduleQuartzJobAsService(
                    q => q.WithJob(() => JobBuilder.Create<InscritosSTC>().Build())
                        .AddTrigger(() => TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(6, 6)).StartAt(DateBuilder.FutureDate(30, 0)).Build()));
            });
        }
    }
}
