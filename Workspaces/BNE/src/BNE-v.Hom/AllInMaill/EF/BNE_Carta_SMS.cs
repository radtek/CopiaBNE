namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Carta_SMS")]
    public partial class BNE_Carta_SMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Carta_SMS { get; set; }

        [Required]
        [StringLength(70)]
        public string Nme_Carta_SMS { get; set; }

        [Required]
        public string Vlr_Carta_SMS { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
