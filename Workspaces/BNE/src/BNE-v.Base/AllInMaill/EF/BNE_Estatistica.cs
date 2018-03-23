namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Estatistica")]
    public partial class BNE_Estatistica
    {
        [Key]
        public int Idf_Estatistica { get; set; }

        public long Qtd_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public long? Qtd_Vaga { get; set; }

        public long Qtd_Empresa { get; set; }
    }
}
