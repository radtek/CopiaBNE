namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Grau_Escolaridade")]
    public partial class TAB_Grau_Escolaridade
    {
        public TAB_Grau_Escolaridade()
        {
            TAB_Nivel_Curso = new HashSet<TAB_Nivel_Curso>();
            TAB_Escolaridade = new HashSet<TAB_Escolaridade>();
            TAB_Curso = new HashSet<TAB_Curso>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Grau_Escolaridade { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Grau_Escolaridade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Nivel_Curso> TAB_Nivel_Curso { get; set; }

        public virtual ICollection<TAB_Escolaridade> TAB_Escolaridade { get; set; }

        public virtual ICollection<TAB_Curso> TAB_Curso { get; set; }
    }
}
