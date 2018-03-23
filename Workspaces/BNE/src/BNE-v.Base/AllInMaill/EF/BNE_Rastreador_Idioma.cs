namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador_Idioma")]
    public partial class BNE_Rastreador_Idioma
    {
        [Key]
        public int Idf_Rastreador_Idioma { get; set; }

        public int Idf_Rastreador { get; set; }

        public int Idf_Idioma { get; set; }

        public virtual BNE_Rastreador BNE_Rastreador { get; set; }

        public virtual TAB_Idioma TAB_Idioma { get; set; }
    }
}
