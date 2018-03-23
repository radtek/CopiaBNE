namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Parcela_Situacao")]
    public partial class BNE_Plano_Parcela_Situacao
    {
        public BNE_Plano_Parcela_Situacao()
        {
            BNE_Plano_Parcela = new HashSet<BNE_Plano_Parcela>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Plano_Parcela_Situacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Status_Pagamento { get; set; }

        public DateTime Dta_cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Plano_Parcela> BNE_Plano_Parcela { get; set; }
    }
}
