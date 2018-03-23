namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_random")]
    public partial class TAB_random
    {
        public long? laga { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(72)]
        public string Nme_Localidade { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public int? Cod_IBGE { get; set; }
    }
}
