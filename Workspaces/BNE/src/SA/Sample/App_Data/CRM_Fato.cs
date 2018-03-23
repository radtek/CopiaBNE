namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CRM_Fato
    {
        [Key]
        public int Idf_Fato { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        public int? Idf_Tempo { get; set; }

        public byte? Idf_Acao { get; set; }

        public byte? Idf_Atendimento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int? Idf_Item { get; set; }

        public virtual CRM_Acao CRM_Acao { get; set; }

        public virtual CRM_Atendimento CRM_Atendimento { get; set; }

        public virtual CRM_Empresa CRM_Empresa { get; set; }
    }
}
