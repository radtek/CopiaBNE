namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Simples")]
    public partial class TAB_Simples
    {
        public TAB_Simples()
        {
            TAB_Pessoa_Juridica = new HashSet<TAB_Pessoa_Juridica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Simples { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Simples { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica> TAB_Pessoa_Juridica { get; set; }
    }
}
