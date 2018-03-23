namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Cartao_LOG")]
    public partial class GLO_Cobranca_Cartao_LOG
    {
        [Key]
        public int Idf_Cobranca_Cartao_log { get; set; }

        [StringLength(100)]
        public string Des_Cobranca { get; set; }

        public DateTime? Dta_Cobranca { get; set; }

        public int? Idf_Mensagem_Operadora_Cartao { get; set; }

        public int? Idf_Cobranca_Cartao { get; set; }

        public int? Idf_Mensagem_Retorno_Cartao { get; set; }

        public virtual GLO_Cobranca_Cartao GLO_Cobranca_Cartao { get; set; }

        public virtual GLO_Mensagem_Retorno_Cartao GLO_Mensagem_Retorno_Cartao { get; set; }

        public virtual GLO_Mensagem_Operadora_Cartao GLO_Mensagem_Operadora_Cartao { get; set; }
    }
}
