using System.Linq;

namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Empresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CRM_Empresa()
        {
            CRM_Fato = new HashSet<CRM_Fato>();
            CRM_Vendedor_Empresa = new HashSet<CRM_Vendedor_Empresa>();
            CRM_Vendedor_Empresa1 = new HashSet<CRM_Vendedor_Empresa1>();
            CRM_Vendedor_Empresa2 = new HashSet<CRM_Vendedor_Empresa2>();
            CRM_Vendedor_Empresa3 = new HashSet<CRM_Vendedor_Empresa3>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        public int Idf_Filial { get; set; }

        [Required]
        [StringLength(200)]
        public string Raz_Social { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Ultimo_Acesso { get; set; }

        public int? Idf_Plano { get; set; }

        [StringLength(100)]
        public string Des_Plano { get; set; }

        public DateTime? Dta_Inicio { get; set; }

        public DateTime? Dta_Fim { get; set; }

        public short? Qtd_Dias_Plano_Ativo { get; set; }

        public int? Qtd_Acabar_Plano { get; set; }

        public int? Idf_Situacao_Atendimento { get; set; }

        public DateTime? Dta_Ultimo_Atendimento { get; set; }

        [StringLength(80)]
        public string Nme_Cidade { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public int? Idf_Cidade { get; set; }

        public DateTime? Dta_Retorno { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public virtual CRM_Situacao_Atendimento CRM_Situacao_Atendimento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Fato> CRM_Fato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa> CRM_Vendedor_Empresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa1> CRM_Vendedor_Empresa1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa2> CRM_Vendedor_Empresa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRM_Vendedor_Empresa3> CRM_Vendedor_Empresa3 { get; set; }

        public CRM_Vendedor RecuperarVendedorResponsavel()
        {
            var data = DateTime.Now;
            var vendedor = this.CRM_Vendedor_Empresa.Where(c => c.Dta_Inicio < data && c.Dta_Fim >= data).OrderBy(c => c.Dta_Inicio).FirstOrDefault();
            if (vendedor != null)
            {
                return vendedor.CRM_Vendedor;
            }
            return null;
        }
    }
}
