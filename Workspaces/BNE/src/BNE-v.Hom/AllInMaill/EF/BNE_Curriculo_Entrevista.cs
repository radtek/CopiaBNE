namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Entrevista")]
    public partial class BNE_Curriculo_Entrevista
    {
        [Key]
        public int Idf_Curriculo_Entrevista { get; set; }

        public int? Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Mensagem_CS { get; set; }

        public int Idf_Curriculo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
