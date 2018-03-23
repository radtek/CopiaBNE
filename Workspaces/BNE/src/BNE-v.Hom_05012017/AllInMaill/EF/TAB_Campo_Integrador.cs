namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Campo_Integrador")]
    public partial class TAB_Campo_Integrador
    {
        public TAB_Campo_Integrador()
        {
            TAB_Regra_Substituicao_Integracao = new HashSet<TAB_Regra_Substituicao_Integracao>();
            TAB_Regra_Substituicao_Integracao1 = new HashSet<TAB_Regra_Substituicao_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Campo_Integrador { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Campo_Integrador { get; set; }

        public virtual ICollection<TAB_Regra_Substituicao_Integracao> TAB_Regra_Substituicao_Integracao { get; set; }

        public virtual ICollection<TAB_Regra_Substituicao_Integracao> TAB_Regra_Substituicao_Integracao1 { get; set; }
    }
}
