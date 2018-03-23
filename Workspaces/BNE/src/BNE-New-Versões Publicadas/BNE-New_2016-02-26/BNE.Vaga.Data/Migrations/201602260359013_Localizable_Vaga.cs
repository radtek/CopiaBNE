namespace BNE.Vaga.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Localizable_Vaga : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "vaga.Translation",
                c => new
                    {
                        Type = c.String(nullable: false, maxLength: 150, unicode: false),
                        FieldName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LanguageCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        PrimaryKeyValue = c.String(nullable: false, maxLength: 15, unicode: false),
                        Text = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => new { t.Type, t.FieldName, t.LanguageCode, t.PrimaryKeyValue });
            
        }
        
        public override void Down()
        {
            DropTable("vaga.Translation");
        }
    }
}
