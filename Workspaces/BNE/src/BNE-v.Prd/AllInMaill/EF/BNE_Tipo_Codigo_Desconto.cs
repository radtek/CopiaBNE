namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Codigo_Desconto")]
    public partial class BNE_Tipo_Codigo_Desconto
    {
        public BNE_Tipo_Codigo_Desconto()
        {
            BNE_Codigo_Desconto = new HashSet<BNE_Codigo_Desconto>();
            BNE_Codigo_Desconto_Plano = new HashSet<BNE_Codigo_Desconto_Plano>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Codigo_Desconto { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Codigo_Desconto { get; set; }

        public int Num_Percentual_Desconto { get; set; }

        public int? Num_Dias_Validade { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto> BNE_Codigo_Desconto { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto_Plano> BNE_Codigo_Desconto_Plano { get; set; }
    }
}
