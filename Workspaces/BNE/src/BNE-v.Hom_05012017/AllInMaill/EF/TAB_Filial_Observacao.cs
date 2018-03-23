namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Filial_Observacao")]
    public partial class TAB_Filial_Observacao
    {
        [Key]
        public int Idf_Filial_Observacao { get; set; }

        public int Idf_Filial { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        public string Des_Observacao { get; set; }

        public bool? Flg_Sistema { get; set; }

        public DateTime? Dta_Proxima_Acao { get; set; }

        [StringLength(2000)]
        public string Des_Proxima_Acao { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
