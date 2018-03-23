namespace API.Gateway.Console.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Perfil")]
    public partial class Perfil
    {
        public Perfil()
        {
            Usuario = new HashSet<Usuario>();
        }

        [Key]
        public int Idf_Perfil { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Perfil { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
