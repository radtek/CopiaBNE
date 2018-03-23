namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Envio_Email_Sal_BR")]
    public partial class TAB_Envio_Email_Sal_BR
    {
        [Key]
        public int Idf_Envio_Email_Sal_BR { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Email { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
