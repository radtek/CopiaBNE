namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InclusaoDestinationRelativePath : DbMigration
    {
        public override void Up()
        {
            AddColumn("ApiGateway.Endpoint", "DestinationRelativePath", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("ApiGateway.Endpoint", "DestinationRelativePath");
        }
    }
}
