namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Transacao_Retorno")]
    public partial class BNE_Transacao_Retorno
    {
        [Key]
        public int Idf_Transacao_Retorno { get; set; }

        public int Idf_Transacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Status { get; set; }

        public bool? Flg_Aprovada { get; set; }

        [StringLength(50)]
        public string Des_Autorizacao { get; set; }

        [StringLength(50)]
        public string Des_Motivo_Nao_Finalizada { get; set; }

        public string Des_Nao_Finalizada { get; set; }

        public virtual BNE_Transacao BNE_Transacao { get; set; }
    }
}
