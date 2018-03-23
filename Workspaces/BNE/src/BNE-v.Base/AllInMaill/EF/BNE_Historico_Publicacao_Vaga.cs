namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Historico_Publicacao_Vaga")]
    public partial class BNE_Historico_Publicacao_Vaga
    {
        [Key]
        public int Idf_Historico_Publicacao_Vaga { get; set; }

        public int Idf_Vaga { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_Historico { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
