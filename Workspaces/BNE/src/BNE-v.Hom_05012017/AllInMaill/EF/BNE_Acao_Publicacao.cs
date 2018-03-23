namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Acao_Publicacao")]
    public partial class BNE_Acao_Publicacao
    {
        public BNE_Acao_Publicacao()
        {
            BNE_Regra_Publicacao = new HashSet<BNE_Regra_Publicacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Acao_Publicacao { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Acao_Publicacao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Regra_Publicacao> BNE_Regra_Publicacao { get; set; }
    }
}
