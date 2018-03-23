namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_DePara_Email")]
    public partial class TAB_DePara_Email
    {
        [Key]
        public int Idf_DePara_Email { get; set; }

        [Required]
        public string Des_Email_Erro { get; set; }

        [Required]
        public string Des_Email_Correto { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
