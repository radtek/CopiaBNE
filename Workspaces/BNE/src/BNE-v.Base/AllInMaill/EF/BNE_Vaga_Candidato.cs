namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Candidato")]
    public partial class BNE_Vaga_Candidato
    {
        public BNE_Vaga_Candidato()
        {
            BNE_Vaga_Candidato_Pergunta = new HashSet<BNE_Vaga_Candidato_Pergunta>();
        }

        public int Idf_Curriculo { get; set; }

        [Key]
        public int Idf_Vaga_Candidato { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Status_Curriculo_Vaga { get; set; }

        public bool? Flg_Auto_Candidatura { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Status_Curriculo_Vaga BNE_Status_Curriculo_Vaga { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }

        public virtual ICollection<BNE_Vaga_Candidato_Pergunta> BNE_Vaga_Candidato_Pergunta { get; set; }
    }
}
