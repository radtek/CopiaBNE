namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Propaganda_Estado")]
    public partial class BNE_Propaganda_Estado
    {
        [Key]
        public int Idf_Propaganda_Estado { get; set; }

        public int Idf_Estado { get; set; }

        public int Num_Dia_Semana { get; set; }

        public virtual TAB_Estado TAB_Estado { get; set; }
    }
}
