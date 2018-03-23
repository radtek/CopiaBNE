namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Boleto_Lista_Remessa")]
    public partial class GLO_Cobranca_Boleto_Lista_Remessa
    {
        [Key]
        public int Idf_Cobranca_Boleto_Lista_Remessa { get; set; }

        public int? Idf_Cobranca_Boleto { get; set; }

        public int? Idf_Cobranca_Boleto_Remessa { get; set; }

        public int? Idf_Status_Cobranca_Boleto { get; set; }

        public virtual GLO_Cobranca_Boleto GLO_Cobranca_Boleto { get; set; }

        public virtual GLO_Mensagem_Retorno_Boleto GLO_Mensagem_Retorno_Boleto { get; set; }
    }
}
