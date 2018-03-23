namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Busca")]
    public partial class BNE_Curriculo_Busca
    {
        [Key]
        public int Idf_Curriculo_Busca { get; set; }

        [Required]
        [StringLength(2000)]
        public string Des_Busca { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }
    }
}
