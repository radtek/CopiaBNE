namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Motivo_Rescisao")]
    public partial class TAB_Motivo_Rescisao
    {
        public TAB_Motivo_Rescisao()
        {
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Motivo_Rescisao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Motivo_Rescisao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(150)]
        public string Sig_Causa_Afastamento { get; set; }

        [StringLength(4)]
        public string Sig_Codigo_Afastamento { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
