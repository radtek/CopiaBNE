namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Cartao")]
    public partial class GLO_Cobranca_Cartao
    {
        public GLO_Cobranca_Cartao()
        {
            GLO_Cobranca_Cartao_LOG = new HashSet<GLO_Cobranca_Cartao_LOG>();
        }

        [Key]
        public int Idf_Cobranca_Cartao { get; set; }

        public int Idf_Transacao { get; set; }

        [StringLength(50)]
        public string Cod_Filiacao { get; set; }

        public int? Idf_Operadora_Cartao { get; set; }

        public int? Idf_Parcela_Cartao { get; set; }

        [StringLength(10)]
        public string Cod_Dta_Juliana { get; set; }

        [StringLength(50)]
        public string Cod_TID { get; set; }

        public decimal? Vlr_Recebimento { get; set; }

        public DateTime? Dta_Recebimento { get; set; }

        public int? Idf_Mensagem_Retorno_Cartao { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao_LOG> GLO_Cobranca_Cartao_LOG { get; set; }

        public virtual GLO_Mensagem_Retorno_Cartao GLO_Mensagem_Retorno_Cartao { get; set; }

        public virtual GLO_Transacao GLO_Transacao { get; set; }

        public virtual TAB_Operadora_Cartao TAB_Operadora_Cartao { get; set; }

        public virtual TAB_Parcela_Cartao TAB_Parcela_Cartao { get; set; }
    }
}
