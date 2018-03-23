namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Regra_Substituicao_Integracao")]
    public partial class TAB_Regra_Substituicao_Integracao
    {
        public TAB_Regra_Substituicao_Integracao()
        {
            TAB_Substituicao_Integracao = new HashSet<TAB_Substituicao_Integracao>();
        }

        [Key]
        public int Idf_Regra_Substituicao_Integracao { get; set; }

        public int? Idf_Campo_Integrador { get; set; }

        public int? Idf_Integrador { get; set; }

        public virtual TAB_Campo_Integrador TAB_Campo_Integrador { get; set; }

        public virtual TAB_Campo_Integrador TAB_Campo_Integrador1 { get; set; }

        public virtual TAB_Integrador TAB_Integrador { get; set; }

        public virtual TAB_Integrador TAB_Integrador1 { get; set; }

        public virtual ICollection<TAB_Substituicao_Integracao> TAB_Substituicao_Integracao { get; set; }
    }
}
