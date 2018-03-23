namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Nivel_Idioma")]
    public partial class TAB_Nivel_Idioma
    {
        public TAB_Nivel_Idioma()
        {
            TAB_Pessoa_Fisica_Idioma = new HashSet<TAB_Pessoa_Fisica_Idioma>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Nivel_Idioma { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Nivel_Idioma { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Idioma> TAB_Pessoa_Fisica_Idioma { get; set; }
    }
}
