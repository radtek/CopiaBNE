namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador_Curriculo")]
    public partial class BNE_Rastreador_Curriculo
    {
        public int Idf_Rastreador { get; set; }

        public int Idf_Curriculo { get; set; }

        [Key]
        public int Idf_Rastreador_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Rastreador BNE_Rastreador { get; set; }
    }
}
