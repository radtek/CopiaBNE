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
    
    public partial class TAB_Pesquisa_Salarial
    {
        public int Idf_Pesquisa_Salarial { get; set; }
        public int Idf_Funcao { get; set; }
        public Nullable<int> Idf_Estado { get; set; }
        public decimal Vlr_Media { get; set; }
        public decimal Vlr_Maximo { get; set; }
        public decimal Vlr_Minimo { get; set; }
        public System.DateTime Dta_Atualizacao { get; set; }
        public System.DateTime Dta_Cadastro { get; set; }
        public long Num_Populacao { get; set; }
        public int Num_Amostra { get; set; }
        public decimal Vlr_Junior { get; set; }
        public decimal Vlr_Treinee { get; set; }
        public decimal Vlr_Senior { get; set; }
        public decimal Vlr_Master { get; set; }
        public decimal Vlr_Pleno { get; set; }
    
        public virtual TAB_Funcao TAB_Funcao { get; set; }
        public virtual TAB_Estado TAB_Estado { get; set; }
    }
}
