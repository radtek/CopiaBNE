namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador")]
    public partial class BNE_Rastreador
    {
        public BNE_Rastreador()
        {
            BNE_Rastreador_Disponibilidade = new HashSet<BNE_Rastreador_Disponibilidade>();
            BNE_Rastreador_Idioma = new HashSet<BNE_Rastreador_Idioma>();
            BNE_Rastreador_Curriculo = new HashSet<BNE_Rastreador_Curriculo>();
        }

        public int? Idf_Filial { get; set; }

        public int? Idf_Funcao { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(50)]
        public string Des_Palavra_Chave { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_Inicio { get; set; }

        [Column(TypeName = "money")]
        public decimal? Vlr_Salario_Fim { get; set; }

        public short? Qtd_Experiencia { get; set; }

        public int? Idf_Escolaridade { get; set; }

        public int? Des_Idade_Inicio { get; set; }

        public int? Des_Idade_Fim { get; set; }

        public int? Idf_Sexo { get; set; }

        [StringLength(50)]
        public string Des_Bairro { get; set; }

        public int? Idf_Categoria_Habilitacao { get; set; }

        [StringLength(100)]
        public string Raz_Social { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public int? Idf_Fonte { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public bool? Flg_Regiao_Metropolitana { get; set; }

        public bool? Flg_Inativo { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public int? Idf_Curso { get; set; }

        public int? Idf_Curso_Fonte { get; set; }

        public int? Idf_Raca { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        [Key]
        public int Idf_Rastreador { get; set; }

        public int? Idf_Tipo_Veiculo { get; set; }

        [StringLength(8)]
        public string Des_CEP_Fim { get; set; }

        public bool? Flg_Filhos { get; set; }

        [StringLength(8)]
        public string Des_CEP_Inicio { get; set; }

        public int? Idf_Curso_Outros_Cursos { get; set; }

        public int? Idf_Fonte_Outros_Cursos { get; set; }

        public int? Idf_Estado { get; set; }

        public int? Idf_Origem { get; set; }

        public virtual ICollection<BNE_Rastreador_Disponibilidade> BNE_Rastreador_Disponibilidade { get; set; }

        public virtual ICollection<BNE_Rastreador_Idioma> BNE_Rastreador_Idioma { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Categoria_Habilitacao TAB_Categoria_Habilitacao { get; set; }

        public virtual TAB_Curso_Fonte TAB_Curso_Fonte { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Curso TAB_Curso { get; set; }

        public virtual TAB_Curso TAB_Curso1 { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Estado_Civil TAB_Estado_Civil { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Estado TAB_Estado { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Fonte TAB_Fonte { get; set; }

        public virtual TAB_Fonte TAB_Fonte1 { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Origem TAB_Origem { get; set; }

        public virtual TAB_Raca TAB_Raca { get; set; }

        public virtual ICollection<BNE_Rastreador_Curriculo> BNE_Rastreador_Curriculo { get; set; }

        public virtual TAB_Sexo TAB_Sexo { get; set; }

        public virtual TAB_Tipo_Veiculo TAB_Tipo_Veiculo { get; set; }
    }
}
