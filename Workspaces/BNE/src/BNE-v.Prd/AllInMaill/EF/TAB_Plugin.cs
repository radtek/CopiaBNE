namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Plugin")]
    public partial class TAB_Plugin
    {
        public TAB_Plugin()
        {
            TAB_Plugins_Compatibilidade = new HashSet<TAB_Plugins_Compatibilidade>();
            TAB_Plugins_Compatibilidade1 = new HashSet<TAB_Plugins_Compatibilidade>();
        }

        [Key]
        public int Idf_Plugin { get; set; }

        public int Idf_Tipo_Plugin { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Plugin { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Plugin_Metadata { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int? Cod_Plugin { get; set; }

        public int Idf_Tipo_Atividade { get; set; }

        public virtual ICollection<TAB_Plugins_Compatibilidade> TAB_Plugins_Compatibilidade { get; set; }

        public virtual ICollection<TAB_Plugins_Compatibilidade> TAB_Plugins_Compatibilidade1 { get; set; }

        public virtual TAB_Tipo_Atividade TAB_Tipo_Atividade { get; set; }

        public virtual TAB_Tipo_Plugin TAB_Tipo_Plugin { get; set; }
    }
}
