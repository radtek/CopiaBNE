namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Sanguineo")]
    public partial class TAB_Tipo_Sanguineo
    {
        public TAB_Tipo_Sanguineo()
        {
            TAB_Pessoa_Fisica_Complemento = new HashSet<TAB_Pessoa_Fisica_Complemento>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Sanguineo { get; set; }

        [Required]
        [StringLength(3)]
        public string Des_Tipo_Sanguineo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Complemento> TAB_Pessoa_Fisica_Complemento { get; set; }
    }
}
