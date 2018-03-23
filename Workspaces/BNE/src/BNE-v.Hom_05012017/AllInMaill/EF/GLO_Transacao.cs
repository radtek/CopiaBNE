namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Transacao")]
    public partial class GLO_Transacao
    {
        public GLO_Transacao()
        {
            GLO_Cobranca_Boleto = new HashSet<GLO_Cobranca_Boleto>();
            GLO_Cobranca_Cartao = new HashSet<GLO_Cobranca_Cartao>();
        }

        [Key]
        public int Idf_Transacao { get; set; }

        [StringLength(36)]
        public string Cod_Guid { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public int? Idf_Sistema { get; set; }

        [StringLength(100)]
        public string Url_Retorno { get; set; }

        [StringLength(80)]
        public string Des_Identificacao { get; set; }

        public int? Idf_Status_Transacao { get; set; }

        public int? Idf_Tipo_Transacao { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto> GLO_Cobranca_Boleto { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao> GLO_Cobranca_Cartao { get; set; }

        public virtual GLO_Status_Transacao GLO_Status_Transacao { get; set; }

        public virtual TAB_Sistema TAB_Sistema { get; set; }

        public virtual TAB_Tipo_Transacao TAB_Tipo_Transacao { get; set; }
    }
}
