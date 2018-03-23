namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Requisicao_Integrador_Curriculo")]
    public partial class TAB_Requisicao_Integrador_Curriculo
    {
        [Key]
        public int Idf_Requisicao_Integrador_Curriculo { get; set; }

        public int Idf_Integrador_Curriculo { get; set; }

        public int Idf_Curriculo { get; set; }

        [StringLength(200)]
        public string Nme_Cliente { get; set; }

        public DateTime Dta_Integracao { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Integrador_Curriculo TAB_Integrador_Curriculo { get; set; }
    }
}
