namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Noticia_Visualizacao")]
    public partial class BNE_Noticia_Visualizacao
    {
        [Key]
        public int Idf_Noticia_Visualizacao { get; set; }

        public int Idf_Noticia { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime? Dta_Visualizacao { get; set; }

        public virtual BNE_Noticia BNE_Noticia { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
