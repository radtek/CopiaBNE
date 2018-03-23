namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Foto")]
    public partial class TAB_Pessoa_Fisica_Foto
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Pessoa_Fisica_Foto { get; set; }

        public byte[] Img_Pessoa { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
