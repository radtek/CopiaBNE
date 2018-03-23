namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.BNE_Tipo_Contrato")]
    public partial class BNE_Tipo_Contrato
    {
        public BNE_Tipo_Contrato()
        {
            BNE_Plano = new HashSet<BNE_Plano>();
        }

        [StringLength(100)]
        public string Des_Tipo_Contrato { get; set; }

        public bool Flg_Inativo { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Contrato { get; set; }

        public virtual ICollection<BNE_Plano> BNE_Plano { get; set; }
    }
}
