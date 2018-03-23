namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Escolaridade")]
    public partial class TAB_Escolaridade
    {
        public TAB_Escolaridade()
        {
            BNE_Formacao = new HashSet<BNE_Formacao>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
            TAB_Funcao = new HashSet<TAB_Funcao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Escolaridade { get; set; }

        [StringLength(50)]
        public string Des_Geral { get; set; }

        [StringLength(50)]
        public string Des_BNE { get; set; }

        [StringLength(50)]
        public string Des_Rais { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Cod_Rais { get; set; }

        public int Cod_Caged { get; set; }

        public int Cod_GRRF { get; set; }

        public bool Flg_BNE { get; set; }

        public bool Flg_Folha { get; set; }

        public int? Seq_BNE { get; set; }

        [Required]
        [StringLength(8)]
        public string Des_Abreviada { get; set; }

        public int Idf_Grau_Escolaridade { get; set; }

        public bool Flg_Escolaridade_Completa { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public short? Seq_Peso { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }

        public virtual ICollection<TAB_Funcao> TAB_Funcao { get; set; }

        public virtual TAB_Grau_Escolaridade TAB_Grau_Escolaridade { get; set; }
    }
}
