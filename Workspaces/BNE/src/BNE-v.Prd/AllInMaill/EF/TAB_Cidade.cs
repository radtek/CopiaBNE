namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Cidade")]
    public partial class TAB_Cidade
    {
        public TAB_Cidade()
        {
            BNE_Agradecimento = new HashSet<BNE_Agradecimento>();
            BNE_Curriculo = new HashSet<BNE_Curriculo>();
            BNE_Curriculo1 = new HashSet<BNE_Curriculo>();
            BNE_Curriculo_Disponibilidade_Cidade = new HashSet<BNE_Curriculo_Disponibilidade_Cidade>();
            BNE_Formacao = new HashSet<BNE_Formacao>();
            BNE_Indicacao_Filial = new HashSet<BNE_Indicacao_Filial>();
            BNE_Lista_Cidade = new HashSet<BNE_Lista_Cidade>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Rastreador_Vaga = new HashSet<BNE_Rastreador_Vaga>();
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            BNE_Simulacao_R1 = new HashSet<BNE_Simulacao_R1>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Solicitacao_R11 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            GLO_Cobranca_Boleto = new HashSet<GLO_Cobranca_Boleto>();
            TAB_Endereco = new HashSet<TAB_Endereco>();
            TAB_Feriado = new HashSet<TAB_Feriado>();
            TAB_Feriado_Modelo = new HashSet<TAB_Feriado_Modelo>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            TAB_Regiao_Metropolitana = new HashSet<TAB_Regiao_Metropolitana>();
            TAB_Regiao_Metropolitana_Cidade = new HashSet<TAB_Regiao_Metropolitana_Cidade>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
            TAB_Endereco_Pessoa_Juridica = new HashSet<TAB_Endereco_Pessoa_Juridica>();
            BNE_Consultor_R1 = new HashSet<BNE_Consultor_R1>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Cidade { get; set; }

        [Required]
        [StringLength(80)]
        public string Nme_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public double Txa_ISS { get; set; }

        public int? Idf_Tipo_Base { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cod_Rais { get; set; }

        public virtual ICollection<BNE_Agradecimento> BNE_Agradecimento { get; set; }

        public virtual ICollection<BNE_Curriculo> BNE_Curriculo { get; set; }

        public virtual ICollection<BNE_Curriculo> BNE_Curriculo1 { get; set; }

        public virtual ICollection<BNE_Curriculo_Disponibilidade_Cidade> BNE_Curriculo_Disponibilidade_Cidade { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }

        public virtual ICollection<BNE_Indicacao_Filial> BNE_Indicacao_Filial { get; set; }

        public virtual ICollection<BNE_Lista_Cidade> BNE_Lista_Cidade { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Rastreador_Vaga> BNE_Rastreador_Vaga { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<BNE_Simulacao_R1> BNE_Simulacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R11 { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto> GLO_Cobranca_Boleto { get; set; }

        public virtual TAB_Cidade_Capital TAB_Cidade_Capital { get; set; }

        public virtual ICollection<TAB_Endereco> TAB_Endereco { get; set; }

        public virtual ICollection<TAB_Feriado> TAB_Feriado { get; set; }

        public virtual ICollection<TAB_Feriado_Modelo> TAB_Feriado_Modelo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<TAB_Regiao_Metropolitana> TAB_Regiao_Metropolitana { get; set; }

        public virtual ICollection<TAB_Regiao_Metropolitana_Cidade> TAB_Regiao_Metropolitana_Cidade { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }

        public virtual ICollection<TAB_Endereco_Pessoa_Juridica> TAB_Endereco_Pessoa_Juridica { get; set; }

        public virtual ICollection<BNE_Consultor_R1> BNE_Consultor_R1 { get; set; }
    }
}
