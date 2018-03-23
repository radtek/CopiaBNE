namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Pergunta")]
    public partial class BNE_Vaga_Pergunta
    {
        public BNE_Vaga_Pergunta()
        {
            BNE_Vaga_Candidato_Pergunta = new HashSet<BNE_Vaga_Candidato_Pergunta>();
        }

        [Key]
        public int Idf_Vaga_Pergunta { get; set; }

        [Required]
        [StringLength(140)]
        public string Des_Vaga_Pergunta { get; set; }

        public bool Flg_Resposta { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_inativo { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }

        public virtual ICollection<BNE_Vaga_Candidato_Pergunta> BNE_Vaga_Candidato_Pergunta { get; set; }
    }
}
