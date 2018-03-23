namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Nivel_Curso")]
    public partial class TAB_Nivel_Curso
    {
        public TAB_Nivel_Curso()
        {
            TAB_Curso = new HashSet<TAB_Curso>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Nivel_Curso { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Nivel_Curso { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Grau_Escolaridade { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<TAB_Curso> TAB_Curso { get; set; }

        public virtual TAB_Grau_Escolaridade TAB_Grau_Escolaridade { get; set; }
    }
}
