namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Status_Curriculo_Vaga")]
    public partial class BNE_Status_Curriculo_Vaga
    {
        public BNE_Status_Curriculo_Vaga()
        {
            BNE_Vaga_Candidato = new HashSet<BNE_Vaga_Candidato>();
        }

        [Key]
        public int Idf_Status_Curriculo_Vaga { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Status_Curriculo_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Vaga_Candidato> BNE_Vaga_Candidato { get; set; }
    }
}
