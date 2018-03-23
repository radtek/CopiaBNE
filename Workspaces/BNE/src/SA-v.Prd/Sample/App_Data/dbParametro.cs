namespace AdminLTE_Application
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("dbo.Parametro")]
    public partial class dbParametro
    {
        [Key]
        public Int16 Id_Parametro { get; set; }

        public string Descricao { get; set; }

        public string Valor { get; set; }

    }
}
