namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica")]
    public partial class TAB_Pessoa_Fisica
    {
        public TAB_Pessoa_Fisica()
        {
            BNE_Curriculo = new HashSet<BNE_Curriculo>();
            BNE_Experiencia_Profissional = new HashSet<BNE_Experiencia_Profissional>();
            BNE_Formacao = new HashSet<BNE_Formacao>();
            BNE_Mini_Curriculo = new HashSet<BNE_Mini_Curriculo>();
            BNE_Rastreador_Vaga = new HashSet<BNE_Rastreador_Vaga>();
            BNE_Usuario = new HashSet<BNE_Usuario>();
            TAB_Pessoa_Fisica_Idioma = new HashSet<TAB_Pessoa_Fisica_Idioma>();
            TAB_Pessoa_Fisica_Foto = new HashSet<TAB_Pessoa_Fisica_Foto>();
            TAB_Pessoa_Fisica_Rede_Social = new HashSet<TAB_Pessoa_Fisica_Rede_Social>();
            TAB_Pessoa_Fisica_Veiculo = new HashSet<TAB_Pessoa_Fisica_Veiculo>();
            TAB_Usuario_Filial_Perfil = new HashSet<TAB_Usuario_Filial_Perfil>();
        }

        [Key]
        public int Idf_Pessoa_Fisica { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa { get; set; }

        [StringLength(30)]
        public string Ape_Pessoa { get; set; }

        public int? Idf_Sexo { get; set; }

        public int? Idf_Nacionalidade { get; set; }

        public int? Idf_Cidade { get; set; }

        public DateTime? Dta_Chegada_Brasil { get; set; }

        public DateTime Dta_Nascimento { get; set; }

        [StringLength(100)]
        public string Nme_Mae { get; set; }

        [StringLength(100)]
        public string Nme_Pai { get; set; }

        [StringLength(20)]
        public string Num_RG { get; set; }

        public DateTime? Dta_Expedicao_RG { get; set; }

        [StringLength(20)]
        public string Nme_Orgao_Emissor { get; set; }

        [StringLength(2)]
        public string Sig_UF_Emissao_RG { get; set; }

        [StringLength(11)]
        public string Num_PIS { get; set; }

        [StringLength(8)]
        public string Num_CTPS { get; set; }

        [StringLength(5)]
        public string Des_Serie_CTPS { get; set; }

        [StringLength(2)]
        public string Sig_UF_CTPS { get; set; }

        public int? Idf_Raca { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public int? Idf_Endereco { get; set; }

        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [StringLength(10)]
        public string Num_Telefone { get; set; }

        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [StringLength(10)]
        public string Num_Celular { get; set; }

        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        public bool? Flg_Possui_Dependentes { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool? Flg_Importado { get; set; }

        public int? Idf_Escolaridade { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa_Pesquisa { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public bool? Flg_Inativo { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public int? Idf_Operadora_Celular { get; set; }

        public int? Idf_Email_Situacao_Confirmacao { get; set; }

        public int? Idf_Email_Situacao_Validacao { get; set; }

        public int? Idf_Email_Situacao_Bloqueio { get; set; }

        public virtual ICollection<BNE_Curriculo> BNE_Curriculo { get; set; }

        public virtual ICollection<BNE_Experiencia_Profissional> BNE_Experiencia_Profissional { get; set; }

        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }

        public virtual ICollection<BNE_Mini_Curriculo> BNE_Mini_Curriculo { get; set; }

        public virtual ICollection<BNE_Rastreador_Vaga> BNE_Rastreador_Vaga { get; set; }

        public virtual ICollection<BNE_Usuario> BNE_Usuario { get; set; }

        public virtual TAB_Endereco TAB_Endereco { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Idioma> TAB_Pessoa_Fisica_Idioma { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Estado_Civil TAB_Estado_Civil { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao1 { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao2 { get; set; }

        public virtual TAB_Operadora_Celular TAB_Operadora_Celular { get; set; }

        public virtual TAB_Pessoa_Fisica_Complemento TAB_Pessoa_Fisica_Complemento { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Foto> TAB_Pessoa_Fisica_Foto { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Rede_Social> TAB_Pessoa_Fisica_Rede_Social { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Veiculo> TAB_Pessoa_Fisica_Veiculo { get; set; }

        public virtual TAB_Raca TAB_Raca { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }

        public virtual ICollection<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Nacionalidade TAB_Nacionalidade { get; set; }
    }
}
