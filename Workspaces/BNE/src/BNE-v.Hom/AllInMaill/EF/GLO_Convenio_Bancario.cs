namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Convenio_Bancario")]
    public partial class GLO_Convenio_Bancario
    {
        [Key]
        public int Idf_Convenio_bancario { get; set; }

        public int? Idf_Banco { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        [StringLength(20)]
        public string Num_Convenio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Sequencia { get; set; }

        public virtual TAB_Banco TAB_Banco { get; set; }
    }
}
