//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LanHouse.Entities.BNE
{
    using System;
    using System.Collections.Generic;
    
    public partial class BNE_Curriculo_Observacao
    {
        public int Idf_Curriculo_Observacao { get; set; }
        public int Idf_Curriculo { get; set; }
        public Nullable<int> Idf_Usuario_Filial_Perfil { get; set; }
        public System.DateTime Dta_Cadastro { get; set; }
        public bool Flg_Inativo { get; set; }
        public string Des_Observacao { get; set; }
        public bool Flg_Sistema { get; set; }
    
        public virtual BNE_Curriculo BNE_Curriculo { get; set; }
        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
