namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Status_Codigo_Desconto")]
    public partial class TAB_Status_Codigo_Desconto
    {
        public TAB_Status_Codigo_Desconto()
        {
            BNE_Codigo_Desconto = new HashSet<BNE_Codigo_Desconto>();
        }

        [Key]
        public int Idf_Status_Codigo_Desconto { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Status_Codigo_Desconto { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto> BNE_Codigo_Desconto { get; set; }
    }
}
