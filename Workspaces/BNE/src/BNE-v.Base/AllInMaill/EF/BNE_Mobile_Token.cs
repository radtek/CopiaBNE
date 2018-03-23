namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Mobile_Token")]
    public partial class BNE_Mobile_Token
    {
        [Key]
        public int Idf_Mobile_Token { get; set; }

        public int Idf_Curriculo { get; set; }

        [Required]
        [StringLength(4096)]
        public string Cod_Token { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public int Idf_Tipo_Sistema_Mobile { get; set; }

        [Required]
        [StringLength(50)]
        public string Cod_Dispositivo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Tipo_Sistema_Mobile BNE_Tipo_Sistema_Mobile { get; set; }
    }
}
