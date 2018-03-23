namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador_Disponibilidade")]
    public partial class BNE_Rastreador_Disponibilidade
    {
        [Key]
        public int Idf_Rastreador_Disponibilidade { get; set; }

        public int Idf_Rastreador { get; set; }

        public int Idf_Disponibilidade { get; set; }

        public virtual BNE_Rastreador BNE_Rastreador { get; set; }

        public virtual Tab_Disponibilidade Tab_Disponibilidade { get; set; }
    }
}
