namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador_Resultado_Vaga")]
    public partial class BNE_Rastreador_Resultado_Vaga
    {
        [Key]
        public int Idf_Rastreador_Resultado_Vaga { get; set; }

        public int Idf_Rastreador_Vaga { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Flg_Inativo { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
