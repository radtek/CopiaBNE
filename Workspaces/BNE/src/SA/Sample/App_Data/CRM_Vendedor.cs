namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Vendedor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CRM_Vendedor()
        {
            CRM_Vendedor_Cidade = new HashSet<CRM_Vendedor_Cidade>();
            CRM_Vendedor_Cidade1 = new HashSet<CRM_Vendedor_Cidade1>();
            CRM_Vendedor_Cidade2 = new HashSet<CRM_Vendedor_Cidade2>();
            CRM_Vendedor_Cidade3 = new HashSet<CRM_Vendedor_Cidade3>();
            CRM_Vendedor_Empresa = new HashSet<CRM_Vendedor_Empresa>();
            CRM_Vendedor_Empresa1 = new HashSet<CRM_Vendedor_Empresa1>();
            CRM_Vendedor_Empresa2 = new HashSet<CRM_Vendedor_Empresa2>();
            CRM_Vendedor_Empresa3 = new HashSet<CRM_Vendedor_Empresa3>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        [Required]
        [StringLength(300)]
        public string Nme_Vendedor { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]  
        public DateTime? Dta_Ultimo_Atendimento { get; set; }

        public bool? Flg_Efetua_Atendimento { get; set; }

        [StringLength(500)]
        public string Eml_Vendedor { get; set; }

        [StringLength(200)]
        public string Sen_Vendedor { get; set; }

        public bool Flg_Inativo { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]  
        public DateTime? Dta_Ultimo_Acesso { get; set; }

        public bool Flg_Administrador { get; set; }

        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime Dta_Ultima_Distribuicao_Empresa { get; set; }

        public int? idf_Tipo_Vendedor { get; set; }

        public virtual CRM_Tipo_Vendedor CRM_Tipo_Vendedor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Cidade> CRM_Vendedor_Cidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Cidade1> CRM_Vendedor_Cidade1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Cidade2> CRM_Vendedor_Cidade2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Cidade3> CRM_Vendedor_Cidade3 { get; set; }

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
