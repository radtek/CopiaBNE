namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Experiencia_Profissional")]
    public partial class BNE_Experiencia_Profissional
    {
        public int? Idf_Area_BNE { get; set; }

        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Experiencia_Profissional { get; set; }

        [Required]
        [StringLength(100)]
        public string Raz_Social { get; set; }

        [StringLength(50)]
        public string Des_Funcao_Exercida { get; set; }

        public int? Idf_Funcao { get; set; }

        public DateTime Dta_Admissao { get; set; }

        public DateTime? Dta_Demissao { get; set; }

        public string Des_Atividade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool? Flg_Importado { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
