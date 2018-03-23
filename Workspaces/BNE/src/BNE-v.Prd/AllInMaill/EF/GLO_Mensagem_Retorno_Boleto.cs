namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Mensagem_Retorno_Boleto")]
    public partial class GLO_Mensagem_Retorno_Boleto
    {
        public GLO_Mensagem_Retorno_Boleto()
        {
            GLO_Cobranca_Boleto = new HashSet<GLO_Cobranca_Boleto>();
            GLO_Cobranca_Boleto_Lista_Remessa = new HashSet<GLO_Cobranca_Boleto_Lista_Remessa>();
            GLO_Cobranca_Boleto_Lista_Retorno = new HashSet<GLO_Cobranca_Boleto_Lista_Retorno>();
            GLO_Cobranca_Boleto_LOG = new HashSet<GLO_Cobranca_Boleto_LOG>();
        }

        [Key]
        public int Idf_Mensagem_Retorno_Boleto { get; set; }

        [StringLength(50)]
        public string Des_Status { get; set; }

        [StringLength(10)]
        public string Cod_Status { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto> GLO_Cobranca_Boleto { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_Lista_Remessa> GLO_Cobranca_Boleto_Lista_Remessa { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_Lista_Retorno> GLO_Cobranca_Boleto_Lista_Retorno { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_LOG> GLO_Cobranca_Boleto_LOG { get; set; }
    }
}
