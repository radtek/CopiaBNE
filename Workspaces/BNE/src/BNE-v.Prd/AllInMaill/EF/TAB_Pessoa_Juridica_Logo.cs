namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Pessoa_Juridica_Logo")]
    public partial class TAB_Pessoa_Juridica_Logo
    {
        [Key]
        public int Idf_Pessoa_Juridica_Logo { get; set; }

        public int Idf_Pessoa_Juridica { get; set; }

        public int Idf_Tipo_Logo { get; set; }

        [Required]
        public byte[] Img_Logo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual TAB_Pessoa_Juridica TAB_Pessoa_Juridica { get; set; }

        public virtual TAB_Tipo_Logo TAB_Tipo_Logo { get; set; }
    }
}
