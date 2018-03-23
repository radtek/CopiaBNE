namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.BNE_Integracao")]
    public partial class BNE_Integracao
    {
        [Key]
        public int Idf_Integracao { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa { get; set; }

        [StringLength(30)]
        public string Ape_Pessoa { get; set; }

        public int Idf_Sexo { get; set; }

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

        public int? Idf_Raca { get; set; }

        public int? Idf_Deficiencia { get; set; }

        [StringLength(100)]
        public string Des_Logradouro { get; set; }

        [StringLength(30)]
        public string Des_Complemento { get; set; }

        [StringLength(15)]
        public string Num_Endereco { get; set; }

        [StringLength(8)]
        public string Num_CEP { get; set; }

        [StringLength(80)]
        public string Des_Bairro { get; set; }

        public int Idf_Cidade { get; set; }

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

        public int? Idf_Escolaridade { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        [StringLength(100)]
        public string Raz_Social { get; set; }

        public int Idf_funcao { get; set; }

        public DateTime? Dta_Admissao { get; set; }

        public DateTime? Dta_Saida_Prevista { get; set; }

        public decimal Vlr_Salario { get; set; }

        [StringLength(15)]
        public string Num_Habilitacao { get; set; }

        public int? Idf_Categoria_Habilitacao { get; set; }

        public bool? Flg_filhos { get; set; }

        public int? Idf_Tipo_Veiculo { get; set; }

        public int? Ano_Veiculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Integracao { get; set; }

        public int Idf_Integracao_Situacao { get; set; }

        public int Idf_Tipo_Vinculo_Integracao { get; set; }

        public int Idf_Motivo_Rescisao { get; set; }

        public virtual BNE_Integracao_Situacao BNE_Integracao_Situacao { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Categoria_Habilitacao TAB_Categoria_Habilitacao { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Estado_Civil TAB_Estado_Civil { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Motivo_Rescisao TAB_Motivo_Rescisao { get; set; }

        public virtual TAB_Raca TAB_Raca { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }

        public virtual TAB_Tipo_Veiculo TAB_Tipo_Veiculo { get; set; }

        public virtual TAB_Tipo_Vinculo_Integracao TAB_Tipo_Vinculo_Integracao { get; set; }
    }
}
