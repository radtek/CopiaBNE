namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Status_Mensagem")]
    public partial class TAB_Status_Mensagem
    {
        [Key]
        public int Idf_Status_Mensagem { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Status_Mensagem { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }
    }
}
