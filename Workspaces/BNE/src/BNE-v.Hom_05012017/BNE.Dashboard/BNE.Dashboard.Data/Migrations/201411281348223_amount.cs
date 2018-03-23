namespace BNE.Dashboard.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dashboard.Watcher", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dashboard.Watcher", "Amount");
        }
    }
}
