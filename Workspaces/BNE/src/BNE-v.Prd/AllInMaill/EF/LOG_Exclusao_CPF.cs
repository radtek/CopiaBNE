namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.LOG_Exclusao_CPF")]
    public partial class LOG_Exclusao_CPF
    {
        [Key]
        public int Idf_Exclusao_CPF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime Dta_Exclusao { get; set; }

        [StringLength(200)]
        public string Des_Usuario_Exclusao { get; set; }

        [StringLength(300)]
        public string Nme_Maquina { get; set; }
    }
}
