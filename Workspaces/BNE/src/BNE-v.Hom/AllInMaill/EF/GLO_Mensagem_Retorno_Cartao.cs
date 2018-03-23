namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Mensagem_Retorno_Cartao")]
    public partial class GLO_Mensagem_Retorno_Cartao
    {
        public GLO_Mensagem_Retorno_Cartao()
        {
            GLO_Cobranca_Cartao = new HashSet<GLO_Cobranca_Cartao>();
            GLO_Cobranca_Cartao_LOG = new HashSet<GLO_Cobranca_Cartao_LOG>();
        }

        [Key]
        public int Idf_Mensagem_Retorno_Cartao { get; set; }

        [StringLength(5)]
        public string Cod_Mensagem { get; set; }

        [StringLength(200)]
        public string Des_Mensagem { get; set; }

        [StringLength(100)]
        public string Des_Mensagem_Global { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao> GLO_Cobranca_Cartao { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao_LOG> GLO_Cobranca_Cartao_LOG { get; set; }
    }
}
