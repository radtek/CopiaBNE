namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Funcao")]
    public partial class TAB_Funcao
    {
        public TAB_Funcao()
        {
            BNE_Amplitude_Salarial = new HashSet<BNE_Amplitude_Salarial>();
            BNE_Curso_Funcao_Tecla = new HashSet<BNE_Curso_Funcao_Tecla>();
            BNE_Experiencia_Profissional = new HashSet<BNE_Experiencia_Profissional>();
            BNE_Funcao_Erro_Sinonimo = new HashSet<BNE_Funcao_Erro_Sinonimo>();
            BNE_Funcao_Pretendida = new HashSet<BNE_Funcao_Pretendida>();
            BNE_Plano_Adquirido_Detalhes = new HashSet<BNE_Plano_Adquirido_Detalhes>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Rastreador_Vaga = new HashSet<BNE_Rastreador_Vaga>();
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            BNE_Simulacao_R1 = new HashSet<BNE_Simulacao_R1>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Solicitacao_R11 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Usuario_Filial = new HashSet<BNE_Usuario_Filial>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Funcao_Mini_Agrupadora = new HashSet<TAB_Funcao_Mini_Agrupadora>();
            TAB_Origem_Filial_Funcao = new HashSet<TAB_Origem_Filial_Funcao>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Salarial = new HashSet<TAB_Pesquisa_Salarial>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Usuario_Funcao = new HashSet<TAB_Usuario_Funcao>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
            TAB_Funcao1 = new HashSet<TAB_Funcao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Funcao { get; set; }

        public int? Idf_Funcao_Agrupadora { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Funcao { get; set; }

        public int Idf_Funcao_Categoria { get; set; }

        public int? Idf_Classe_Salarial { get; set; }

        public int? Idf_Escolaridade { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public bool? Flg_Menor_Aprendiz { get; set; }

        [StringLength(2000)]
        public string Des_Job { get; set; }

        [StringLength(2000)]
        public string Des_Experiencia { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Piso_Profissional { get; set; }

        [StringLength(2000)]
        public string Des_Cursos { get; set; }

        [StringLength(2000)]
        public string Des_Competencias { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Funcao_Pesquisa { get; set; }

        [StringLength(10)]
        public string Cod_Funcao_Folha { get; set; }

        [Required]
        [StringLength(6)]
        public string Cod_CBO { get; set; }

        public bool Flg_Conselho { get; set; }

        public bool Flg_Curso_Especializacao { get; set; }

        public bool? Flg_RAIS_Analfabeto { get; set; }

        public bool? Flg_RAIS_Nivel_Superior { get; set; }

        public bool Flg_Categoria_Sindical { get; set; }

        public bool Flg_Auditada { get; set; }

        public short? Qtd_Carga_Horaria_Diaria { get; set; }

        [StringLength(2000)]
        public string Des_Local_Trabalho { get; set; }

        [StringLength(2000)]
        public string Des_EPI { get; set; }

        [StringLength(2000)]
        public string Des_PPRA { get; set; }

        [StringLength(2000)]
        public string Des_PCMSO { get; set; }

        [StringLength(2000)]
        public string Des_Equipamentos { get; set; }

        public virtual ICollection<BNE_Amplitude_Salarial> BNE_Amplitude_Salarial { get; set; }

        public virtual ICollection<BNE_Curso_Funcao_Tecla> BNE_Curso_Funcao_Tecla { get; set; }

        public virtual ICollection<BNE_Experiencia_Profissional> BNE_Experiencia_Profissional { get; set; }

        public virtual ICollection<BNE_Funcao_Erro_Sinonimo> BNE_Funcao_Erro_Sinonimo { get; set; }

        public virtual ICollection<BNE_Funcao_Pretendida> BNE_Funcao_Pretendida { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido_Detalhes> BNE_Plano_Adquirido_Detalhes { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Rastreador_Vaga> BNE_Rastreador_Vaga { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<BNE_Simulacao_R1> BNE_Simulacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R11 { get; set; }

        public virtual ICollection<BNE_Usuario_Filial> BNE_Usuario_Filial { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Funcao_Mini_Agrupadora> TAB_Funcao_Mini_Agrupadora { get; set; }

        public virtual ICollection<TAB_Origem_Filial_Funcao> TAB_Origem_Filial_Funcao { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Salarial> TAB_Pesquisa_Salarial { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<TAB_Usuario_Funcao> TAB_Usuario_Funcao { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Classe_Salarial TAB_Classe_Salarial { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Funcao_Categoria TAB_Funcao_Categoria { get; set; }

        public virtual ICollection<TAB_Funcao> TAB_Funcao1 { get; set; }

        public virtual TAB_Funcao TAB_Funcao2 { get; set; }
    }
}
