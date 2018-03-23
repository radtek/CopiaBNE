namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Situacao_Atendimento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CRM_Situacao_Atendimento()
        {
            CRM_Empresa = new HashSet<CRM_Empresa>();
            CRM_Vendedor_Empresa = new HashSet<CRM_Vendedor_Empresa>();
            CRM_Vendedor_Empresa1 = new HashSet<CRM_Vendedor_Empresa1>();
            CRM_Vendedor_Empresa2 = new HashSet<CRM_Vendedor_Empresa2>();
            CRM_Vendedor_Empresa3 = new HashSet<CRM_Vendedor_Empresa3>();
        }

        [Key]
        public int Idf_Situacao_Atendimento { get; set; }

        [StringLength(30)]
        public string Des_Situacao_atendimento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Empresa> CRM_Empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa> CRM_Vendedor_Empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa1> CRM_Vendedor_Empresa1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa2> CRM_Vendedor_Empresa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa3> CRM_Vendedor_Empresa3 { get; set; }
    }
}
