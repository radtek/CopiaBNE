namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Disponibilidade_Cidade")]
    public partial class BNE_Curriculo_Disponibilidade_Cidade
    {
        [Key]
        public int Idf_Curriculo_Disponibilidade_Cidade { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
