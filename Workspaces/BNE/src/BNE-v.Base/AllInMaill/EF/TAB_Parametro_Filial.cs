namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Parametro_Filial")]
    public partial class TAB_Parametro_Filial
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Parametro { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Filial { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        public string Vlr_Parametro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
