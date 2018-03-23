namespace APIGateway.Data.Migrations
{
    using APIGateway.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<APIGateway.Data.APIGatewayContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(APIGateway.Data.APIGatewayContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Authentication.AddOrUpdate(
            //    new Authentication { Interface = "Base64ApiKey", Descricao = "Authenticação através do token do BNE" }
            //);

            //var auths = context
            //    .Authentication
            //    .ToDictionary(c => c, c => c.Nome);

            //context.Api.AddOrUpdate(
            //    new Api { AuthenticationType = new Authentication { Interface = "Base64ApiKey" }, BaseUrl = "http://localhost", UrlSuffix = "apiBne" }
            //);
        }
    }
}
