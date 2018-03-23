namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Tipo_Vinculo")]
    public partial class BNE_Vaga_Tipo_Vinculo
    {
        [Key]
        public int Idf_Vaga_Tipo_Vinculo { get; set; }

        public int Idf_Tipo_Vinculo { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Tipo_Vinculo BNE_Tipo_Vinculo { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
