namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Boleto_Arquivo_Remessa")]
    public partial class GLO_Cobranca_Boleto_Arquivo_Remessa
    {
        [Key]
        public int Idf_Cobranca_Boleto_Arquivo_Remessa { get; set; }

        public DateTime? Dta_Remessa { get; set; }

        public string Arq_Remessa { get; set; }

        [StringLength(100)]
        public string Nme_Arquivo { get; set; }
    }
}
