namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Agradecimento")]
    public partial class BNE_Agradecimento
    {
        [Key]
        public int Idf_Agradecimento { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa { get; set; }

        [Required]
        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        public int Idf_Cidade { get; set; }

        [StringLength(1000)]
        public string Des_Mensagem { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public bool Flg_Auditado { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
