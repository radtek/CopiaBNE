namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Disponibilidade")]
    public partial class BNE_Vaga_Disponibilidade
    {
        [Key]
        public int Idf_Vaga_Disponibilidade { get; set; }

        public int Idf_Disponibilidade { get; set; }

        public int Idf_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }

        public virtual Tab_Disponibilidade Tab_Disponibilidade { get; set; }
    }
}
