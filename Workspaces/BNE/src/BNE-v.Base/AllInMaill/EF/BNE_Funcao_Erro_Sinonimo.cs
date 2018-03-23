namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Funcao_Erro_Sinonimo")]
    public partial class BNE_Funcao_Erro_Sinonimo
    {
        [Key]
        public int Idf_Funcao_Erro_Sinonimo { get; set; }

        [Required]
        [StringLength(255)]
        public string Des_Funcao_Erro_Sinonimo { get; set; }

        public bool Flg_Erro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Funcao { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
