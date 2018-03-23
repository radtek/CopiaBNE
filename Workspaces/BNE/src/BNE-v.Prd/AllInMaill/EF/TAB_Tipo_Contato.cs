namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Contato")]
    public partial class TAB_Tipo_Contato
    {
        public TAB_Tipo_Contato()
        {
            TAB_Contato = new HashSet<TAB_Contato>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Contato { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Contato { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Contato> TAB_Contato { get; set; }
    }
}
