namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Inscricao_Estadual")]
    public partial class TAB_Inscricao_Estadual
    {
        [Key]
        public int Idf_Inscricao_Estadual { get; set; }

        public int Idf_Pessoa_Juridica { get; set; }

        public int Idf_Estado { get; set; }

        [Required]
        [StringLength(15)]
        public string Num_Inscricao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual TAB_Estado TAB_Estado { get; set; }

        public virtual TAB_Pessoa_Juridica TAB_Pessoa_Juridica { get; set; }
    }
}
