namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Campanha_CV_Desatualizado")]
    public partial class TAB_Campanha_CV_Desatualizado
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }
    }
}
