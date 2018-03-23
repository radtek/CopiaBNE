namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllowAnonymousToEndpoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("ApiGateway.Endpoint", "AllowAnonymous", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("ApiGateway.Endpoint", "AllowAnonymous");
        }
    }
}
