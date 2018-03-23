using System;
using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.JornalVagasPopup
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(c =>
            {
                c.ScheduleQuartzJobAsService(
                    q => q.WithJob(() => JobBuilder.Create<JornalVagasPopupJob>().Build())
                        .AddTrigger(() => TriggerBuilder.Create().WithSchedule(CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(6, 0, DayOfWeek.Tuesday, DayOfWeek.Friday)).Build()));
            });
        }
    }
}