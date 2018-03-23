namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Usuario_Filial")]
    public partial class BNE_Usuario_Filial
    {
        [Key]
        public int Idf_Usuario_Filial { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public int? Idf_Funcao { get; set; }

        [StringLength(50)]
        public string Des_Funcao { get; set; }

        [StringLength(4)]
        public string Num_Ramal { get; set; }

        [StringLength(2)]
        public string Num_DDD_Comercial { get; set; }

        [StringLength(10)]
        public string Num_Comercial { get; set; }

        [StringLength(100)]
        public string Eml_Comercial { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public int? Idf_Email_Situacao_Bloqueio { get; set; }

        public int? Idf_Email_Situacao_Confirmacao { get; set; }

        public int? Idf_Email_Situacao_Validacao { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao1 { get; set; }

        public virtual TAB_Email_Situacao TAB_Email_Situacao2 { get; set; }
    }
}
