namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.BNE_Integracao_Situacao")]
    public partial class BNE_Integracao_Situacao
    {
        public BNE_Integracao_Situacao()
        {
            BNE_Integracao = new HashSet<BNE_Integracao>();
            BNE_Integracao_Admissao = new HashSet<BNE_Integracao_Admissao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Integracao_Situacao { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Integracao_Situacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }

        public virtual ICollection<BNE_Integracao_Admissao> BNE_Integracao_Admissao { get; set; }
    }
}
