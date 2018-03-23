namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Intencao_Filial")]
    public partial class BNE_Intencao_Filial
    {
        [Key]
        public int Idf_Intencao_Filial { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Filial { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
