namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Avaliacao")]
    public partial class TAB_Avaliacao
    {
        public TAB_Avaliacao()
        {
            BNE_Curriculo_Classificacao = new HashSet<BNE_Curriculo_Classificacao>();
        }

        [Key]
        public int Idf_Avaliacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Avaliacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Curriculo_Classificacao> BNE_Curriculo_Classificacao { get; set; }
    }
}
