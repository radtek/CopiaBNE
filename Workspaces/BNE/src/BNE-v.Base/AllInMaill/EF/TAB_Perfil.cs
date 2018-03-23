namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Perfil")]
    public partial class TAB_Perfil
    {
        public TAB_Perfil()
        {
            TAB_Perfil_Usuario = new HashSet<TAB_Perfil_Usuario>();
            TAB_Usuario_Filial_Perfil = new HashSet<TAB_Usuario_Filial_Perfil>();
            TAB_Permissao = new HashSet<TAB_Permissao>();
        }

        [Key]
        public int Idf_Perfil { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Perfil { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public int Idf_Tipo_Perfil { get; set; }

        public virtual BNE_Tipo_Perfil BNE_Tipo_Perfil { get; set; }

        public virtual ICollection<TAB_Perfil_Usuario> TAB_Perfil_Usuario { get; set; }

        public virtual ICollection<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<TAB_Permissao> TAB_Permissao { get; set; }
    }
}
