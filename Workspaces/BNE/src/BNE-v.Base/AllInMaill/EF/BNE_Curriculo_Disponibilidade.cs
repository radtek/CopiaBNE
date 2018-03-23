namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Disponibilidade")]
    public partial class BNE_Curriculo_Disponibilidade
    {
        [Key]
        public int Idf_Curriculo_Disponibilidade { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Disponibilidade { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual Tab_Disponibilidade Tab_Disponibilidade { get; set; }
    }
}
