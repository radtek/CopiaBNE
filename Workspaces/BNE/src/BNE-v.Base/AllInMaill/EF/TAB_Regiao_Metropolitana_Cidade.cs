namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Regiao_Metropolitana_Cidade")]
    public partial class TAB_Regiao_Metropolitana_Cidade
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Regiao_Metropolitana { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Cidade { get; set; }

        public DateTime Dta_cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Regiao_Metropolitana TAB_Regiao_Metropolitana { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
