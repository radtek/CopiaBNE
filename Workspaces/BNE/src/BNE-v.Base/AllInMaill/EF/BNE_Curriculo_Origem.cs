namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Origem")]
    public partial class BNE_Curriculo_Origem
    {
        public int Idf_Origem { get; set; }

        [Key]
        public int Idf_Curriculo_Origem { get; set; }

        public int Idf_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Origem TAB_Origem { get; set; }
    }
}
