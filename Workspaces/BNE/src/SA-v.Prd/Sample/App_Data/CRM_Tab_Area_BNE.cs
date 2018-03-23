namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Tab_Area_BNE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Area_BNE { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Area_BNE { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
