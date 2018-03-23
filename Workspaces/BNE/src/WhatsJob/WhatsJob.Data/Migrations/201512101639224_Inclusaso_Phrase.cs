namespace WhatsJob.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusaso_Phrase : DbMigration
    {
        public override void Up()
        {
            AddColumn("WhatsJob.Phrase", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("WhatsJob.Phrase", "Ativo");
        }
    }
}
