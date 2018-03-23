namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_FPAS")]
    public partial class TAB_FPAS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_FPAS { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Des_FPAS { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
