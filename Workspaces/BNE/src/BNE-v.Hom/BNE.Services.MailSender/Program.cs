using Quartz;
using Topshelf;
using Topshelf.Quartz;

namespace BNE.Services.MailSender
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(c =>
            {
                c.Service<ContainerService>(s =>
                {
                    s.ConstructUsing(name => new ContainerService());
                    s.WhenStarted((service, control) => service.Start());
                    s.WhenStopped((service, control) => service.Stop());

                    s.ScheduleQuartzJob(q => q.WithJob(
                               () => JobBuilder.Create<MensagemCSJob>().Build()).AddTrigger(
                               () => TriggerBuilder.Create().StartNow().WithIdentity("MensagemCSJob", null).Build()));

                });

            });
        }

        public class ContainerService
        {
            public bool Start()
            {
                return true;
            }

            public bool Stop()
            {
                return true;
            }
        }
    }
}