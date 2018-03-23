namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Campanha_Curriculo")]
    public partial class BNE_Campanha_Curriculo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Curriculo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Campanha { get; set; }

        [Required]
        [StringLength(500)]
        public string Nme_Pessoa { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [Required]
        [StringLength(9)]
        public string Num_Celular { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Campanha BNE_Campanha { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }
    }
}
