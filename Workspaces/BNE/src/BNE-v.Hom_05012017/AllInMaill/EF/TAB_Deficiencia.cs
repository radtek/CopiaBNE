namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Deficiencia")]
    public partial class TAB_Deficiencia
    {
        public TAB_Deficiencia()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Deficiencia { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Deficiencia { get; set; }

        public int Cod_Caged { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
