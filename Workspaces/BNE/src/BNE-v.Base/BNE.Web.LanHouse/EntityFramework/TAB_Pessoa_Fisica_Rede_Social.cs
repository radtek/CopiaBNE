//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BNE.Web.LanHouse.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAB_Pessoa_Fisica_Rede_Social
    {
        public int Idf_Pessoa_Fisica { get; set; }
        public int Idf_Pessoa_Fisica_Rede_Social { get; set; }
        public int Idf_Rede_Social_CS { get; set; }
        public string Cod_Identificador { get; set; }
        public bool Flg_Inativo { get; set; }
        public System.DateTime Dta_Cadastro { get; set; }
        public string Cod_Interno_Rede_Social { get; set; }
    
        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
