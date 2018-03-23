namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Usuario")]
    public partial class BNE_Usuario
    {
        public BNE_Usuario()
        {
            TAB_Atividade = new HashSet<TAB_Atividade>();
        }

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

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual ICollection<TAB_Atividade> TAB_Atividade { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
