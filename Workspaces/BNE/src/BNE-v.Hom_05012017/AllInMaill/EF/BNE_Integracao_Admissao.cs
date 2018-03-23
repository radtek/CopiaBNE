namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.BNE_Integracao_Admissao")]
    public partial class BNE_Integracao_Admissao
    {
        [Key]
        public int Idf_Integracao_Admissao { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Integracao { get; set; }

        public int Idf_Integracao_Situacao { get; set; }

        public virtual BNE_Integracao_Situacao BNE_Integracao_Situacao { get; set; }
    }
}
