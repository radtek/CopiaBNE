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
    
    public partial class BNE_Situacao_Formacao
    {
        public BNE_Situacao_Formacao()
        {
            this.BNE_Formacao = new HashSet<BNE_Formacao>();
        }
    
        public short Idf_Situacao_Formacao { get; set; }
        public string Des_Situacao_Formacao { get; set; }
        public bool Flg_Inativo { get; set; }
        public System.DateTime Dta_Cadastro { get; set; }
    
        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }
    }
}
