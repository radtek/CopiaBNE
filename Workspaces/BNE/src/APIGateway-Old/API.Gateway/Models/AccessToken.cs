namespace API.Gateway.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccessToken")]
    public partial class AccessToken
    {
        [Key]
        public int Idf_AccessToken { get; set; }

        public int Idf_Usuario { get; set; }

        public DateTime Dta_Validade { get; set; }

        [Required]
        public string Token { get; set; }

        public int ExpiresIn { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
