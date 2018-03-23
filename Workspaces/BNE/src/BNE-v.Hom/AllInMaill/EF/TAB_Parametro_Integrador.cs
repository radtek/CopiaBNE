namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Parametro_Integrador")]
    public partial class TAB_Parametro_Integrador
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Integrador { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Parametro { get; set; }

        [Required]
        public string Vlr_Parametro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual TAB_Integrador TAB_Integrador { get; set; }

        public virtual TAB_Parametro TAB_Parametro { get; set; }
    }
}
