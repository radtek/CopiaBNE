namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Divulgacao")]
    public partial class BNE_Vaga_Divulgacao
    {
        [Key]
        public int Idf_Vaga_Divulgacao { get; set; }

        public int Idf_Vaga { get; set; }

        public int Idf_Curriculo { get; set; }

        public int? Idf_Rede_Social_CS { get; set; }

        public DateTime? Dta_Envio { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Rede_Social_CS BNE_Rede_Social_CS { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
