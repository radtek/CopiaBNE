namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Forma_Pagamento")]
    public partial class BNE_Plano_Forma_Pagamento
    {
        public BNE_Plano_Forma_Pagamento()
        {
            BNE_Plano = new HashSet<BNE_Plano>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Plano_Forma_Pagamento { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Plano_Forma_Pagamento { get; set; }

        [Required]
        [StringLength(50)]
        public string Txt_Formula { get; set; }

        public DateTime Dta_Cadasrto { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Plano> BNE_Plano { get; set; }
    }
}
