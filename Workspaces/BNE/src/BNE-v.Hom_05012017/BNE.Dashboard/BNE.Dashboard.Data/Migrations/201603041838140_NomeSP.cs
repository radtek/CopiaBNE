namespace BNE.Dashboard.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NomeSP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dashboard.WindowsService", "StoredProcedureName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dashboard.WindowsService", "StoredProcedureName");
        }
    }
}
