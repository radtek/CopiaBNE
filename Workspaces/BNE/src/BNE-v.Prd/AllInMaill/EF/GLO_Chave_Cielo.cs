namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Chave_Cielo")]
    public partial class GLO_Chave_Cielo
    {
        [Key]
        public int Idf_Chave_Cielo { get; set; }

        [Required]
        [StringLength(64)]
        public string Cod_Chave_Cielo { get; set; }

        [StringLength(200)]
        public string Url_web_Service { get; set; }

        [Required]
        [StringLength(50)]
        public string Cod_Filiacao { get; set; }
    }
}
