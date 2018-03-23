namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Mini_Agrupadora")]
    public partial class TAB_Mini_Agrupadora
    {
        [Key]
        public int Idf_Mini_Agrupadora { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Mini_Agrupadora { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }
    }
}
