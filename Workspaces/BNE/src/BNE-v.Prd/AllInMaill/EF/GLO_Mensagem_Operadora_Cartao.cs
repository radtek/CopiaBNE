namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Mensagem_Operadora_Cartao")]
    public partial class GLO_Mensagem_Operadora_Cartao
    {
        public GLO_Mensagem_Operadora_Cartao()
        {
            GLO_Cobranca_Cartao_LOG = new HashSet<GLO_Cobranca_Cartao_LOG>();
        }

        [Key]
        public int Idf_Mensagem_Operadora_Cartao { get; set; }

        [StringLength(10)]
        public string Cod_Mensagem_Cartao { get; set; }

        [StringLength(100)]
        public string Des_Mensagem_Cartao { get; set; }

        [StringLength(100)]
        public string Des_Global_Mensagem { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao_LOG> GLO_Cobranca_Cartao_LOG { get; set; }
    }
}
