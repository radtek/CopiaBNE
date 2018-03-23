namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Teste_Cores")]
    public partial class BNE_Teste_Cores
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string titulo { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "ntext")]
        public string texto { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid rowguid { get; set; }
    }
}
