namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Regra_Campo_Publicacao")]
    public partial class BNE_Regra_Campo_Publicacao
    {
        [Key]
        public int Idf_Regra_Campo_Publicacao { get; set; }

        public int Idf_Regra_Publicacao { get; set; }

        public int Idf_Campo_Publicacao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Campo_Publicacao BNE_Campo_Publicacao { get; set; }

        public virtual BNE_Regra_Publicacao BNE_Regra_Publicacao { get; set; }
    }
}
