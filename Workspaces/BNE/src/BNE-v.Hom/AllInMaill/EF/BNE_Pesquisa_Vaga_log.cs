namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Pesquisa_Vaga_log")]
    public partial class BNE_Pesquisa_Vaga_log
    {
        [Key]
        public long Idf_Pesquisa_Vaga_log { get; set; }

        public int? Idf_Funcao { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(500)]
        public string Des_Metabusca_Rapida { get; set; }

        public int TotalRegistros { get; set; }

        public int? Idf_Curriculo { get; set; }

        public int? Idf_Vaga { get; set; }

        public int? Idf_Origem { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public int? Idf_Filial { get; set; }

        public int? Idf_Funcao_Area { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public int? Idf_Escolaridade { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Vlr_Salario_Min { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Vlr_Salario_Max { get; set; }

        [StringLength(100)]
        public string Idfs_Disponibilidade { get; set; }

        [StringLength(100)]
        public string Idfs_Tipo_Vinculo { get; set; }

        [StringLength(50)]
        public string Raz_Social { get; set; }

        [StringLength(10)]
        public string Cod_Vaga { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public int? Idf_Sexo { get; set; }

        public DbGeography Coordenada { get; set; }

        public int? Num_Raio { get; set; }

        public bool? Mostra_Vagas_BNE { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
