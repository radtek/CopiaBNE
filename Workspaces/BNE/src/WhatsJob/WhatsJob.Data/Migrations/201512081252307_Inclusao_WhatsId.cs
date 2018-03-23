namespace WhatsJob.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_WhatsId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("WhatsJob.Message");
            AddColumn("WhatsJob.Message", "WhatsId", c => c.String(maxLength: 150, unicode: false));
            AlterColumn("WhatsJob.Message", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("WhatsJob.Message", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("WhatsJob.Message");
            AlterColumn("WhatsJob.Message", "Id", c => c.String(nullable: false, maxLength: 150, unicode: false));
            DropColumn("WhatsJob.Message", "WhatsId");
            AddPrimaryKey("WhatsJob.Message", "Id");
        }
    }
}
