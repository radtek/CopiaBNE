namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Logo")]
    public partial class TAB_Tipo_Logo
    {
        public TAB_Tipo_Logo()
        {
            TAB_Pessoa_Juridica_Logo = new HashSet<TAB_Pessoa_Juridica_Logo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Logo { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Logo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica_Logo> TAB_Pessoa_Juridica_Logo { get; set; }
    }
}
