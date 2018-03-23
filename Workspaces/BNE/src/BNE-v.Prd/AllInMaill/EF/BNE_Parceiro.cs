namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Parceiro")]
    public partial class BNE_Parceiro
    {
        public BNE_Parceiro()
        {
            BNE_Codigo_Desconto = new HashSet<BNE_Codigo_Desconto>();
        }

        [Key]
        public int Idf_Parceiro { get; set; }

        public int Idf_Tipo_Parceiro { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Parceiro { get; set; }

        public bool Flg_Empresa { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        [StringLength(200)]
        public string Des_Email { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto> BNE_Codigo_Desconto { get; set; }

        public virtual TAB_Tipo_Parceiro TAB_Tipo_Parceiro { get; set; }
    }
}
