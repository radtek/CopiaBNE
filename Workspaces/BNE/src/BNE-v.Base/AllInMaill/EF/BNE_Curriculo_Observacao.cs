namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Observacao")]
    public partial class BNE_Curriculo_Observacao
    {
        [Key]
        public int Idf_Curriculo_Observacao { get; set; }

        public int Idf_Curriculo { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        public string Des_Observacao { get; set; }

        public bool Flg_Sistema { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
