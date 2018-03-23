namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Propaganda_Email")]
    public partial class BNE_Propaganda_Email
    {
        [Key]
        [Column(Order = 0)]
        public int Idf_Propaganda_Email { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Num_Cpf { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Propaganda { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Dta_Cadastro { get; set; }
    }
}
