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
    
    public partial class BNE_Usuario_Filial
    {
        public int Idf_Usuario_Filial { get; set; }
        public int Idf_Usuario_Filial_Perfil { get; set; }
        public Nullable<int> Idf_Funcao { get; set; }
        public string Des_Funcao { get; set; }
        public string Num_Ramal { get; set; }
        public string Num_DDD_Comercial { get; set; }
        public string Num_Comercial { get; set; }
        public string Eml_Comercial { get; set; }
        public Nullable<System.DateTime> Dta_Cadastro { get; set; }
        public Nullable<int> Idf_Email_Situacao_Bloqueio { get; set; }
        public Nullable<int> Idf_Email_Situacao_Confirmacao { get; set; }
        public Nullable<int> Idf_Email_Situacao_Validacao { get; set; }
    
        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
