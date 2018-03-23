namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Acao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CRM_Acao()
        {
            CRM_Fato = new HashSet<CRM_Fato>();
        }

        [Key]
        public byte Idf_Acao { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Acao { get; set; }

        public int Vlr_Peso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Fato> CRM_Fato { get; set; }
    }
}
