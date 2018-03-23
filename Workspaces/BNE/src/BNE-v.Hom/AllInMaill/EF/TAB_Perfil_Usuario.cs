namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Perfil_Usuario")]
    public partial class TAB_Perfil_Usuario
    {
        public int Idf_Usuario_Filial_Perfil { get; set; }

        [Key]
        public int Idf_Perfil_Usuario { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Inicio { get; set; }

        public DateTime? Dta_Fim { get; set; }

        public virtual TAB_Perfil TAB_Perfil { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
