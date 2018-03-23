using BNE.Dashboard.Entities;

namespace BNE.Dashboard.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DashboardEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DashboardEntities context)
        {
            //context.MessageQueue.AddOrUpdate(m => m.MessageQueueName, new MessageQueue
            //{
            //    MaximumMessageCount = 5,
            //    MessageQueueName = "teste",
            //    MessageQueueServer = "10.1.2.70"
            //});

            //context.Watcher.AddOrUpdate(m => m.MessageQueue.MessageQueueName, new Watcher
            //{
            //    TimeToRefresh = 5000,
            //    Service = context.MessageQueue.FirstOrDefault(m => m.MessageQueueName == "teste")
            //});

            context.GoogleAnalyticsSites.AddOrUpdate(m => m.Site, new GoogleAnalyticsSites
            {
                Site = "www.bne.com.br",
                ViewID = "44866307"
            });

            context.GoogleAnalyticsSites.AddOrUpdate(m => m.Site, new GoogleAnalyticsSites
            {
                Site = "www.sine.com.br",
                ViewID = "38509689"
            });

            context.Status.AddOrUpdate(m => m.Name, new Status
            {
                Name = "OK",
                StatusId = 1,
            });

            context.Status.AddOrUpdate(m => m.Name, new Status
            {
                Name = "ERROR",
                StatusId = 2,

            });

            #region Queues
            context.MessageQueue.AddOrUpdate(m => m.MessageQueueName, new MessageQueue
            {
                MaximumMessageCount = 5000,
                MessageQueueName = "bne_envioemail",
                MessageQueueServer = "EMPVW0355"
            });

            context.Watcher.AddOrUpdate(m => m.Name, new Watcher
            {
                Name = "BNE - Queue Envio E-Mail",
                MessageQueue = context.MessageQueue.First(m => m.MessageQueueName == "bne_envioemail"),
            });
            #endregion

            //     VerificarPlanoNaoEncerradoPessoaFisica = 1,
            //VerificarPlanoNaoEncerradoPessoaJuridica = 2


            context.SaveChanges();
        }
    }
}
