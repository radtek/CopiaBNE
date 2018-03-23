namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Fonte")]
    public partial class TAB_Fonte
    {
        public TAB_Fonte()
        {
            BNE_Formacao = new HashSet<BNE_Formacao>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Rastreador1 = new HashSet<BNE_Rastreador>();
            TAB_Curso_Fonte = new HashSet<TAB_Curso_Fonte>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Curriculo1 = new HashSet<TAB_Pesquisa_Curriculo>();
        }

        [Key]
        public int Idf_Fonte { get; set; }

        [Required]
        [StringLength(20)]
        public string Sig_Fonte { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Fonte { get; set; }

        public bool Flg_Auditada { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_MEC { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_cnpj { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador1 { get; set; }

        public virtual ICollection<TAB_Curso_Fonte> TAB_Curso_Fonte { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo1 { get; set; }
    }
}
