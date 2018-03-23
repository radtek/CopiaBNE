namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Candidato_Pergunta")]
    public partial class BNE_Vaga_Candidato_Pergunta
    {
        [Key]
        public int Idf_Vaga_Candidato_Pergunta { get; set; }

        public int Idf_Vaga_Pergunta { get; set; }

        public int Idf_Vaga_Candidato { get; set; }

        public bool Flg_Resposta { get; set; }

        public virtual BNE_Vaga_Candidato BNE_Vaga_Candidato { get; set; }

        public virtual BNE_Vaga_Pergunta BNE_Vaga_Pergunta { get; set; }
    }
}
