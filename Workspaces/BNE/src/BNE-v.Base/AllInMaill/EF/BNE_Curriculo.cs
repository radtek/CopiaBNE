namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo")]
    public partial class BNE_Curriculo
    {
        public BNE_Curriculo()
        {
            BNE_Cadastro_Parceiro = new HashSet<BNE_Cadastro_Parceiro>();
            BNE_Campanha_Curriculo = new HashSet<BNE_Campanha_Curriculo>();
            BNE_Conversas_Ativas = new HashSet<BNE_Conversas_Ativas>();
            BNE_Vaga_Candidato = new HashSet<BNE_Vaga_Candidato>();
            BNE_Curriculo_Classificacao = new HashSet<BNE_Curriculo_Classificacao>();
            BNE_Curriculo_Disponibilidade_Cidade = new HashSet<BNE_Curriculo_Disponibilidade_Cidade>();
            BNE_Curriculo_Disponibilidade = new HashSet<BNE_Curriculo_Disponibilidade>();
            BNE_Curriculo_Fila = new HashSet<BNE_Curriculo_Fila>();
            BNE_Curriculo_Idioma = new HashSet<BNE_Curriculo_Idioma>();
            BNE_Curriculo_Auditoria = new HashSet<BNE_Curriculo_Auditoria>();
            BNE_Curriculo_Correcao = new HashSet<BNE_Curriculo_Correcao>();
            BNE_Curriculo_Entrevista = new HashSet<BNE_Curriculo_Entrevista>();
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
            BNE_Curriculo_Origem = new HashSet<BNE_Curriculo_Origem>();
            BNE_Curriculo_Permissao = new HashSet<BNE_Curriculo_Permissao>();
            BNE_Curriculo_Publicacao = new HashSet<BNE_Curriculo_Publicacao>();
            BNE_Intencao_Filial = new HashSet<BNE_Intencao_Filial>();
            BNE_Curriculo_Visualizacao = new HashSet<BNE_Curriculo_Visualizacao>();
            BNE_Curriculo_Visualizacao_Historico = new HashSet<BNE_Curriculo_Visualizacao_Historico>();
            BNE_Funcao_Pretendida = new HashSet<BNE_Funcao_Pretendida>();
            BNE_Vaga_Divulgacao = new HashSet<BNE_Vaga_Divulgacao>();
            BNE_Rastreador_Curriculo = new HashSet<BNE_Rastreador_Curriculo>();
            BNE_Inscricao_Curso = new HashSet<BNE_Inscricao_Curso>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            BNE_Historico_Publicacao_Curriculo = new HashSet<BNE_Historico_Publicacao_Curriculo>();
            BNE_Curriculo_Quem_Me_Viu = new HashSet<BNE_Curriculo_Quem_Me_Viu>();
            BNE_Mobile_Token = new HashSet<BNE_Mobile_Token>();
            BNE_Curriculo_Observacao = new HashSet<BNE_Curriculo_Observacao>();
            TAB_Parametro_Curriculo = new HashSet<TAB_Parametro_Curriculo>();
            TAB_Requisicao_Integrador_Curriculo = new HashSet<TAB_Requisicao_Integrador_Curriculo>();
        }

        [Key]
        public int Idf_Curriculo { get; set; }

        public int Idf_Pessoa_Fisica { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Vlr_Pretensao_Salarial { get; set; }

        public int Idf_Tipo_Curriculo { get; set; }

        public int Idf_Situacao_Curriculo { get; set; }

        public string Des_Mini_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Atualizacao { get; set; }

        public bool? Flg_Manha { get; set; }

        public bool? Flg_Tarde { get; set; }

        public bool? Flg_Noite { get; set; }

        [StringLength(2000)]
        public string Obs_Curriculo { get; set; }

        [StringLength(2000)]
        public string Des_Analise { get; set; }

        [StringLength(2000)]
        public string Des_Sugestao_Carreira { get; set; }

        [StringLength(2000)]
        public string Des_Cursos_Oferecidos { get; set; }

        public bool Flg_Inativo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Vlr_Ultimo_Salario { get; set; }

        [Required]
        [StringLength(15)]
        public string Des_IP { get; set; }

        public int? Idf_Cidade_Pretendida { get; set; }

        public bool? Flg_Final_Semana { get; set; }

        public bool Flg_VIP { get; set; }

        public bool? Flg_Boas_Vindas { get; set; }

        public bool Flg_MSN { get; set; }

        public int? Idf_Cidade_Endereco { get; set; }

        public DateTime? Dta_Modificacao_CV { get; set; }

        public DbGeography Des_Localizacao { get; set; }

        public virtual ICollection<BNE_Cadastro_Parceiro> BNE_Cadastro_Parceiro { get; set; }

        public virtual ICollection<BNE_Campanha_Curriculo> BNE_Campanha_Curriculo { get; set; }

        public virtual ICollection<BNE_Conversas_Ativas> BNE_Conversas_Ativas { get; set; }

        public virtual ICollection<BNE_Vaga_Candidato> BNE_Vaga_Candidato { get; set; }

        public virtual ICollection<BNE_Curriculo_Classificacao> BNE_Curriculo_Classificacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Disponibilidade_Cidade> BNE_Curriculo_Disponibilidade_Cidade { get; set; }

        public virtual ICollection<BNE_Curriculo_Disponibilidade> BNE_Curriculo_Disponibilidade { get; set; }

        public virtual ICollection<BNE_Curriculo_Fila> BNE_Curriculo_Fila { get; set; }

        public virtual ICollection<BNE_Curriculo_Idioma> BNE_Curriculo_Idioma { get; set; }

        public virtual ICollection<BNE_Curriculo_Auditoria> BNE_Curriculo_Auditoria { get; set; }

        public virtual ICollection<BNE_Curriculo_Correcao> BNE_Curriculo_Correcao { get; set; }

        public virtual ICollection<BNE_Curriculo_Entrevista> BNE_Curriculo_Entrevista { get; set; }

        public virtual BNE_Curriculo_Fulltext BNE_Curriculo_Fulltext { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }

        public virtual ICollection<BNE_Curriculo_Origem> BNE_Curriculo_Origem { get; set; }

        public virtual ICollection<BNE_Curriculo_Permissao> BNE_Curriculo_Permissao { get; set; }

        public virtual ICollection<BNE_Curriculo_Publicacao> BNE_Curriculo_Publicacao { get; set; }

        public virtual ICollection<BNE_Intencao_Filial> BNE_Intencao_Filial { get; set; }

        public virtual ICollection<BNE_Curriculo_Visualizacao> BNE_Curriculo_Visualizacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Visualizacao_Historico> BNE_Curriculo_Visualizacao_Historico { get; set; }

        public virtual ICollection<BNE_Funcao_Pretendida> BNE_Funcao_Pretendida { get; set; }

        public virtual ICollection<BNE_Vaga_Divulgacao> BNE_Vaga_Divulgacao { get; set; }

        public virtual ICollection<BNE_Rastreador_Curriculo> BNE_Rastreador_Curriculo { get; set; }

        public virtual ICollection<BNE_Inscricao_Curso> BNE_Inscricao_Curso { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<BNE_Historico_Publicacao_Curriculo> BNE_Historico_Publicacao_Curriculo { get; set; }

        public virtual ICollection<BNE_Curriculo_Quem_Me_Viu> BNE_Curriculo_Quem_Me_Viu { get; set; }

        public virtual BNE_Situacao_Curriculo BNE_Situacao_Curriculo { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual BNE_Tipo_Curriculo BNE_Tipo_Curriculo { get; set; }

        public virtual ICollection<BNE_Mobile_Token> BNE_Mobile_Token { get; set; }

        public virtual TAB_Cidade TAB_Cidade1 { get; set; }

        public virtual ICollection<BNE_Curriculo_Observacao> BNE_Curriculo_Observacao { get; set; }

        public virtual ICollection<TAB_Parametro_Curriculo> TAB_Parametro_Curriculo { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<TAB_Requisicao_Integrador_Curriculo> TAB_Requisicao_Integrador_Curriculo { get; set; }
    }
}
