namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Adicional_Plano_Situacao")]
    public partial class BNE_Adicional_Plano_Situacao
    {
        public BNE_Adicional_Plano_Situacao()
        {
            BNE_Adicional_Plano = new HashSet<BNE_Adicional_Plano>();
        }

        [Key]
        public int Idf_Adicional_Plano_Situacao { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Adicional_Plano_Situacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Adicional_Plano> BNE_Adicional_Plano { get; set; }
    }
}
