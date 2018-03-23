namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Auditor_Publicador")]
    public partial class BNE_Auditor_Publicador
    {
        [Key]
        public int Idf_Auditor_Publicador { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public int Idf_Publicador { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool? Flg_Inativo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil1 { get; set; }
    }
}
