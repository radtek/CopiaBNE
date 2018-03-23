namespace API.Gateway.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KeyToken")]
    public partial class KeyToken
    {
        [Key]
        [StringLength(40)]
        public string KeyValue { get; set; }

        public bool Flg_Ativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }
    }
}
