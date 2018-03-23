namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Indicacao_Filial")]
    public partial class BNE_Indicacao_Filial
    {
        [Key]
        public int Idf_Indicacao_Filial { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Empresa { get; set; }

        public int Idf_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
