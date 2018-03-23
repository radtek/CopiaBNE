namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.log_queue")]
    public partial class log_queue
    {
        [Key]
        [Column(Order = 0)]
        public int Idf_log_queue { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Des_ERROR_MESSAGE { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Dta_Cadastro { get; set; }
    }
}
