namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Licenca_Cobre_Bem")]
    public partial class GLO_Licenca_Cobre_Bem
    {
        [Key]
        public int Idf_Licenca_Cobre_Bem { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        public string Arq_Licenca { get; set; }
    }
}
