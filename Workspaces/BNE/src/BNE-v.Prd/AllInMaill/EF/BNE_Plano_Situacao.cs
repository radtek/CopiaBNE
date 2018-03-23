namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Situacao")]
    public partial class BNE_Plano_Situacao
    {
        public BNE_Plano_Situacao()
        {
            BNE_Plano_Adquirido = new HashSet<BNE_Plano_Adquirido>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Plano_Situacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Plano_Situacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido> BNE_Plano_Adquirido { get; set; }
    }
}
