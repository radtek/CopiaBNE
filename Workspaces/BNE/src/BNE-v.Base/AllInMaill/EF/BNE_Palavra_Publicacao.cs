namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Palavra_Publicacao")]
    public partial class BNE_Palavra_Publicacao
    {
        public BNE_Palavra_Publicacao()
        {
            BNE_Regra_Publicacao = new HashSet<BNE_Regra_Publicacao>();
        }

        [Key]
        public int Idf_Palavra_Publicacao { get; set; }

        [Required]
        [StringLength(300)]
        public string Des_Palavra_Publicacao { get; set; }

        [StringLength(200)]
        public string Des_Palavra_Substituicao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Regra_Publicacao> BNE_Regra_Publicacao { get; set; }
    }
}
