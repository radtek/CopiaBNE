namespace BNE.ValidaCelular.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class codigo11chars : DbMigration
    {
        public override void Up()
        {
            AlterColumn("validacelular.TAB_Codigo_Confirmacao_Celular", "Cod_Confirmacao", c => c.String(nullable: false, maxLength: 11));
        }
        
        public override void Down()
        {
            AlterColumn("validacelular.TAB_Codigo_Confirmacao_Celular", "Cod_Confirmacao", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
