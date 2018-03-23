namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Rede_Social")]
    public partial class TAB_Pessoa_Fisica_Rede_Social
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Pessoa_Fisica_Rede_Social { get; set; }

        public int Idf_Rede_Social_CS { get; set; }

        [Required]
        [StringLength(350)]
        public string Cod_Identificador { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(30)]
        public string Cod_Interno_Rede_Social { get; set; }

        public virtual BNE_Rede_Social_CS BNE_Rede_Social_CS { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
