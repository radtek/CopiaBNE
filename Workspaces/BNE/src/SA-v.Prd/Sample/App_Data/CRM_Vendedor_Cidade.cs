namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Vendedor_Cidade
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Cidade { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual CRM_Vendedor CRM_Vendedor { get; set; }
    }
}
