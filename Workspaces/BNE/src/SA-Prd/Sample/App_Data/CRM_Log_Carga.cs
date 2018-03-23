namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Log_Carga
    {
        [Key]
        public int Idf_Log_Carga { get; set; }

        public DateTime Dta_Log_Carga { get; set; }
    }
}
