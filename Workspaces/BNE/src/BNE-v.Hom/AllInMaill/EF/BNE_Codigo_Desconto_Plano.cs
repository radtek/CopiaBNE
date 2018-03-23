namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Codigo_Desconto_Plano")]
    public partial class BNE_Codigo_Desconto_Plano
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Codigo_Desconto { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Plano { get; set; }

        public int? Num_Percentual_Desconto { get; set; }

        public int? Qtd_SMS { get; set; }

        public virtual BNE_Plano BNE_Plano { get; set; }

        public virtual BNE_Tipo_Codigo_Desconto BNE_Tipo_Codigo_Desconto { get; set; }
    }
}
