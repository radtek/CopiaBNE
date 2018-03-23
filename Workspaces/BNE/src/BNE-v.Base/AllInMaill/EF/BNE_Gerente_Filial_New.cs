namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Gerente_Filial_New")]
    public partial class BNE_Gerente_Filial_New
    {
        public long? Idf_Gerente_Filial { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string Nme_Gerente_Filial { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Filial { get; set; }

        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string Ape_Filial { get; set; }
    }
}
