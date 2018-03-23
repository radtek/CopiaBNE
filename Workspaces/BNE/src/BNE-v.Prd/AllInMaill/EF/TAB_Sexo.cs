namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Sexo")]
    public partial class TAB_Sexo
    {
        public TAB_Sexo()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Sexo { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Sexo { get; set; }

        [Required]
        [StringLength(1)]
        public string Sig_Sexo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
