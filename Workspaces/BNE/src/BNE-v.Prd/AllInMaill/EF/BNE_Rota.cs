namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rota")]
    public partial class BNE_Rota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Rota { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Rota { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_URL { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_Caminho_Fisico { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Num_Peso { get; set; }
    }
}
