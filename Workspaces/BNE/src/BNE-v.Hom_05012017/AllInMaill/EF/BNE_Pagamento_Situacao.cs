namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Pagamento_Situacao")]
    public partial class BNE_Pagamento_Situacao
    {
        public BNE_Pagamento_Situacao()
        {
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Pagamento_Situacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Pagamento_Situacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }
    }
}
