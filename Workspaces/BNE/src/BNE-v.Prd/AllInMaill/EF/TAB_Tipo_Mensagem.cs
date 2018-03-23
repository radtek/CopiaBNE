namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Mensagem")]
    public partial class TAB_Tipo_Mensagem
    {
        [Key]
        public int Idf_Tipo_Mensagem { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Mensagem { get; set; }

        public bool Flg_inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
