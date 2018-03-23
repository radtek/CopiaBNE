namespace API.Gateway.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Endpoint")]
    public partial class Endpoint
    {
        public Endpoint()
        {
            Quota = new HashSet<Quota>();
            Requisicao = new HashSet<Requisicao>();
        }

        [Key]
        public int Idf_Endpoint { get; set; }

        [Required]
        [StringLength(10)]
        public string VersionAPI { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Api { get; set; }

        [Required]
        [StringLength(100)]
        public string Controller { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual WebApi WebApi { get; set; }

        public virtual ICollection<Quota> Quota { get; set; }

        public virtual ICollection<Requisicao> Requisicao { get; set; }
    }
}
