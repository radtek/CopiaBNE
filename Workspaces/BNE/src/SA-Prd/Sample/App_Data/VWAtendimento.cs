namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VWAtendimentos")]
    public partial class VWAtendimentos
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal CNPJ { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string Tipo_Atendimento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CPF { get; set; }

        public byte? Idf_Tipo_Atendimento { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Atendimento { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Flg_Administrador { get; set; }

        [Column(Order = 3)]
        [StringLength(2000)]
        public string Des_Observacao { get; set; }

        [Column(Order = 4)]
        [StringLength(200)]
        public string Nme_Vendedor { get; set; }
    }
}
