namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Solicitacao_R1")]
    public partial class BNE_Solicitacao_R1
    {
        [Key]
        public int Idf_Solicitacao_R1 { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Solicitante { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Solicitante { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Telefone_Solicitante { get; set; }

        [StringLength(200)]
        public string Eml_Solicitante { get; set; }

        public int Idf_Cidade_Solicitante { get; set; }

        public int Idf_Funcao_Solicitante { get; set; }

        public int Idf_Funcao { get; set; }

        public int? Qtd_Experiencia { get; set; }

        public int Qtd_Vagas { get; set; }

        public int? Idf_Area_BNE { get; set; }

        [StringLength(2000)]
        public string Des_Atividade { get; set; }

        [StringLength(2000)]
        public string Des_Requisito_Obrigatorio { get; set; }

        [StringLength(2000)]
        public string Des_Requisito_Desejavel { get; set; }

        public short? Num_Idade_Minima { get; set; }

        public short? Num_Idade_Maxima { get; set; }

        public int Idf_Escolaridade { get; set; }

        [StringLength(2000)]
        public string Des_Conhecimento_Informatica { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_De { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_Ate { get; set; }

        public int? Idf_Sexo { get; set; }

        [StringLength(2000)]
        public string Des_Beneficio { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        public int Idf_Cidade { get; set; }

        public int? Idf_Categoria_Habilitacao { get; set; }

        [StringLength(2000)]
        public string Des_Informacao_Adicional { get; set; }

        public int Idf_Consultor_R1 { get; set; }

        public int? Idf_Simulacao_R1 { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Consultor_R1 BNE_Consultor_R1 { get; set; }

        public virtual BNE_Simulacao_R1 BNE_Simulacao_R1 { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Categoria_Habilitacao TAB_Categoria_Habilitacao { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Cidade TAB_Cidade1 { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Estado_Civil TAB_Estado_Civil { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Funcao TAB_Funcao1 { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
