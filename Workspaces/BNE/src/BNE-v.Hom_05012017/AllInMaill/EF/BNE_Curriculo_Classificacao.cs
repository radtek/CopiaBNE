namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Classificacao")]
    public partial class BNE_Curriculo_Classificacao
    {
        [Key]
        public int Idf_Curriculo_Classificacao { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Filial { get; set; }

        [StringLength(2000)]
        public string Des_Observacao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Avaliacao { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Avaliacao TAB_Avaliacao { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
