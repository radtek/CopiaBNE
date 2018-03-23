namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Operadora")]
    public partial class BNE_Operadora
    {
        public BNE_Operadora()
        {
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
            BNE_Transacao = new HashSet<BNE_Transacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Operadora { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Operadora { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual ICollection<BNE_Transacao> BNE_Transacao { get; set; }
    }
}
