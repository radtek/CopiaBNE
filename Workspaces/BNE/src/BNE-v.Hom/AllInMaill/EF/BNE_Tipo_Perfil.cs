namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Perfil")]
    public partial class BNE_Tipo_Perfil
    {
        public BNE_Tipo_Perfil()
        {
            TAB_Perfil = new HashSet<TAB_Perfil>();
        }

        [Key]
        public int Idf_Tipo_Perfil { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<TAB_Perfil> TAB_Perfil { get; set; }
    }
}
