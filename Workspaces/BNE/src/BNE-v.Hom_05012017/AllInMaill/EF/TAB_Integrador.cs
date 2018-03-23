namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Integrador")]
    public partial class TAB_Integrador
    {
        public TAB_Integrador()
        {
            BNE_Vaga_Integracao = new HashSet<BNE_Vaga_Integracao>();
            TAB_Regra_Substituicao_Integracao = new HashSet<TAB_Regra_Substituicao_Integracao>();
            TAB_Parametro_Integrador = new HashSet<TAB_Parametro_Integrador>();
            TAB_Regra_Substituicao_Integracao1 = new HashSet<TAB_Regra_Substituicao_Integracao>();
        }

        [Key]
        public int Idf_Integrador { get; set; }

        public int Idf_Filial { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Tipo_Integrador { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<BNE_Vaga_Integracao> BNE_Vaga_Integracao { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual ICollection<TAB_Regra_Substituicao_Integracao> TAB_Regra_Substituicao_Integracao { get; set; }

        public virtual ICollection<TAB_Parametro_Integrador> TAB_Parametro_Integrador { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Tipo_Integrador TAB_Tipo_Integrador { get; set; }

        public virtual ICollection<TAB_Regra_Substituicao_Integracao> TAB_Regra_Substituicao_Integracao1 { get; set; }
    }
}
