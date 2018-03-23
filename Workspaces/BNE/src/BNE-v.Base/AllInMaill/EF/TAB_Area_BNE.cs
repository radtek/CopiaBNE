namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Area_BNE")]
    public partial class TAB_Area_BNE
    {
        public TAB_Area_BNE()
        {
            BNE_Experiencia_Profissional = new HashSet<BNE_Experiencia_Profissional>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Funcao = new HashSet<TAB_Funcao>();
            TAB_CNAE_Divisao = new HashSet<TAB_CNAE_Divisao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Area_BNE { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Area_BNE { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Area_BNE_Pesquisa { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Experiencia_Profissional> BNE_Experiencia_Profissional { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<TAB_Funcao> TAB_Funcao { get; set; }

        public virtual ICollection<TAB_CNAE_Divisao> TAB_CNAE_Divisao { get; set; }
    }
}
