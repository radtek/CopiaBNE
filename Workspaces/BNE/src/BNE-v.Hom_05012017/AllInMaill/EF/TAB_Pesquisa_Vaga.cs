namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Vaga")]
    public partial class TAB_Pesquisa_Vaga
    {
        public TAB_Pesquisa_Vaga()
        {
            TAB_Pesquisa_Vaga_Disponibilidade = new HashSet<TAB_Pesquisa_Vaga_Disponibilidade>();
            TAB_Pesquisa_Vaga_Tipo_Vinculo = new HashSet<TAB_Pesquisa_Vaga_Tipo_Vinculo>();
        }

        [Key]
        public int Idf_Pesquisa_Vaga { get; set; }

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

        [Column(TypeName = "numeric")]
        public decimal? Num_Salario_Min { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Salario_Max { get; set; }

        [StringLength(200)]
        public string Des_Cod_Vaga { get; set; }

        [StringLength(100)]
        public string Raz_Social { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public bool Flg_Pesquisa_Avancada { get; set; }

        public int? Qtd_Vaga_Retorno { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Deficiencia TAB_Deficiencia { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga_Disponibilidade> TAB_Pesquisa_Vaga_Disponibilidade { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga_Tipo_Vinculo> TAB_Pesquisa_Vaga_Tipo_Vinculo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
