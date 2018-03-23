namespace API.GatewayV2.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quota")]
    public partial class Quota
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Perfil { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Endpoint { get; set; }

        public long Per_Second { get; set; }

        public long Per_Minute { get; set; }

        public long Per_Hour { get; set; }

        public long Per_Day { get; set; }

        public long Per_Week { get; set; }

        public long Per_Month { get; set; }

        public long Total_Limit { get; set; }

        public virtual Endpoint Endpoint { get; set; }
    }
}
