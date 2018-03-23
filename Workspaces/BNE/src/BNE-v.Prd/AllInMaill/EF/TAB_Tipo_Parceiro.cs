namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Tipo_Parceiro")]
    public partial class TAB_Tipo_Parceiro
    {
        public TAB_Tipo_Parceiro()
        {
            BNE_Parceiro = new HashSet<BNE_Parceiro>();
            TAB_Filial = new HashSet<TAB_Filial>();
        }

        [Key]
        public int Idf_Tipo_Parceiro { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Parceiro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Parceiro> BNE_Parceiro { get; set; }

        public virtual ICollection<TAB_Filial> TAB_Filial { get; set; }
    }
}
