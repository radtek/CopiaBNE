namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga")]
    public partial class BNE_Vaga
    {
        public BNE_Vaga()
        {
            BNE_Curriculo_Entrevista = new HashSet<BNE_Curriculo_Entrevista>();
            BNE_Historico_Publicacao_Vaga = new HashSet<BNE_Historico_Publicacao_Vaga>();
            BNE_Rastreador_Resultado_Vaga = new HashSet<BNE_Rastreador_Resultado_Vaga>();
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            BNE_Vaga_Integracao = new HashSet<BNE_Vaga_Integracao>();
            BNE_Vaga_Divulgacao = new HashSet<BNE_Vaga_Divulgacao>();
            BNE_Vaga_Home = new HashSet<BNE_Vaga_Home>();
            BNE_Vaga_Candidato = new HashSet<BNE_Vaga_Candidato>();
            BNE_Vaga_Disponibilidade = new HashSet<BNE_Vaga_Disponibilidade>();
            BNE_Vaga_Palavra_Chave = new HashSet<BNE_Vaga_Palavra_Chave>();
            BNE_Vaga_Pergunta = new HashSet<BNE_Vaga_Pergunta>();
            BNE_Vaga_Rede_Social = new HashSet<BNE_Vaga_Rede_Social>();
            BNE_Vaga_Tipo_Vinculo = new HashSet<BNE_Vaga_Tipo_Vinculo>();
        }

        [Key]
        public int Idf_Vaga { get; set; }

        public int? Idf_Funcao { get; set; }

        public int Idf_Cidade { get; set; }

        [StringLength(10)]
        public string Cod_Vaga { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_De { get; set; }

        public DateTime? Dta_Abertura { get; set; }

        public DateTime? Dta_Prazo { get; set; }

        [StringLength(100)]
        public string Eml_Vaga { get; set; }

        [StringLength(2000)]
        public string Des_Requisito { get; set; }

        public short? Qtd_Vaga { get; set; }

        [StringLength(100)]
        public string Nme_Empresa { get; set; }

        public bool Flg_Vaga_Rapida { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Filial { get; set; }

        public bool Flg_Confidencial { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public int? Idf_Escolaridade { get; set; }

        public short? Num_Idade_Minima { get; set; }

        public short? Num_Idade_Maxima { get; set; }

        public int? Idf_Sexo { get; set; }

        [StringLength(2000)]
        public string Des_Beneficio { get; set; }

        [StringLength(2000)]
        public string Des_Atribuicoes { get; set; }

        [StringLength(2)]
        public string Num_DDD { get; set; }

        [StringLength(10)]
        public string Num_Telefone { get; set; }

        public bool? Flg_Receber_Cada_CV { get; set; }

        public bool? Flg_Receber_Todos_CV { get; set; }

        [StringLength(50)]
        public string Des_Funcao { get; set; }

        public bool? Flg_Auditada { get; set; }

        public bool Flg_BNE_Recomenda { get; set; }

        public bool Flg_Vaga_Arquivada { get; set; }

        public bool Flg_Vaga_Massa { get; set; }

        public int Idf_Origem { get; set; }

        public bool? Flg_Liberada { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public DateTime? Dta_Auditoria { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_Para { get; set; }

        public bool? Flg_Deficiencia { get; set; }

        public bool Flg_Empresa_Em_Auditoria { get; set; }

        public virtual ICollection<BNE_Curriculo_Entrevista> BNE_Curriculo_Entrevista { get; set; }

        public virtual ICollection<BNE_Historico_Publicacao_Vaga> BNE_Historico_Publicacao_Vaga { get; set; }

        public virtual ICollection<BNE_Rastreador_Resultado_Vaga> BNE_Rastreador_Resultado_Vaga { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<BNE_Vaga_Integracao> BNE_Vaga_Integracao { get; set; }

        public virtual BNE_Vaga_Fulltext BNE_Vaga_Fulltext { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<BNE_Vaga_Divulgacao> BNE_Vaga_Divulgacao { get; set; }

        public virtual ICollection<BNE_Vaga_Home> BNE_Vaga_Home { get; set; }

        public virtual ICollection<BNE_Vaga_Candidato> BNE_Vaga_Candidato { get; set; }

        public virtual ICollection<BNE_Vaga_Disponibilidade> BNE_Vaga_Disponibilidade { get; set; }

        public virtual ICollection<BNE_Vaga_Palavra_Chave> BNE_Vaga_Palavra_Chave { get; set; }

        public virtual ICollection<BNE_Vaga_Pergunta> BNE_Vaga_Pergunta { get; set; }

        public virtual ICollection<BNE_Vaga_Rede_Social> BNE_Vaga_Rede_Social { get; set; }

        public virtual ICollection<BNE_Vaga_Tipo_Vinculo> BNE_Vaga_Tipo_Vinculo { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Origem TAB_Origem { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }
    }
}
