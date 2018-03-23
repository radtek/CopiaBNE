namespace API.GatewayV2.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        public Cliente()
        {
            Requisicao = new HashSet<Requisicao>();
        }

        [Key]
        public Guid Idf_Cliente { get; set; }

        [Required]
        public string Des_Cliente { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<Requisicao> Requisicao { get; set; }
    }
}
