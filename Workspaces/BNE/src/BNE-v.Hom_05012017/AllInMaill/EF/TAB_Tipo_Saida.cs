namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Saida")]
    public partial class TAB_Tipo_Saida
    {
        public TAB_Tipo_Saida()
        {
            TAB_Atividade = new HashSet<TAB_Atividade>();
        }

        [Key]
        public int Idf_Tipo_Saida { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Saida { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Atividade> TAB_Atividade { get; set; }
    }
}
