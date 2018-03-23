namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Resposta_Automatica")]
    public partial class BNE_Resposta_Automatica
    {
        [Key]
        public int Idf_Resposta_Automatica { get; set; }

        [Required]
        [StringLength(1000)]
        public string Des_Resposta_Automatica { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(255)]
        public string Nme_Resposta_Automatica { get; set; }
    }
}
