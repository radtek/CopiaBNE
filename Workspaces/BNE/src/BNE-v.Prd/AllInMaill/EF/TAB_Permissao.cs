namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Permissao")]
    public partial class TAB_Permissao
    {
        public TAB_Permissao()
        {
            TAB_Perfil = new HashSet<TAB_Perfil>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Permissao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Permissao { get; set; }

        public int Idf_Categoria_Permissao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(10)]
        public string Cod_Permissao { get; set; }

        public virtual TAB_Categoria_Permissao TAB_Categoria_Permissao { get; set; }

        public virtual ICollection<TAB_Perfil> TAB_Perfil { get; set; }
    }
}
