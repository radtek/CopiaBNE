namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Filial")]
    public partial class TAB_Filial
    {
        public TAB_Filial()
        {
            BNE_Curriculo_Classificacao = new HashSet<BNE_Curriculo_Classificacao>();
            BNE_Curriculo_Permissao = new HashSet<BNE_Curriculo_Permissao>();
            BNE_Curriculo_Quem_Me_Viu = new HashSet<BNE_Curriculo_Quem_Me_Viu>();
            BNE_Curriculo_Visualizacao = new HashSet<BNE_Curriculo_Visualizacao>();
            BNE_Curriculo_Visualizacao_Historico = new HashSet<BNE_Curriculo_Visualizacao_Historico>();
            BNE_Grupo_Cidade = new HashSet<BNE_Grupo_Cidade>();
            BNE_Intencao_Filial = new HashSet<BNE_Intencao_Filial>();
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
            BNE_Plano_Adquirido = new HashSet<BNE_Plano_Adquirido>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Usuario = new HashSet<BNE_Usuario>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Atividade = new HashSet<TAB_Atividade>();
            TAB_Integrador = new HashSet<TAB_Integrador>();
            TAB_Filial_Observacao = new HashSet<TAB_Filial_Observacao>();
            TAB_Origem_Filial = new HashSet<TAB_Origem_Filial>();
            TAB_Filial_BNE = new HashSet<TAB_Filial_BNE>();
            TAB_Integrador_Curriculo = new HashSet<TAB_Integrador_Curriculo>();
            TAB_Parametro_Filial = new HashSet<TAB_Parametro_Filial>();
            TAB_Filial_Logo = new HashSet<TAB_Filial_Logo>();
            TAB_Usuario_Filial_Perfil = new HashSet<TAB_Usuario_Filial_Perfil>();
        }

        [Key]
        public int Idf_Filial { get; set; }

        public bool Flg_Matriz { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        [Required]
        [StringLength(100)]
        public string Raz_Social { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Fantasia { get; set; }

        public int? Idf_CNAE_Principal { get; set; }

        public int? Idf_Natureza_Juridica { get; set; }

        public int Idf_Endereco { get; set; }

        [StringLength(100)]
        public string End_Site { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Comercial { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Comercial { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public int? Qtd_Usuario_Adicional { get; set; }

        public int Qtd_Funcionarios { get; set; }

        [Required]
        [StringLength(15)]
        public string Des_IP { get; set; }

        public bool Flg_Oferece_Cursos { get; set; }

        public int Idf_Situacao_Filial { get; set; }

        [StringLength(200)]
        public string Des_Pagina_Facebook { get; set; }

        [StringLength(10)]
        public string Num_Comercial2 { get; set; }

        public DbGeography Des_Localizacao { get; set; }

        public int? Idf_Tipo_Parceiro { get; set; }

        public virtual ICollection<BNE_Curriculo_Classificacao> BNE_Curriculo_Classificacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Permissao> BNE_Curriculo_Permissao { get; set; }

        public virtual ICollection<BNE_Curriculo_Quem_Me_Viu> BNE_Curriculo_Quem_Me_Viu { get; set; }

        public virtual ICollection<BNE_Curriculo_Visualizacao> BNE_Curriculo_Visualizacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Visualizacao_Historico> BNE_Curriculo_Visualizacao_Historico { get; set; }

        public virtual BNE_Empresa_Home BNE_Empresa_Home { get; set; }

        public virtual ICollection<BNE_Grupo_Cidade> BNE_Grupo_Cidade { get; set; }

        public virtual ICollection<BNE_Intencao_Filial> BNE_Intencao_Filial { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido> BNE_Plano_Adquirido { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Usuario> BNE_Usuario { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Atividade> TAB_Atividade { get; set; }

        public virtual TAB_Endereco TAB_Endereco { get; set; }

        public virtual ICollection<TAB_Integrador> TAB_Integrador { get; set; }

        public virtual TAB_Tipo_Parceiro TAB_Tipo_Parceiro { get; set; }

        public virtual TAB_CNAE_Sub_Classe TAB_CNAE_Sub_Classe { get; set; }

        public virtual ICollection<TAB_Filial_Observacao> TAB_Filial_Observacao { get; set; }

        public virtual ICollection<TAB_Origem_Filial> TAB_Origem_Filial { get; set; }

        public virtual TAB_Situacao_Filial TAB_Situacao_Filial { get; set; }

        public virtual ICollection<TAB_Filial_BNE> TAB_Filial_BNE { get; set; }

        public virtual ICollection<TAB_Integrador_Curriculo> TAB_Integrador_Curriculo { get; set; }

        public virtual TAB_Natureza_Juridica TAB_Natureza_Juridica { get; set; }

        public virtual ICollection<TAB_Parametro_Filial> TAB_Parametro_Filial { get; set; }

        public virtual ICollection<TAB_Filial_Logo> TAB_Filial_Logo { get; set; }

        public virtual ICollection<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }
    }
}
