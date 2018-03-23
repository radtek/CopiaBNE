namespace API.GatewayV2.BNEModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Usuario_Filial_Perfil")]
    public partial class TAB_Usuario_Filial_Perfil
    {
        public int? Idf_Filial { get; set; }

        [Key]
        public int Idf_Usuario_Filial_Perfil { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Pessoa_Fisica { get; set; }

        public int? Idf_Perfil { get; set; }

        [Required]
        [StringLength(10)]
        public string Sen_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Usuario_Responsavel { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
