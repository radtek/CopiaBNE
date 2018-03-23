namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Feriado")]
    public partial class TAB_Feriado
    {
        [Key]
        public int Idf_Feriado { get; set; }

        public DateTime Dta_Feriado { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Motivo_Feriado { get; set; }

        public int Idf_Tipo_Feriado { get; set; }

        public int? Idf_Estado { get; set; }

        public int? Idf_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Permanente { get; set; }

        public int Idf_Usuario_Gerador { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Estado TAB_Estado { get; set; }

        public virtual TAB_Tipo_Feriado TAB_Tipo_Feriado { get; set; }
    }
}
