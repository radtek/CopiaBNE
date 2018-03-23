namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Visualizacao_Historico")]
    public partial class BNE_Curriculo_Visualizacao_Historico
    {
        public int Idf_Filial { get; set; }

        public int Idf_Curriculo { get; set; }

        [Key]
        public long Idf_Curriculo_Visualizacao { get; set; }

        public DateTime Dta_Visualizacao { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public bool Flg_Curriculo_VIP { get; set; }

        public bool? Flg_Visualizacao_Completa { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
