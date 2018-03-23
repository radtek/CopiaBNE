namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Situacao_Formacao")]
    public partial class BNE_Situacao_Formacao
    {
        public BNE_Situacao_Formacao()
        {
            BNE_Formacao = new HashSet<BNE_Formacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Idf_Situacao_Formacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Situacao_Formacao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }
    }
}
