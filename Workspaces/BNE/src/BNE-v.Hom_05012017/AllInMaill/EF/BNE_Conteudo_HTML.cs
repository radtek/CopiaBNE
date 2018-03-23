namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Conteudo_HTML")]
    public partial class BNE_Conteudo_HTML
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Conteudo { get; set; }

        [Required]
        [StringLength(70)]
        public string Nme_Conteudo { get; set; }

        [Required]
        public string Vlr_Conteudo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
