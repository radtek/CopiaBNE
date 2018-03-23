namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Plugin")]
    public partial class TAB_Tipo_Plugin
    {
        public TAB_Tipo_Plugin()
        {
            TAB_Plugin = new HashSet<TAB_Plugin>();
        }

        [Key]
        public int Idf_Tipo_Plugin { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Plugin { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Plugin> TAB_Plugin { get; set; }
    }
}
