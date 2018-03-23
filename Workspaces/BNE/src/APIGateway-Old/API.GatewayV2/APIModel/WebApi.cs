namespace API.GatewayV2.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WebApi")]
    public partial class WebApi
    {
        public WebApi()
        {
            Endpoint = new HashSet<Endpoint>();
        }

        [Key]
        [StringLength(100)]
        public string Nme_Api { get; set; }

        [Required]
        public string Location { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<Endpoint> Endpoint { get; set; }
    }
}
