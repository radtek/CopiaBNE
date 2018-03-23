namespace APIGateway.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AutenticacaoOpcional : DbMigration
    {
        public override void Up()
        {
            DropIndex("ApiGateway.Api", new[] { "AuthenticationType_Interface" });
            AlterColumn("ApiGateway.Api", "AuthenticationType_Interface", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("ApiGateway.Api", "AuthenticationType_Interface");
        }
        
        public override void Down()
        {
            DropIndex("ApiGateway.Api", new[] { "AuthenticationType_Interface" });
            AlterColumn("ApiGateway.Api", "AuthenticationType_Interface", c => c.String(nullable: false, maxLength: 20, unicode: false));
            CreateIndex("ApiGateway.Api", "AuthenticationType_Interface");
        }
    }
}
