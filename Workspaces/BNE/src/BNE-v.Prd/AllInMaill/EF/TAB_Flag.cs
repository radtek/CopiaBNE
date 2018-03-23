namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Flag")]
    public partial class TAB_Flag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Flag { get; set; }

        [Required]
        [StringLength(10)]
        public string Des_Flag { get; set; }
    }
}
