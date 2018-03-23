namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Rede_Social")]
    public partial class BNE_Vaga_Rede_Social
    {
        [Key]
        public int Idf_Vaga_Rede_Social { get; set; }

        [Required]
        [StringLength(2000)]
        public string Des_Vaga_Rede_Social { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Rede_Social_cs { get; set; }

        public virtual BNE_Rede_Social_CS BNE_Rede_Social_CS { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
