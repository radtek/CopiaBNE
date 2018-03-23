namespace AdminLTE_Application
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("estagio.CRM_Log_Carga")]
    public partial class CRM_Log_Carga1
    {
        [Key]
        public int Idf_Log_Carga { get; set; }

        public DateTime Dta_Log_Carga { get; set; }
    }
}
