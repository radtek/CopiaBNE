namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Curso")]
    public partial class TAB_Curso
    {
        public TAB_Curso()
        {
            BNE_Formacao = new HashSet<BNE_Formacao>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Rastreador1 = new HashSet<BNE_Rastreador>();
            TAB_Curso_Fonte = new HashSet<TAB_Curso_Fonte>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Curriculo1 = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Grau_Escolaridade = new HashSet<TAB_Grau_Escolaridade>();
        }

        [Key]
        public int Idf_Curso { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Curso { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Auditado { get; set; }

        public bool Flg_MEC { get; set; }

        public int Idf_Nivel_Curso { get; set; }

        [StringLength(50)]
        public string Cod_Curso { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador1 { get; set; }

        public virtual ICollection<TAB_Curso_Fonte> TAB_Curso_Fonte { get; set; }

        public virtual TAB_Nivel_Curso TAB_Nivel_Curso { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo1 { get; set; }

        public virtual ICollection<TAB_Grau_Escolaridade> TAB_Grau_Escolaridade { get; set; }
    }
}
