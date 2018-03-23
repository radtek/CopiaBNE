namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Fila")]
    public partial class BNE_Curriculo_Fila
    {
        [Key]
        public int Idf_Curriculo_Fila { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Publicador { get; set; }

        public int? Idf_Revidor_Order { get; set; }

        public int? Idf_Curriculo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Publicador BNE_Publicador { get; set; }

        public virtual BNE_Revidor BNE_Revidor { get; set; }
    }
}
