namespace API.Gateway.Console.BNEModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Usuario")]
    public partial class BNE_Usuario
    {
        [Key]
        public int Idf_Usuario { get; set; }

        public int Idf_Pessoa_Fisica { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        [StringLength(10)]
        public string Sen_Usuario { get; set; }

        public int? Idf_Ultima_Filial_Logada { get; set; }

        [StringLength(50)]
        public string Des_Session_ID { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public DateTime? Dta_Ultima_Atividade { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
