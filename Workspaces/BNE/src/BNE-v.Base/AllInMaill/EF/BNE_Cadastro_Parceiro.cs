namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Cadastro_Parceiro")]
    public partial class BNE_Cadastro_Parceiro
    {
        [Key]
        public int Idf_Cadastro_Parceiro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Login { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Senha { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Parceiro_Tecla { get; set; }

        public virtual BNE_Parceiro_Tecla BNE_Parceiro_Tecla { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }
    }
}
