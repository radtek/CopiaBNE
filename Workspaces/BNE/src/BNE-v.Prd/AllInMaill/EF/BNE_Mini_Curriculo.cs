namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Mini_Curriculo")]
    public partial class BNE_Mini_Curriculo
    {
        public int? Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Mini_Curriculo { get; set; }

        [Required]
        public string Des_Mini_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int? Idf_Pessoa_Fisica_Temp { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }

        public virtual TAB_Pessoa_Fisica_Temp TAB_Pessoa_Fisica_Temp { get; set; }
    }
}
