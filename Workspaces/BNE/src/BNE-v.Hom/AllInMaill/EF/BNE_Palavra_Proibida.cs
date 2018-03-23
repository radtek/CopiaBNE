namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Palavra_Proibida")]
    public partial class BNE_Palavra_Proibida
    {
        [Key]
        public int Idf_Palavra_Proibida { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Palavra_Proibida { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
