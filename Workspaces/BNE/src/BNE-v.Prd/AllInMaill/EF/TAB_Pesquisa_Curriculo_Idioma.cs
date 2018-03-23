namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Curriculo_Idioma")]
    public partial class TAB_Pesquisa_Curriculo_Idioma
    {
        [Key]
        public int Idf_Pesquisa_Curriculo_Idioma { get; set; }

        public int Idf_Pesquisa_Curriculo { get; set; }

        public int Idf_Idioma { get; set; }

        public virtual TAB_Idioma TAB_Idioma { get; set; }

        public virtual TAB_Pesquisa_Curriculo TAB_Pesquisa_Curriculo { get; set; }
    }
}
