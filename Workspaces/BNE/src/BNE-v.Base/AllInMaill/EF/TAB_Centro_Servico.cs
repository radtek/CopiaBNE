namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Centro_Servico")]
    public partial class TAB_Centro_Servico
    {
        public TAB_Centro_Servico()
        {
            TAB_Mensagem = new HashSet<TAB_Mensagem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Centro_Servico { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Centro_Servico { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(30)]
        public string Log_DB { get; set; }

        [Required]
        [StringLength(50)]
        public string Sen_DB { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Schema { get; set; }

        public bool Flg_Robo_SMS { get; set; }

        public bool Flg_Robo_Email { get; set; }

        public virtual ICollection<TAB_Mensagem> TAB_Mensagem { get; set; }
    }
}
