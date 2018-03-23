namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Adicional")]
    public partial class BNE_Tipo_Adicional
    {
        public BNE_Tipo_Adicional()
        {
            BNE_Adicional_Plano = new HashSet<BNE_Adicional_Plano>();
        }

        [Key]
        public int Idf_Tipo_Adicional { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Adicional { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Adicional_Plano> BNE_Adicional_Plano { get; set; }
    }
}
