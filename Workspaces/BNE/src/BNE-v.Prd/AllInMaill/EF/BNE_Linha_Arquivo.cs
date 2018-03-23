namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Linha_Arquivo")]
    public partial class BNE_Linha_Arquivo
    {
        public int Idf_Arquivo { get; set; }

        public int Idf_Transacao { get; set; }

        public int Num_Linha { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Linha_Arquivo { get; set; }

        public int Idf_Cobranca_Boleto { get; set; }

        public virtual BNE_Arquivo BNE_Arquivo { get; set; }

        public virtual GLO_Cobranca_Boleto GLO_Cobranca_Boleto { get; set; }

        public virtual BNE_Transacao BNE_Transacao { get; set; }
    }
}
