namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Idioma")]
    public partial class BNE_Curriculo_Idioma
    {
        [Key]
        public int Idf_Curriculo_Idioma { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Idioma { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Idioma TAB_Idioma { get; set; }
    }
}
