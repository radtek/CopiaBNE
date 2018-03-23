namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Integracao_SINE_2")]
    public partial class TAB_Integracao_SINE_2
    {
        [Key]
        public int Idf_Integracao_SINE_2 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public string Des_Observacao { get; set; }
    }
}
