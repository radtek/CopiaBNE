namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Vendedor_Empresa
    {
        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        public DateTime Dta_Inicio { get; set; }

        public DateTime? Dta_Fim { get; set; }

        [Key]
        public int Idf_Vendedor_Empresa { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        [StringLength(100)]
        public string Des_Obs_VendedorEmpresa { get; set; }

        public int? Idf_Situacao_Atendimento { get; set; }

        public virtual CRM_Empresa CRM_Empresa { get; set; }

        public virtual CRM_Situacao_Atendimento CRM_Situacao_Atendimento { get; set; }

        public virtual CRM_Vendedor CRM_Vendedor { get; set; }
    }
}
