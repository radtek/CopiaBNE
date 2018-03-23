namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Boleto_Arquivo_Retorno")]
    public partial class GLO_Cobranca_Boleto_Arquivo_Retorno
    {
        [Key]
        public int Idf_Cobranca_Boleto_Arquivo_Retorno { get; set; }

        public DateTime? Dta_Retorno { get; set; }

        public string Arq_Retorno { get; set; }

        [StringLength(100)]
        public string Nme_Arquivo { get; set; }
    }
}
