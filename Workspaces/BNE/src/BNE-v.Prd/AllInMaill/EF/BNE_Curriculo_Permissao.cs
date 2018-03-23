namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Permissao")]
    public partial class BNE_Curriculo_Permissao
    {
        public int Idf_Filial { get; set; }

        [Key]
        public int Idf_Curriculo_Permissao { get; set; }

        public int Idf_Curriculo { get; set; }

        public bool Flg_Invisivel { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        [Required]
        [StringLength(15)]
        public string Des_IP { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
