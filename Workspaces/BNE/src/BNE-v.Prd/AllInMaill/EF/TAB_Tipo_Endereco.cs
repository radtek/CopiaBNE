namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Endereco")]
    public partial class TAB_Tipo_Endereco
    {
        public TAB_Tipo_Endereco()
        {
            TAB_Endereco_Pessoa_Juridica = new HashSet<TAB_Endereco_Pessoa_Juridica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Endereco { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Endereco { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Endereco_Pessoa_Juridica> TAB_Endereco_Pessoa_Juridica { get; set; }
    }
}
