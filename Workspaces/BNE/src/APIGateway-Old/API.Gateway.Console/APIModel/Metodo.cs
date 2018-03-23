namespace API.Gateway.Console.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Metodo")]
    public partial class Metodo
    {
        public Metodo()
        {
            Requisicao = new HashSet<Requisicao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Idf_Metodo { get; set; }

        [Required]
        [StringLength(10)]
        public string Des_Metodo { get; set; }

        public virtual ICollection<Requisicao> Requisicao { get; set; }
    }
}
