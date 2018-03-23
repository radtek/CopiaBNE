namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Idioma")]
    public partial class TAB_Pessoa_Fisica_Idioma
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Pessoa_Fisica_Idioma { get; set; }

        public int Idf_Idioma { get; set; }

        public int Idf_Nivel_Idioma { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Idioma TAB_Idioma { get; set; }

        public virtual TAB_Nivel_Idioma TAB_Nivel_Idioma { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
