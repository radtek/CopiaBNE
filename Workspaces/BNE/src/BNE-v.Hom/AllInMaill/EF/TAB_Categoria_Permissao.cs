namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Categoria_Permissao")]
    public partial class TAB_Categoria_Permissao
    {
        public TAB_Categoria_Permissao()
        {
            TAB_Permissao = new HashSet<TAB_Permissao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Categoria_Permissao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Categoria_Permissao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Permissao> TAB_Permissao { get; set; }
    }
}
