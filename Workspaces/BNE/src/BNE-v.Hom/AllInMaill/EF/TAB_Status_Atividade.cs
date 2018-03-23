namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Status_Atividade")]
    public partial class TAB_Status_Atividade
    {
        public TAB_Status_Atividade()
        {
            TAB_Atividade = new HashSet<TAB_Atividade>();
        }

        [Key]
        public int Idf_Status_Atividade { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Status_Atividade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Atividade> TAB_Atividade { get; set; }
    }
}
