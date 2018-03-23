namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Substituicao_Integracao")]
    public partial class TAB_Substituicao_Integracao
    {
        [Key]
        [Column(Order = 0)]
        public int Idf_Substituicao_Integracao { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string Des_Antiga { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(500)]
        public string Des_Nova { get; set; }

        public int? Idf_Regra_Substituicao_Integracao { get; set; }

        public virtual TAB_Regra_Substituicao_Integracao TAB_Regra_Substituicao_Integracao { get; set; }
    }
}
