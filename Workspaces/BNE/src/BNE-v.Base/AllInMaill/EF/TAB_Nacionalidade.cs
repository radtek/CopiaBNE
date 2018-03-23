namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Nacionalidade")]
    public partial class TAB_Nacionalidade
    {
        public TAB_Nacionalidade()
        {
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Nacionalidade { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Nacionalidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }
    }
}
