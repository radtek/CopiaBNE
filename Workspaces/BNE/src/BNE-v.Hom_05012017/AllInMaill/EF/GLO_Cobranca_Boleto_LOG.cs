namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Boleto_LOG")]
    public partial class GLO_Cobranca_Boleto_LOG
    {
        [Key]
        public int Idf_Cobranca_Boleto_LOG { get; set; }

        public int? Idf_Cobranca_Boleto { get; set; }

        public DateTime? Dta_Transacao { get; set; }

        [StringLength(50)]
        public string Des_Transacao { get; set; }

        public int? Idf_Mensagem_Retorno_Boleto { get; set; }

        public virtual GLO_Cobranca_Boleto GLO_Cobranca_Boleto { get; set; }

        public virtual GLO_Mensagem_Retorno_Boleto GLO_Mensagem_Retorno_Boleto { get; set; }
    }
}
