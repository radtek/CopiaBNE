namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Atividade")]
    public partial class TAB_Tipo_Atividade
    {
        public TAB_Tipo_Atividade()
        {
            TAB_Plugin = new HashSet<TAB_Plugin>();
        }

        [Key]
        public int Idf_Tipo_Atividade { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Atividade { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Num_Dias_Expiracao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Plugin> TAB_Plugin { get; set; }
    }
}
