namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Curriculo")]
    public partial class TAB_Pesquisa_Curriculo
    {
        public TAB_Pesquisa_Curriculo()
        {
            TAB_Pesquisa_Curriculo_Disponibilidade = new HashSet<TAB_Pesquisa_Curriculo_Disponibilidade>();
            TAB_Pesquisa_Curriculo_Idioma = new HashSet<TAB_Pesquisa_Curriculo_Idioma>();
        }

        [Key]
        public int Idf_Pesquisa_Curriculo { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public int? Idf_Curriculo { get; set; }

        public int? Idf_Funcao { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(100)]
        public string Des_Palavra_Chave { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public int? Idf_Escolaridade { get; set; }

        public int? Idf_Sexo { get; set; }

        public DateTime? Dta_Idade_Min { get; set; }

        public DateTime? Dta_Idade_Max { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Salario_Min { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Salario_Max { get; set; }

        public long? Qtd_Experiencia { get; set; }

        public int? Idf_Idioma { get; set; }

        [StringLength(200)]
        public string Des_Cod_CPF_Nome { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        [StringLength(50)]
        public string Des_Bairro { get; set; }

        [StringLength(100)]
        public string Des_Logradouro { get; set; }

        [StringLength(8)]
        public string Num_CEP_Min { get; set; }

        [StringLength(8)]
        public string Num_CEP_Max { get; set; }

        [StringLength(100)]
        public string Des_Experiencia { get; set; }

        public int? Idf_Curso_Tecnico_Graduacao { get; set; }

        public int? Idf_Fonte_Tecnico_Graduacao { get; set; }

        [StringLength(100)]
        public string Raz_Social { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public int? Idf_Categoria_Habilitacao { get; set; }

        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [StringLength(8)]
        public string Num_Telefone { get; set; }

        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public int? Idf_Tipo_Veiculo { get; set; }

        public int? Idf_Curso_Outros_Cursos { get; set; }

        public int? Idf_Fonte_Outros_Cursos { get; set; }

        public int? Idf_Raca { get; set; }

        public bool? Flg_Filhos { get; set; }

        public bool Flg_Pesquisa_Avancada { get; set; }

        [StringLength(100)]
        public string Des_Curso_Tecnico_Graduacao { get; set; }

        [StringLength(100)]
        public string Des_Curso_Outros_Cursos { get; set; }

        public int? Qtd_Curriculo_Retorno { get; set; }

        [StringLength(50)]
        public string Idf_Escolaridade_WebStagio { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Curso TAB_Curso { get; set; }

        public virtual TAB_Curso TAB_Curso1 { get; set; }

        public virtual TAB_Fonte TAB_Fonte { get; set; }

        public virtual TAB_Fonte TAB_Fonte1 { get; set; }

        public virtual TAB_Idioma TAB_Idioma { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo_Disponibilidade> TAB_Pesquisa_Curriculo_Disponibilidade { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo_Idioma> TAB_Pesquisa_Curriculo_Idioma { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Categoria_Habilitacao TAB_Categoria_Habilitacao { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Estado_Civil TAB_Estado_Civil { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Raca TAB_Raca { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Tipo_Veiculo TAB_Tipo_Veiculo { get; set; }
    }
}
