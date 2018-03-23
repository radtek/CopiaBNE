namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rede_Social_Conta")]
    public partial class BNE_Rede_Social_Conta
    {
        [Key]
        public int Idf_Rede_Social_Conta { get; set; }

        public bool Flg_Vaga { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Rede_Social_cs { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Login { get; set; }

        [Required]
        [StringLength(255)]
        public string Des_Senha { get; set; }

        [StringLength(10)]
        public string Des_Comunidade { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public int? Idf_Cidade { get; set; }

        public int? Idf_Funcao_Categoria { get; set; }

        public int? Idf_Vaga { get; set; }

        public int? Idf_Funcao { get; set; }

        public int? Idf_Conta_Twitter { get; set; }

        public virtual BNE_Conta_Twitter BNE_Conta_Twitter { get; set; }

        public virtual BNE_Rede_Social_CS BNE_Rede_Social_CS { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Funcao_Categoria TAB_Funcao_Categoria { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
