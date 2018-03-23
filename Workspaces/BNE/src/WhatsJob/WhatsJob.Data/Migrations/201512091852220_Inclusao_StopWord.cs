namespace WhatsJob.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_StopWord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "WhatsJob.StopWord",
                c => new
                    {
                        Word = c.String(nullable: false, maxLength: 128, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Word);
            
            AddColumn("WhatsJob.Channel", "Ativo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("WhatsJob.Channel", "Ativo");
            DropTable("WhatsJob.StopWord");
        }
    }
}
