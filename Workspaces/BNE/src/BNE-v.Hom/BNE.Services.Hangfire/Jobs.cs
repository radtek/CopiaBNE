using System;
using BNE.Services.Hangfire.BLL;
using Hangfire;

namespace BNE.Services.Hangfire
{
    public class Jobs
    {

        public static void ConfigureAll()
        {
            RecurringJob.AddOrUpdate("BNE.Services.EnvioCampanha", () => EnvioCampanha.Enviar(), "0 8-20/4 * * *", TimeZoneInfo.Local);
            //MigrarFilial.Do();

            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");

            //Para usar expressões no Cron veja: http://www.cronmaker.com/
            //RecurringJob.AddOrUpdate("BNE.Services.Sitemap", () => JobSitemap.AtualizaSitemap(), Cron.Daily(1, 30));
            //BackgroundJob.Enqueue(() => MigrarFilial.Do());
            //RecurringJob.AddOrUpdate("BNE.Services.MigrarFilial", () => MigrarFilial.Do(), "* 1 * * *", TimeZoneInfo.Local);



            //RecurringJob.RemoveIfExists("BNE.Services.EnvioCampanha");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello", () => Teste(), "* */4 * * *");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello2", () => Teste(), "*/30 0/4 * * *");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello3", () => Teste(), "* 4 * * *");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello3", () => Teste(), "0 */4 * * *");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello4", () => Teste(), "0 8-20/4 * * *");
            //RecurringJob.AddOrUpdate("BNE.Services.Hello5", () => Teste(), "/6 10-16 * * *");
            // RecurringJob.AddOrUpdate("BNE.Services.Hello6", () => Teste(), "0-37/6 17 * * *");

            //minutes, hours, days, months, days of week).
        }
       
    }
}