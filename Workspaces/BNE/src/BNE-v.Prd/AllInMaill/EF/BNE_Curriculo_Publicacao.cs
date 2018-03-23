namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Publicacao")]
    public partial class BNE_Curriculo_Publicacao
    {
        [Key]
        public int Idf_Curriculo_Publicacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public int Idf_Curriculo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
