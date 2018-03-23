namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Tipo_Vendedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CRM_Tipo_Vendedor()
        {
            CRM_Vendedor = new HashSet<CRM_Vendedor>();
        }

        [Key]
        public int idf_Tipo_Vendedor { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Tipo_Vendedor { get; set; }

        public bool flg_Inativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor> CRM_Vendedor { get; set; }
    }
}
