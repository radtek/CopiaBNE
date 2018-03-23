namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Empresa")]
    public partial class TAB_Empresa
    {
        [Key]
        public int Idf_Empresa { get; set; }

        public int Idf_Grupo_Economico { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        [Required]
        [StringLength(60)]
        public string Raz_Social { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public virtual TAB_Grupo_Economico TAB_Grupo_Economico { get; set; }
    }
}
