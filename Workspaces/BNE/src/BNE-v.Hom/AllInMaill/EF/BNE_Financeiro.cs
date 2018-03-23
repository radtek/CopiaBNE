namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Financeiro")]
    public partial class BNE_Financeiro
    {
        [Key]
        public int Idf_Financeiro { get; set; }

        public DateTime Dta_Importacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Caminho { get; set; }

        public int Idf_Usuario { get; set; }

        public bool Flg_Situacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
