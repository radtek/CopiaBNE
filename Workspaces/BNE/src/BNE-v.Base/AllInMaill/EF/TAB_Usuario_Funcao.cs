namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Usuario_Funcao")]
    public partial class TAB_Usuario_Funcao
    {
        [Key]
        public int Idf_Usuario_Funcao { get; set; }

        public int Idf_Funcao { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
