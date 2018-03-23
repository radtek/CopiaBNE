namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Propaganda")]
    public partial class BNE_Propaganda
    {
        [Key]
        public int Idf_Propaganda { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Propaganda { get; set; }

        public string Des_Email_Propaganda { get; set; }

        [StringLength(200)]
        public string Des_SMS_Propaganda { get; set; }

        public bool Flg_Empresa { get; set; }

        public bool Flg_Candidato { get; set; }

        public int? Idf_Cidade { get; set; }

        public bool? Flg_Diario { get; set; }

        public int? Idf_Dia_Semana { get; set; }

        public int? Idf_Dia_Mes { get; set; }

        public bool? Flg_Inativo { get; set; }

        [StringLength(200)]
        public string Des_Titulo_Propaganda { get; set; }

        [StringLength(200)]
        public string Eml_Remetente { get; set; }
    }
}
