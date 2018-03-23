namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Funcao_Pretendida")]
    public partial class BNE_Funcao_Pretendida
    {
        public int Idf_Curriculo { get; set; }

        [Key]
        public int Idf_Funcao_Pretendida { get; set; }

        public int? Idf_Funcao { get; set; }

        public int Qtd_Experiencia { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(50)]
        public string Des_Funcao_Pretendida { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
